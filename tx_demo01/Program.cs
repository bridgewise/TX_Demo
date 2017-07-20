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

            TX_Step01_Form dlg = new TX_Step01_Form();
            dlg.ShowDialog();

        }
    }
}
