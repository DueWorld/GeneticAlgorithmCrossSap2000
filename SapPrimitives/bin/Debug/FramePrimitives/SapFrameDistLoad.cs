namespace SapPrimitives.FramePrimitives
{
    using SapPrimitives.GeneralizedPrimitives;

    public class SapFrameDistLoad
    {
        private SapLoadPattern loadPattern;
        private int type;
        private int direction;
        private double distance1;
        private double distance2;
        private double value1;
        private double value2;

        internal SapLoadPattern LoadPattern { get => loadPattern; set => loadPattern = value; }
        public int Type { get => type; set => type = value; }
        public int Direction { get => direction; set => direction = value; }
        public double Distance1 { get => distance1; set => distance1 = value; }
        public double Distance2 { get => distance2; set => distance2 = value; }
        public double Value1 { get => value1; set => value1 = value; }
        public double Value2 { get => value2; set => value2 = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadPattern"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <param name="distance1"></param>
        /// <param name="distance2"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        public SapFrameDistLoad(SapLoadPattern loadPattern, int type, int direction, double distance1, double distance2, double value1, double value2)
        {
            this.loadPattern = loadPattern;
            this.type = type;
            this.direction = direction;
            this.distance1 = distance1;
            this.distance2 = distance2;
            this.value1 = value1;
            this.value2 = value2;
        }
    }
}
