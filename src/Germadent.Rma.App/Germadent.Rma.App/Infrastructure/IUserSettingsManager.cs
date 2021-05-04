using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.App.Infrastructure
{
    public interface IUserSettingsManager : IDisposable
    {
        string LastLogin { get; set; }

        ColumnInfo[] Columns { get; set; }

        void Save();
    }
}
