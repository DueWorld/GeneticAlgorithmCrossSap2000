namespace SapPrimitives.FactorySetting
{
    using SapPrimitives.FramePrimitives;

    public enum InsertionType
    {
        Column = 1,
        Beam = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class InsertionProperties
    {
        private string[] startArr;
        private string[] endArr;
        private double[] lengths;
        private int number;
        private int[] positionType;
        private int[] el22;
        private int[] el33;
        private int color;
        private string note;
        private string GUID;


        public string[] StartArr { get => startArr; set => startArr = value; }
        public string[] EndArr { get => endArr; set => endArr = value; }
        public double[] Lengths { get => lengths; set => lengths = value; }
        public int[] PositionType { get => positionType; set => positionType = value; }
        public int[] EL22 { get => el22; set => el22 = value; }
        public int[] EL33 { get => el33; set => el33 = value; }
        public int Color { get => color; set => color = value; }
        public string Note { get => note; set => note = value; }
        public string GUID1 { get => GUID; set => GUID = value; }
        public int Number { get => number; set => number = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sec1"></param>
        /// <param name="sec2"></param>
        /// <param name="mytype"></param>
        public InsertionProperties(SapSteelSection sec1, SapSteelSection sec2, InsertionType mytype)
        {
            switch (mytype)
            {
                case InsertionType.Column:
                    lengths = new double[2];
                    lengths[0] = 0.7;
                    lengths[1] = 0.3;
                    color = 5;
                    startArr = new string[2];
                    startArr[0] = sec2.Name;
                    startArr[1] = sec2.Name;
                    endArr = new string[2];
                    endArr[0] = sec2.Name;
                    endArr[1] = sec1.Name;
                    break;


                case InsertionType.Beam:
                    lengths = new double[2];
                    lengths[0] = 0.3;
                    lengths[1] = 0.7;
                    color = 2;
                    startArr = new string[2];
                    startArr[0] = sec1.Name;
                    startArr[1] = sec2.Name;
                    endArr = new string[2];
                    endArr[0] = sec2.Name;
                    endArr[1] = sec2.Name;
                    break;
            }


            positionType = new int[2];
            positionType[0] = 1;
            positionType[1] = 1;

            el22 = new int[2];
            el22[0] = 1;
            el22[1] = 1;

            el33 = new int[2];
            el33[0] = 1;
            el33[1] = 1;

            Number = 2;

            note = "";
            GUID = "";

        }
    }
}
