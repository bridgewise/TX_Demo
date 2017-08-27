using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TX_TOOLBOX;

///=============================================================================================
/// 钢筋对象数据显示在界面上
/// 
/// 表格应该是5列，第二列是下拉列表   [可以直接把这个表格复制过去用]
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step04_Form : Form
    {
        public TX_Step04_Form()
        {
            InitializeComponent();

            if (entData == null) return;


            IllustratorTool.SetSteelToDataGrid(dataGridViewSteelTable, entData);


        }

        public static TX_Step03_EntityData entData = null;


        private void buttonOK_Click(object sender, EventArgs e)
        {



            IllustratorTool.GetSteelFromDataGrid(dataGridViewSteelTable);

            //////如果有  mapSteelTypeDiameter --- 则要加下面的代码
            ////foreach (KeyValuePair<ISteelBar, ISteelBar> one in abutment.cbSteelData.mapSteelTypeDiameter)
            ////{
            ////    one.Key.stype = one.Value.stype;
            ////    one.Key.Diameter = one.Value.Diameter;
            ////}

        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            if (entData == null) return;
            buttonOK_Click(null, null);

            IPaperLayoutGroup paperGroup = entData.GetPaperGroup(0);
            paperGroup.ExportToCAD(entData, this);

        }

    }
}
