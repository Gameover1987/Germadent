﻿namespace Germadent.Model.Production
{
    public class TechnologyOperationDto
    {
        public TechnologyOperationDto()
        {
            Rates = new RateDto[0];
        }

        public int TechnologyOperationId { get; set; }

        public int EmployeePositionId { get; set; }

        public string UserCode { get; set; }

        public string Name { get; set; }

        public RateDto[] Rates { get; set; }

        public bool IsObsolete { get; set; }
    }
}
