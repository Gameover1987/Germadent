using System;

namespace Germadent.Rma.App.Printing
{
    /// <summary>
    /// Подготовленные данные для печати заказ-нарядов
    /// </summary>
    public class PrintableOrder
    {
        public int WorkOrderID { get; set; }
        public string DocNumber { get; set; }
        public string BranchType { get; set; }
        public string CustomerName { get; set; }
        public string PatientFullName { get; set; }
        public int PatientAge { get; set; }
        public string PatientGender { get; set; }
        public string ResponsiblePerson { get; set; }
        public string TechnicPhone { get; set; }
        public DateTime Created { get; set; }
        public string ToothCardDescription { get; set; }
        public string FlagWorkAccept { get; set; }
        public DateTime? Closed { get; set; }
        public string OfficeAdmin { get; set; }
        public string AdditionalInfo { get; set; }
        public string CarcassColor { get; set; }
        public string ImplantSystem { get; set; }
        public string IndividualAbutmentProcessing { get; set; }
        public string Understaff { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public DateTime? FittingDate { get; set; }
        public string ColorAndFeatures { get; set; }
        public string TransparenceName { get; set; }
        public string ProstheticArticul { get; set; }
        public string MaterialsStr { get; set; }
        public string AdditionalEquipment { get; set; }
        public string WorkDescription { get; set; }
    }
}
