namespace Germadent.UserManagementCenter.Model.Rights
{
    public class RightDto
    {
        public int RightId { get; set; }

        /// <summary>
        /// Название права
        /// </summary>
        public string RightName { get; set; }

        /// <summary>
        /// Название подсистемы
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
