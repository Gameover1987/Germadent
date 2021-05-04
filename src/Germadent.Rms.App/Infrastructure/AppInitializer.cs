using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Client.Common.Infrastructure;

namespace Germadent.Rms.App.Infrastructure
{
    public class AppInitializer : IAppInitializer
    {
        public void Initialize()
        {
            
        }

        public event EventHandler<InitalizationStepEventArgs> InitializationProgress;
        public event EventHandler InitializationCompleted;
        public event EventHandler InitializationFailed;
    }
}
