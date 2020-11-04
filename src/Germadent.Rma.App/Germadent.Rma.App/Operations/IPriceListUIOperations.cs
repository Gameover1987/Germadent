using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.Properties;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Operations
{
    public interface IPriceListUIOperations
    {
        PriceGroupDto AddPriceGroup(BranchType branchType);

        PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto);

        void DeletePriceGroup(int priceGroupId);
    }

    public class PriceListUIOperations : IPriceListUIOperations
    {
        private readonly IShowDialogAgent _dialogAgent;

        public PriceListUIOperations(IShowDialogAgent dialogAgent)
        {
            _dialogAgent = dialogAgent;
        }

        public PriceGroupDto AddPriceGroup(BranchType branchType)
        {
            var priceGroupName = _dialogAgent.ShowInputBox("Добавление ценовой группы", "Ценовая группа");
            if (priceGroupName == null)
                return null;

            return new PriceGroupDto {BranchType = branchType, Name = priceGroupName};
        }

        public PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto)
        {
            throw new NotImplementedException();
        }

        public void DeletePriceGroup(int priceGroupId)
        {
            throw new NotImplementedException();
        }
    }
}