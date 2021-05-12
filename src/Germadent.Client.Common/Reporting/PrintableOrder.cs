using System;
using Germadent.Client.Common.Reporting.PropertyGrid;

namespace Germadent.Client.Common.Reporting
{
    /// <summary>
    /// Подготовленные данные для печати заказ-нарядов
    /// </summary>
    public class PrintableOrder
    {
        public int WorkOrderID { get; set; }

        [PrintableOrderProperty(DisplayName = "Номер заказ-наряда", GroupName = "Общие данные")]
        public string DocNumber { get; set; }
        public string BranchType { get; set; }
        
        [PrintableOrderProperty(DisplayName = "Заказчик", GroupName = "Общие данные")]
        public string CustomerName { get; set; }

        [PrintableOrderProperty(DisplayName = "Пациент", GroupName = "Общие данные")]
        public string PatientFullName { get; set; }

        [PrintableOrderProperty(DisplayName = "Возраст пациента", GroupName = "Общие данные")]
        public int? PatientAge { get; set; }

        [PrintableOrderProperty(DisplayName = "Пол пациента", GroupName = "Общие данные")]
        public string? PatientGender { get; set; }

        [PrintableOrderProperty(DisplayName = "Ответственное лицо", GroupName = "Общие данные")]
        public string ResponsiblePerson { get; set; }

        [PrintableOrderProperty(DisplayName = "Телефон", GroupName = "Общие данные")]
        public string TechnicPhone { get; set; }

        [PrintableOrderProperty(DisplayName = "Дата создания", GroupName = "Общие данные")]
        public DateTime Created { get; set; }

        
        [PrintableOrderProperty(DisplayName = "Описание", GroupName = "Зубная карта")]
        public string ToothCardDescription { get; set; }

        [PrintableOrderProperty(DisplayName = "Материалы", GroupName = "Зубная карта")]
        public string MaterialsStr { get; set; }

        [PrintableOrderProperty(DisplayName = "Цвет конструкции", GroupName = "Зубная карта")]
        public string ColorAndFeatures { get; set; }
        
        public string CarcassColor { get; set; }

        [PrintableOrderProperty(DisplayName = "Система имплантов", GroupName = "Зубная карта")]
        public string ImplantSystem { get; set; }

        public string FlagWorkAccept { get; set; }
        public DateTime? Closed { get; set; }

        [PrintableOrderProperty(DisplayName = "ФИО администратора", GroupName = "Дополнительная информация")]
        public string OfficeAdminName { get; set; }
        
        public string AdditionalInfo { get; set; }

        public string Understaff { get; set; }
        public DateTime? DateOfCompletion { get; set; }

        [PrintableOrderProperty(DisplayName = "Дата примерки", GroupName = "Дополнительная информация")]
        public DateTime? FittingDate { get; set; }

        public string TransparenceName { get; set; }

        [PrintableOrderProperty(DisplayName = "Простетика", GroupName = "Дополнительная информация")]
        public string ProstheticArticul { get; set; }

        [PrintableOrderProperty(DisplayName = "Доп. оснастка", GroupName = "Дополнительная информация")]
        public string AdditionalEquipment { get; set; }

        [PrintableOrderProperty(DisplayName = "Комментарий к работам", GroupName = "Дополнительная информация")]
        public string WorkDescription { get; set; }

        [PrintableOrderProperty(DisplayName = "Комментарий к срокам", GroupName = "Дополнительная информация")]
        public string DateComment { get; internal set; }
    }
}
