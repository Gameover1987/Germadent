using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.ServiceClient.Model
{
    public class Order
    {
        public int Id { get; }

        public DateTime Created { get; set; }

        public string Customer { get; set; }

        public string Patient { get; set; }

        public string Employee { get; set; }

        public string Material { get; set; }
    }
}
