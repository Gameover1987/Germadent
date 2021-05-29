namespace Germadent.Model.Rights
{
    /// <summary>
    /// Права для РМА
    /// </summary>
    [RightGroup(ApplicationModule.Rma)]
    public class RmaUserRights : UserRightsBase
    {
        [ApplicationRight("Запуск приложения")]
        public const string RunApplication = "Germadent.Rma.RunApplication";
        
        [ApplicationRight("Редактирование заказ-наряда")]
        public const string EditOrders = "Germadent.Rma.EditOrders";

        [ApplicationRight("Просмотр всех заказ-нарядов, а не только назначенных")]
        public const string ViewAllOrders = "Germadent.Rma.ViewAllOrders";

        [ApplicationRight("Просмотр прайслистов")]
        public const string ViewPriceList = "Germadent.Rma.ViewPriceList";

        [ApplicationRight("Редактирование прайслистов")]
        public const string EditPriceList = "Germadent.Rma.EditPriceList";

        [ApplicationRight("Расчет заработной платы")]
        public const string SalaryCalculation = "Germadent.Rma.SalaryCalculation";
    }
}