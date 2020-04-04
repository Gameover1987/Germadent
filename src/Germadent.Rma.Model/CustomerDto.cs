using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Rma.Model
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Description { get; set; }
    }
}
