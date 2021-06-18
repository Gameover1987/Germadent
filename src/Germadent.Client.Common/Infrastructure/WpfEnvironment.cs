using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Germadent.Client.Common.Infrastructure
{
    public class WpfEnvironment : IEnvironment
    {
        public void Restart()
        {
            var pathToDll = Application.ResourceAssembly.Location;
            var pathToExe = Path.ChangeExtension(pathToDll, ".exe");
            Process.Start(pathToExe);
            Process.GetCurrentProcess().Kill();
        }

        public void Shutdown()
        {
            Application.Current.Shutdown(0);
        }
    }
}