using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;


namespace TX_Demo
{
    ///=============================================================================================
    ///
    /// 
    ///=============================================================================================
    public class Step01_Command : BaseBlock
    {

        [CommandMethod("tx_midas")]
        public static void TestDrawCommond()
        {
            TX_Step01_Midas dlg = new TX_Step01_Midas();
            dlg.ShowDialog();
        }
    }



}
