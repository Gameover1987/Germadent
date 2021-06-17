using System;
using System.Collections.Generic;

namespace Germadent.Rms.App.Infrastructure
{
    public interface IRmsUserSettingsManager : IDisposable
    {
        string LastLogin { get; set; }

        List<string> UserNames { get; set; }

        void Save();
    }
}
