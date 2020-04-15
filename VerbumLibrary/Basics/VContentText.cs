// <copyright file="VContentText.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumLibrary.Basics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;
    using NpgsqlTypes;
    using VerbumLibrary.Resources;

    /// <summary>
    /// Represents content of type string.
    /// </summary>
    public class VContentText : IVContent, INotifyPropertyChanged
    {
        private readonly VQuerySchedule querySchedule;
        private readonly VServerConnections serverConnections;
        private string question;
        private string content;

        /// <summary>
        /// Initializes a new instance of the <see cref="VContentText"/> class.
        /// </summary>
        /// <param name="querySchedule">The <see cref="VQuerySchedule"/> handling the <see cref="VQuery"/> of the <see cref="VContentText"/>.</param>
        /// <param name="serverConnections">The <see cref="VServerConnections"/> handling the <see cref="NpgsqlConnection"/> of the <see cref="VContentText"/>.</param>
        /// <param name="id">The identifier of the <see cref="VContentText"/>.</param>
        public VContentText(VQuerySchedule querySchedule, VServerConnections serverConnections, int id)
        {
            this.querySchedule = querySchedule ?? throw new ArgumentNullException(nameof(querySchedule));
            this.serverConnections = serverConnections ?? throw new ArgumentNullException(nameof(serverConnections));
            this.ID = id;

            if (this.ID == -1)
            {
                this.querySchedule.AddQuery(new VQuery(this.GenerateInsertionQuery, this.AssignId, Resources.ErrorInsertContentText));
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the content related to the question.
        /// </summary>
        public string Content
        {
            get
            {
                _ = this.LoadContentAsync();
                return this.content;
            }

            set
            {
                this.SaveContent(value);
            }
        }

        /// <inheritdoc/>
        public int ID { get; private set; }

        /// <inheritdoc/>
        public string Question
        {
            get
            {
                _ = this.LoadQuestionAsync();
                return this.question;
            }

            set
            {
                this.SaveQuestion(value);
            }
        }

        /// <summary>
        /// Loads the content from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<string> LoadContentAsync()
        {
            if (this.content is null)
            {
                this.content = await this.LoadObjectFromDbAsync("content").ConfigureAwait(false) as string ?? string.Empty;
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Content)));
            }

            return this.content;
        }

        /// <inheritdoc/>
        public async Task<string> LoadQuestionAsync()
        {
            if (this.question is null)
            {
                this.question = await this.LoadObjectFromDbAsync("question").ConfigureAwait(false) as string ?? string.Empty;
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Question)));
            }

            return this.question;
        }

        /// <summary>
        /// Assigns the content by a <see cref="string"/> and saves it in the database.
        /// </summary>
        /// <param name="value">The <see cref="string"/> that should be assigned and saved.</param>
        public void SaveContent(string value)
        {
            this.content = value;
            this.querySchedule.AddQuery(new VQuery(this.GenerateSaveContentQuery, null, Resources.ErrorSaveContentTextContent));
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Content)));
        }

        /// <inheritdoc/>
        public void SaveQuestion(string value)
        {
            this.question = value;
            this.querySchedule.AddQuery(new VQuery(this.GenerateSaveQuestionQuery, null, Resources.ErrorSaveContentTextQuestion));
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Question)));
        }

        /// <summary>
        /// Invokes the PropertyChanged of the <see cref="VContentText"/>.
        /// </summary>
        /// <param name="eventArgs">The PropertyChangedEventArgs of the mehtod.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            this.PropertyChanged?.Invoke(this, eventArgs);
        }

        private void AssignId(DbDataReader reader)
        {
            this.ID = reader.GetInt32(0);
        }

        private NpgsqlCommand GenerateInsertionQuery(NpgsqlConnection connection)
        {
            return new NpgsqlCommand("INSERT INTO content_text DEFAULT VALUES RETURNING id;", connection);
        }

        private NpgsqlCommand GenerateSaveContentQuery(NpgsqlConnection connection)
        {
            NpgsqlCommand command = new NpgsqlCommand("UPDATE content_text SET content = @content WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@content", NpgsqlDbType.Text, this.content);
            command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, this.ID);
            return command;
        }

        private NpgsqlCommand GenerateSaveQuestionQuery(NpgsqlConnection connection)
        {
            NpgsqlCommand command = new NpgsqlCommand("UPDATE content_text SET question = @question WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@question", NpgsqlDbType.Text, this.question);
            command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, this.ID);
            return command;
        }

        private async Task<object> LoadObjectFromDbAsync(string propertyName)
        {
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT " + propertyName + " FROM content_text WHERE id = @id;", await this.serverConnections.GetConnectionAsync().ConfigureAwait(false)))
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, this.ID);
                return await command.ExecuteScalarAsync().ConfigureAwait(false);
            }
        }
    }
}
