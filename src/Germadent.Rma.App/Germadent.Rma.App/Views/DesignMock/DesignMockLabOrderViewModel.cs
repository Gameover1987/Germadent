using System;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels;

using System;
using System.Collections.ObjectModel;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Operation;
using Germadent.UI.ViewModels;

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
