using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace TX_Demo
{

    ///=============================================================================================
    /// 说明对象与界面相互交互
    /// 
    ///   
    ///   在对话框中设置静态对象，弹出对话框前将当前对象与对话框的静态对象相互连接起来
    ///=============================================================================================
    public class TX_Step02_EntityData : BaseEntity
    {

        public override string GetEntityShowName()
        {
            return "我的构件02";
        }

        public override int GetEntityShowIndex()
        {
            return 99002;
        }

        public override Form GetEntityForm(string nodeText)
        {
            if (nodeText == this.GetEntityShowName())
            {
                TX_Step02_Form.entData = this;
                TX_Step02_Form dlg = new TX_Step02_Form();
                return dlg;
            }
            return null;
        }


        #region 数据
        public double value1 = 500;
        public double value2 = 501;
        public double value3 = 502;
        public double value4 = 503;


        public bool check1 = true;

        public int index1 = 1;
        public int index2 = 1;

        public List<double> list1 = new List<double>() { 50, 60 };
        #endregion
    }


}
