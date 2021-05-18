using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;
using Germadent.Rms.App.ViewModels;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockRmsServiceClient : IRmsServiceClient
    {
        public AuthorizationInfoDto AuthorizationInfo { get; set; }
        public void Authorize(string login, string password)
        {
            throw new NotImplementedException();
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            throw new NotImplementedException();
        }

        public OrderDto GetOrderById(int workOrderId)
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

        public TechnologyOperationByUserDto[] GetRelevantWorkListByWorkOrder(int workOrderId)
        {
            return new TechnologyOperationByUserDto[]
            {
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

        public void StartWorks(WorkDto[] works)
        {
            throw new NotImplementedException();
        }

        OrderScope IRmsServiceClient.GetOrderById(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public void StartWork(WorkDto work, int lastEditorId)
        {
            throw new NotImplementedException();
        }

        public void UpdateWork(WorkDto work, int lastEditorId)
        {
            throw new NotImplementedException();
        }

        public void DeleteWork(WorkDto work)
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockOrderSummaryViewModel : OrderSummaryViewModel
    {
        public DesignMockOrderSummaryViewModel() : base(new PrintableOrderConverter(), new PropertyItemsCollector(), new DesignMockRmsServiceClient())
        {
            Initialize(new OrderDto() { BranchType = BranchType.MillingCenter });

            foreach (var technologyOperationByUserViewModel in Operations)
            {
                technologyOperationByUserViewModel.IsChecked = true;
            }
        }
    }
}
