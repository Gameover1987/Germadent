using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IRepositoryContainer
    {
        ICustomerRepository CustomerRepository { get; }

        IResponsiblePersonRepository ResponsiblePersonRepository { get; }

        IDictionaryRepository DictionaryRepository { get; }
    }

    public class RepositoryContainer : IRepositoryContainer
    {
        public RepositoryContainer(ICustomerRepository customerRepository,
            IResponsiblePersonRepository responsiblePersonRepository,
            IDictionaryRepository dictionaryRepository)
        {
            CustomerRepository = customerRepository;
            ResponsiblePersonRepository = responsiblePersonRepository;
            DictionaryRepository = dictionaryRepository;
        }

        public ICustomerRepository CustomerRepository { get; }

        public IResponsiblePersonRepository ResponsiblePersonRepository { get; }

        public IDictionaryRepository DictionaryRepository { get; }
    }
}
