using System;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockRmsServiceClient : IRmsServiceClient
    {
        public TechnologyOperationByUserDto[] GetRelevantWorkListByWorkOrder(int workOrderId)
        {
            return new TechnologyOperationByUserDto[]
            {
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        UserCode = "001",
                        Name = "Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, "
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch Windows to FreeBSD"
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, "
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch Windows to FreeBSD"
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, "
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch Windows to FreeBSD"
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, "
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch Windows to FreeBSD"
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, Patch KDE 2.0 to FreeBSD, "
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
                new TechnologyOperationByUserDto()
                {
                    Operation = new TechnologyOperationDto
                    {
                        Name = "Patch Windows to FreeBSD"
                    },
                    ProductCount = 2,
                    Rate = new decimal(1.05), UrgencyRatio = 1.33f
                },
            };
        }

        public void StartWorks(WorkDto[] work)
        {
            
        }


        public AuthorizationInfoDto AuthorizationInfo { get; }
        public IClientConfiguration Configuration { get; }
        public void Authorize(string login, string password)
        {
            throw new NotImplementedException();
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            throw new NotImplementedException();
        }

        public OrderScope GetOrderById(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public void UnLockOrder(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            throw new NotImplementedException();
        }
    }
}