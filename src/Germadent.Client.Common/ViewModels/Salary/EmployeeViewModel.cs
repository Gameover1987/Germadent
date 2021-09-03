using Germadent.Model;

namespace Germadent.Client.Common.ViewModels.Salary
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel(UserDto user)
        {
            UserId = user.UserId;
            DisplayName = user.GetFullName();
        }

        public int? UserId { get; protected set; }

        public string DisplayName { get; protected set; }
    }

    public class AllEmployeeViewModel : EmployeeViewModel
    {
        private static AllEmployeeViewModel _instance;

        public AllEmployeeViewModel() : base(new UserDto())
        {
            UserId = null;
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