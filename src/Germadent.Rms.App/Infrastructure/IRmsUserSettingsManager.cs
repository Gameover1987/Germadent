using System;

namespace Germadent.Rms.App.Infrastructure
{
    public interface IRmsUserSettingsManager : IDisposable
    {
        string LastLogin { get; set; }

        void Save();
    }
}
