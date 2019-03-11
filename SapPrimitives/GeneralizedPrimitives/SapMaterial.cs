using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v20;
using SapPrimitives.FramePrimitives;
using SapPrimitives.PointPrimitives;


namespace SapPrimitives.GeneralizedPrimitives
{
    public enum MaterialType
    {
        STEEL = 1,
        CONCRETE = 2,
        NODESIGN = 3,
        ALUMINUM = 4,
        COLDFORMED = 5,
        REBAR = 6,
        TENDON = 7
    }


    public enum SapColor
    {
        Default = -1,
    }


    public class SapMaterial
    {
        private string name;
        private SapColor color;




        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public SapColor Color { get => color; set => color = value; }
        public cSapModel SapModel { get; set; }




        public SapMaterial(cSapModel sapModel, string name, MaterialType type, SapColor color = SapColor.Default)
        {
            this.SapModel = sapModel;
            this.name = name;
            SetMaterial(name, type, color);
        }




        public void SetMaterial(string name, MaterialType type, SapColor color = SapColor.Default)
        {
            SapModel.PropMaterial.SetMaterial(name, (eMatType)type, (int)color);
        }
        public void SetSteelMaterial(double Fy, double Fu, double EFy, double EFu)
        {
            SapModel.PropMaterial.SetOSteel_1(name, Fy, Fu, EFy, EFu, 1, 2, 0.02, 0.1, 0.2, -0.1);
        }
        public void SetDefaultSteel()
        {
            SapModel.PropMaterial.SetOSteel_1("ST52/3", 36000, 52000, 36000, 52000, 1, 2, 0.02, 0.1, 0.2, -1);
            SetWeight(7.8);
        }
        public void SetConcreteMaterial(double Fc, bool IsLightWeight = false, double FcsFactor = 0)
        {
            int check = SapModel.PropMaterial.SetOConcrete_1(name, Fc, IsLightWeight, FcsFactor, 1, 2, 0.0022, 0.0052, -0.1);
        }

        public void SetIsotropic(double E, double U, double A)
        {
            int check = SapModel.PropMaterial.SetMPIsotropic(name, E, U, A);
        }
        public void SetWeight(double value)
        {
            int check = SapModel.PropMaterial.SetWeightAndMass(name, 1, value);
        }

    }
}
