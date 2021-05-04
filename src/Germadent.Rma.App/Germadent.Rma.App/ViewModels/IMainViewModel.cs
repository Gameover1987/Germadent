using System;
using Germadent.Client.Common.ViewModels;
using Germadent.Rma.App.Infrastructure;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels
{
    public interface IMainViewModel
    {
        IDelegateCommand OpenOrderCommand { get; }

        OrderLiteViewModel SelectedOrder { get; }

        IUserSettingsManager SettingsManager { get; }

        event EventHandler ColumnSettingsChanged;

        void Initialize();
    }
}