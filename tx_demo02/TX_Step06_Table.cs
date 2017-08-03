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
    /// 说明表格的使用
    /// 
    /// 1.设置表格内容，表格行高，列宽
    /// 2.合并表格内的部分内容
    /// 
    ///=============================================================================================
    public class Step06_Table : BaseBlock
    {

        public override void Draw(DatabaseToAcad block)
        {
            double dimScale = 100;

            string[] strHead = new string[] { "A列", "B1列", "B2列", "B3列", "B4列", "B5列", "B6列", "B7列", "B8列", "B9列" };
            TxTable tb = TxTable.CreateTable(5, 10);      // 创立一个 5行10列的表


            //----------  设置表格内容 ------------------
            tb.SetTextString(0, 0, "AA");

            for (int i = 1; i < tb.RowCount; i++)
            {
                tb.SetTextString(i, 6, "Row" + i.ToString());
            }

            //----------  行列的合并内容 ------------------
            tb.MergeCells(1, 1, 3, 3);        // 合并表格 左上表格 及 右下表格


            // ----------- 设置一下格式，或者某一列的宽度
            tb.FormatTable(CommonSimbel.tableRowHeight, CommonSimbel.tableRowHeight * 3, 100);
            tb.SetColumnWidth(1, 20 * block.style.BlockScale);

            block.AddTable(tb);
            block.AddTableTitle(tb, "表头");
        }


        [CommandMethod("tx_step06")]
        public static void TestDrawCommond()
        {
            if (StyleInit.StyleInitMethod() == false) return;

            Step06_Table bk1 = new Step06_Table();
            bk1.DimScale = 50;  ///设定图块比例

            PaperLayout paper = new PaperLayout(100);///设定图纸比例
            paper.AddBlock(bk1);
            paper.AutoLayoutBlock_OneColumn(true);

            PaperLayoutGroup paperGroup = new PaperLayoutGroup();
            paperGroup.AddToPaperList_Right600(paper);
            paperGroup.ExportToCAD(null, null);

            //ExportToCAD.ExportToAutoCAD(null, paper);
        }
    }



}
