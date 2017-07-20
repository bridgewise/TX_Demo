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
    /// 说明对象有多个输入对话框的情况
    /// 
    ///   实现虚函数
    ///   GetEntityListName  --- 显示多个名称
    ///   GetEntityForm      --- 根据名称弹出不同对话框
    /// 
    ///=============================================================================================
    public class TX_Step03_EntityData : BaseEntity
    {
        #region ShowForm
        public override string GetEntityShowName()
        {
            return "我的构件02";
        }

        public override List<string> GetEntityListName()
        {
            List<string> listName = new List<string>();
            listName.Add("构造");
            listName.Add("钢筋");
            return listName;
        }

        public override int GetEntityShowIndex()
        {
            return 99002;
        }

        public override Form GetEntityForm(string nodeText)
        {
            if (nodeText == "构造")
            {
                TX_Step03_Form.entData = this;
                TX_Step03_Form dlg = new TX_Step03_Form();
                return dlg;
            }
            else if (nodeText == "钢筋")
            {
                return new Form();
            }
            return null;
        }
        #endregion


        public double value1 = 500;
        public double value2 = 501;
        public double value3 = 502;
        public double value4 = 503;


        public bool check1 = true;

        public int index1 = 1;
        public int index2 = 1;

        public List<double> list1 = new List<double>() { 50, 60 };
    }


}
