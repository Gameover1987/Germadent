﻿using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.Salary;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockSalaryCalculationViewModel : SalaryCalculationViewModel
    {
        public DesignMockSalaryCalculationViewModel() 
            : base(new DesignMockRmaServiceClient(), 
                new DesignMockCommandExceptionHandler())
        {
            ThreadTaskExtensions.IsSyncRun = true;

            Initialize();

            SelectedEmployee = Employees.FirstOrDefault();

            CalculateSalaryCommand.TryExecute();
        }
    }
}