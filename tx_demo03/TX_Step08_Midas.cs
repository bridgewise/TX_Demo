using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace TX_Demo
{

    ///=============================================================================================
    /// 说明导出Midas脚本文件CMT
    /// 
    /// 
    /// 
    ///=============================================================================================
    public class Step07_Tendon
    {

        [CommandMethod("tx_step08")]
        public static void TestDrawCommond()
        {

            TX_TOOLBOX.StructFEM_Midas midas = new StructFEM_Midas();

            TxLine line = new TxLine(new Point3d(0,0,0),new Point3d(5000,0,0));
            List<Point3d> nodePnts = AcadAssist.SplitCurveByDist(line,1000);

            midas.AddMaterial( StructFEM_DB.

            for(int i=0; i<nodePnts.Count; i++)
            {
                midas.AddNodeMM(nodePnts[i], nodePnts[i]);

                if (i != 0)
                {
                    midas.AddElement(i,i+1,
                }
            }

        }


    }



}
