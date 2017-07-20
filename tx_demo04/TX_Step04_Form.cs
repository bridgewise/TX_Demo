using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

///=============================================================================================
/// 说明窗体数据的处理
/// 
/// 1.在窗体内定义一个静态对象，用于数据交互[entData]
/// 
/// 2.显示数据，和对确定按钮的处理
/// 
/// 
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step04_Form : Form
    {
        public TX_Step04_Form()
        {
            InitializeComponent();

            if (entData == null) return;

            textBox1.Text = entData.value1.ToString();
            textBox2.Text = entData.value2.ToString();
            textBox3.Text = entData.value3.ToString();

            checkBox1.Checked = entData.check1;

            comboBox1.SelectedIndex = entData.index1;

            //--- 表格数据 --
            dataGridView1.RowCount = entData.list1.Count + 1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = entData.list1[i].ToString();
            }
        }

        public static TX_Step04_EntityData entData = new TX_Step04_EntityData();



        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (entData == null) return;

            entData.value1 = Convert.ToDouble(textBox1.Text);
            entData.value2 = Convert.ToDouble(textBox2.Text);
            entData.value3 = Convert.ToDouble(textBox3.Text);

            entData.check1 = checkBox1.Checked;

            entData.index1 = comboBox1.SelectedIndex;

            //--- 表格数据 --
            entData.list1.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                int v1 = (dataGridView1.Rows[i].Cells[0].Value == null) ? 0 : Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString());
                entData.list1.Add(v1);
            }
        }




    }

    /// <summary>
    /// 数据交互的对象
    /// </summary>
    public class TX_Step04_EntityData
    {
        public double value1 = 500;
        public double value2 = 501;
        public double value3 = 502;
        public double value4 = 503;


        public bool check1 = true;

        public int index1 = 1;
        public int index2 = 1;

        public List<double> list1 = new List<double>() { 50, 60 };
    }
}
