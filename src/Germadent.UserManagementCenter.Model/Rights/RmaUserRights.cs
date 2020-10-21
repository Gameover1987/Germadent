using System.ComponentModel;

namespace Germadent.UserManagementCenter.Model.Rights
{
    public class ApplicationRightAttribute : System.Attribute
    {
        public ApplicationRightAttribute(string rightDescription, ApplicationModule applicationModule)
        {
            RightDescription = rightDescription;
            ApplicationModule = applicationModule;
        }

        public string RightDescription { get; set; }

        public ApplicationModule ApplicationModule { get;set; }
    }

    /// <summary>
    /// Права для РМА
    /// </summary>
    [Description("Рабочее место администратора")]
    public class RmaUserRights : UserRightsBase
    {
        [ApplicationRight("Запуск приложения", ApplicationModule.Rma)]
        public const string RunApplication = "Germadent.Rma.RunApplication";
        
        [ApplicationRight("Редактирование заказ-наряда", ApplicationModule.Rma)]
        public const string EditOrders = "Germadent.Rma.EditOrders";

        [ApplicationRight("Просмотр всех заказ-нарядов, а не только назначенных", ApplicationModule.Rma)]
        public const string ViewAllOrders = "Просмотр всех заказ-нарядов, а не только назначенных";
    }
}