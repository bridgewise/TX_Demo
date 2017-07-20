using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry2;
using Autodesk.AutoCAD.Runtime;
using TX_TOOLBOX;

namespace TX_Demo
{

    ///=============================================================================================
    /// 说明钢钢板的绘制和钢板的标注
    ///=============================================================================================
    public class Step09_SteelSlab : BaseBlock
    {

        public double H = 1800;
        public double W1 = 500;
        public double W2 = 600;


        public ISteelSlab0 isb_top = new ISteelSlab0("1", 20, SteelMaterialType.Q345, 101, "dingban");
        public ISteelSlab0 isb_dwn = new ISteelSlab0("2", 20, SteelMaterialType.Q345, 102, "dingban");  /// ---可以吧宽度构造的时候定义
        public ISteelSlab0 isb_fbs = new ISteelSlab0("2", 20, SteelMaterialType.Q345, 102, "dingban");
        public ISteelSlab0 isb_fbz = new ISteelSlab0("2", 20, SteelMaterialType.Q345, 102, "dingban");
        public ISteelSlab0 isb_rib = new ISteelSlab0("2", 20, SteelMaterialType.Q345, 102, "dingban");
        public ISteelSlab0 isb_nail = new ISteelSlab0("6", 10, SteelMaterialType.Q345, 102, "handing"); ///剪力钉

        public double length = 500;


        public override void Draw(DatabaseToAcad block)
        {

            Point3d topPt1 = new Point3d(-0.5 * W1, 0, 0);
            Point3d topPt2 = new Point3d(+0.5 * W1, 0, 0);

            /// ------------- 各种类来表示钢板 -------------
            /// SteelSlabPlanLine
            /// SteelSlabPlanPoint4X
            /// 
            /// ---- 最后一个参数控制 传入的线是钢板的中心线还是左侧线还是右侧线
            SteelSlabPlanLine sb_top = SteelSlabFactory.CreateSteelSlabPlanLine(isb_top, topPt1, topPt2, isb_top.Thick, SteelSlabPlanLine.Align.Left);
            block.AddSteelEntity(sb_top);

            //--- 取得中心线和左右边线
            List<TxLine> lns = sb_top.GetCenterAndBorderLine();


            SteelSlabPlanPoint6 sb_top2 = SteelSlabFactory.CreateSteelSlabPlanPoint4X(isb_top, topPt1, topPt2, isb_top.Thick, SteelSlabPlanPoint6.Align.Left);
            sb_top2.SetStartPointAlign(null);
            sb_top2.SetEndPointAlign(null);
            block.AddSteelEntity(sb_top);

            /// ------------- 加劲肋 -------------
            /// SteelSlabRib_PntVect   --- 一个加劲肋
            /// SteelSlabRib_OnLine    --- 多个，在一条线上
            /// SteelSlabRib_Array



            /// ------------- 焊钉 -------------
            /// SteelStud_PntVect   --- 一个焊钉
            /// SteelStud_OnLine    --- 多个，在一条线上
            /// SteelStud_Array
            //焊钉
            SteelStud_OnLine nail = SteelSlabFactory.CreateSteelNail(isb_nail, new Point3d(topPt1.X + 100, 0, 0), new Point3d(topPt2.X - 100, 0, 0), 4, 100);
            nail.StudLength = 200;
            nail.StudWidth = 10;
            block.AddSteelEntity(nail);


            //SteelSlabPlanConnect sb_con = new SteelSlabPlanConnect(isb_rib,...);
            //     节点板 
            //     |---------------------| 
            //     |                     | 
            //      \                   /
            //       \-----------------/
            //
            //
            //

        }


        [CommandMethod("tx_step09")]
        public static void TestDrawCommond()
        {

            Step09_SteelSlab bk1 = new Step09_SteelSlab();
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
