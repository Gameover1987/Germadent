using System;
using Germadent.Common.Logging;
using Germadent.Rma.App.ServiceClient.Repository;

namespace Germadent.Rma.App.Infrastructure
{
    public class AppInitializer : IAppInitializer
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IPricePositionRepository _pricePositionRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;

        public AppInitializer(ICustomerRepository customerRepository,
            IResponsiblePersonRepository responsiblePersonRepository,
            IDictionaryRepository dictionaryRepository,
            IPriceGroupRepository priceGroupRepository,
            IPricePositionRepository pricePositionRepository,
            IProductRepository productRepository,
            ILogger logger)
        {
            _customerRepository = customerRepository;
            _responsiblePersonRepository = responsiblePersonRepository;
            _dictionaryRepository = dictionaryRepository;
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                SendMessage("Инициализация репозитория заказчиков ...");
                _customerRepository.Initialize();

                SendMessage("Инициализация репозитория ответственных лиц ...");
                _responsiblePersonRepository.Initialize();

                SendMessage("Инициализация репозитория словарей ...");
                _dictionaryRepository.Initialize();

                SendMessage("Инициализация репозитория ценовых групп ...");
                _priceGroupRepository.Initialize();

                SendMessage("Инициализация репозитория ценовых позиций ...");
                _pricePositionRepository.Initialize();

                SendMessage("Инициализация репозитория изделий ...");
                _productRepository.Initialize();

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
