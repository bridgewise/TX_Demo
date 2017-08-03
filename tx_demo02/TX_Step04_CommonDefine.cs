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
    /// 说明绘图的总体参数
    /// 
    /// CommonSimbel --- 对应总体样式设置
    /// CommonLayer  --- 图层
    /// RealCommonSimbel  --- 某些样式如字高等，考虑了图块比例的情况
    /// 
    /// 
    /// 用 StyleInit.StyleInitMethod() 来初始化图纸，包括图层，文字，标注样式；
    /// 
    /// 图块是有比例的，但是比例一般不用特殊处理，标注比例自动会与图块比例保持一致
    /// 
    /// 绘图都是按照实际绘图（mm），但是标注是可以厘米标注或者毫米标注的，标注默认情况自己会处理，但是如果是自己写上去的文字就要自己处理了
    /// 
    ///=============================================================================================
    public class Step04_CommonDefine : BaseBlock
    {

        public override void Draw(DatabaseToAcad block)
        {
            ///-------- 
            string name1 = CommonSimbel.textStyleName;  /// 
            string name2 = CommonSimbel.dimStyleName;

            ////------- 默认情况下所有对象的属性都是随层 ----------
            string layerName1 = CommonLayer.gz1layer;       //粗实线
            string layerName2 = CommonLayer.gz2layer;       //粗虚线
            string layerName3 = CommonLayer.gz3layer;       //细实线
            string layerName4 = CommonLayer.gz4layer;       //细虚线
            string layerName5 = CommonLayer.gjlayer;        ///钢筋图层
            string layerName6 = CommonLayer.gslayer;        ///钢束图层
            string layerName7 = CommonLayer.hidelayer;      ///不打印图层

            double scale = block.style.BlockScale;      ///当前图块的比例

            ///标注的注脚距离标注线的尺寸
            ///              100
            ///   |----------------------------|   -|
            ///   |                            |    | DimFirstLayer
            ///   |                            |   _|
            ///
            double dimLayerDist1 = block.style.DimFirstLayer;   ///标注的偏移
            double dimLayerDist2 = block.style.DimSecondLayer;
            double dimLayerDist3 = block.style.DimThirdLayer;
            double dimLayerDist4 = block.style.DimForthLayer;

            // 文字高度
            double textHeight = block.style.RealTextHeigth;  //默认自高等于  block.style.RealTextHeigth = CommonSimbel.dimTextHeight * block.style.BlockScale            

            // 绘制文字
            block.AddText(new Point3d(800, 800, 0), "Hello1");  ///按照默认字高输出，或下面的函数可以详细控制[自高，旋转，图层，对其方式]
            block.AddText(new Point3d(900, 900, 0), "Hello2", block.style.RealTextHeigth, Math.PI * 0.25, CommonLayer.dimlayer, TxTextHorizontalMode.TextRight, TxTextVerticalMode.TextTop);///详细控制文字


            ///加标题
            if (CommonSimbel.IsDownTitle)
            {
                Point3d dwnPoint = new Point3d(0, -5000, 0);
                block.AddTitle(dwnPoint, "图块标题放在下面"); /// 应该考虑图块放在上面还是下面 
            }
            else
            {
                Point3d upPoint = new Point3d(0, 5000, 0);
                block.AddTitle(upPoint, "图块标题放在上面");  /// 应该考虑图块放在上面还是下面 
            }


        }

        [CommandMethod("tx_step04")]
        public static void TestDrawCommond()
        {
            /// 初始化不成功就不继续绘图了
            if (StyleInit.StyleInitMethod() == false) return;


            Step04_CommonDefine bk1 = new Step04_CommonDefine();
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
