namespace SapPrimitives.FramePrimitives
{
    using System.Collections.Generic;
    using SAP2000v20;
    using SapPrimitives.PointPrimitives;

    public class SapNonePrismaticElement
    {

        private SapPoint point1;
        private SapPoint point2;
        private SapNonPrismaticSection NPSection;
        private string label;
        private string name;
        private List<SapFrameDistLoad> distibutedLoads;
        private cSapModel mymodel;




        public SapPoint Point1 { get => point1; set => point1 = value; }
        public SapPoint Point2 { get => point2; set => point2 = value; }
        public string Label { get => label; set => label = value; }
        public string Name { get => name; set => name = value; }
        internal SapNonPrismaticSection SteelSection { get => NPSection; set => NPSection = value; }
        public cSapModel Mymodel { get => mymodel; set => mymodel = value; }
        internal List<SapFrameDistLoad> DistibutedLoads { get => distibutedLoads; set => distibutedLoads = value; }




        public SapNonePrismaticElement(cSapModel mymodel, SapPoint point1, SapPoint point2, SapNonPrismaticSection section, string label, string framename)
        {
            this.mymodel = mymodel;
            this.point1 = point1;
            this.point2 = point2;
            this.NPSection = section;
            this.label = label;
            this.name = framename;
            this.distibutedLoads = new List<SapFrameDistLoad>();
            this.mymodel.FrameObj.AddByPoint(this.point1.Name, this.point2.Name, ref this.label, this.NPSection.Name, this.name);
        }

        public void AddDistributedLoad(SapFrameDistLoad distributedLoad)
        {
            this.distibutedLoads.Add(distributedLoad);
            int check = this.mymodel.FrameObj.SetLoadDistributed(this.name, distributedLoad.LoadPattern.Name, distributedLoad.Type, distributedLoad.Direction, distributedLoad.Distance1, distributedLoad.Distance2, distributedLoad.Value1, distributedLoad.Value2, "GLOBAL", true, true, 0);
        }

    }
}
