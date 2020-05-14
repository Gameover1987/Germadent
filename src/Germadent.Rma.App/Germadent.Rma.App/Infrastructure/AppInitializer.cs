using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Germadent.Rma.App.ServiceClient.Repository;

namespace Germadent.Rma.App.Infrastructure
{
    public class InitalizationStepEventArgs : EventArgs
    {
        public InitalizationStepEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public interface IAppInitializer
    {
        void Initialize();

        event EventHandler<InitalizationStepEventArgs> InitializationProgress;

        event EventHandler InitializationCompleted;
    }

    public class AppInitializer : IAppInitializer
    {
        private readonly IRepositoryContainer _repositoryContainer;

        public AppInitializer(IRepositoryContainer repositoryContainer)
        {
            _repositoryContainer = repositoryContainer;
        }

        public void Initialize()
        {
            SendMessage("Инициализация репозитория заказчиков ...");
            _repositoryContainer.CustomerRepository.Initialize();

            SendMessage("Инициализация репозитория ответственных лиц ...");
            _repositoryContainer.ResponsiblePersonRepository.Initialize();

            SendMessage("Инициализация репозитория словарей ...");
            _repositoryContainer.DictionaryRepository.Initialize();

            InitializationCompleted?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<InitalizationStepEventArgs> InitializationProgress;
        public event EventHandler InitializationCompleted;

        private void SendMessage(string message)
        {
            InitializationProgress?.Invoke(this, new InitalizationStepEventArgs(message));
        }
    }
}
