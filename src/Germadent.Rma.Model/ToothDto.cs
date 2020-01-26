using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Rma.Model
{
    public class ToothDto
    {
        public int ToothNumber { get; set; }

        public string MaterialName { get; set; }

        public string ProstheticsName { get; set; }

        public bool HasBridge { get; set; }
    }
}
