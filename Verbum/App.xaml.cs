// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Verbum
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using VerbumEssentials.Basics;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
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

        private void OnStartup(object sender, StartupEventArgs eventArgs)
        {
            ServerErrors = new VServerErrors();
            ServerConnections = new VServerConnections(new VServerConnectionArguments("sql.minmer.nazwa.pl", 5432, "minmer_mainverbum", "minmer_mainverbum", "VerbumCaroFactumEst0"), ServerErrors);
            QuerySchedule = new VQuerySchedule(ServerConnections, ServerErrors);
        }
    }
}
