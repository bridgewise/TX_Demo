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
    /// 说明对象有多个输入对话框的情况
    /// 
    ///   实现虚函数
    ///   GetEntityListName  --- 显示多个名称
    ///   GetEntityForm      --- 根据名称弹出不同对话框
    /// 
    ///=============================================================================================
    public class TX_Step03_EntityData : BaseEntity
    {
        #region ShowForm
        public override string GetEntityShowName()
        {
            return "我的构件03";
        }

        public override List<string> GetEntityListName()
        {
            List<string> listName = new List<string>();
            listName.Add("构造");
            listName.Add("钢筋");
            return listName;
        }

        public override int GetEntityShowIndex()
        {
            return 99003;
        }

        public override Form GetEntityForm(string nodeText)
        {
            if (nodeText == "构造")
            {
                TX_Step03_Form.entData = this;
                TX_Step03_Form dlg = new TX_Step03_Form();
                return dlg;
            }
            else if (nodeText == "钢筋")
            {
                TX_Step04_Form.entData = this;
                TX_Step04_Form dlg = new TX_Step04_Form();
                return dlg;
            }
            return null;
        }
        #endregion

        /// 显示构造数据
        public double value1 = 500;
        public double value2 = 501;
        public double value3 = 502;
        public double value4 = 503;
        public bool check1 = true;
        public int index1 = 1;
        public int index2 = 1;
        public List<double> list1 = new List<double>() { 50, 60 };

        /// 钢筋数据
        public double dist_loop_mid = 150;
        public double dist_loop_sid = 100;
        public double dist_hook = 450;
        public double count_fs = 3;
        public double count_fz = 3;

        // 
        public ISteelBar ist1 = new ISteelBar("1", 20, ISteelBar.DefSteelType1, "台帽纵向钢筋", 111);
        public ISteelBar ist2 = new ISteelBar("1", 20, ISteelBar.DefSteelType1, "台帽纵向钢筋", 112);
        public ISteelBar ist3 = new ISteelBar("1", 20, ISteelBar.DefSteelType1, "台帽纵向钢筋", 113);
        public ISteelBar ist4 = new ISteelBar("1", 20, ISteelBar.DefSteelType1, "台帽纵向钢筋", 114);
        public ISteelBar ist5 = new ISteelBar("1", 20, ISteelBar.DefSteelType1, "台帽纵向钢筋", 115);

        public ISteelBar ist_tm_hori_bq_top = new ISteelBar("2", 25, ISteelBar.DefSteelType1, "背墙纵向钢筋", 101);   //hori
        public ISteelBar ist_tm_hori_db_top = new ISteelBar("3", 22, ISteelBar.DefSteelType1, "背墙搭板钢筋", 103);   //hori
        public ISteelBar ist_tm_hori_tm_dwn = new ISteelBar("4", 22, ISteelBar.DefSteelType1, "背墙下缘钢筋", 104);           //hori
        public ISteelBar ist_tm_hori_tm_sid = new ISteelBar("5", 12, ISteelBar.DefSteelType1, "背墙侧面钢筋", 105);           //hori
        public ISteelBar ist_tm_loop_big_zl = new ISteelBar("6", 16, ISteelBar.DefSteelType1, "台帽箍筋-主梁", 106);      //台帽[放置主梁]-箍筋


        ///publc Dictionary<ISteelBar, ISteelBar> mapSteelTypeDiameter = new Dictionary<ISteelBar, ISteelBar>();   /// 名字不能变 ----


        public void AutoSetName()
        {
            int index = 1;
            ist1.Name = index.ToString(); index++;
            ist2.Name = index.ToString(); index++;
            ist3.Name = index.ToString(); index++;
            ist4.Name = ist3.Name + "a";
            ist5.Name = index.ToString(); index++;

        }

        public override IPaperLayoutGroup GetPaperGroup(int flag)
        {
            this.AutoSetName();

            PaperLayoutGroup paperGroup = new PaperLayoutGroup();


            PaperLayout paperLayout1 = new PaperLayout(100);

            TX_Step03_SteelLM blockLM = this.GetBlockLM();
            TX_Step03_SteelPM blockPM = this.GetBlockPM();
            TX_Step03_SteelDM blockDM = this.GetBlockDM();

            paperLayout1.AddBlock(blockLM);
            paperLayout1.AddBlock(blockPM);
            paperLayout1.AddBlock(blockDM);

            //排图函数一定要调用，否则不绘制图形
            paperLayout1.AutoLayoutBlock_OneColumn(true);

            paperGroup.AddToPaperListDownArray(paperLayout1);
            return paperGroup;
        }

        public TX_Step03_SteelLM GetBlockLM()
        {
            TX_Step03_SteelLM blockLM = new TX_Step03_SteelLM();

            blockLM.ist1 = this.ist1;
            blockLM.ist2 = this.ist2;
            blockLM.ist3 = this.ist3;
            blockLM.ist4 = this.ist4;


            blockLM.index1 = this.index1;
            blockLM.index2 = this.index2;
            return blockLM;
        }
        public TX_Step03_SteelPM GetBlockPM()
        {
            TX_Step03_SteelPM blockLM = new TX_Step03_SteelPM();
            blockLM.index1 = this.index1;
            blockLM.index2 = this.index2;
            return blockLM;
        }
        public TX_Step03_SteelDM GetBlockDM()
        {
            TX_Step03_SteelDM blockLM = new TX_Step03_SteelDM();
            blockLM.index1 = this.index1;
            blockLM.index2 = this.index2;
            return blockLM;
        }

    }


}
