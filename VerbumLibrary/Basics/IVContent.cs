// <copyright file="IVContent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumLibrary.Basics
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that represents a VerbumContent.
    /// </summary>
    public interface IVContent
    {
        /// <summary>
        /// Gets the string representation of the <see cref="IVContent"/> type.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the identifier of the <see cref="IVContent"/>.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets the questions linked to the <see cref="IVContent"/>.
        /// </summary>
        List<IVContent> Links { get; }

        /// <summary>
        /// Gets or sets the question related to the <see cref="IVContent"/>.
        /// </summary>
        string Question { get; set; }

        /// <summary>
        /// Loads the question from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<string> LoadQuestionAsync();

        /// <summary>
        /// Assigns the question by a <see cref="string"/> and saves it in the database.
        /// </summary>
        /// <param name="value">The <see cref="string"/> that should be assigned and saved.</param>
        void SaveQuestion(string value);
    }
}
