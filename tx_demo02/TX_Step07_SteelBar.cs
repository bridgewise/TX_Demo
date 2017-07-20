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
    /// 说明钢筋的绘制和钢筋的标注
    /// 
    /// 
    /// 
    ///=============================================================================================
    public class Step07_Steelbar : BaseBlock
    {


        public override void Draw(DatabaseToAcad block)
        {
            double len = 2000;
            Point3d pt1 = new Point3d(0, 0, 0);
            Point3d pt2 = new Point3d(len, len, 0);

            Point3d pt3 = new Point3d(0, len, 0);
            Point3d pt4 = new Point3d(len, 0, 0);

            Point3d structPt1 = new Point3d(0, -5000, 0);

            Vector3d vect = new Vector3d(3, 9, 0);      ///
            Vector3d vectUnit = vect.GetNormal();       ///取单位向量
            Vector3d vectPer = vectUnit.RotateBy(Math.PI * 0.5, Vector3d.ZAxis);  ///向量的垂直，顺时针方向转90度


            Point3d structPt2 = structPt1 + vectUnit.MultiplyBy(2000);      ///点可以是点和向量相加
            Point3d structPt3 = structPt1 + vectPer.MultiplyBy(2000);



            // ======================== 1.定义钢筋 ====================================
            ISteelBar ist1 = new ISteelBar("1", 12, SteelBarType.HRB400, "钢筋", 101);
            ISteelBar ist2 = new ISteelBar("2", 12, SteelBarType.HRB400, "钢筋", 102);
            ISteelBar ist3 = new ISteelBar("3", 12, SteelBarType.HRB400, "钢筋", 103);
            ISteelBar ist4 = new ISteelBar("4", 12, SteelBarType.HRB400, "钢筋", 104);
            ISteelBar ist5 = new ISteelBar("5", 12, SteelBarType.HRB400, "钢筋", 105);

            double BHC_DOT = 40;
            double BHC_LIN = 40;
            // ======================== 2.绘制钢筋点 ====================================

            /// ------ 绘制点钢筋 ---------
            /// SteelDotByLine,SteelDotByCurve,SteelDotsArray   ----- 结构线可以延长，缩短；   点可以控制是否有起终点；   


            //------- 沿着直线布置一排钢筋点 ----------
            SteelDotByLine st_dot1 = SteelFactory.CreateSteelDots(ist1, structPt1, structPt2, BHC_DOT);  /// ---------- 用  SteelFactory.Create -- 来生成； BHC_LINE ---表示钢筋线到结构线的偏移距离
            st_dot1.SetDist(150, TX_Math.DesignNew.BJType.ADJ_BOTH_MIN_YS);  ///----------  控制点钢筋的间距用，首位间距自动调整
            st_dot1.ishaveLastDot = false;
            st_dot1.ishaveStartDot = true;      // 默认就是true
            st_dot1.SetExtend(500, -100);       // 两端可以调整--往里面收或者往外延伸
            block.AddSteelEntity(st_dot1);


            SteelDotByLine st_dot2 = SteelFactory.CreateSteelDots(ist2, structPt1, structPt2, BHC_DOT);
            st_dot2.SetDist(100, TX_Math.DesignNew.BJType.AVERAGE);            /// 控制所有的间距都相等
            st_dot2.SetByCount(5);                                              /// 标注几个点
            block.AddSteelEntity(st_dot2);

            SteelDotByLine st_dot3 = SteelFactory.CreateSteelDots(ist2, structPt2, structPt3, BHC_DOT);
            st_dot3.SetDist(100, TX_Math.DesignNew.BJType.AVERAGE);            /// 控制所有的间距都相等
            st_dot3.SetByDist(100);                                              /// 标注间距 @100
            block.AddSteelEntity(st_dot3);


            SteelDotsArray st_dot4 = SteelFactory.CreateSteelDots(ist2, st_dot2.GetDotPoints());
            //st_dot4.SetDots_MoveVertiToPolyLine(....



            // ======================== 3.绘制钢筋点 ====================================
            /// ------ 绘制线钢筋 ---------
            /// SteelLine,SteelPolyline   ----- 单根线
            /// SteelLines,SteelCurves    ----- 多根线
            /// SteelbarConnect            ------------- 钩子钢筋，格几个钢筋点布置一个钩子钢筋
            /// SteelbarConnectLoop           --- 忘记了，以后补充
            /// SteelbarLoopMutil         -----------  箍筋，比其他要复杂一些
            SteelLines ist_line1 = SteelFactory.CreateSteelParalLine(ist3, structPt1, structPt3, BHC_LIN, 45, 2000);
            block.AddSteelEntity(ist_line1);

            //多根曲线
            SteelCurves ist_curves = SteelFactory.CreateSteelCurves(ist4, ist_line1.GetTxLines());
            //ist_curves.ExtendHeadToCurve(...)
            //ist_curves.ExtendBackToCurve(...)
            block.AddSteelEntity(ist_curves);

            /// ======================== 3.标注钢筋点 ====================================
            /// SteelbarOneDimension                         ----- 从一个点引出钢筋标注，是最简单的一种
            /// SteelbarDotPalXYDimension
            ///
            /// ======================== 3.标注钢筋线 ====================================
            /// SteelbarVLineDimension，SteelbarHLineDimension，SteelbarXLineDimension   --- 拉一根线来标注（通常用于标注线）钢筋；可以多个箭头；三个差不多，差别在于水平，竖直，斜向
            /// SteelbarConnectDimension            ------------- 配合SteelbarConnect使用
            ///

            /// SteelbarLoopMutil  --- 箍筋的处理
            /// SteelbarDotPalXYDimension dim1 = SteelDimFactory.CreateSteelParallDimensionNew(ist1, st_dot1, new Point3d(), SteelbarDotPalXYDimensionNew.LineType.PALX_OFF_DOT_RIGHT, SteelbarDotPalXYDimensionNew.ArrowType.ARR_TOSTART);

            SteelbarDotPalXYDimensionNew dim1 = SteelDimFactory.CreateSteelParallDimensionNew(ist1, st_dot1.GetDotPoints(), new Point3d(), SteelbarDotPalXYDimensionNew.LineType.PALX_OFF_DOT_RIGHT, SteelbarDotPalXYDimensionNew.ArrowType.ARR_TOSTART);
            dim1.SteelDimLeaderLenNotScale = 10;  ///  标注线如果要加长可以设置这个参数
            dim1.additionalDimLen = 500;          ///  标注线如果要加长可以设置这个参数
            ///  
        }


        public SteelTable GetSteelTalbe()
        {
            SteelTable tb = new SteelTable();
            return tb;
        }

        [CommandMethod("tx_step07")]
        public static void TestDrawCommond()
        {

            Step07_Steelbar bk1 = new Step07_Steelbar();
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
