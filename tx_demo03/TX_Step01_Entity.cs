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
    /// 说明如何创建对象
    /// 
    ///   对象从 BaseEntity 上派生
    ///   
    ///   实现的虚函数：
    ///   GetEntityShowName --- 树结构上显示名称
    ///   GetEntityShowIndex -- 显示的次序
    ///   GetEntityForm    ---- 双击对象会弹出对话框
    ///=============================================================================================
    public class Step01_MyEntity : BaseEntity
    {
        #region ShowForm
        public override string GetEntityShowName()
        {
            return "我的构件033";
        }

        public override int GetEntityShowIndex()
        {
            return 99001;
        }


        public override List<string> GetEntityListName()
        {
            List<string> resNames = new List<string>();
            resNames.Add("Test1");
            resNames.Add("Test2");
            resNames.Add("Test3");
            return resNames;
        }

        public override Form GetEntityForm(string nodeText)
        {
            if (nodeText == "Test1")
            {
                TX_Step01_Form.data = this;
                TX_Step01_Form dlg = new TX_Step01_Form();
                return dlg;
            }
            else if (nodeText == "Test2")
            {
                TX_Step01_Form.data = this;
                TX_Step01_Form dlg = new TX_Step01_Form();
                return dlg;
            }
            else if (nodeText == "Test3")
            {
                TX_Step01_Form.data = this;
                TX_Step01_Form dlg = new TX_Step01_Form();
                return dlg;
            }
            return null;
        }

        //public override Form GetEntityForm(string nodeText)
        //{
        //    if (nodeText == this.GetEntityShowName())
        //    {
        //        Form dlg = new Form();
        //        return dlg;
        //    }
        //    return null;
        //}
        #endregion
    }



}
