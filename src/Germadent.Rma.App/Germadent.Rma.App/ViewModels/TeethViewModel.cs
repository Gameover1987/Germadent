using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels
{
    public class TeethViewModel
    {
        public int Number { get; set; }

        public bool IsChecked { get; set; }

        public bool HasBridge { get; set; }

        public Material Material { get; set; }

        public Teeth ToModel()
        {
            return new Teeth
            {
                Number = Number,
                HasBridge = HasBridge,
                Material = Material
            };
        }

        public override string ToString()
        {
            return string.Format("{0}, IsChecked={1}, HasBridge={2}", Number, IsChecked, HasBridge);
        }
    }
}
