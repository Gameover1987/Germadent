using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.ViewModels;
using Germadent.ServiceClient.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views
{
    public interface IWindowManager
    {
        Order CreateLabOrder();

        Order CreateMillingCenterOrder();
    }

    public class WindowManager : IWindowManager
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ILabOrderViewModel _labOrderViewModel;
        private readonly IMillingOrderViewModel _millingOrderViewModel;

        public WindowManager(IShowDialogAgent dialogAgent, ILabOrderViewModel labOrderViewModel, IMillingOrderViewModel millingOrderViewModel)
        {
            _dialogAgent = dialogAgent;
            _labOrderViewModel = labOrderViewModel;
            _millingOrderViewModel = millingOrderViewModel;
        }

        public Order CreateLabOrder()
        {
            _labOrderViewModel.Initialize(false);
            if (_dialogAgent.ShowDialog<LabOrderWindow>(_labOrderViewModel) == true)
            {
                return new Order();
            }

            return null;
        }

        public Order CreateMillingCenterOrder()
        {
            _millingOrderViewModel.Initialize(false);
            if (_dialogAgent.ShowDialog<MillingCenterOrderWindow>(_millingOrderViewModel) == true)
            {
                return new Order();
            }

            return null;
        }
    }
}
