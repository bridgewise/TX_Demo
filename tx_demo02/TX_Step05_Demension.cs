using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry2;
using Autodesk.AutoCAD.Runtime;

namespace TX_Demo
{

    ///=============================================================================================
    /// 说明标注
    /// 
    /// 
    /// 1.图块是有比例的，但是比例一般不用特殊处理，标注比例自动会与图块比例保持一致
    /// 
    /// 2.标注一般需要给定标注对应的点和标注的偏移[标注的偏移表示标注点到横线的距离，一般可以用  block.style.DimFirstLayer， block.style.DimSecondLayer]
    ///   表示标注第一层，第二层的间距[其他以此类推]
    /// 
    /// 3.绘图都是按照实际绘图（mm），但是标注是可以厘米标注或者毫米标注的，标注默认情况自己会处理，但是如果是自己写上去的文字就要自己处理了
    ///=============================================================================================
    public class Step05_Demension : BaseBlock
    {

        public override void Draw(DatabaseToAcad block)
        {
            ///        _                         |/______________________________________|/
            ///       |                         /|                                      /|
            ///       |                          |                                       | 
            ///   DimSecondLayer     |-          |/______________________|/______________|/
            ///       |              |          /|                      /|              /|
            ///       |          DimFirstLayer   |                       |               |
            ///       |_             |_          |                       |               |
            ///                                 1                       2               3
            ///  

            Point3d pt1 = new Point3d(0, 0, 0);
            Point3d pt2 = new Point3d(1000, 1000, 0);
            Point3d pt3 = new Point3d(4000, 4000, 0);
            block.AddDimAligned(pt1, pt2, block.style.DimFirstLayer);
            block.AddDimAligned(pt2, pt3, block.style.DimFirstLayer);
            block.AddDimAligned(pt1, pt3, block.style.DimSecondLayer);
            block.AddDimAligned(pt1, pt3, block.style.DimThirdLayer);


            Point3d pta = new Point3d(2000, 0, 0);
            Point3d ptb = new Point3d(4000, 0, 0);
            Point3d ptc = new Point3d(2000, block.style.DimForthLayer, 0);
            double len = pta.DistanceTo(ptb);
            block.AddDimRotated(pta, ptb, block.style.DimFirstLayer, 0, "L=" + CommonSimbel.ConvertToDimStr(len), DatabaseToAcad.DimTextPos.Auto);
            block.AddDimension(pta, ptb, ptc, "", CommonLayer.dimlayer);


            //------ 连续标注多个 -- 水平标注
            Point3d dimPointHori = new Point3d(-500, -500, 0);
            block.AddDimContinueHori(dimPointHori, block.style.DimFirstLayer, 500, 400, 300, 700);
            block.AddDimContinueHori(dimPointHori, block.style.DimSecondLayer, 500 + 400 + 300 + 700);

            Point3d dimPointVert = new Point3d(-800, -500, 0);
            block.AddDimContinueVertiUD(dimPointVert, block.style.DimFirstLayer, 500, 400, 300, 700);
            block.AddDimContinueVertiUD_AutoAdjust(dimPointVert, block.style.DimSecondLayer, 500 + 400 + 300 + 700);


            //block.AddDimension_UserTextPoint()///  一般情况下文字位置自定处理的，但是也可以设定
        }

        [CommandMethod("tx_step05")]
        public static void TestDrawCommond()
        {

            Step05_Demension bk1 = new Step05_Demension();
            bk1.DimScale = 50;  ///设定图块比例

            PaperLayout paper = new PaperLayout(50);///设定图纸比例
            paper.AddBlock(bk1);
            paper.AutoLayoutBlock_OneColumn(true);

            PaperLayoutGroup paperGroup = new PaperLayoutGroup();
            paperGroup.AddToPaperList_Right600(paper);
            paperGroup.ExportToCAD(null, null);
            //ExportToCAD.ExportToAutoCAD(null, paper);
        }


    }



}
