using System.Collections.Generic;
using SAP2000v20;
using SapPrimitives.FramePrimitives;
using SapPrimitives.PointPrimitives;
using SapPrimitives.GeneralizedPrimitives;
namespace SapPrimitives.FactorySetting
{
    /// <summary>
    /// 
    /// </summary>
    public class Factory
    {
        private SapModel sapModel;
        private SapPoint[] startCo = new SapPoint[4];
        private SapPoint[] endCo = new SapPoint[4];
        private SapNonePrismaticElement[] colArr1;
        private SapNonePrismaticElement[] colArr2;
        private SapNonePrismaticElement[] beamArr1;
        private SapNonePrismaticElement[] beamArr2;
        private SapSteelSection sec1;
        private SapSteelSection sec2;
        private InsertionProperties colProp;
        private SapNonPrismaticSection colSection;
        private SapNonPrismaticSection beamSection;
        private InsertionProperties beamProp;
        private SapMaterial ST;
        private SapLoadPattern[] loadPatterns;
        private SapFrameDistLoad[] distributedLoads;
        private bool isSafe;
        private double steelWeight;
        private List<string> factoryNames;

        double maxLength;
        int framesNumber;
        double[] spacingArr;
        double height;
        double spacing;

        public bool IsSafe { get => isSafe; set => isSafe = value; }
        public double SteelWeight => steelWeight;

        public List<string> FactoryNames { get => factoryNames;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapModel"></param>
        /// <param name="sec1"></param>
        /// <param name="sec2"></param>
        /// <param name="spacing"></param>
        /// <param name="height"></param>
        /// <param name="maxLength"></param>
        public Factory(SapModel sapModel, SapSteelSection sec1, SapSteelSection sec2, double spacing, double height, double maxLength = 100)
        {
            startCo = new SapPoint[4];
            endCo = new SapPoint[4];
            this.sec1 = sec1;
            this.sec2 = sec2;
            this.ST = sec2.Material;
            this.sapModel = sapModel;
            factoryNames = new List<string>();
            beamProp = new InsertionProperties(sec1, sec2, InsertionType.Beam);
            colProp = new InsertionProperties(sec1, sec2, InsertionType.Column);
            beamSection = new SapNonPrismaticSection(sapModel.SapObjectModel, "B-Section", beamProp);
            colSection = new SapNonPrismaticSection(sapModel.SapObjectModel, "C-Section", colProp);
            this.maxLength = maxLength;
            this.spacingArr = CalculateFrameArray(spacing);
            this.framesNumber = this.spacingArr.Length + 1;
            this.height = height;
            this.spacing = spacing;
            InitializeFirstSectionDim(height);
            double comX = 0;
            colArr1 = new SapNonePrismaticElement[framesNumber];
            colArr2 = new SapNonePrismaticElement[framesNumber];
            beamArr1 = new SapNonePrismaticElement[framesNumber];
            beamArr2 = new SapNonePrismaticElement[framesNumber];
            loadPatterns = new SapLoadPattern[2];
            loadPatterns[0] = new SapLoadPattern(sapModel.SapObjectModel, "SuperImposed", LoadPatternType.Dead, 0, true);
            loadPatterns[1] = new SapLoadPattern(sapModel.SapObjectModel, "Live", LoadPatternType.Live, 0, true);
            distributedLoads = new SapFrameDistLoad[2];
            distributedLoads[0] = new SapFrameDistLoad(loadPatterns[0], 1, 10, 0, 1, 3.125 * spacing * 0.06, 3.125 * spacing * 0.06);
            distributedLoads[1] = new SapFrameDistLoad(loadPatterns[1], 1, 10, 0, 1, 3.125 * spacing * 0.06, 3.125 * spacing * 0.06);
            for (int i = 0; i < framesNumber; i++)
            {
                colArr1[i] = new SapNonePrismaticElement(sapModel.SapObjectModel, startCo[0], endCo[0], colSection, "", "Col1" + i);
                beamArr1[i] = new SapNonePrismaticElement(sapModel.SapObjectModel, startCo[1], endCo[1], beamSection, "", "Beam1" + i);
                beamArr2[i] = new SapNonePrismaticElement(sapModel.SapObjectModel, startCo[2], endCo[2], beamSection, "", "Beam2" + i);
                colArr2[i] = new SapNonePrismaticElement(sapModel.SapObjectModel, startCo[3], endCo[3], colSection, "", "Col2" + i);

                factoryNames.Add("Col1" + i);
                factoryNames.Add("Beam1" + i);
                factoryNames.Add("Beam2" + i);
                factoryNames.Add("Col2" + i);
                sapModel.RefreshView();
                startCo[0].SetRestraint(RestrainCase.Pinned);
                startCo[3].SetRestraint(RestrainCase.Pinned);

                sapModel.RefreshView();


                if (i != framesNumber - 1)
                {
                    comX += spacingArr[i];

                    startCo[0] = startCo[0].AddXYZ(sapModel, comX, Direction.Y);
                    startCo[1] = startCo[1].AddXYZ(sapModel, comX, Direction.Y);
                    startCo[2] = startCo[2].AddXYZ(sapModel, comX, Direction.Y);
                    startCo[3] = startCo[3].AddXYZ(sapModel, comX, Direction.Y);
                    endCo[0] = endCo[0].AddXYZ(sapModel, comX, Direction.Y);
                    endCo[1] = endCo[1].AddXYZ(sapModel, comX, Direction.Y);
                    endCo[2] = endCo[2].AddXYZ(sapModel, comX, Direction.Y);
                    endCo[3] = endCo[3].AddXYZ(sapModel, comX, Direction.Y);
                }
            }
            SapSteelElement[] purlinArr = DrawPurlinCollection("Purlin");
            for (int i = 0; i < purlinArr.Length; i++)
            {
                purlinArr[i].AddDistributedLoad(this.distributedLoads[0]);
                purlinArr[i].AddDistributedLoad(this.distributedLoads[1]);
            }
            RetireveModelInformation();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteAll()
        {
            sapModel.SapObjectModel.SelectObj.All();
            sapModel.SapObjectModel.SetModelIsLocked(false);
            sapModel.SapObjectModel.FrameObj.Delete("", eItemType.SelectedObjects);
            sapModel.SapObjectModel.PointObj.DeleteRestraint("", eItemType.SelectedObjects);
            sapModel.SapObjectModel.PointObj.DeleteSpecialPoint("", eItemType.SelectedObjects);
            sapModel.RefreshView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void RetireveModelInformation()
        {
            bool isSafe = true;
            string[] frameName = new string[5];
            double[] ratio = new double[5];
            int[] ratioType = new int[6];
            double[] location = new double[5];
            string[] comboName = new string[5];
            string[] errorSummary = new string[5];
            string[] warningSummary = new string[5];
            int frameNumberItems = 1;
            sapModel.RunAnalysis();
            sapModel.SapObjectModel.DesignSteel.StartDesign();
            for (int i = 0; i < factoryNames.Count; i++)
            {
                sapModel.SapObjectModel.DesignSteel.GetSummaryResults(factoryNames[i], ref frameNumberItems, ref frameName, ref ratio, ref ratioType, ref location, ref comboName, ref errorSummary, ref warningSummary);
                if (ratio[0] > 1)
                {
                    isSafe = false;
                }
            }
            this.isSafe = isSafe;
            GetSteelWeight();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        public void InitializeFirstSectionDim(double height)
        {
            startCo[0] = new SapPoint(sapModel.SapObjectModel, 0, 0, 0);
            endCo[0] = new SapPoint(sapModel.SapObjectModel, 0, 0, height);

            startCo[1] = new SapPoint(sapModel.SapObjectModel, 0, 0, height);
            endCo[1] = new SapPoint(sapModel.SapObjectModel, 12.5, 0, height + 1.25);

            startCo[2] = new SapPoint(sapModel.SapObjectModel, 25, 0, height);
            endCo[2] = new SapPoint(sapModel.SapObjectModel, 12.5, 0, height + 1.25);

            startCo[3] = new SapPoint(sapModel.SapObjectModel, 25, 0, 0);
            endCo[3] = new SapPoint(sapModel.SapObjectModel, 25, 0, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private SapSteelElement[] DrawPurlinCollection(string name)
        {
            startCo[0] = startCo[0].AddXYZ(sapModel, 0, Direction.Y);
            endCo[0] = endCo[0].AddXYZ(sapModel, 0, Direction.Y);
            endCo[1] = endCo[1].AddXYZ(sapModel, 0, Direction.Y);
            startCo[2] = startCo[2].AddXYZ(sapModel, 0, Direction.Y);
            startCo[3] = startCo[3].AddXYZ(sapModel, 0, Direction.Y);
            SapSteelSection purlinSection = new SapSteelSection(sapModel.SapObjectModel, ST, 0.15, 0.15, 0.02, 0.01, 0.02, 0.15, "P-Section");

            SapPoint p1 = startCo[0];
            SapPoint p2 = endCo[0];
            SapPoint p3 = endCo[1];
            SapPoint p4 = startCo[2];
            SapPoint p5 = startCo[3];

            SapPoint pUpMidLeft = MidPoint(p2, p3);
            SapPoint pUpMidRight = MidPoint(p3, p4);
            SapPoint pBeforeLeft = MidPoint(p2, pUpMidLeft);
            SapPoint pAfterLeft = MidPoint(p3, pUpMidLeft);
            SapPoint pBeforeRight = MidPoint(p3, pUpMidRight);
            SapPoint pAfterRight = MidPoint(p4, pUpMidRight);

            double cumulativeSpacing = 0;
            double prevCumulativeSpacing = 0;
            List<SapSteelElement> elementsList = new List<SapSteelElement>();
            SapPoint pMidLeft = new SapPoint(sapModel.SapObjectModel, (p1.X + p2.X) / 2, 0, (p1.Z + p2.Z) / 2);
            SapPoint pMidRight = new SapPoint(sapModel.SapObjectModel, (p4.X + p5.X) / 2, 0, (p4.Z + p5.Z) / 2);

            List<SapPoint> purlinPointsList = new List<SapPoint>() { p2, p3, p4, pBeforeLeft, pUpMidLeft, pUpMidRight, pAfterLeft, pBeforeRight, pAfterRight };
            for (int i = 0; i < framesNumber - 1; i++)
            {
                cumulativeSpacing += spacingArr[i];
                for (int j = 0; j < purlinPointsList.Count; j++)
                {
                    elementsList.Add(DrawPurlin(purlinPointsList[j].AddXYZ(sapModel, prevCumulativeSpacing, Direction.Y), purlinPointsList[j].AddXYZ(sapModel, cumulativeSpacing, Direction.Y), name + j.ToString() + i, purlinSection));
                }
                DrawPurlin(pMidLeft.AddXYZ(sapModel, prevCumulativeSpacing, Direction.Y), pMidLeft.AddXYZ(sapModel, cumulativeSpacing, Direction.Y), name, purlinSection);
                DrawPurlin(pMidRight.AddXYZ(sapModel, prevCumulativeSpacing, Direction.Y), pMidRight.AddXYZ(sapModel, cumulativeSpacing, Direction.Y), name, purlinSection);
                prevCumulativeSpacing += spacingArr[i];
                sapModel.RefreshView();
            }
            return elementsList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="frameName"></param>
        /// <returns></returns>
        private SapSteelElement DrawPurlin(SapPoint p1, SapPoint p2, string frameName, SapSteelSection purlinSection)
        {
            var purlin = new SapSteelElement(sapModel.SapObjectModel, p1, p2, purlinSection,"", frameName);
            return purlin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private SapPoint MidPoint(SapPoint p1, SapPoint p2)
        {
            return new SapPoint(sapModel.SapObjectModel, (p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spacing"></param>
        /// <returns></returns>
        private double[] CalculateFrameArray(double spacing)
        {
            double overflow;
            int frameNumber = (int)(maxLength / spacing) + 1;
            if (maxLength % spacing == 0)
            {
                overflow = 0;
            }
            else
            {
                overflow = (maxLength - (spacing * frameNumber - 1)) / 2;
            }
            double[] result = new double[frameNumber - 1];
            for (int i = 0; i < frameNumber - 1; i++)
            {
                if (i == 0 || i == frameNumber - 2)
                {
                    result[i] = spacing + overflow;
                }
                else
                {
                    result[i] = spacing;
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        private void GetSteelWeight()
        {

            int numberOfResults = 0;
            string[] loadCase = new string[0];
            string[] stepType = new string[0];
            double[] stepNum = new double[0];
            double[] fx = new double[0];
            double[] fy = new double[0];
            double[] fz = new double[0];
            double[] mx = new double[0];
            double[] my = new double[0];
            double[] mz = new double[0];
            double[] f1 = new double[0];
            double[] f2 = new double[0];
            double[] f3 = new double[0];
            double gx = 0;
            double gy = 0;
            double gz = 0;


            sapModel.SapObjectAPI.SapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();

            sapModel.SapObjectAPI.SapModel.Results.Setup.SetCaseSelectedForOutput("DEAD");

            sapModel.SapObjectAPI.SapModel.Results.BaseReact(ref numberOfResults, ref loadCase, ref stepType, ref stepNum, ref fx, ref fy, ref fz, ref mx, ref my, ref mz, ref gx, ref gy, ref gz);

            steelWeight = fz[0];
        }
    }
}








