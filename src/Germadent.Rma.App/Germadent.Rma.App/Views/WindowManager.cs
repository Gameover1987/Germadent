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

        public WindowManager(IShowDialogAgent dialogAgent, ILabOrderViewModel labOrderViewModel)
        {
            _dialogAgent = dialogAgent;
            _labOrderViewModel = labOrderViewModel;
        }

        public Order CreateLabOrder()
        {
            _labOrderViewModel.Initialize(false);
            if (_dialogAgent.ShowDialog<LabOrderWindow>(_labOrderViewModel) == true)
            {

            }

            return new Order();
        }

        public Order CreateMillingCenterOrder()
        {
            throw new NotImplementedException();
        }
    }
}
