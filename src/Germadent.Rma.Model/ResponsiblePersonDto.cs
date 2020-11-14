
namespace Germadent.Rma.Model
{
    public class ResponsiblePersonDto : IIdentityDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position  { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int GetIdentificator()
        {
            return Id;
        }
    }
}
