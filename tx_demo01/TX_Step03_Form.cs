using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

///=============================================================================================
/// 说明表格的使用
/// 
/// 
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step03_Form : Form
    {
        public TX_Step03_Form()
        {
            InitializeComponent();

            menu1.SetContextMenuStrip(dataGridView1);


            CreateData();

            int cnt = Math.Min(pts1.Count, pts2.Count);
            dataGridView1.RowCount = cnt + 1;    // 表格的行数要比数组多一行，方便填充
            for (int i = 0; i < cnt; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = pts1[i].ToString();   // 把数组的第一行 的X值转为字符 填进去 
                dataGridView1.Rows[i].Cells[1].Value = pts2[i].ToString();   // 把数组的第一行 的Y值转为字符 填进去
            }



        }



        TX_Math.ContextMenuWrap menu1 = new TX_Math.ContextMenuWrap();

        List<double> pts1 = new List<double>();      //相当于一个数组
        List<double> pts2 = new List<double>();      //相当于一个数组

        public void CreateData()
        {

            pts1.Clear();
            pts2.Clear();

            pts1.Add(1);
            pts1.Add(2);
            pts1.Add(3);
            pts1.Add(4);
            pts1.Add(5);

            pts2.Add(101);
            pts2.Add(102);
            pts2.Add(103);
            pts2.Add(104);
            pts2.Add(105);


        }

        private void button3_Click(object sender, EventArgs e)  // 把datagridview 的数据提出来放到list 的对象pts中
        {
            pts1.Clear();               // 清空 list 中的数据
            pts2.Clear();               // 清空 list 中的数据
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                int v1 = (dataGridView1.Rows[i].Cells[0].Value == null) ? 0 : Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString());
                int v2 = (dataGridView1.Rows[i].Cells[1].Value == null) ? 0 : Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                pts1.Add(v1);
                pts2.Add(v2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string value = dataGridView1.Rows[0].Cells[0].Value.ToString();
            System.Windows.Forms.MessageBox.Show("Hello Wordl! " + value);
        }
    }
}
