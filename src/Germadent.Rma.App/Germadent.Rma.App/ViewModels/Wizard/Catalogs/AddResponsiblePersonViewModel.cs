using System;
using Germadent.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public enum CardViewMode
    {
        Add = 0,
        Edit = 1,
        View = 2
    }

    public class AddResponsiblePersonViewModel : ValidationSupportableViewModel, IAddResponsiblePersonViewModel
    {
        private int _id;
        private string _fullName;
        private string _phone;
        private string _email;
        private string _position;
        private string _description;

        public AddResponsiblePersonViewModel()
        {
            AddValidationFor(() => FullName)
                .When(() => string.IsNullOrWhiteSpace(FullName), () => "Укажите ФИО");
            AddValidationFor(() => Position)
                .When(() => string.IsNullOrWhiteSpace(Position), () => "Укажите должность");

            OKCommand = new DelegateCommand(() => { }, CanOKCommandHandler);
        }

        public string Title => GetTitle(ViewMode);

        public CardViewMode ViewMode { get; private set; }

        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName == value)
                    return;
                _fullName = value;
                OnPropertyChanged(() => FullName);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone == value)
                    return;
                _phone = value;
                OnPropertyChanged(() => Phone);
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email == value)
                    return;
                _email = value;
                OnPropertyChanged(() => Email);
            }
        }

        public string Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;
                _position = value;
                OnPropertyChanged(() => Position);
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;
                _description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public IDelegateCommand OKCommand { get; }

        public void Initialize(CardViewMode viewMode, ResponsiblePersonDto responsiblePerson)
        {
            ResetValidation();

            ViewMode = viewMode;

            _id = responsiblePerson.Id;
            _fullName = responsiblePerson.FullName;
            _phone = responsiblePerson.Phone;
            _email = responsiblePerson.Email;
            _position = responsiblePerson.Position;
            _description = responsiblePerson.Description;
        }

        public ResponsiblePersonDto GetResponsiblePerson()
        {
            return new ResponsiblePersonDto
            {
                Id = _id,
                FullName = FullName,
                Phone = Phone,
                Email = Email,
                Position = Position,
                Description = Description,
            };
        }

        private bool CanOKCommandHandler()
        {
            return !HasErrors && !string.IsNullOrWhiteSpace(FullName) && !string.IsNullOrWhiteSpace(Position);
        }

        private string GetTitle(CardViewMode cardViewMode)
        {
            switch (cardViewMode)
            {
                case CardViewMode.Add:
                    return "Добавление ответственного лица";

                case CardViewMode.Edit:
                    return "Редактирование данных ответственного лица";

                case CardViewMode.View:
                    return "Просмотр данных ответственного лица";

                default:
                    throw new NotImplementedException("Неизвестный режим представления");
            }
        }
    }
}