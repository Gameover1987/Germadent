namespace Germadent.Model.Rights
{
    public class UserRightsBase { }

    public enum ApplicationModule
    {
        /// <summary>
        /// Рабочее место администратора
        /// </summary>
        Rma = 1,

        /// <summary>
        /// Центр управления пользователями
        /// </summary>
        Umc = 2,

        /// <summary>
        /// Рабочее место специалиста
        /// </summary>
        Rms
    }
}