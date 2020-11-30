using System;
using System.Collections.Generic;
using System.Text;
using Germadent.UI.Infrastructure;

namespace Germadent.UI.ViewModels.DesignTime
{
    public class DesignMockUiTimer : IUiTimer
    {
        public event EventHandler<EventArgs> Tick;
        public void Initialize(TimeSpan interval)
        {
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}
