﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Germadent.Rma.App.Infrastructure
{
    public interface IEnvironment
    {
        void Restart();

        void Shutdown();
    }

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
