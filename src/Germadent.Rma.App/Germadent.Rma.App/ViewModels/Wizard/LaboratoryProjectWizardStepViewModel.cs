﻿using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class LaboratoryProjectWizardStepViewModel : WizardStepViewModelBase, IToothCardContainer
    {
        private readonly IDictionaryRepository _dictionaryRepository;
        private string _workDescription;
        private string _colorAndFeatures;
        private string _prostheticArticul;

        private DictionaryItemDto _selectedTransparency;

        public LaboratoryProjectWizardStepViewModel(IToothCardViewModel toothCard, IOrderFilesContainerViewModel orderFilesContainer, IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
            FilesContainer = orderFilesContainer;
            ToothCard = toothCard;
        }

        public override string DisplayName => "Проект";

        public override bool IsValid => !HasErrors && ToothCard.IsValid;

        public string WorkDescription
        {
            get => _workDescription;
            set
            {
                if (_workDescription == value)
                    return;
                _workDescription = value;
                OnPropertyChanged(() => WorkDescription);
            }
        }

        public string ColorAndFeatures
        {
            get => _colorAndFeatures;
            set {
                if (_colorAndFeatures == value)
                    return;
                _colorAndFeatures = value;
                OnPropertyChanged(() => ColorAndFeatures);
            }
        }

        public string ProstheticArticul
        {
            get => _prostheticArticul;
            set
            {
                if (_prostheticArticul == value)
                    return;
                _prostheticArticul = value;
                OnPropertyChanged(() => ProstheticArticul);
            }
        }

        public ObservableCollection<DictionaryItemDto> Transparences { get; } = new ObservableCollection<DictionaryItemDto>();

        public DictionaryItemDto SelectedTransparency
        {
            get => _selectedTransparency;
            set
            {
                if (_selectedTransparency == value)
                    return;
                _selectedTransparency = value;
                OnPropertyChanged(() => SelectedTransparency);
            }
        }

        public IToothCardViewModel ToothCard { get; }

        public IOrderFilesContainerViewModel FilesContainer { get; }

        public override void Initialize(OrderDto order)
        {
            _workDescription = order.WorkDescription;
            _colorAndFeatures = order.ColorAndFeatures;
            _prostheticArticul = order.ProstheticArticul;

            Transparences.Clear();
            var transparences = _dictionaryRepository.GetItems(DictionaryType.Transparency);
            transparences.ForEach(x => Transparences.Add(x));

            SelectedTransparency = Transparences.First(x => x.Id == order.Transparency);

            ToothCard.Initialize(order.ToothCard);
            FilesContainer.Initialize(order);

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.WorkDescription = WorkDescription;
            order.ColorAndFeatures = ColorAndFeatures;
            order.Transparency = SelectedTransparency.Id;
            order.ProstheticArticul = ProstheticArticul;

            order.ToothCard = ToothCard.ToDto();
            order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);

            FilesContainer.AssemblyOrder(order);
        }
    }
}