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

        [ApplicationRight("Просмотр прайслистов")]
        public const string ViewPriceList = "Germadent.Rma.ViewPriceList";

        [ApplicationRight("Редактирование прайслистов")]
        public const string EditPriceList = "Germadent.Rma.EditPriceList";

        [ApplicationRight("Просмотр и редактирование технологических операций")]
        public const string EditTechnologyOperations = "Germadent.Rma.EditTechnologyOperations";

        [ApplicationRight("Расчет заработной платы")]
        public const string SalaryCalculation = "Germadent.Rma.SalaryCalculation";
    }
}