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
    /// 说明几何对象的使用
    /// 
    /// 基本的对象有点，向量，    点和向量可以加减，旋转  [数组尽量用 List<>]
    /// 
    /// 图形几何对象包括，直线，圆弧，多义线[包含直线和圆弧的线]
    /// 几何对象可以移动，求交，偏移，求切向量，距离定点，裁剪等  ----- 在AcadAssist里面有函数
    /// 
    /// 
    /// 用DatabaseToAcad block 对象可以绘制各种图形，基本的包括直线圆弧多义线，
    /// 其他则包括，文字，标注，带指引线的文字，图块标题，标高符号，桩号，坡度，折断线等等
    ///=============================================================================================
    public class Step02_Geometry : BaseBlock
    {

        public override void Draw(DatabaseToAcad block)
        {

            // ======================== 点和向量操作 ====================================
            /// 点和向量的计算
            Point3d structPt1 = new Point3d(0, -5000, 0);

            Vector3d vect = new Vector3d(3, 9, 0);      ///
            Vector3d vectUnit = vect.GetNormal();       ///取单位向量
            Vector3d vectPer = vectUnit.RotateBy(Math.PI * 0.5, Vector3d.ZAxis);  ///向量的垂直，顺时针方向转90度


            Point3d structPt2 = structPt1 + vectUnit.MultiplyBy(2000);      ///点可以是点和向量相加
            Point3d structPt3 = structPt1 + vectPer.MultiplyBy(2000);

            /// 直线
            TxLine line1 = new TxLine(new Point3d(0, 0, 0), new Point3d(500, 500, 0));
            TxLine line2 = AcadAssist.GetOffsetLineToLeft(line1, 200);                  //线的偏移
            TxLine line3 = new TxLine(new Point3d(100, -500, 0), new Point3d(100, 5000, 0));
            TxLine line4 = new TxLine(new Point3d(1000, -500, 0), new Point3d(1000, 5000, 0));
            List<Point3d> xpnt = AcadAssist.GetIntersectionPoints(line1, line3); /// 两条线求交

            /// 圆弧
            TxArc arc = new TxArc(new Point3d(), new Point3d(100, 100, 0), new Point3d(200, 0, 0));///三点确定圆弧


            /// 圆弧转化为多义线
            TxPolyline pl2 = AcadAssist.ConvertToTxPolyline(arc);


            /// 多义线 ---- 比较重要的几何对象,含有直线和圆弧，如果是圆弧则 bulge 参数不等于0
            TxPolyline pl1 = new TxPolyline();
            pl1.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
            pl1.AddVertexAt(1, new Point2d(0, 500), 0, 0, 0);
            pl1.AddVertexAt(2, new Point2d(500, 500), 0, 0, 0);
            pl1.AddVertexAt(3, new Point2d(300, 100), 0.5, 0, 0); /// 这个参数不是0，则是曲线
            pl1.AddVertexAt(4, new Point2d(0, 0), 0, 0, 0);

            ///
            List<Point3d> points = new List<Point3d>();
            TX_Math.Array2.Resize(ref points, 5);
            points[0] = new Point3d(0, 0, 0);
            points[1] = new Point3d(0, 500, 0);
            points[2] = new Point3d(900, 50, 0);
            points[3] = new Point3d(900, 2350, 0);
            points[4] = new Point3d(900, 5698, 0);


            // ======================== 基本图形的运算  AcadAssist.() ====================================
            /// 多义线裁对称
            //pl3 = AcadAssist.MillarTxPolyline(pl3, 0);            ///曲线根据某个位置对称
            TxPolyline pl3 = AcadAssist.MillarTxPolyline(pl2, 0);  ///  曲线根据某个位置对称


            /// 多义线裁剪和延长
            //TxPolyline pl3 = AcadAssist.TrimPolylineGetFront(pl2, new Point3d(500, 500, 0));  ///  裁剪，求线的前面部分

            TxLine lineExt1 = AcadAssist.ExtendLineBoth(line1, 500);
            TxLine lineExt2 = AcadAssist.ExtendLineHead(line1, pl2);


            /// 求交点
            List<Point3d> pnts1 = AcadAssist.GetIntersectionPoints(pl1, pl2);
            //Point3d pnt2 = AcadAssist.GetIntersectionPoint_ExtLines(line1, line2);

            /// 曲线在X坐标上的点
            List<Point3d> pnts3 = AcadAssist.GetPoint3dArrayAtCurveX(pl1, 500);

            List<Point3d> splitPoints = AcadAssist.SplitCurveByCount(pl1, 3, true, true);

            //List<TxCurve> splitCurves = AcadAssist.SplitCurveByPoints(pl1, 3, true, true);



            //-----------两条线都偏移，然后再求交-- 
            Point3d ptx = AcadAssist.XOffsetLinesToLeft(points[0], points[1], points[2], 30, 30);









            // ======================== 绘制图形 ====================================
            /// -------- 绘制线 -----------
            block.AddLine(new Point3d(0, 0, 0), new Point3d(500, 500, 0), CommonLayer.zerolayer);
            /// --------绘制线  ----------------------- 图层说明 -- 构造有四个图层，  加入的对象颜色和线型都是随层的
            block.AddLine(line1, CommonLayer.gz1layer);  ///粗实线
            block.AddLine(line2, CommonLayer.gz2layer);  ///细实线
            block.AddLine(line3, CommonLayer.gz3layer);  ///粗虚线
            block.AddLine(line4, CommonLayer.gz4layer);  ///细虚线

            /// --------- 绘制其它基本图形
            block.AddCircle(new Point3d(0, 0, 0), 500, CommonLayer.zerolayer);
            block.AddCurve(arc, CommonLayer.dimlayer);



            /// --------绘制线 
            block.AddCurve(pl1);
            block.AddCurve(pl2);
            block.AddCurve(pl3);

            ///标注文字和线 [带指引线的文字]
            block.AddTextDim(new Point3d(0, 500, 0), "带指引线的文字", 2); /// 最后一个参数表示是哪个象限[1,2,3,4]
            block.AddTextDimAngle(new Point3d(0, 1500, 0), "sss", new Point3d(2500, 5000, 0), 250);

            ///加阴影部分
            TxPolyline plx = AcadAssist.CloneTxPolyline(pl1);
            block.AddHatch(plx, 1, CommonLayer.gz1layer);


            // -------- 绘制文字 ------
            block.AddText(new Point3d(800, 800, 0), "Hello1");
            block.AddText(new Point3d(900, 900, 0), "Hello2", block.style.RealTextHeigth, Math.PI * 0.25, CommonLayer.dimlayer, TxTextHorizontalMode.TextRight, TxTextVerticalMode.TextTop);

            ///加标题 -- 下面有两条线，右边有比例1:50
            block.AddTitle(new Point3d(500, 500, 0), "图块标题"); /// 应该考虑图块放在上面还是下面 


            ///加折断线 ,有三种，单折断线，双折断线，圆柱折断线
            block.AddBreakLine(structPt2, structPt3, 0.5 * block.style.DimFirstLayer);


            //public void AddSectionLineHori(Point3d midPoint, string name, bool isTextUp, bool isArrowLeft);       /// 剖断线
            //public void AddSectionLineVert(Point3d midPoint, string name, bool isTextLeft, bool isArrowUpside);   /// 剖断线
        }


        [CommandMethod("tx_step03")]
        public static void TestDrawCommond()
        {

            Step02_Geometry bk1 = new Step02_Geometry();
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
