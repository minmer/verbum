// <copyright file="ContentTextWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Verbum.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Npgsql;
    using VerbumLibrary.Basics;

    /// <summary>
    /// Interaction logic for ContentTextWindow.xaml.
    /// </summary>
    public partial class ContentTextWindow : Window
    {
        private readonly ObservableCollection<IVContent> contents;
        private readonly VContentText contentText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTextWindow"/> class.
        /// </summary>
        /// <param name="contentText">The dataContext of the <see cref="ContentTextWindow"/>.</param>
        public ContentTextWindow(VContentText contentText)
        {
            this.contents = new ObservableCollection<IVContent>();
            this.contentText = contentText;
            this.DataContext = this.contentText;
            this.InitializeComponent();
            _ = this.LoadLinksAsync();
        }

        private async Task LoadLinksAsync()
        {
            this.ComboBoxLinks.ItemsSource = this.contents;
            _ = this.LoadAllContentsAsync().ConfigureAwait(false);
            await this.contentText.LoadLinksAsync(App.QuerySchedule).ConfigureAwait(false);
            this.Dispatcher.Invoke(() =>
            {
                this.ItemsControlLinks.Items.Refresh();
            });
        }

        private void OnFocusLoaded(object sender, RoutedEventArgs eventArgs)
        {
            (sender as FrameworkElement).Focus();
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

        private void ButtonAddLinkClick(object sender, RoutedEventArgs eventArgs)
        {
            this.contentText.AddLink(this.ComboBoxLinks.SelectedItem as IVContent, App.QuerySchedule);
            this.ItemsControlLinks.Items.Refresh();
        }

        private void ButtonRemoveLinkClick(object sender, RoutedEventArgs eventArgs)
        {
            this.contentText.RemoveLink((sender as FrameworkElement).DataContext as IVContent, App.QuerySchedule);
            this.ItemsControlLinks.Items.Refresh();
        }
    }
}
