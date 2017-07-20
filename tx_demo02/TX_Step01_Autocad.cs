using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TX_TOOLBOX;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;


namespace TX_Demo
{
    ///=============================================================================================
    /// 说明用基本的CAD方法绘制图形
    ///
    /// 用纯CAD的方法来实现，cad的其他操作，请参考其官方的帮助文档和例子[可在CAD网站免费下载]
    /// 
    ///=============================================================================================
    public class Step01_Command : BaseBlock
    {

        [CommandMethod("tx_step01")]
        public static void TestDrawCommond()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;

            ///--- 向CAD输出命令
            ed.WriteMessage("aaa");


            PromptEntityResult prEntityRs = ed.GetEntity("选择线");
            if (prEntityRs.Status != PromptStatus.OK) return;





            // 开启一个事务,在CAD中要做什么事情是通过事务来完成的，这个事务做了添加线到模型空间
            // 当Commit时向CAD提交这个事务，提交的事务可能成功，但是也会失败，这里对失败没有做处理
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //  取得模型空间
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);


                Line ln1 = (Line)trans.GetObject(prEntityRs.ObjectId, OpenMode.ForRead);

                //  生成一个线
                Line ln2 = new Line(new Point3d(0, 0, 0), new Point3d(500, 500, 0));

                Circle c = new Circle(ln1.StartPoint, Vector3d.ZAxis, 300);

                //  加入到CAD中去
                btr.AppendEntity(ln2);
                trans.AddNewlyCreatedDBObject(ln2, true);

                btr.AppendEntity(c);
                trans.AddNewlyCreatedDBObject(c, true);


                trans.Commit();
            }
        }
    }



}
