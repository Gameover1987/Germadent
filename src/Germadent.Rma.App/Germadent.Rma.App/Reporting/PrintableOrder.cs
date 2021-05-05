using System;

namespace Germadent.Rma.App.Reporting
{
    /// <summary>
    /// Подготовленные данные для печати заказ-нарядов
    /// </summary>
    public class PrintableOrder
    {
        public int WorkOrderID { get; set; }
        public string DocNumber { get; set; }
        public string BranchType { get; set; }

        // Общие данные
        public string CustomerName { get; set; }
        // Общие данные
        public string PatientFullName { get; set; }
        // Общие данные
        public int? PatientAge { get; set; }
        // Общие данные
        public string? PatientGender { get; set; }
        // Общие данные
        public string ResponsiblePerson { get; set; }
        // Общие данные
        public string TechnicPhone { get; set; }
        // Общие данные
        public DateTime Created { get; set; }

        // Зубная карта
        public string ToothCardDescription { get; set; }
        // Зубная карта
        public string MaterialsStr { get; set; }

        public string FlagWorkAccept { get; set; }
        public DateTime? Closed { get; set; }

        // Доп. инфа
        public string OfficeAdminName { get; set; }
        // Доп. инфа
        public string AdditionalInfo { get; set; }
        // Доп. инфа
        public string CarcassColor { get; set; }
        // Доп. инфа
        public string ImplantSystem { get; set; }
        // Доп. инфа
        public string Understaff { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public DateTime? FittingDate { get; set; }

        // Доп. инфа
        public string ColorAndFeatures { get; set; }
        public string TransparenceName { get; set; }
        public string ProstheticArticul { get; set; }

       
        public string AdditionalEquipment { get; set; }
        // Доп. инфа
        public string WorkDescription { get; set; }
        // Доп. инфа
        public string DateComment { get; internal set; }
    }
}
