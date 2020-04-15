// <copyright file="VQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumLibrary.Basics
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Text;
    using Npgsql;

    /// <summary>
    /// Describes a query for the server.
    /// </summary>
    public struct VQuery : IEquatable<VQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VQuery"/> struct.
        /// </summary>
        /// <param name="command">A function that generates the command of the <see cref="VQuery"/>.</param>
        /// <param name="action">The Action of the <see cref="VQuery"/>.</param>
        /// <param name="description">The description of the query for exceptions.</param>
        public VQuery(Func<NpgsqlConnection, NpgsqlCommand> command, Action<DbDataReader> action, string description)
        {
            this.Command = command;
            this.Action = action;
            this.Description = description;
            this.Time = DateTime.Now;
            this.IsSaved = false;
        }

        /// <summary>
        /// Gets the Action of the <see cref="VQuery"/>.
        /// </summary>
        public Action<DbDataReader> Action { get; }

        /// <summary>
        /// Gets a function that generates the command of the <see cref="VQuery"/>.
        /// </summary>
        public Func<NpgsqlConnection, NpgsqlCommand> Command { get; }

        /// <summary>
        /// Gets a description of the <see cref="VQuery"/>.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the query was saved.
        /// </summary>
        public bool IsSaved { get; set; }

        /// <summary>
        /// Gets the time when the error was raised.
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Checks two parameters for equality.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the equality check.</returns>
        public static bool operator ==(VQuery left, VQuery right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks two parameters for inequality.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the inequality check.</returns>
        public static bool operator !=(VQuery left, VQuery right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is VQuery arguments)
            {
                return this.Equals(arguments);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(VQuery other)
        {
            if ((this.Action == other.Action) && (this.Command == other.Command) && (this.Description == other.Description) && (this.Time == other.Time))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (int)this.Time.Ticks;
        }
    }
}
