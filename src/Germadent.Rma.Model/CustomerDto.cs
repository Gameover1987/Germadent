namespace Germadent.Rma.Model
{
    public interface IIdentityDto
    {
        int GetIdentificator();
    }

    public class CustomerDto : IIdentityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Description { get; set; }

        public int GetIdentificator()
        {
            return Id;
        }
    }
}
