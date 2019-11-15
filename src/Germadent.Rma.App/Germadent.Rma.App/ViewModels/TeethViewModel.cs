using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Rma.App.ViewModels
{
    public class TeethViewModel
    {
        public int Number { get; set; }

        public bool IsChecked { get; set; }

        public bool HasBridge { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, IsChecked={1}, HasBridge={2}", Number, IsChecked, HasBridge);
        }
    }
}
