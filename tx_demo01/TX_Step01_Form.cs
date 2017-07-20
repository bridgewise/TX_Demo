using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

///=============================================================================================
/// 说明示意图的制作和显示
///   
///   1.在cad中绘制好图形
///   2.加载示意图工具，设置ID，（sid命令）
///   3.用命令保存示意图(ill)
///   4.在生成的应用程序目录建一个TX_illPic文件夹，将示意图放在里面
/// 
///  
/// 
///  TX_TOOLBOX.AcadAssist.AttrachFormEvent(this, curEvent);   -------------- 将鼠标操作和相应函数关联起来，使示意图起作用
///  原则为：
///  
///   textBox1 对于ID为1，textBox2对应ID为2，textBox3对应ID为3 -- 以此类推
///   dataGridView1 的第一列为 1001，dataGridView1 的第二列为 1002  -- 以此类推
/// 
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step01_Form : Form
    {
        public TX_Step01_Form()
        {
            InitializeComponent();


            //----------图形显示---------------------------
            m_Illustrator = new Illustrator();
            Size size = groupBox1.ClientSize;
            m_Illustrator.Size = size;
            groupBox1.Controls.Add(m_Illustrator);
            TX_TOOLBOX.IllustratorTool.LoadPic(m_Illustrator, "钢筋图-承台");
            //----------图形显示---------------------------   




            /// 将鼠标操作和相应函数关联起来，使示意图起作用
            MouseEventHandler curEvent = new System.Windows.Forms.MouseEventHandler(this.textBox_MouseClick);
            TX_TOOLBOX.AcadAssist.AttrachFormEvent(this, curEvent);

        }

        Illustrator m_Illustrator = null;

        private void textBox_MouseClick(object sender, EventArgs e)
        {
            TX_TOOLBOX.IllustratorTool.Control_MouseClick(m_Illustrator, sender, e);
        }
    }
}
