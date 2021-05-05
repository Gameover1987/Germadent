using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.ServiceClient;
using Germadent.Common.Logging;

namespace Germadent.Rms.App.Infrastructure
{
    public class AppInitializer : IAppInitializer
    {
        private readonly ILogger _logger;
        private readonly IDictionaryRepository _dictionaryRepository;

        public AppInitializer(ILogger logger, IDictionaryRepository dictionaryRepository)
        {
            _logger = logger;
            _dictionaryRepository = dictionaryRepository;
        }

        public void Initialize()
        {
            try
            {
                SendMessage("Инициализация репозитория словарей...");
                _dictionaryRepository.Initialize();

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
