using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.AutoCAD.Geometry2;
using Autodesk.AutoCAD.Runtime;
using TX_TOOLBOX;

namespace TX_Demo
{

    ///=============================================================================================
    /// 说明标注
    /// 
    /// 
    /// 
    ///=============================================================================================
    public class Step08_Tendon : BaseBlock
    {

        public override void Draw(DatabaseToAcad block)
        {

   
        }

        [CommandMethod("tx_step08")]
        public static void TestDrawCommond()
        {

            Step08_Tendon bk1 = new Step08_Tendon();
            bk1.DimScale = 50;  ///设定图块比例

            PaperLayout paper = new PaperLayout(50);///设定图纸比例
            paper.AddBlock(bk1);
            paper.AutoLayoutBlock_OneColumn(true);


            //ExportToCAD.ExportToAutoCAD(null, paper);
        }

    }



}
