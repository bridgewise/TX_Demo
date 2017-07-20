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
/// 
/// 
/// 1.显示数据
/// 
/// 
/// 
/// 2.保存和打开 
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



        }
    }
}
