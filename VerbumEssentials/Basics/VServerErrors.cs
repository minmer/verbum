// <copyright file="VServerErrors.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumEssentials.Basics
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a unit handling and providing <see cref="VServerError"/>.
    /// </summary>
    public class VServerErrors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VServerErrors"/> class.
        /// </summary>
        public VServerErrors()
        {
            this.Errors = new List<VServerError>();
        }

        /// <summary>
        /// Handles the addition of an <see cref="VServerError"/>.
        /// </summary>
        /// <param name="error">The <see cref="VServerError"/>, that was added.</param>
        public delegate void ErrorAddedHandler(VServerError error);

        /// <summary>
        /// An event raised when an <see cref="VServerError"/> was added.
        /// </summary>
        public event ErrorAddedHandler ErrorAdded;

        /// <summary>
        /// Gets a list of all registered <see cref="VServerError"/>.
        /// </summary>
        public List<VServerError> Errors { get; }

        /// <summary>
        /// Adds an <see cref="VServerError"/>.
        /// </summary>
        /// <param name="error">The <see cref="VServerError"/>, that should be added.</param>
        public void AddError(VServerError error)
        {
            this.Errors.Add(error);
            this.OnErrorAdded(error);
        }

        /// <summary>
        /// Invokes the ErrorAdded event of the <see cref="VServerErrors"/>.
        /// </summary>
        /// <param name="serverError">The <see cref="VServerError"/>, that was added.</param>
        protected virtual void OnErrorAdded(VServerError serverError)
        {
            this.ErrorAdded?.Invoke(serverError);
        }
    }
}
