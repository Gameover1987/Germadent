using System;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMillingCenterOrderViewModel : MillingCenterOrderViewModel
    {
        public DesignMockMillingCenterOrderViewModel()
            : base(new MockRmaOperations())
        {
            Customer = "Какой то заказчик для фрезерного центра";
            PatientFio = "Какой то пациент";
            TechnicFio = "Какой то техник";
            TechnicPhoneNumber = "79968887755";
            WorkReceived = DateTime.Now;
            AdditionalMillingInfo = AdditionalMillingInfo.Painted;
            CarcasColor = CarcasColor.Opak;
            ImplantSystemDescription = "Какие то импланты";

            Matching = true;
            Polishing = true;
            LeedgePlunge05 = true;
            LeedgePlunge10 = true;
            LedgePlungeCustom = 13;

            Screw = "Какие то винты";
            TitaniumBase = "Какая то титановая основа";

            WorkDescription = "Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем!";
            Agreement = true;

            Initialize(false);
        }
    }
}