// <copyright file="ContentTextWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Verbum.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using VerbumLibrary.Basics;

    /// <summary>
    /// Interaction logic for ContentTextWindow.xaml.
    /// </summary>
    public partial class ContentTextWindow : Window
    {
        private readonly VContentText contentText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTextWindow"/> class.
        /// </summary>
        /// <param name="contentText">The dataContext of the <see cref="ContentTextWindow"/>.</param>
        public ContentTextWindow(VContentText contentText)
        {
            this.contentText = contentText;
            this.DataContext = this.contentText;
            this.InitializeComponent();
        }

        private void OnFocusLoaded(object sender, RoutedEventArgs eventArgs)
        {
            (sender as FrameworkElement).Focus();
        }
    }
}
