using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;
using SapPrimitives.FramePrimitives;
using SapPrimitives.GeneralizedPrimitives;

namespace SapPrimitives.PointPrimitives
{

    public enum RestrainCase
    {
        Fixed,
        Pinned,
        Roller,
        NoRestraint
    }

    public class SapJointRestraint
    {
        private bool[] restrains;

        public bool[] Restrains
        {
            get { return restrains; }
            set { restrains = value; }
        }


        //Asign the enum case to the restrain array
        public SapJointRestraint(RestrainCase type)
        {
            restrains = new bool[6];
            switch (type)
            {
                case RestrainCase.Fixed:
                    for (int i = 0; i < 6; i++)
                    {
                        restrains[i] = true;
                    }
                    break;

                case RestrainCase.Pinned:
                    for (int i = 0; i < 3; i++)
                    {
                        restrains[i] = true;
                    }
                    break;

                case RestrainCase.Roller:
                    restrains[2] = true;
                    break;

                case RestrainCase.NoRestraint:
                    break;
            }


        }

        //used for custom setting --> extending enum might be good option as well.
        public void SetRestraint(bool U1, bool U2, bool U3, bool R1, bool R2, bool R3)
        {
            restrains[0] = U1;
            restrains[1] = U2;
            restrains[2] = U3;
            restrains[3] = R1;
            restrains[4] = R2;
            restrains[5] = R3;
        }




    }
}
