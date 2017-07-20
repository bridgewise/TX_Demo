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
    /// 说明图块的使用
    /// 
    /// 用CAD的方法绘制图形太麻烦了，因此需要封装；用图块来封装绘图的内容
    /// 从BaseBlock上派生图块，实现其Draw函数
    ///
    /// 向CAD输出的时候用了 PaperLayout ,代表一张或多张图纸；
    /// 图块用Add函数添加到图纸里面，然后用ExportToAutoCAD_NotUserInteraction 输出到CAD中
    ///=============================================================================================
    public class MyBlock : BaseBlock
    {
        public double start = 100;
        public double len = 500;
        public override void Draw(DatabaseToAcad block)
        {
            Point3d pt1 = new Point3d(0, 0, 0);
            Point3d pt2 = new Point3d(len, len, 0);

            Point3d pt3 = new Point3d(0, len, 0);
            Point3d pt4 = new Point3d(len, 0, 0);

            //---- 最后一个参数是图层，所有的对象属性是按层的，0图层默认就有，不用创建
            block.AddLine(pt1, pt2, CommonLayer.zerolayer);

            block.AddCircle(new Point3d(0, 0, 0), 500, CommonLayer.zerolayer);

            block.AddText(new Point3d(300, 0, 0), "Test1", true);
            block.AddText(new Point3d(300, 500, 0), "Test2", block.style.RealTextHeigth, 0, CommonLayer.zerolayer, TxTextHorizontalMode.TextLeft, TxTextVerticalMode.TextBottom);


            TxLine ln = new TxLine(pt3, pt4);
            block.AddLine(ln, CommonLayer.zerolayer);
        }

        [CommandMethod("tx_step02")]
        public static void TestDrawCommond()
        {

            MyBlock bk1 = new MyBlock();
            bk1.DimScale = 50;  ///设定图块比例
            bk1.len = 2000;

            MyBlock bk2 = new MyBlock();
            bk2.DimScale = 50;
            bk1.len = 3000;

            PaperLayout paper = new PaperLayout(50);///设定图纸比例
            paper.AddBlock(bk1);
            paper.AddBlock(bk2);
            paper.AutoLayoutBlock_OneColumn(true);


            PaperLayoutGroup paperGroup = new PaperLayoutGroup();
            paperGroup.AddToPaperList_Right600(paper);

            //paperGroup.ExportToCAD(null, null);
            ExportToCAD.ExportToAutoCAD(null, paperGroup);
        }

    }



}
