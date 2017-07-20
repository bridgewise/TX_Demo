using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

///=============================================================================================
/// 说明数据保存打开
/// 
///  调用 TX_Math.XMlFiler.ReadFile(dlg.FileName, this) 和 
///       TX_Math.XMlFiler.SaveFile(this, dlg.FileName, "test_data")
///  保存打开界面数据文件
/// 
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step05_Form : Form
    {
        public TX_Step05_Form()
        {
            InitializeComponent();



            //----------图形显示---------------------------
            m_Illustrator = new Illustrator();
            Size size = groupBox1.ClientSize;
            m_Illustrator.Size = size;
            groupBox1.Controls.Add(m_Illustrator);
            TX_TOOLBOX.IllustratorTool.LoadPic(m_Illustrator, "Wang");
            //----------图形显示---------------------------   



            if (entData == null) return;

            textBox1.Text = entData.value1.ToString();
            textBox2.Text = entData.value2.ToString();
            textBox3.Text = entData.value3.ToString();

            checkBox1.Checked = entData.check1;

            comboBox1.SelectedIndex = entData.index1;

            //--- 表格数据 --
            dataGridView1.RowCount = entData.list1.Count + 1;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = entData.list1[i].ToString();
            }


            /// 给表格增加右键菜单和复制黏贴功能
            menu1.SetContextMenuStrip(dataGridView1);

            /// 将鼠标操作和相应函数关联起来，使示意图起作用
            MouseEventHandler curEvent = new System.Windows.Forms.MouseEventHandler(this.textBox_MouseClick);
            TX_TOOLBOX.AcadAssist.AttrachFormEvent(this, curEvent);

        }


        Illustrator m_Illustrator = null;
        TX_Math.ContextMenuWrap menu1 = new TX_Math.ContextMenuWrap();

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



        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.test_data|*.test_data";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                TX_Math.XMlFiler.ReadFile(dlg.FileName, this);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.test_data|*.test_data";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                TX_Math.XMlFiler.SaveFile(this, dlg.FileName, "test_data");
            }
        }

        private void textBox_MouseClick(object sender, EventArgs e)
        {
            TX_TOOLBOX.IllustratorTool.Control_MouseClick(m_Illustrator, sender, e);
        }




    }

}
