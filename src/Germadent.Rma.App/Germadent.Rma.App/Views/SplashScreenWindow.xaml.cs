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
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();

            DataContextChanged+= OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var splashScreenViewModel = (ISplashScreenViewModel) e.NewValue;
            splashScreenViewModel.InitializationCompleted += SplashScreenViewModelOnInitializationCompleted;
        }

        private void SplashScreenViewModelOnInitializationCompleted(object sender, EventArgs e)
        {
            Close();
        }
    }
}
