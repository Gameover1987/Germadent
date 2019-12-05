﻿using System;

namespace Germadent.Rma.Model
{
    public class Order
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public BranchType BranchType { get; set; }

        public string Customer { get; set; }

        public string Patient { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public Material[] Materials { get; set; }

        public Teeth[] Mouth { get; set; }

    }

    public class LaboratoryOrder : Order
    {
        public string Doctor { get; set; }

        public Sex Sex { get; set; }

        public int Age { get; set; }

        public DateTime? FittingDate { get; set; }

        public string WorkDescription { get; set; }

        public DateTime Completed { get; set; }

        public string ColorAndFeatures { get; set; }

        public bool NonOpacity { get; set; }

        public bool LowOpacity { get; set; }

        public bool HighOpacity { get; set; }

        public bool Mamelons { get; set; }

        public bool SecondaryDentin { get; set; }

        public bool PaintedFissurs { get; set; }
    }

    public class MillingCenterOrder : Order
    {
        public string TechnicFio { get; set; }

        public string TechnikPhone { get; set; }

        public string AdditionalInfo { get; set; }

        public string CarcassColor { get; set; }

        public string ImplantSystem { get; set; }

        public string IndividualAbutmentProcessing { get; set; }

        public string WorkDescription { get; set; }
    }
}
