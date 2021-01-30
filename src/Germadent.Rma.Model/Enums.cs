using System.ComponentModel;

namespace Germadent.Rma.Model
{
    public enum Gender
    {
        Female = 0,
        Male = 1, 
    }

        
    public enum BranchType
    {
        [Description("Фрезерный центр")]
        MillingCenter = 1,

        [Description("Лаборатория")]
        Laboratory = 2
    }
}
