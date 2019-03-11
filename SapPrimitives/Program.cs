using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SapPrimitives.FramePrimitives;
using SapPrimitives.GeneralizedPrimitives;
using SapPrimitives.FactorySetting;
using SapPrimitives.PointPrimitives;
using SAP2000v20;

namespace SapPrimitives
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainInterface());

        }

    }
}
