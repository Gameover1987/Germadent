using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public WindowManager(IShowDialogAgent dialogAgent)
        {
            _dialogAgent = dialogAgent;
        }

        public Order CreateLabOrder()
        {
            throw new NotImplementedException();
        }

        public Order CreateMillingCenterOrder()
        {
            throw new NotImplementedException();
        }
    }
}
