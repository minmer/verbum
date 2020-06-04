// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Verbum
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Npgsql;
    using Verbum.Windows;
    using VerbumLibrary.Basics;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<IVContent> contents;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.contents = new ObservableCollection<IVContent>();
            this.InitializeComponent();
            this.ItemsControlContents.ItemsSource = this.contents;
        }

        private void OnButtonCreateNewContentTextClick(object sender, RoutedEventArgs eventArgs)
        {
            var contentText = new VContentText(App.QuerySchedule, -1);
            this.contents.Add(contentText);
            new ContentTextWindow(contentText).ShowDialog();
        }

        private void OnButtonLoadAllContentTextClick(object sender, RoutedEventArgs eventArgs)
        {
            _ = this.LoadAllContentsAsync();
        }

        private async Task LoadAllContentsAsync()
        {
            using NpgsqlCommand command = new NpgsqlCommand("SELECT id FROM content_text;", await App.ServerConnections.GetConnectionAsync().ConfigureAwait(false));
            using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (!this.contents.Any(content => content.ID == reader.GetInt32(0)))
                    {
                        this.contents.Add(new VContentText(App.QuerySchedule, reader.GetInt32(0)));
                    }
                });
            }
        }

        private void OnButtonEditContentTextClick(object sender, RoutedEventArgs eventArgs)
        {
            new ContentTextWindow((sender as Button).DataContext as VContentText).ShowDialog();
        }

        private void OnLoaded(object sender, RoutedEventArgs eventArgs)
        {
            this.ConnectToServerAsync();
        }

        private async void ConnectToServerAsync()
        {
            await App.ServerConnections.ConnectAsync().ConfigureAwait(false);
            this.Dispatcher.Invoke(() =>
            {
                this.BorderLoading.Visibility = Visibility.Collapsed;
            });
        }
    }
}
