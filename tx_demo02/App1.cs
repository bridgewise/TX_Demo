using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;



namespace TX_TOOLBOX
{

    public class MyApp : IExtensionApplication
    {

        public MyApp()
        {
        }

        public void Initialize()
        {
            ExportToCAD.iexportToCAD = new ExportToCAD_Imp();

        }

        public void Terminate()
        {
        }
    }
}

