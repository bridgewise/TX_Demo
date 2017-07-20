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
/// 说明显示对象数据
///
/// 
/// 
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step01_Form : Form,IFormOK
    {
        public TX_Step01_Form()
        {
            InitializeComponent();
        }

        #region IFormOK 成员

        public void ClickOK()
        {
            button1_Click(null, null);
        }

        #endregion


        public static Step01_MyEntity data = null;

        private void button1_Click(object sender, EventArgs e)
        {
            ///... ok
        }


    }
}
