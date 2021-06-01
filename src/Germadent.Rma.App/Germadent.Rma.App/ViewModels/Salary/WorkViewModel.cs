using System;
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

        public DateTime WorkStarted => _work.WorkStarted;

        public DateTime WorkCompleted => _work.WorkCompleted.Value;

        public int WorkOrderId => _work.WorkOrderId;

        public string DocNumber => _work.DocNumber;

        public decimal OperationCost => _work.OperationCost;
    }
}