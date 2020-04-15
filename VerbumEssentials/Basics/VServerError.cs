// <copyright file="VServerError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumEssentials.Basics
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Describes an error occured requesting the server.
    /// </summary>
    public struct VServerError : IEquatable<VServerError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VServerError"/> struct.
        /// </summary>
        /// <param name="description">The description of the <see cref="VServerError"/>.</param>
        /// <param name="exception">The exception object of the <see cref="VServerError"/>.</param>
        public VServerError(string description, Exception exception)
        {
            this.Description = description;
            this.Exception = exception;
            this.Time = DateTime.Now;
        }

        /// <summary>
        /// Gets a description of the <see cref="VServerError"/>.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the exception object of the <see cref="VServerError"/>.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the time when the <see cref="VServerError"/> was raised.
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Checks two parameters for equality.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the equality check.</returns>
        public static bool operator ==(VServerError left, VServerError right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks two parameters for inequality.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the inequality check.</returns>
        public static bool operator !=(VServerError left, VServerError right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is VServerError arguments)
            {
                return this.Equals(arguments);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(VServerError other)
        {
            if ((this.Description == other.Description) && (this.Exception == other.Exception) && (this.Time == other.Time))
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
