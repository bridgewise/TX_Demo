using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.Geometry;
using TX_Math;
using TX_TOOLBOX.SteelSlab;
using Autodesk.AutoCAD.Runtime;

namespace TX_TOOLBOX
{



    /// <summary>
    /// 钢组合梁断面桥面板
    /// </summary>
    public class TopConcreteSlab : BaseBlock
    {

        public TopConcreteSlab()
            : base("钢组合梁断面桥面板")
        {

        }

        //public SteelBeamWrap sbWrap = null;
        public bool IsRemoveDimObject = false;
        public double Side = 0;       //桥宽和实际桥宽的差值
        public double WL = 1550;        //翼缘长
        public double WR = 1550;        //翼缘长
        public double B1 = 150;         //翼缘加厚段的长度
        public double B2 = 100;         //翼缘加厚过渡段的长度
        public double B3 { get { return WL - B1 - B2 - B4; } }         //翼缘加厚过渡段的长度2
        public double B4 = 400;         //钢混交界处焊钉段的长度
        public double IB1 = 300;         //钢混交界处过渡段的长度
        public double T1 = 220;         //翼缘加厚段的厚度
        public double T2 = 180;         //翼缘最薄处的厚度
        public double H1 = 340;         //钢混交界处桥面板厚
        public double H2 = 240;          //非钢混交界处桥面板厚
        public double bridgeWidthL = 20000; //桥宽
        public double bridgeWidthR = 20000; //桥宽
        public double bridgeWidth { get { return bridgeWidthL + bridgeWidthR; } }
        public double BeamW = 3800;//{ get { return sbWrap.steelBeam.NormalSect.bL_SumW; } }         //单片梁宽
        public bool IsOutSideL = true;
        public bool IsOutSideR = true;
        public bool IsByUserBeamPoint = false;
        public List<Point3d> user_beamPosition = new List<Point3d>();
        public void SetBeamPosition(bool isByUser, List<Point3d> pnts) { IsByUserBeamPoint = isByUser; user_beamPosition.Clear(); user_beamPosition.AddRange(pnts); }
        #region Clone
        public object Clone()
        {
            TopConcreteSlab newObj = (TopConcreteSlab)this.MemberwiseClone();
            return newObj;
        }
        #endregion


        public List<Point3d> GetBeamSectPosition()
        {
            if (IsByUserBeamPoint && user_beamPosition.Count > 0)
            {
                return new List<Point3d>(user_beamPosition);
            }
            else
            {
                return AutoBeamSectPosition(bridgeWidthL, bridgeWidthR, BeamW, WL);
            }
        }

        [CommandMethod("lyqtest")]
        public void TestDrawTopMethod()
        {
            if (!StyleInit.StyleInitMethod()) return;



            PaperLayout paper = new PaperLayout(100);
            TopConcreteSlab block = new TopConcreteSlab();
            //block.bridgeWidth = 20000;
            //block.BeamW = 3200;


            paper.AddBlock(block);

            ExportToCAD.ExportToAutoCAD_NotUserInteraction(paper);
        }

        public TxPolyline GetTopConcreteSlab()
        {
            TxPolyline resPolyline = new TxPolyline();

            //                   B
            //     |--------------------------------|                                              |---------------|
            //    
            //    1|-----------------------------------------------------------------------------------------------|
            //     |    T2                                                                                         |
            //  T1 |     _____                                                                H1                   |  H2
            //     |____/4    -----_____                       __________________________             _____________|
            //    2    3                -----_____            /7                         \           /           
            //                                  -----________/                            \_________/            
            //                                       5    B3   6                                                 
            //      B1  B2                                   B4
            //     |---|--|-----------B3-------------|--B4---|--|                                
            //

            List<Point3d> beamPostion = GetBeamSectPosition();
            System.Diagnostics.Debug.Assert(beamPostion.Count > 0);

            //GetBeamSectPosition()得到的位置不准确，还需要修改，以下先用确定值代替

            //List<Point3d> beamPostion = new List<Point3d>();
            //beamPostion.add(new Point3d())

            List<Point3d> connectWebPnts = new List<Point3d>();
            for (int i = 0; i < beamPostion.Count; i++)
            {
                connectWebPnts.Add(new Point3d(beamPostion[i].X - BeamW * 0.5, beamPostion[i].Y, beamPostion[i].Z));
                connectWebPnts.Add(new Point3d(beamPostion[i].X + BeamW * 0.5, beamPostion[i].Y, beamPostion[i].Z));
            }


            //左翼缘端部的桥面板轮廓点
            Point3d pnt1 = new Point3d(-bridgeWidthL + Side, H1, 0);
            Point3d pnt2 = new Point3d(-bridgeWidthL + Side, H1 - T1, 0);
            Point3d pnt3 = new Point3d(-bridgeWidthL + Side + B1, H1 - T1, 0);
            Point3d pnt4 = new Point3d(-bridgeWidthL + Side + B1 + B2, H1 - T2, 0);
            //左翼缘根部的桥面板轮廓点
            Point3d pnt5 = new Point3d(connectWebPnts[0].X - B4 * 0.5, 0, 0);
            Point3d pnt6 = new Point3d(connectWebPnts[0].X + B4 * 0.5, 0, 0);
            Point3d pnt7 = new Point3d(connectWebPnts[0].X + B4 * 0.5 + IB1, H1 - H2, 0);
            //右翼缘根部的桥面板轮廓点
            Point3d pnt8 = new Point3d(connectWebPnts[connectWebPnts.Count - 1].X - B4 * 0.5 - IB1, H1 - H2, 0);
            Point3d pnt9 = new Point3d(connectWebPnts[connectWebPnts.Count - 1].X - B4 * 0.5, 0, 0);
            Point3d pnt10 = new Point3d(connectWebPnts[connectWebPnts.Count - 1].X + B4 * 0.5, 0, 0);
            //右翼缘端部的桥面板轮廓点
            Point3d pnt11 = new Point3d(bridgeWidthR - Side - B1 - B2, H1 - T2, 0);
            Point3d pnt12 = new Point3d(bridgeWidthR - Side - B1, H1 - T1, 0);
            Point3d pnt13 = new Point3d(bridgeWidthR - Side, H1 - T1, 0);
            Point3d pnt14 = new Point3d(bridgeWidthR - Side, H1, 0);    ///------ 1点对称

            List<Point3d> concreteSlabPnts = new List<Point3d>();
            if (IsOutSideL)
            {
                concreteSlabPnts.Add(pnt1);
                concreteSlabPnts.Add(pnt2);
                concreteSlabPnts.Add(pnt3);
                concreteSlabPnts.Add(pnt4);
                concreteSlabPnts.Add(pnt5);
                concreteSlabPnts.Add(pnt6);
                concreteSlabPnts.Add(pnt7);
            }
            else
            {
                concreteSlabPnts.Add(pnt1);
                concreteSlabPnts.Add(new Point3d(pnt1.X, pnt1.Y - H2, 0));
            }

            for (int i = 0; i < connectWebPnts.Count; i++)
            {
                if (i == 0 && IsOutSideL == true) continue;
                if (i == connectWebPnts.Count - 1 && IsOutSideR == true) continue;

                concreteSlabPnts.Add(new Point3d(connectWebPnts[i].X - B4 * 0.5 - IB1, H1 - H2, 0));
                concreteSlabPnts.Add(new Point3d(connectWebPnts[i].X - B4 * 0.5, 0, 0));
                concreteSlabPnts.Add(new Point3d(connectWebPnts[i].X + B4 * 0.5, 0, 0));
                concreteSlabPnts.Add(new Point3d(connectWebPnts[i].X + B4 * 0.5 + IB1, H1 - H2, 0));
            }

            if (IsOutSideR)
            {
                concreteSlabPnts.Add(pnt8);
                concreteSlabPnts.Add(pnt9);
                concreteSlabPnts.Add(pnt10);
                concreteSlabPnts.Add(pnt11);
                concreteSlabPnts.Add(pnt12);
                concreteSlabPnts.Add(pnt13);
                concreteSlabPnts.Add(pnt14);
            }
            else
            {
                concreteSlabPnts.Add(new Point3d(pnt14.X, pnt14.Y - H2, 0));
                concreteSlabPnts.Add(pnt14);
            }


            for (int i = 0; i < concreteSlabPnts.Count; i++)
            {
                resPolyline.AddVertexAt(resPolyline.NumberOfVertices, new Point2d(concreteSlabPnts[i].X, concreteSlabPnts[i].Y), 0, 0, 0);
            }
            resPolyline.AddVertexAt(resPolyline.NumberOfVertices, new Point2d(concreteSlabPnts[0].X, concreteSlabPnts[0].Y), 0, 0, 0);



            return resPolyline;
        }

        /// <summary>
        /// 将一块桥面板分割成多块桥面板
        /// </summary>
        public List<TopConcreteSlab> GetSeparateDeck()
        {
            List<Point3d> beamPoints = this.GetBeamSectPosition();
            beamPoints.Insert(0, new Point3d(-bridgeWidthL, 0, 0));
            beamPoints.Add(new Point3d(+bridgeWidthR, 0, 0));

            List<TopConcreteSlab> topDecks = new List<TopConcreteSlab>();
            for (int i = 1; i < beamPoints.Count - 1; i++)
            {
                TopConcreteSlab oneDeck = (TopConcreteSlab)this.Clone();
                oneDeck.user_beamPosition.Clear();

                oneDeck.bridgeWidthL = ((i == 1) ? 1 : 0.5) * (beamPoints[i].X - beamPoints[i - 1].X);
                oneDeck.bridgeWidthR = ((i == beamPoints.Count - 2) ? 1 : 0.5) * (beamPoints[i + 1].X - beamPoints[i].X);
                oneDeck.IsOutSideL = (i == 1) ? true : false;
                oneDeck.IsOutSideR = (i == beamPoints.Count - 2) ? true : false;
                oneDeck.WL = oneDeck.bridgeWidthL - oneDeck.BeamW * 0.5;
                oneDeck.WR = oneDeck.bridgeWidthR - oneDeck.BeamW * 0.5;
                oneDeck.SetBeamPosition(true, new List<Point3d>() { new Point3d(0, 0, 0) });
                topDecks.Add(oneDeck);
            }
            return topDecks;
        }

        public List<TxPolyline> GetSeparateDeck_Polylines()
        {
            List<TopConcreteSlab> topDecks = this.GetSeparateDeck();

            List<TxPolyline> resPolyline = new List<TxPolyline>();
            for (int i = 0; i < topDecks.Count; i++)
            {
                resPolyline.Add(topDecks[i].GetTopConcreteSlab());
            }
            return resPolyline;
        }

        /// <summary>
        /// XBW -- 外侧悬臂尺寸
        /// </summary>
        public static List<Point3d> AutoBeamSectPosition(double bridgeWidthL, double bridgeWidthR, double BeamW, double XBW)
        {
            double deckW = bridgeWidthL + bridgeWidthR;

            if (deckW < 5000) { System.Windows.Forms.MessageBox.Show("桥面过窄，请修改尺寸"); return new List<Point3d>(); }

            int cntBeam = Convert.ToInt32(deckW / (BeamW + BeamW));

            if (cntBeam < 1) { System.Windows.Forms.MessageBox.Show("桥面过窄，请修改尺寸"); return new List<Point3d>(); }

            List<Point3d> beamPosition = new List<Point3d>();
            if (cntBeam == 1)
            {
                beamPosition.Add(new Point3d((-bridgeWidthL + bridgeWidthR) * 0.5, 0, 0));
            }
            else
            {
                double real_beamMidW = (deckW - (XBW * 2 + BeamW)) / (cntBeam - 1) - BeamW;
                double wMidBeam = -0.5 * (cntBeam * BeamW + (cntBeam - 1) * real_beamMidW) + BeamW * 0.5;
                double wL2 = -bridgeWidthL + XBW + BeamW * 0.5;
                for (int i = 0; i < cntBeam; i++)
                {
                    beamPosition.Add(new Point3d(wMidBeam, 0, 0));
                    wMidBeam += BeamW + real_beamMidW;
                }
            }
            return beamPosition;
        }


        public List<Point3d> GetBeamSectWithBorderPosition()
        {
            List<Point3d> res = new List<Point3d>();

            Point3d sideL = new Point3d(-bridgeWidth * 0.5, 0, 0);
            Point3d sideR = new Point3d(+bridgeWidth * 0.5, 0, 0);
            res.Add(sideL);
            res.AddRange(this.GetBeamSectPosition());
            res.Add(sideR);
            return res;
        }

        public override void Draw(DatabaseToAcad block)
        {

            TxPolyline topSlab = this.GetTopConcreteSlab();
            block.AddCurve(topSlab, CommonLayer.gz1layer);


            ///标题
            if (CommonSimbel.IsDownTitle)
            {
                Point3d titlePoint = new Point3d(0, -H1 - 5 * block.style.DimFirstLayer, 0);
                block.AddTitle(titlePoint, BlockName);
            }
            else
            {
                Point3d titlePoint = new Point3d(0, 8 * block.style.DimFirstLayer, 0);
                block.AddTitle(titlePoint, BlockName);
            }


            if (IsRemoveDimObject)
            {
                block.RemoveDimLayer();
                for (int i = block.result2.Count - 1; i >= 0; i--)
                {
                    if (block.result2[i] is DimHanFen)
                    {
                        block.result2.RemoveAt(i);
                    }
                    if (block.result2[i] is TextDim)
                    {
                        block.result2.RemoveAt(i);
                    }
                    if (block.result2[i] is TextDim2)
                    {
                        block.result2.RemoveAt(i);
                    }

                }
            }
        }




    }
}
