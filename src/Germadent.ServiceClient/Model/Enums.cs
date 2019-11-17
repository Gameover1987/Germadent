using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.ServiceClient.Model
{
    public enum Sex
    {
        Male, Female
    }

    public enum AdditionalMillingInfo
    {
        NotPainted,
        Painted,
        PrePainted
    }

    public enum CarcasColor
    {
        VitaClassical, Opak, Translucen
    }

    public enum BranchType
    {
        MillingCenter,
        Laboratory
    }
}
