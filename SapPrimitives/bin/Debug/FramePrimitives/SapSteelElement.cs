namespace SapPrimitives.FramePrimitives
{
    using System.Collections.Generic;
    using SAP2000v20;
    using SapPrimitives.PointPrimitives;

    /// <summary>
    /// 
    /// </summary>
    public class SapSteelElement
    {

        private SapPoint point1;
        private SapPoint point2;
        private SapSteelSection steelSection;
        private string label;
        private string name;
        private List<SapFrameDistLoad> distibutedLoads;
        private cSapModel mymodel;




        public SapPoint Point1 { get => point1; set => point1 = value; }
        public SapPoint Point2 { get => point2; set => point2 = value; }
        public string Label { get => label; set => label = value; }
        public string Name { get => name; set => name = value; }
        internal SapSteelSection SteelSection { get => steelSection; set => steelSection = value; }
        public cSapModel Mymodel { get => mymodel; set => mymodel = value; }
        internal List<SapFrameDistLoad> DistibutedLoads { get => distibutedLoads; set => distibutedLoads = value; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mymodel"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="section"></param>
        /// <param name="label"></param>
        /// <param name="framename"></param>
        public SapSteelElement(cSapModel mymodel, SapPoint point1, SapPoint point2, SapSteelSection section, string label, string framename)
        {
            this.mymodel = mymodel;
            this.point1 = point1;
            this.point2 = point2;
            this.steelSection = section;
            this.label = label;
            this.name = framename;
            this.distibutedLoads = new List<SapFrameDistLoad>();
            this.mymodel.FrameObj.AddByPoint(this.point1.Name, this.point2.Name, ref this.label, this.steelSection.Name, this.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributedload"></param>
        public void AddDistributedLoad(SapFrameDistLoad distributedload)
        {
            this.distibutedLoads.Add(distributedload);
            int check = this.mymodel.FrameObj.SetLoadDistributed(this.name, distributedload.LoadPattern.Name, distributedload.Type, distributedload.Direction, distributedload.Distance1, distributedload.Distance2, distributedload.Value1, distributedload.Value2, "GLOBAL", true, true, 0);
        }

    }
}
