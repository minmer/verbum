// <copyright file="VServerConnectionArguments.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumEssentials.Basics
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Describes objects necessary for a serverconnection.
    /// </summary>
    public struct VServerConnectionArguments : IEquatable<VServerConnectionArguments>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VServerConnectionArguments"/> struct.
        /// </summary>
        /// <param name="server">A string representation of the server.</param>
        /// <param name="port">The port of the connection.</param>
        /// <param name="user">A string representation of the user.</param>
        /// <param name="database">A string representation of the database.</param>
        /// <param name="password">A string representation of the password.</param>
        public VServerConnectionArguments(string server, int port, string user, string database, string password)
        {
            this.Server = server;
            this.Port = port;
            this.User = user;
            this.Database = database;
            this.Password = password;
        }

        /// <summary>
        /// Gets a string representation of the database.
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// Gets a string representation of the password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the port of the connection.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets a string representation of the server.
        /// </summary>
        public string Server { get; }

        /// <summary>
        /// Gets a string representation of the user.
        /// </summary>
        public string User { get; }

        /// <summary>
        /// Checks two parameters for equality.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the equality check.</returns>
        public static bool operator ==(VServerConnectionArguments left, VServerConnectionArguments right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks two parameters for inequality.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the inequality check.</returns>
        public static bool operator !=(VServerConnectionArguments left, VServerConnectionArguments right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is VServerConnectionArguments arguments)
            {
                return this.Equals(arguments);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(VServerConnectionArguments other)
        {
            if ((this.Database == other.Database) && (this.Password == other.Password) && (this.Port == other.Port) && (this.Server == other.Server) && (this.User == other.User))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (this.Database.GetHashCode(StringComparison.CurrentCultureIgnoreCase) * 3) + (this.Password.GetHashCode(StringComparison.CurrentCultureIgnoreCase) * 5) + (this.Port * 7) + (this.Server.GetHashCode(StringComparison.CurrentCultureIgnoreCase) * 11) + (this.User.GetHashCode(StringComparison.CurrentCultureIgnoreCase) * 13);
        }
    }
}
