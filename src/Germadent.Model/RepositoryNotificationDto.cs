namespace Germadent.Model
{
    public enum RepositoryType
    {
        PriceGroup,
        PricePosition,
        Product,
        Customer,
        ResponsiblePerson,
        TechnologyOperation
    }

    public class RepositoryNotificationDto
    {
        public RepositoryNotificationDto(RepositoryType repositoryType)
        {
            RepositoryType = repositoryType;
        }

        public RepositoryType RepositoryType { get;  }

        public object[] AddedItems { get; set; }

        public object[] ChangedItems { get; set; }

        public int[] DeletedItems { get; set; }
    }
}
