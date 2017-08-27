using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry2;

namespace TX_Demo
{

    ///=============================================================================================
    /// 钢筋对象数据显示在界面上
    /// 
    /// 1.钢筋表格用于修改钢筋类型和钢筋直径
    ///=============================================================================================
    public class TX_Step03_SteelDM : BaseBlock
    {


        public double value1 = 500;
        public double value2 = 501;
        public double value3 = 502;
        public double value4 = 503;


        public bool check1 = true;

        public int index1 = 1;
        public int index2 = 1;

        public List<double> list1 = new List<double>() { 50, 60 };

        public override void Draw(DatabaseToAcad block)
        {
            block.AddCircle(new Point3d(0, 0, 0), 100, CommonLayer.zerolayer);

        }
    }


}
