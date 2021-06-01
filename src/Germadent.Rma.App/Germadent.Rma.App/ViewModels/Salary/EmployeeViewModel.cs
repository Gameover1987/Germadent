using Germadent.Model;

namespace Germadent.Rma.App.ViewModels.Salary
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel(UserDto user)
        {
            DisplayName = user.GetFullName();
        }

        public string DisplayName { get; protected set; }
    }

    public class AllEmployeeViewModel : EmployeeViewModel
    {
        private static AllEmployeeViewModel _instance;

        public AllEmployeeViewModel() : base(new UserDto())
        {
            DisplayName = "Все сотрудники";
        }

        public static AllEmployeeViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AllEmployeeViewModel();
                return _instance;
            }
        }
    }
}