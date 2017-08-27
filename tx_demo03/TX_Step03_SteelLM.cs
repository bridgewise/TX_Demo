using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry2;

namespace TX_Demo
{

    ///=============================================================================================
    /// 钢筋对象数据显示在界面上
    /// 
    /// 1.钢筋表格用于修改钢筋类型和钢筋直径
    ///=============================================================================================
    public class TX_Step03_SteelLM : BaseBlock
    {


        public double value1 = 500;
        public double value2 = 501;
        public double value3 = 502;
        public double value4 = 503;


        public bool check1 = true;

        public int index1 = 1;
        public int index2 = 1;


        //public ISteelBar ist_tm_hori_zl_top = new ISteelBar("1", 20, ISteelBar.DefSteelType1, "台帽纵向钢筋", 102);   //hori
        //public ISteelBar ist_tm_hori_bq_top = new ISteelBar("2", 25, ISteelBar.DefSteelType1, "背墙纵向钢筋", 101);   //hori
        //// ======================== 1.定义钢筋 ====================================
        public ISteelBar ist1 = new ISteelBar("1", 12, SteelBarType.HRB400, "钢筋", 101);
        public ISteelBar ist2 = new ISteelBar("2", 12, SteelBarType.HRB400, "钢筋", 102);
        public ISteelBar ist3 = new ISteelBar("3", 12, SteelBarType.HRB400, "钢筋", 103);
        public ISteelBar ist4 = new ISteelBar("4", 12, SteelBarType.HRB400, "钢筋", 104);
        public ISteelBar ist5 = new ISteelBar("5", 12, SteelBarType.HRB400, "钢筋", 105);

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

            // ======================== 定义钢筋分类 ====================================
            // 点钢筋  -- 沿着直线布置，交叉位置会布置一根钢筋，中间间距相等，起始和终止位置间距调整
            // 线钢筋  -- 中间间距相等，起始和终止位置间距调整
            // 钩子钢筋  hook  --- 与点钢筋有对应关系，放在点钢筋边上    SteelConnect ,所以是几个点(nGap)布置一个钩子钢筋，如果不整除，程序自动调整，起终点都会固定布置一个钩子钢筋，但是可以通过设置，是否绘制起终点钩子钢筋
            // 箍筋    -- 三种布置方式，目前只有在绘图之后才知道最终形状
            // 骨架钢筋  



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


            /// SteelbarOneDimension                         ----- 从一个点引出钢筋标注，是最简单的一种

            /// SteelbarDotPalXYDimension

            Point3d dim1_pnt = new Point3d(500, 500, 0);
            SteelbarDotPalXYDimensionNew dim_dots1 = SteelDimFactory.CreateSteelParallDimensionNew(ist1, st_dot1.GetDotPoints(), dim1_pnt, SteelbarDotPalXYDimensionNew.LineType.PALX_OFF_DOT_RIGHT, SteelbarDotPalXYDimensionNew.ArrowType.ARR_TOSTART);

            //SteelbarDotDimension dim2 = SteelDimFactory.CreateSteelDotTDimension();

            // ======================== 3.绘制钢筋线 ====================================
            /// ------ 绘制线钢筋 ---------
            /// SteelLine,SteelPolyline   ----- 单根线
            /// SteelLines,SteelCurves    ----- 多根线
            SteelLines ist_line1 = SteelFactory.CreateSteelParalLine(ist3, structPt1, structPt3, BHC_LIN, 45, 2000);
            block.AddSteelEntity(ist_line1);

            //多根曲线
            SteelCurves ist_curves = SteelFactory.CreateSteelCurves(ist4, ist_line1.GetTxLines());
            //ist_curves.ExtendHeadToCurve(...)
            //ist_curves.ExtendBackToCurve(...)
            block.AddSteelEntity(ist_curves);


            /// SteelbarLoopMutil  --- 箍筋的处理

            /// 水平箭头
            SteelbarHLineDimension dim1 = SteelDimFactory.CreateHLineDimension(ist1, new Point3d(0, 1000, 0), new Point3d(500, 1000, 0), true);
            block.AddSteelEntity(dim1);

            /// 竖直箭头
            SteelbarVLineDimension dim2 = SteelDimFactory.CreateVLineDimension(ist1, new Point3d(500, 1000, 0), new Point3d(500, 2000, 0), true);
            block.AddSteelEntity(dim2);

            /// 斜向的线
            SteelbarDotPalXYDimensionNew dim3 = SteelDimFactory.CreateSteelParallDimensionNew(ist1, st_dot1.GetDotPoints(), new Point3d(), SteelbarDotPalXYDimensionNew.LineType.PALX_OFF_DOT_RIGHT, SteelbarDotPalXYDimensionNew.ArrowType.ARR_TOSTART);
            dim3.SteelDimLeaderLenNotScale = 10;  ///  标注线如果要加长可以设置这个参数
            dim3.additionalDimLen = 500;          ///  标注线如果要加长可以设置这个参数
            block.AddSteelEntity(dim3);


            /// SteelbarVLineDimension，SteelbarHLineDimension，SteelbarXLineDimension   --- 拉一根线来标注（通常用于标注线）钢筋；可以多个箭头；三个差不多，差别在于水平，竖直，斜向


            // ======================== 3.绘制钩子钢筋 ====================================
            /// ------ 绘制线钢筋 ---------
            /// SteelbarConnect            ------------- 钩子钢筋，格几个钢筋点布置一个钩子钢筋
            /// SteelbarConnectLoop           --- 忘记了，以后补充
            /// SteelbarLoopMutil         -----------  箍筋，比其他要复杂一些

            /// 钩子钢筋  hook  --- 与点钢筋有对应关系，放在点钢筋边上    SteelConnect ,所以是几个点(nGap)布置一个钩子钢筋，如果不整除，程序自动调整，起终点都会固定布置一个钩子钢筋，但是可以通过设置，是否绘制起终点钩子钢筋
            /// 
            /// --- 钩子钢筋标注---- （和其他钢筋标注不同点）左右都有箭头（在一边时要改进）
            //SteelbarConnectDimension dim4 = SteelDimFactory.CreateSteelConnectDimension(ist_ .CreateSteelConnectDimension(..,_)//
            /// SteelbarConnectDimension            ------------- 配合SteelbarConnect使用



            /// ======================== 3.标注钢筋点 ====================================
            ///
            ///
        }
    }


}
