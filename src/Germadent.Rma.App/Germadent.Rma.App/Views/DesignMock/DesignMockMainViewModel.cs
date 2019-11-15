﻿using System.Windows;
using Germadent.Rma.App.ViewModels;
using Germadent.ServiceClient.Operation;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(
                new MockRmaOperations(),
                new WindowManager(new ShowDialogAgent(
                    new DispatcherAdapter(Application.Current.Dispatcher)), 
                    new DesignMockLabOrderViewModel(),
                    new DesignMockMillingCenterOrderViewModel()))
        {
        }
    }
}
