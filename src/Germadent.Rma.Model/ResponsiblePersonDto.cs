
namespace Germadent.Rma.Model
{
    public class ResponsiblePersonDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position  { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }

    public class ResponsiblePersonDeleteResult
    {
        public int ResponsiblePersonId { get; set; }

        public int Count { get; set; }
    }
}
