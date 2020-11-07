using System;
using System.Windows;
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
            splashScreenViewModel.InitializationFailed += SplashScreenViewModelOnInitializationFailed;
        }

        private void SplashScreenViewModelOnInitializationCompleted(object sender, EventArgs e)
        {
            DialogResult = true;
        }

        private void SplashScreenViewModelOnInitializationFailed(object sender, EventArgs e)
        {
            DialogResult = false;
        }
    }
}
