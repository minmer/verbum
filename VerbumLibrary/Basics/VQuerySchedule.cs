// <copyright file="VQuerySchedule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumLibrary.Basics
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;
    using VerbumLibrary.HelperClass;
    using VerbumLibrary.Resources;

    /// <summary>
    /// Represents a unit handling and providing <see cref="VQuery"/>.
    /// </summary>
    public class VQuerySchedule
    {
        private readonly VServerConnections serverConnections;
        private readonly VServerErrors serverErrors;
        private int lastSavedQueryIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="VQuerySchedule"/> class.
        /// </summary>
        /// <param name="serverConnections">The <see cref="VServerConnections"/> handling the <see cref="NpgsqlConnection"/> of the schedule.</param>
        /// <param name="serverErrors">The <see cref="VServerErrors"/> handling the <see cref="VServerError"/> of the schedule.</param>
        public VQuerySchedule(VServerConnections serverConnections, VServerErrors serverErrors)
        {
            this.serverConnections = serverConnections ?? throw new ArgumentNullException(nameof(serverConnections));
            this.serverErrors = serverErrors ?? throw new ArgumentNullException(nameof(serverErrors));
            this.Queries = new List<VQuery>();
            this.lastSavedQueryIndex = 0;
        }

        /// <summary>
        /// Handles the addition of an <see cref="VQuery"/>.
        /// </summary>
        /// <param name="query">The <see cref="VQuery"/>, that was added.</param>
        public delegate void QueryAddedHandler(VQuery query);

        /// <summary>
        /// Handles the completion of all <see cref="VQuery"/>.
        /// </summary>
        public delegate void CompletionHandler();

        /// <summary>
        /// An event raised when an <see cref="VQuery"/> was added.
        /// </summary>
        public event QueryAddedHandler QueryAdded;

        /// <summary>
        /// An event raised when all <see cref="VQuery"/> were uploaded.
        /// </summary>
        public event CompletionHandler Completion;

        /// <summary>
        /// Gets a list of all scheduled <see cref="VQuery"/>.
        /// </summary>
        public List<VQuery> Queries { get; }

        /// <summary>
        /// Adds an <see cref="VQuery"/>.
        /// </summary>
        /// <param name="query">The <see cref="VQuery"/>, that should be added.</param>
        public void AddQuery(VQuery query)
        {
            this.Queries.Add(query);
            this.OnQueryAdded(query);
            if (this.lastSavedQueryIndex < this.Queries.Count)
            {
                _ = this.UploadRemainingQueriesAsync();
            }
        }

        /// <summary>
        /// Invokes the QueryAdded event of the <see cref="VQuerySchedule"/>.
        /// </summary>
        /// <param name="query">The <see cref="VQuery"/>, that was added.</param>
        protected virtual void OnQueryAdded(VQuery query)
        {
            this.QueryAdded?.Invoke(query);
        }

        /// <summary>
        /// Invokes the Completion event of the <see cref="VQuerySchedule"/>.
        /// </summary>
        protected virtual void OnCompletion()
        {
            this.Completion?.Invoke();
        }

        private async Task UploadRemainingQueriesAsync()
        {
            while (this.Queries.Count > this.lastSavedQueryIndex)
            {
                await this.serverErrors.NpgsqlWhileNullLoopAsync(Resources.ErrorUploadQuery, async () =>
                {
                    await this.UploadQueryAsync(this.Queries[this.lastSavedQueryIndex]).ConfigureAwait(false);
                    this.lastSavedQueryIndex++;
                }).ConfigureAwait(false);
            }

            this.OnCompletion();
        }

        private async Task UploadQueryAsync(VQuery query)
        {
            using (NpgsqlCommand command = query.Command.Invoke(await this.serverConnections.GetConnectionAsync().ConfigureAwait(false)))
            {
                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(true))
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync().ConfigureAwait(true);
                        query.Action?.Invoke(reader);
                    }

                    query.IsSaved = true;
                }
            }
        }
    }
}
