using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.ViewModels;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockLabOrderViewModel : LabOrderViewModel
    {
        public DesignMockLabOrderViewModel()
            : base(new MockRmaOperations())
        {
            Customer = "ООО 'Рога и копыта'";
            DoctorFio = "проф. Преображенский";
            PatientFio = "Шариков Полиграф Полиграфович";
            Created = DateTime.Now;
            WorkDescription = "Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем!";

            WorkStarted = DateTime.Now;

            Color = "Белоснежный";

            NonOpacity = true;
            HighOpacity = true;
            LowOpacity = true;
            Mamelons = true;
            SecondaryDentin = true;
            PaintedFissurs = true;

            Sex = Sex.Female;
            Age = 22;

            Initialize(false);
        }
    }
}
