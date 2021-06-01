using System;
using Accessibility;
using Germadent.Model.Production;

namespace Germadent.Rma.App.ViewModels.Salary
{
    public class WorkViewModel
    {
        private readonly WorkDto _work;

        public WorkViewModel(WorkDto work)
        {
            _work = work;
        }

        public int WorkId => _work.WorkId;

        public string TechnologyOperationName => _work.TechnologyOperationName;

        public string TechnologyOperationUserCode => _work.TechnologyOperationUserCode;

        public int Quantity => _work.Quantity;

        public float UrgencyRatio => _work.UrgencyRatio;

        public DateTime WorkStarted => _work.WorkStarted;

        public DateTime WorkCompleted => _work.WorkCompleted.Value;

        public int WorkOrderId => _work.WorkOrderId;

        public string DocNumber => _work.DocNumber;

        public decimal OperationCost => _work.OperationCost;

        public decimal Rate => _work.Rate;
    }
}