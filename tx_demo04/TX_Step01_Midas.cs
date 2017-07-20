using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry;

///=============================================================================================
/// 说明示意图的制作和显示
///   
///   1.在cad中绘制好图形
///   2.加载示意图工具，设置ID，（sid命令）
///   3.用命令保存示意图(ill)
///   4.在生成的应用程序目录建一个TX_illPic文件夹，将示意图放在里面
/// 
///  
/// 
///  TX_TOOLBOX.AcadAssist.AttrachFormEvent(this, curEvent);   -------------- 将鼠标操作和相应函数关联起来，使示意图起作用
///  原则为：
///  
///   textBox1 对于ID为1，textBox2对应ID为2，textBox3对应ID为3 -- 以此类推
///   dataGridView1 的第一列为 1001，dataGridView1 的第二列为 1002  -- 以此类推
/// 
///=============================================================================================
namespace TX_Demo
{

    public partial class TX_Step01_Midas : Form
    {
        public TX_Step01_Midas()
        {
            InitializeComponent();


            //----------图形显示---------------------------
            m_Illustrator = new Illustrator();
            Size size = groupBox1.ClientSize;
            m_Illustrator.Size = size;
            groupBox1.Controls.Add(m_Illustrator);
            TX_TOOLBOX.IllustratorTool.LoadPic(m_Illustrator, "Wang");
            //----------图形显示---------------------------   




            /// 将鼠标操作和相应函数关联起来，使示意图起作用
            MouseEventHandler curEvent = new System.Windows.Forms.MouseEventHandler(this.textBox_MouseClick);
            TX_TOOLBOX.AcadAssist.AttrachFormEvent(this, curEvent);

        }

        Illustrator m_Illustrator = null;

        private void textBox_MouseClick(object sender, EventArgs e)
        {
            TX_TOOLBOX.IllustratorTool.Control_MouseClick(m_Illustrator, sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.Filter = "*.mct|*.mct";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StringBuilder builder = new StringBuilder();

                StructFEM_Midas fem_Midas = this.CreateMidas();
                fem_Midas.WriteMidasFile(builder);

                System.IO.TextWriter writer = new System.IO.StreamWriter(dlg.FileName, false, Encoding.Default);
                writer.Write(builder.ToString());
                writer.Close();
            }
        }

        public StructFEM_Midas CreateMidas()
        {
            StructFEM_Midas midas = new StructFEM_Midas();


            //------ 增加材料 ----            
            int mat1 = midas.AddMaterial(StructFEM_Midas.Material.CreateMaterialCon(1, "C50"));
            int mat2 = midas.AddMaterial(StructFEM_Midas.Material.CreateMaterialSteel(2, "ST"));

            //------ 增加截面 ----            
            int sect1 = midas.AddSection(MidasSectCreator.CreateSectValue_RectSectMM(1, "RECT", 2000, 1000, StructFEM_Midas.SectOffset.CT));
            //int iSec_Temp = fem_Midas.AddSectionFree_AutoIndex(MidasSect_FreeCreator.CreateMidasSect_FreeCreatorMM(fem_Midas.indexNewSect(), "桥面板" + (i + 1).ToString(), deckCuruves[i], deckSlab[i].WL, 0));


            Point3d pt1 = new Point3d(0, 0, 0);
            Point3d pt2 = new Point3d(20000, 0, 0);
            TxLine line = new TxLine(pt1, pt2);
            List<Point3d> pts = AcadAssist.SplitCurveByCount(line, 10);
            //List<Point3d> pts2 = AcadAssist.
            for (int i = 0; i < pts.Count; i++)
            {
                midas.AddNodeMM(pts[i], pts[i]);
            }

            int elementCount = pts.Count - 1;
            for (int i = 0; i < pts.Count - 1; i++)
            {
                /// iPro--截面号,iMat--材料号[都是从1开始的]
                midas.AddElement2(i + 1, i + 2, sect1, mat1);
            }



            midas.AddStaticLoadCase("一期恒载", "CS");  //施工阶段荷载
            midas.AddStaticLoadCase("预应力", "CS");    //施工阶段荷载
            midas.AddStaticLoadCase("二期恒载", "CS");  //施工阶段荷载
            midas.AddStaticLoadCase("整体升温", "T");
            midas.AddStaticLoadCase("整体降温", "T");
            midas.AddStaticLoadCase("正温度梯度", "TPG");
            midas.AddStaticLoadCase("负温度梯度", "TPG");

            // -- 增加结构组
            String nodeGrop1 = String.Format("{0} to {1}", 1, pts.Count);
            String elementGrop1 = String.Format("{0} to {1}", 1, pts.Count - 1);
            midas.AddElementGroup("单元", nodeGrop1, elementGrop1);


            //--- 增加边界
            midas.AddConstraint("支座", StructFEM_Midas.Constraint.SupportType.Z_Fix, 1, pts.Count);

            for (int i = 1; i <= elementCount; i++)
            {
                midas.AddBeamLoad(i, 100, "二期", "二期恒载");
            }
            midas.AddNodeLoad(1, 0, 0, 10, "二期", "二期恒载");


            midas.AddElasticLink(1, 2, "支座");

            midas.AddStage("ST1", 10, "单元", "支座", "");


            ///---- 移动荷载 [加车道，加汽车荷载] ------
            double Vel_LSPAN = 30;
            string curLanName1 = "行车道线1";
            string curLanName2 = "行车道线2";
            midas.AddVechiLine(0, Vel_LSPAN, 1, 1, elementCount, curLanName1);
            midas.AddVechiLine(0, Vel_LSPAN, 1, 1, elementCount, curLanName2);

            List<String> Vel_LanName = new List<string>();
            Vel_LanName.Add(curLanName1);
            Vel_LanName.Add(curLanName2);

            midas.AddVechiLoadCase("汽车荷载工况", new List<int>() { Vel_LanName.Count }, Vel_LanName);



            return midas;
        }

        ////public static MidasSect_FreeCreator CreateSection(int isect, string name, SteelBeamDM sect, double topSlabH)
        ////{
        ////    MidasSect_FreeCreator msect = new MidasSect_FreeCreator();
        ////    msect.iSec = isect;
        ////    msect.name = name;
        ////    msect.sectOffset = StructFEM_Midas.SectOffset.CT;
        ////    msect.enSectType = MidasSect_FreeCreator.SectType.VALUE;
        ////    msect.IsByOffset = true;
        ////    //msect.offsetX = -sect.bL_SumW * 0.001 * 0.5;
        ////    //msect.offsetY = -sect.H * 0.001 - 0.3;
        ////    msect.offsetX = +sect.bL_SumW * 0.001 * 0.5;
        ////    msect.offsetY = -topSlabH * 0.001;
        ////
        ////    /// -------------------------
        ////    Point3d[] sidWeb = sect.GetSidWebPoints();
        ////    List<Point3d> downRibPnts = sect.GetDownRibPoints();
        ////    TxPolyline pl = new TxPolyline();
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[0].X, sidWeb[0].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[1].X, sidWeb[1].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[1].X - sect.downExt, sidWeb[1].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[1].X - sect.downExt, sidWeb[1].Y - sect.ist_dwn.Thick), 0, 0, 0);
        ////    for (int i = 0; i < downRibPnts.Count; i++)
        ////    {
        ////        ////pl.AddVertexAt(pl.NumberOfVertices, new Point2d(downRibPnts[i].X - 0.5 * sect.ist_dwn_rib.Thick, downRibPnts[i].Y), 0, 0, 0);
        ////        ////pl.AddVertexAt(pl.NumberOfVertices, new Point2d(downRibPnts[i].X - 0.5 * sect.ist_dwn_rib.Thick, downRibPnts[i].Y + sect.ist_dwn_rib.Width), 0, 0, 0);
        ////        ////pl.AddVertexAt(pl.NumberOfVertices, new Point2d(downRibPnts[i].X + 0.5 * sect.ist_dwn_rib.Thick, downRibPnts[i].Y + sect.ist_dwn_rib.Width), 0, 0, 0);
        ////        ////pl.AddVertexAt(pl.NumberOfVertices, new Point2d(downRibPnts[i].X + 0.5 * sect.ist_dwn_rib.Thick, downRibPnts[i].Y), 0, 0, 0);
        ////    }
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[3].X + sect.downExt, sidWeb[3].Y - sect.ist_dwn.Thick), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[3].X + sect.downExt, sidWeb[3].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[3].X, sidWeb[3].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[2].X, sidWeb[2].Y), 0, 0, 0);
        ////    double wFbxL = sect.ist_fbs.Thick * Math.Sqrt(1 + sect.GetVTangFbL() * sect.GetVTangFbL());
        ////    double wFbxR = sect.ist_fbs.Thick * Math.Sqrt(1 + sect.GetVTangFbR() * sect.GetVTangFbR());
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[2].X - wFbxR, sidWeb[2].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[3].X - wFbxR, sidWeb[3].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[1].X + wFbxL, sidWeb[1].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[0].X + wFbxL, sidWeb[0].Y), 0, 0, 0);
        ////    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(sidWeb[0].X, sidWeb[0].Y), 0, 0, 0);
        ////    /// -------------------------
        ////
        ////    //pl.Displacement(new Vector3d(0, sect.H * 0.5 - 300, 0));
        ////
        ////    msect.outerShape = pl;
        ////
        ////    return msect;
        ////}
    }
}
