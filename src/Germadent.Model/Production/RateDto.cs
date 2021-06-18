using System;

namespace Germadent.Model.Production
{
    public class RateDto
    {
        public RateDto()
        {
            DateBeginning = DateTime.Now;
            DateEnd = DateTime.Now;
        }

        public int TechnologyOperationId { get; set; }
                
        public int QualifyingRank { get; set; }
                
        public decimal Rate { get; set; }
             
        public DateTime DateBeginning { get; set; }
               
        public DateTime DateEnd { get; set; }
    }
}
