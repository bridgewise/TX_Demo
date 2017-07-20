using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;



namespace TX_TOOLBOX
{

    public class MyApp : IExtensionApplication
    {

        public MyApp()
        {
            //=========判断系统时间==============================
            if (WupiEngine.Wupi.CheckLicense(1) == false)
            {
                System.Windows.Forms.MessageBox.Show("没有找到软件狗!", "License Error VAEntity",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new System.Exception("");
            }

            if (DateTime.Now.Year >= 2018)
            {
                throw new System.Exception("该模块无法使用，请购买正版或联系技术支持。");
            }
            //=========判断系统时间==============================
        }

        public void Initialize()
        {
            if (WupiEngine.Wupi.CheckLicense(1) == false)
            {
                System.Windows.Forms.MessageBox.Show("没有找到软件狗!", "License Error VAEntity",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new System.Exception("");
            }

            //=========判断系统时间==============================
            if (DateTime.Now.Year >= 2018)
            {
                throw new System.Exception("该模块无法使用，请购买正版或联系技术支持。");
            }
            //=========判断系统时间==============================


            String curfile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String curDirectory = Path.GetDirectoryName(curfile);
            AppDomain.CurrentDomain.AppendPrivatePath(curDirectory);

            //=========提供输出信息==============================
            MessageLines.WriteErrorMessageToAcad("\n加载文件" + curfile);
            //=========提供输出信息==============================
        }

        public void Terminate()
        {
        }
    }
}

