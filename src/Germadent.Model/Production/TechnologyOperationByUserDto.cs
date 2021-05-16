using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Model.Production
{
    public class TechnologyOperationByUserDto
    {
        public UserDto User { get; set; }

        public TechnologyOperationDto Operation { get; set; }

        public float UrgencyRatio { get; set; }

        public decimal Rate { get; set; }

        public int ProductCount { get; set; }

        public int? ProductId { get; set; }

        public decimal TotalCost
        {
            get { return ProductCount * Rate * (decimal) UrgencyRatio; }
        }
    }
}
