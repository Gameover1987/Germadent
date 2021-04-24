using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Rma.Model.Production
{
    public class RateDto
    {
        public int TechnologyOperationId { get; set; }
                
        public int QualifyingRank { get; set; }
                
        public decimal Rate { get; set; }
             
        public DateTime DateBeginning { get; set; }
               
        public DateTime DateEnd { get; set; }
    }
}
