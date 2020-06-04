// <copyright file="IVContentExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumLibrary.Basics
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;
    using NpgsqlTypes;
    using VerbumLibrary.HelperClass;
    using VerbumLibrary.Resources;

    /// <summary>
    /// Methods that extends the <see cref="IVContent"/>.
    /// </summary>
    public static class IVContentExtensions
    {
        /// <summary>
        /// Adds the question to the <see cref="List{IVContent}"/> and saves it in the database.
        /// </summary>
        /// <param name="content">The root <see cref="IVContent"/>.</param>
        /// <param name="link">The <see cref="IVContent"/> that should be added and saved.</param>
        /// <param name="querySchedule">The <see cref="VQuerySchedule"/> handling the <see cref="VQuery"/> of the <see cref="IVContent"/>.</param>
        public static void AddLink(this IVContent content, IVContent link, VQuerySchedule querySchedule)
        {
            content.ThrowExceptionIfNull(nameof(content)).Links.Add(link);
            querySchedule.ThrowExceptionIfNull(nameof(querySchedule)).AddQuery(new VQuery(GenerateAddLinkQuery(content, link), null, Resources.ErrorAddLink));
        }

        /// <summary>
        /// Loads the links from the database.
        /// </summary>
        /// <param name="content">The root <see cref="IVContent"/>.</param>
        /// <param name="querySchedule">The <see cref="VQuerySchedule"/> handling the <see cref="VQuery"/> of the <see cref="IVContent"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task LoadLinksAsync(this IVContent content, VQuerySchedule querySchedule)
        {
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT linkid, linktype FROM links WHERE questionid = @questionid  AND questiontype = @questiontype;", await querySchedule.ThrowExceptionIfNull(nameof(querySchedule)).ServerConnections.GetConnectionAsync().ConfigureAwait(false)))
            {
                command.Parameters.AddWithValue("@questionid", NpgsqlDbType.Integer, content.ThrowExceptionIfNull(nameof(content)).ID);
                command.Parameters.AddWithValue("@questiontype", NpgsqlDbType.Text, content.ThrowExceptionIfNull(nameof(content)).ContentType);
                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        if (!content.ThrowExceptionIfNull(nameof(content)).Links.Any(link => link.ID == reader.GetInt32(0) && link.ContentType == reader.GetString(1)))
                        {
                            content.ThrowExceptionIfNull(nameof(content)).Links.Add(InitializeContentByType(querySchedule, reader.GetInt32(0), reader.GetString(1)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes the question from the <see cref="List{IVContent}"/> and removes it in the database.
        /// </summary>
        /// <param name="content">The root <see cref="IVContent"/>.</param>
        /// <param name="link">The <see cref="IVContent"/> that should be removed.</param>
        /// <param name="querySchedule">The <see cref="VQuerySchedule"/> handling the <see cref="VQuery"/> of the <see cref="IVContent"/>.</param>
        public static void RemoveLink(this IVContent content, IVContent link, VQuerySchedule querySchedule)
        {
            content.ThrowExceptionIfNull(nameof(content)).Links.Remove(link);
            querySchedule.ThrowExceptionIfNull(nameof(querySchedule)).AddQuery(new VQuery(GenerateRemoveLinkQuery(content, link), null, Resources.ErrorAddLink));
        }

        private static IVContent InitializeContentByType(VQuerySchedule querySchedule, int id, string type)
        {
            switch (type)
            {
                case "text":
                    {
                        return new VContentText(querySchedule.ThrowExceptionIfNull(nameof(querySchedule)), id);
                    }
            }

            return null;
        }

        private static Func<NpgsqlConnection, NpgsqlCommand> GenerateAddLinkQuery(IVContent content, IVContent link)
        {
            return (NpgsqlConnection connection) =>
            {
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO links (questionid, questiontype, linkid, linktype) VALUES (@questionid, @questiontype, @linkid, @linktype);", connection);
                command.Parameters.AddWithValue("@questionid", NpgsqlDbType.Integer, content.ID);
                command.Parameters.AddWithValue("@questiontype", NpgsqlDbType.Text, content.ContentType);
                command.Parameters.AddWithValue("@linkid", NpgsqlDbType.Integer, link.ID);
                command.Parameters.AddWithValue("@linktype", NpgsqlDbType.Text, link.ContentType);
                return command;
            };
        }

        private static Func<NpgsqlConnection, NpgsqlCommand> GenerateRemoveLinkQuery(IVContent content, IVContent link)
        {
            return (NpgsqlConnection connection) =>
            {
                NpgsqlCommand command = new NpgsqlCommand("DELETE FROM links WHERE questionid = @questionid AND questiontype = @questiontype AND linkid = @linkid AND linktype = @linktype;", connection);
                command.Parameters.AddWithValue("@questionid", NpgsqlDbType.Integer, content.ID);
                command.Parameters.AddWithValue("@questiontype", NpgsqlDbType.Text, content.ContentType);
                command.Parameters.AddWithValue("@linkid", NpgsqlDbType.Integer, link.ID);
                command.Parameters.AddWithValue("@linktype", NpgsqlDbType.Text, link.ContentType);
                return command;
            };
        }
    }
}
