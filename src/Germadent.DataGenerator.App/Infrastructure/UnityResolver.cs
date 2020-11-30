using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Germadent.Common.Logging;
using Germadent.DataGenerator.App.ViewModels;
using Germadent.UI.Infrastructure;
using Unity;

namespace Germadent.DataGenerator.App.Infrastructure
{
    public class UnityResolver : IDisposable
    {
        private readonly IUnityContainer _container = new UnityContainer();

        public UnityResolver()
        {
            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);

            _container.RegisterSingleton<IShowDialogAgent, ShowDialogAgent>();
            _container.RegisterSingleton<IConnectionViewModel, ConnectionViewModel>();
            _container.RegisterSingleton<IMainViewModel, MainViewModel>();
        }

        public IConnectionViewModel GetConnectionViewModel()
        {
            return _container.Resolve<IConnectionViewModel>();
        }

        public IMainViewModel GetMainViewModel()
        {
            return _container.Resolve<IMainViewModel>();
        }

        public ICommandExceptionHandler GetCommandExceptionHandler()
        {
            return _container.Resolve<ICommandExceptionHandler>();
        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}
