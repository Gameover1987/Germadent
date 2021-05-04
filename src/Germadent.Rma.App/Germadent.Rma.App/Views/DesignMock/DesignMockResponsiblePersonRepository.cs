using System;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockResponsiblePersonRepository : IResponsiblePersonRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public ResponsiblePersonDto[] Items
        {
            get
            {
                return new ResponsiblePersonDto[]
                {
                    new ResponsiblePersonDto
                    {
                        Id = 1,
                        FullName = "Ivanov"
                    },
                    new ResponsiblePersonDto
                    {
                        Id = 2,
                        FullName = "Petrov"
                    },
                };
            }
        }

        public event EventHandler<RepositoryChangedEventArgs<ResponsiblePersonDto>> Changed;
    }
}