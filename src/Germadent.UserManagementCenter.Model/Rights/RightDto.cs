using System.Security;

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
        /// Описание права
        /// </summary>
        public string RightDescription { get; set; }

        /// <summary>
        /// Название подсистемы
        /// </summary>
        public ApplicationModule ApplicationModule { get; set; }

        /// <summary>
        /// Признак что право устарело
        /// </summary>
        public bool IsObsolete { get; set; }
    }
}
