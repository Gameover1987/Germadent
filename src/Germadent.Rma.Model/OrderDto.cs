﻿using System;

namespace Germadent.Rma.Model
{
    /// <summary>
    /// Заказ наряд
    /// </summary>
    public class OrderDto
    {
        public OrderDto()
        {
            OfficeAdminName = "Мега админ";
            ToothCard = new ToothDto[0];
        }

        public int WorkOrderId { get; set; }

        public int Status { get; set; }

        public string DocNumber { get; set; }

        public BranchType BranchType { get; set; }

        public string Customer { get; set; }

        public string Patient { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public string ResponsiblePerson { get; set; }

        public string ResponsiblePersonPhone { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public DateTime? FittingDate { get; set; }

        public string WorkDescription { get; set; }

        public string ColorAndFeatures { get; set; }

        public string AdditionalInfo { get; set; }

        public string CarcassColor { get; set; }

        public string ImplantSystem { get; set; }

        public string IndividualAbutmentProcessing { get; set; }

        public bool WorkAccepted { get; set; }

        public int Transparency { get; set; }

        public string Understaff { get; set; }

        public string OfficeAdminName { get; set; }

        public ToothDto[] ToothCard { get; set; }

        public string ProstheticArticul { get; set; }

        public byte[] StlFile { get; set; }

        public byte[] PhotoFile { get; set; }
    }
}