using System;

namespace Germadent.Model
{
    /// <summary>
    /// Заказ наряд
    /// </summary>
    public class OrderDto
    {
        public const float NormalUrgencyRatio = 1.0f;
        public const float HighUrgencyRatio = 1.3f;

        public OrderDto()
        {
            Created = DateTime.Now;
            LockDate = DateTime.Now;
            Gender = Gender.Male;
            CreatorFullName = " ";
            ToothCard = new ToothDto[0];
            AdditionalEquipment = new AdditionalEquipmentDto[0];
            Attributes = new AttributeDto[0];
            WorkAccepted = true;
            UrgencyRatio = NormalUrgencyRatio;
        }

        public int WorkOrderId { get; set; }

        public OrderStatus Status { get; set; }

        public string DocNumber { get; set; }

        public BranchType BranchType { get; set; }

        public int CustomerId { get; set; }

        public string Customer { get; set; }

        public string Patient { get; set; }

        public DateTime Created { get; set; }

        public string DateComment { get; set; }

        public int ResponsiblePersonId { get; set; }

        public string ResponsiblePerson { get; set; }

        public string ResponsiblePersonPhone { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public DateTime? FittingDate { get; set; }

        public DateTime? DateOfCompletion { get; set; }

        public string WorkDescription { get; set; }

        public bool WorkAccepted { get; set; }

        public float UrgencyRatio { get; set; }

        public bool Stl { get; set; }

        public bool Cashless { get; set; }

        public string CreatorFullName { get; set; }

        public int CreatorId { get; set; }

        public ToothDto[] ToothCard { get; set; }

        public string ProstheticArticul { get; set; }

        public string MaterialsStr { get; set; }

        public AdditionalEquipmentDto[] AdditionalEquipment { get; set; }

        public AttributeDto[] Attributes { get; set; }

        public UserDto LockedBy { get; set; }

        public DateTime LockDate { get; set; }
    }
}