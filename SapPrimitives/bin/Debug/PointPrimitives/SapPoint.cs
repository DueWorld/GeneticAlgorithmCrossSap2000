namespace SapPrimitives.PointPrimitives
{
    using SAP2000v20;
    using SapPrimitives.GeneralizedPrimitives;

    public enum Direction
    {
        X = 1, Y = 2, Z = 3,
    }
    public class SapPoint
    {
        private string name;
        private double x;
        private double y;
        private double z;
        private SapJointRestraint jointRestraint;
        public SapJointRestraint JointRestraint
        {
            get { return jointRestraint; }
            set { jointRestraint = value; UpdateRestrains(); }
        }

        public cSapModel SapModel { get; set; }
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public double Z
        {
            get { return z; }
            set { z = value; }
        }
        public string Name
        {
            get { return name; }
            set
            {
                // changing the name of point
                SapModel.PointObj.ChangeName(name, value);
                name = value;
            }
        }


        public SapPoint(cSapModel model, double x, double y, double z)
        {
            this.SapModel = model;
            this.x = x;
            this.y = y;
            this.z = z;
            model.PointObj.AddCartesian(x, y, z, ref name);
            //Iniitialize the point with no restraints;
            jointRestraint = new SapJointRestraint(RestrainCase.NoRestraint);

        }
        public SapPoint(cSapModel model) : this(model, 0, 0, 0)
        {
        }




        public void SetRestraint(RestrainCase restrains)
        {
            jointRestraint = new SapJointRestraint(restrains);
            bool[] tempRestrains = jointRestraint.Restrains;
            SapModel.PointObj.SetRestraint(name, ref tempRestrains, 0);
        }
        public void SetRestraint(bool U1, bool U2, bool U3, bool R1, bool R2, bool R3)
        {
            jointRestraint.SetRestraint(U1, U2, U3, R1, R2, R3);
            bool[] tempRestrains = jointRestraint.Restrains;
            SapModel.PointObj.SetRestraint(name, ref tempRestrains, 0);
        }
        public void DeleteRestraint()
        {
            SapModel.PointObj.DeleteRestraint(name);
        }

        private void UpdateRestrains()
        {
            bool[] tempRestrains = jointRestraint.Restrains;
            SapModel.PointObj.SetRestraint(name, ref tempRestrains);
        }
        public static long Count(cSapModel SapModel)
        {
            return SapModel.PointObj.Count();
        }
        public static string[] GetNameList(cSapModel sapModel)
        {
            int NumberNames = 0;
            //get the count of the point to initialize the array size first
            long PointCount = Count(sapModel);
            string[] PointNames = new string[PointCount];

            sapModel.PointObj.GetNameList(ref NumberNames, ref PointNames);
            return PointNames;

        }

        public SapPoint AddXYZ(SapModel sapModel, double space, Direction dir)
        {
            switch (dir)
            {
                case Direction.X:
                    this.x = space;
                    break;
                case Direction.Y:
                    this.Y = space;
                    break;
                case Direction.Z:
                    this.Z = space;
                    break;
            }
            return new SapPoint(sapModel.SapObjectModel, this.x, this.y, this.z);
        }

    }
}
