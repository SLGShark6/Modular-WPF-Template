using System;
using System.Windows;
using System.Reactive.Linq;
using System.Collections.Generic;
using ModularWPFTemplate.Services.Startup;

namespace ModularWPFTemplate.Views
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public string Status { get; set; }

        private IList<IStartupCheck> _StartupChecks;

        public StartupWindow(IList<IStartupCheck> startupChecks)
        {
            _StartupChecks = startupChecks;

            InitializeComponent();

            DataContext = this;
        }

        private void PerformStartupCheck()
        {
            foreach (var startupCheck in _StartupChecks)
            {
                using (startupCheck.DisplayStatus.Subscribe(status => Status = status))
                {
                    startupCheck.DoCheck();
                }

                Status = "Performing startup checks";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PerformStartupCheck();
        }
    }
}
