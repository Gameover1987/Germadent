using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Germadent.Common.Logging;
using Germadent.Rma.App.ServiceClient.Repository;

namespace Germadent.Rma.App.Infrastructure
{
    public class AppInitializer : IAppInitializer
    {
        private readonly IRepositoryContainer _repositoryContainer;
        private readonly ILogger _logger;

        public AppInitializer(IRepositoryContainer repositoryContainer, ILogger logger)
        {
            _repositoryContainer = repositoryContainer;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                SendMessage("Инициализация репозитория заказчиков ...");
                _repositoryContainer.CustomerRepository.Initialize();

                SendMessage("Инициализация репозитория ответственных лиц ...");
                _repositoryContainer.ResponsiblePersonRepository.Initialize();

                SendMessage("Инициализация репозитория словарей ...");
                _repositoryContainer.DictionaryRepository.Initialize();

                InitializationCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                InitializationFailed?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<InitalizationStepEventArgs> InitializationProgress;

        public event EventHandler InitializationCompleted;

        public event EventHandler InitializationFailed; 

        private void SendMessage(string message)
        {
            _logger.Info(message);
            InitializationProgress?.Invoke(this, new InitalizationStepEventArgs(message));
        }
    }
}
