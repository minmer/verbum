// <copyright file="IVContent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumEssentials.Basics
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that represents a VerbumContent.
    /// </summary>
    public interface IVContent
    {
        /// <summary>
        /// Gets the identifier of the <see cref="IVContent"/>.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets or sets the question related to the content.
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
