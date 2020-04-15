// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VerbumApp
{
    using System;
    using VerbumLibrary.Basics;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.MainPage = new MainPage();
        }

        /// <summary>
        /// Gets the main <see cref="VServerConnections"/> of the <see cref="App"/>.
        /// </summary>
        public static VServerConnections ServerConnections { get; private set; }

        /// <summary>
        /// Gets the main <see cref="VServerErrors"/> of the <see cref="App"/>.
        /// </summary>
        public static VServerErrors ServerErrors { get; private set; }

        /// <summary>
        /// Gets the main <see cref="VQuerySchedule"/> of the <see cref="App"/>.
        /// </summary>
        public static VQuerySchedule QuerySchedule { get; private set; }

        /// <inheritdoc/>
        protected override void OnStart()
        {
            ServerErrors = new VServerErrors();
            ServerConnections = new VServerConnections(new VServerConnectionArguments("sql.minmer.nazwa.pl", 5432, "minmer_mainverbum", "minmer_mainverbum", "VerbumCaroFactumEst0"), ServerErrors);
            QuerySchedule = new VQuerySchedule(ServerConnections, ServerErrors);
        }

        /// <inheritdoc/>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <inheritdoc/>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
