using System.Text;
using Germadent.CorrectionConstructionFile.App.Model;
using Germadent.UI.ViewModels;

namespace Germadent.CorrectionConstructionFile.App.ViewModel
{
    public class ImplantSystemViewModel : ViewModelBase
    {
        private ImplantSystem _model;

        public ImplantSystemViewModel(ImplantSystem implantSystem)
        {
            _model = implantSystem;
        }

        public string Name => _model.Name;

        public string Description
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach (var item in _model.CorrectionDictionary)
                {
                    stringBuilder.Append(item.Value + ", ");
                }

                var result = stringBuilder.ToString().Trim(',', ' ');
                return result;
            }
        }

        public void Update(ImplantSystem implantSystem)
        {
            _model = implantSystem;
            OnPropertyChanged();
        }

        public ImplantSystem ToModel()
        {
            return _model;
        }
    }
}