using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.App.Infrastructure
{
    public interface IRmaUserSettingsManager : IDisposable
    {
        string LastLogin { get; set; }

        List<string> UserNames { get; set; }

        ColumnInfo[] Columns { get; set; }

        void Save();
    }
}
