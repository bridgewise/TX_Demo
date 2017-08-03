using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TX_Demo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TX_Step05_Form.entData = new TX_Step04_EntityData();
            TX_Step05_Form dlg = new TX_Step05_Form();
            dlg.ShowDialog();

        }
    }
}
