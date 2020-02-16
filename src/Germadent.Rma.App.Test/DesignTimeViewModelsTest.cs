using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.Views.DesignMock;
using NUnit.Framework;

namespace Germadent.Rma.App.Test
{
    /// <summary>
    /// Проверка создания моделей для дизайнера
    /// </summary>
    [TestFixture]
    public class DesignTimeViewModelsTest
    {
        [Test]
        public void ShouldCreateDesignTimeViewModels()
        {
            var laboratoryProjectWizardStepViewModel = new DesignMockLaboratoryProjectWizardStepViewModel();
        }
    }
}
