// <copyright file="VObjectExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumEssentials.HelperClass
{
    using System;
    using System.Threading.Tasks;
    using Npgsql;
    using VerbumEssentials.Basics;

    /// <summary>
    /// Provides extension methods for objects in the VerbumEssentials library.
    /// </summary>
    internal static class VObjectExtensions
    {
        /// <summary>
        /// Returns the input value, or throw an exception if the value is null.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="object">The checked object.</param>
        /// <param name="paramName">The parameter name of the object.</param>
        /// <returns>The object if it is not null.</returns>
        internal static T ThrowExceptionIfNull<T>(this T @object, string paramName)
            where T : class
        {
            if (@object is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return @object;
        }

        /// <summary>
        /// Executes a loop catching server exceptions till the function had not excecuted up to the end.
        /// </summary>
        /// <param name="errors">The <see cref="VServerErrors"/> handling the <see cref="VServerError"/>.</param>
        /// <param name="message">The message, that should be shown in the error.</param>
        /// <param name="function">The function, that should be invoked.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal static async Task NpgsqlWhileNullLoopAsync(this VServerErrors errors, string message, Func<Task> function)
        {
            while (true)
            {
                try
                {
                    await function.ThrowExceptionIfNull(nameof(function)).Invoke().ConfigureAwait(false);
                    break;
                }
                catch (Exception exception) when (exception is PostgresException || exception is NpgsqlException || exception is NpgsqlOperationInProgressException || exception is InvalidOperationException || exception is NullReferenceException || exception is IndexOutOfRangeException)
                {
                    errors.AddError(new VServerError(message, exception));
                }
            }
        }
    }
}
