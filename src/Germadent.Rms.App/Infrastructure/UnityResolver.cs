﻿using System;
using System.Windows;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Client.Common.Reporting.TemplateProcessing;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Client.Common.ViewModels;
using Germadent.Client.Common.ViewModels.Salary;
using Germadent.Common;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Rms.App.Infrastructure.Repository;
using Germadent.Rms.App.ServiceClient;
using Germadent.Rms.App.ViewModels;
using Germadent.Rms.App.ViewModels.Salary;
using Germadent.Rms.App.Views;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using Unity;
using Unity.Lifetime;

namespace Germadent.Rms.App.Infrastructure
{
    public class UnityResolver : IDisposable
    {
        private IUnityContainer _container;
        private IClientConfiguration _configuration;

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

            _configuration = new ClientConfiguration();
            _container.RegisterInstance<IClientConfiguration>(_configuration, new ContainerControlledLifetimeManager());

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);

            _container.RegisterType<IRmsUserSettingsManager, RmsUserSettingsManager>(new ContainerControlledLifetimeManager());
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);
            _container.RegisterType<IAppInitializer, AppInitializer>();
            _container.RegisterType<ISplashScreenViewModel, SplashScreenViewModel>();
            _container.RegisterType<IAuthorizationViewModel, AuthorizationViewModel>();
            _container.RegisterType<IUserManager, RmsUserManager>();
            _container.RegisterType<IDateTimeProvider, DateTimeProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUiTimer, UiTimer>(new TransientLifetimeManager());
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IEnvironment, WpfEnvironment>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrdersFilterViewModel, OrdersFilterViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IStartWorkListViewModel, StartWorkListViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFinishWorkListViewModel, FinishWorkListViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderDetailsViewModel, OrderDetailsViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISalaryCalculationViewModel, MySalaryCalculationViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPropertyItemsCollector, PropertyItemsCollector>(new ContainerControlledLifetimeManager());
            
            _container.RegisterType<ISignalRClient, SignalRClient>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDictionaryRepository, DictionaryRepository>(new ContainerControlledLifetimeManager());

            var signalR = _container.Resolve<ISignalRClient>();
            var rmsServiceClient = new RmsServiceClient(_configuration, signalR);
            _container.RegisterInstance<IRmsServiceClient>(rmsServiceClient);
            _container.RegisterInstance<IBaseClientOperationsServiceClient>(rmsServiceClient);

            _container.RegisterType<IPrintableOrderConverter, PrintableOrderConverter>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFileManager, FileManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IWordAssembler, WordJsonAssembler>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPrintModule, PrintModule>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClipboardHelper, ClipboardHelper>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandExceptionHandler, CommandExceptionHandler>(new ContainerControlledLifetimeManager());
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
