﻿namespace Germadent.Model.Production
{
    public class EmployeePositionDto
    {
        public int UserId { get; set; }

        public EmployeePosition EmployeePosition{ get; set; }

        public string EmployeePositionName { get; set; }

        public int QualifyingRank { get; set; }
    }
}