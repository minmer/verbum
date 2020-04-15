// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumApp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;
    using VerbumLibrary.Basics;
    using Xamarin.Forms;

    /// <summary>
    /// Interaction logic for MainPage.xaml.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private readonly ObservableCollection<IVContent> contents;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.contents = new ObservableCollection<IVContent>();
            this.InitializeComponent();
            this.CollectionViewContents.ItemsSource = this.contents;
        }

        private void OnButtonLoadAllContentTextClicked(object sender, EventArgs eventArgs)
        {
            _ = this.LoadAllContentsAsync();
        }

        private async Task LoadAllContentsAsync()
        {
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT id FROM content_text;", await App.ServerConnections.GetConnectionAsync().ConfigureAwait(false)))
            {
                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            if (!this.contents.Any(content => content.ID == reader.GetInt32(0)))
                            {
                                this.contents.Add(new VContentText(App.QuerySchedule, App.ServerConnections, reader.GetInt32(0)));
                            }
                        }).ConfigureAwait(false);
                    }
                }
            }
        }

        private void OnLoaded(object sender, EventArgs eventArgs)
        {
            this.ConnectToServerAsync();
        }

        private async void ConnectToServerAsync()
        {
            await App.ServerConnections.ConnectAsync().ConfigureAwait(false);
            await Device.InvokeOnMainThreadAsync(() =>
            {
                this.GridLoading.IsVisible = false;
            }).ConfigureAwait(false);
        }
    }
}
