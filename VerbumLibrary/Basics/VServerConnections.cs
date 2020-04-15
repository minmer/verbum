// <copyright file="VServerConnections.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumLibrary.Basics
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;
    using VerbumLibrary.HelperClass;
    using VerbumLibrary.Resources;

    /// <summary>
    /// Represents a unit handling and providing <see cref="NpgsqlConnection"/>.
    /// </summary>
    public class VServerConnections
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VServerConnections"/> class.
        /// </summary>
        /// <param name="arguments">The <see cref="VServerConnectionArguments"/> used to connect with the server.</param>
        /// <param name="errors">The <see cref="VServerErrors"/> used to handle <see cref="VServerError"/>.</param>
        public VServerConnections(VServerConnectionArguments arguments, VServerErrors errors)
        {
            this.Arguments = arguments;
            this.Errors = errors;
            this.Connections = new List<NpgsqlConnection>();
        }

        /// <summary>
        /// Gets the <see cref="VServerConnectionArguments"/> used to connect with the server.
        /// </summary>
        public VServerConnectionArguments Arguments { get; }

        /// <summary>
        /// Gets a list of the <see cref="NpgsqlConnection"/> of the <see cref="VServerConnections"/>.
        /// </summary>
        public List<NpgsqlConnection> Connections { get; private set; }

        /// <summary>
        /// Gets the <see cref="VServerErrors"/> used to handle <see cref="VServerError"/>.
        /// </summary>
        public VServerErrors Errors { get; }

        /// <summary>
        /// Builds asynchrously a connection to the server.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ConnectAsync()
        {
            await this.Errors.NpgsqlWhileNullLoopAsync(Resources.ErrorBuildServerConnection, async () =>
            {
                NpgsqlConnection connection = new NpgsqlConnection(new NpgsqlConnectionStringBuilder { { "Server", this.Arguments.Server }, { "Port", this.Arguments.Port }, { "User Id", this.Arguments.User }, { "Password", this.Arguments.Password }, { "Database", this.Arguments.Database }, { "Timeout", 15 } }.ToString());
                await this.BuildConnectionAsync(connection).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns an open connection to the server.
        /// </summary>
        /// <returns>A <see cref="Task{NpgsqlConnection}"/> representing the asynchronous operation.</returns>
        public async Task<NpgsqlConnection> GetConnectionAsync()
        {
            int index = this.GetOpenConnectionIndex();
            if (index == -1)
            {
                await this.CloneConnectionAsync().ConfigureAwait(false);
                await Task.Delay(50).ConfigureAwait(false);
                return await this.GetConnectionAsync().ConfigureAwait(false);
            }
            else
            {
                return this.Connections[index];
            }
        }

        private async Task CloneConnectionAsync()
        {
            await this.Errors.NpgsqlWhileNullLoopAsync(Resources.ErrorCloneServerConnection, async () =>
            {
                NpgsqlConnection internalConnection = this.Connections[0].CloneWith(this.Connections[0].ConnectionString);
                await internalConnection.OpenAsync().ConfigureAwait(false);
                this.Connections.Add(internalConnection);
            }).ConfigureAwait(false);
        }

        private async Task BuildConnectionAsync(NpgsqlConnection connection)
        {
            await connection.OpenAsync().ConfigureAwait(false);
            this.Connections.Add(connection);
        }

        private int GetOpenConnectionIndex()
        {
            for (int index = 1; index < this.Connections.Count; index++)
            {
                this.CheckForClosedConnection(index);
                this.CheckForBrokenConnection(index);

                if ((this.Connections[index].FullState == ConnectionState.Open) && (this.Connections[index].FullState != ConnectionState.Connecting))
                {
                    return index;
                }
            }

            return -1;
        }

        private void CheckForBrokenConnection(int index)
        {
            if (this.Connections[index].FullState == ConnectionState.Broken)
            {
                this.Connections[index].Close();
            }
        }

        private void CheckForClosedConnection(int index)
        {
            if (this.Connections[index].FullState == ConnectionState.Closed)
            {
                this.Connections[index].Open();
            }
        }
    }
}
