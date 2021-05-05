﻿namespace Germadent.Model.Rights
{
    /// <summary>
    /// Права для РМC
    /// </summary>
    [RightGroup(ApplicationModule.Rms)]
    public class RmsUserRights : UserRightsBase
    {
        [ApplicationRight("Запуск приложения")]
        public const string RunApplication = "Germadent.Rms.RunApplication";
    }
}