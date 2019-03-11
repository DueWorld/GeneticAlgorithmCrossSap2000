namespace SapPrimitives.FramePrimitives
{
    using SAP2000v20;
    using SapPrimitives.FactorySetting;

    /// <summary>
    /// 
    /// </summary>
    public class SapNonPrismaticSection
    {
        cSapModel Sapmodel;
        private string name;
        private int numberItems;
        private string[] startSec;
        private string[] endSec;
        private double[] length;
        private int[] positionType;
        private int[] EL22;
        private int[] EL33;
        private int color;
        private string note;
        private string GUID;



        public string Name { get => name; set => name = value; }
        public int NumberItems { get => numberItems; set => numberItems = value; }
        public string[] StartSec { get => startSec; set => startSec = value; }
        public string[] EndSec { get => endSec; set => endSec = value; }
        public double[] Length { get => length; set => length = value; }
        public int[] PositionType { get => positionType; set => positionType = value; }
        public int[] EL221 { get => EL22; set => EL22 = value; }
        public int[] EL331 { get => EL33; set => EL33 = value; }
        public int Color { get => color; set => color = value; }
        public string Note { get => note; set => note = value; }
        public string GUID1 { get => GUID; set => GUID = value; }
        public cSapModel SapModel { get => Sapmodel; set => Sapmodel = value; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapModel"></param>
        /// <param name="name"></param>
        /// <param name="pro"></param>
        public SapNonPrismaticSection(cSapModel sapModel, string name, InsertionProperties pro)
        {
            this.name = name;
            this.numberItems = pro.Number;
            this.startSec = pro.StartArr;
            this.endSec = pro.EndArr;
            this.length = pro.Lengths;
            this.positionType = pro.PositionType;
            this.EL22 = pro.EL22;
            this.EL33 = pro.EL33;
            this.color = pro.Color;
            this.GUID = pro.GUID1;
            this.note = pro.Note;
            sapModel.PropFrame.SetNonPrismatic(name, numberItems, ref startSec, ref endSec, ref length, ref positionType, ref EL22, ref EL33, color, note, GUID);
        }


    }
}
