namespace SapPrimitives.FramePrimitives
{
    using SAP2000v20;
    using SapPrimitives.GeneralizedPrimitives;

    /// <summary>
    /// 
    /// </summary>
    public class SapSteelSection
    {
        private string name;
        private SapMaterial material;
        private double t2;
        private double t3;
        private double tf;
        private double tw;
        private double t2b;
        private double tfb;
        private string note;
        private string guid;
        private double[] modifiers;
        private int color;
        private cSapModel sapModel;

        public string Name { get => name; set => name = value; }
        internal SapMaterial Material { get => material; set => material = value; }
        public double T2 { get => t2; set => t2 = value; }
        public double T3 { get => t3; set => t3 = value; }
        public double TF { get => tf; set => tf = value; }
        public double TW { get => tw; set => tw = value; }
        public double T2B { get => t2b; set => t2b = value; }
        public double TFB { get => tfb; set => tfb = value; }
        public string Note { get => note; set => note = value; }
        public string Guid { get => guid; set => guid = value; }
        public double[] Modifiers { get => modifiers; set => modifiers = value; }
        public int Color { get => color; set => color = value; }
        public cSapModel Mysapmodel { get => sapModel; set => sapModel = value; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapModel"></param>
        /// <param name="material"></param>
        /// <param name="t3"></param>
        /// <param name="t2"></param>
        /// <param name="tf"></param>
        /// <param name="tw"></param>
        /// <param name="tfb"></param>
        /// <param name="t2b"></param>
        /// <param name="name"></param>
        /// <param name="note"></param>
        /// <param name="guid"></param>
        /// <param name="color"></param>
        public SapSteelSection(cSapModel sapModel, SapMaterial material, double t3, double t2, double tf, double tw, double tfb, double t2b, string name = "", string note = "", string guid = "", int color = 2)
        {
            this.sapModel = sapModel;
            this.material = material;
            this.name = name;
            this.t2 = t2;
            this.t3 = t3;
            this.tf = tf;
            this.tw = tw;
            this.t2b = t2b;
            this.tfb = tfb;
            this.note = note;
            this.guid = guid;
            this.color = color;
            sapModel.PropFrame.SetISection(this.name, this.material.Name, this.t3, this.t2, this.tf, this.tw, this.t2b, this.tfb, this.color, this.note, this.guid);
        }
    }
}
