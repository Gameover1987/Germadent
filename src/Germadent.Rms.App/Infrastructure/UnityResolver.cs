﻿using System;
using System.Windows;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ViewModels;
using Germadent.Common;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Rms.App.Infrastructure.Repository;
using Germadent.Rms.App.ServiceClient;
using Germadent.Rms.App.ViewModels;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using Unity;
using Unity.Lifetime;
using OrdersFilterViewModel = Germadent.Rms.App.ViewModels.OrdersFilterViewModel;

namespace Germadent.Rms.App.Infrastructure
{
    public class UnityResolver : IDisposable
    {
        private IUnityContainer _container;
        private IConfiguration _configuration;

        public UnityResolver()
        {
            FillContainer();
        }

        public ISplashScreenViewModel GetSplashScreenViewModel()
        {
            return _container.Resolve<ISplashScreenViewModel>();
        }

        public IAuthorizationViewModel GetAuthorizationViewModel()
        {
            return _container.Resolve<IAuthorizationViewModel>();
        }

        public IMainViewModel GetMainViewModel()
        {
            return _container.Resolve<IMainViewModel>();
        }

        public ICommandExceptionHandler GetCommandExceptionHandler()
        {
            return _container.Resolve<ICommandExceptionHandler>();
        }

        public ILogger GetLogger()
        {
            return _container.Resolve<ILogger>();
        }

        private void FillContainer()
        {
            _container = new UnityContainer();

            _configuration = new RmsConfiguration();
            _container.RegisterInstance<IConfiguration>(_configuration, new ContainerControlledLifetimeManager());

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);
            _container.RegisterType<IAppInitializer, AppInitializer>();
            _container.RegisterType<ISplashScreenViewModel, SplashScreenViewModel>();
            _container.RegisterType<IAuthorizationViewModel, AuthorizationViewModel>();
            _container.RegisterType<IUserManager, RmsUserManager>();

            RegisterServiceClient();
            RegisterCommonComponents();
            RegisterViewModels();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IDateTimeProvider, DateTimeProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUiTimer, UiTimer>(new TransientLifetimeManager());
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IEnvironment, WpfEnvironment>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrdersFilterViewModel, OrdersFilterViewModel>(new ContainerControlledLifetimeManager());
        }

        private void RegisterCommonComponents()
        {
            _container.RegisterType<IFileManager, FileManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClipboardHelper, ClipboardHelper>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandExceptionHandler, CommandExceptionHandler>(new ContainerControlledLifetimeManager());
        }

        private void RegisterServiceClient()
        {
            _container.RegisterType<IRmsServiceClient, RmsServiceClient>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDictionaryRepository, DictionaryRepository>(new ContainerControlledLifetimeManager());
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}