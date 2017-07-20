using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

///=============================================================================================
/// 说明用窗体数据的处理
/// 
/// 1.表格增加菜单和复制黏贴功能
/// 
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step02_Form : Form
    {
        public TX_Step02_Form()
        {
            InitializeComponent();

            menu1.SetContextMenuStrip(dataGridView1);
            menu2.SetContextMenuStrip(dataGridView2);
        }



        TX_Math.ContextMenuWrap menu1 = new TX_Math.ContextMenuWrap();
        TX_Math.ContextMenuWrap menu2 = new TX_Math.ContextMenuWrap();
    }
}
