namespace SapPrimitives
{
    using System;
    using System.Windows.Forms;
    using SapPrimitives.FramePrimitives;
    using SapPrimitives.GeneralizedPrimitives;
    using SapPrimitives.FactorySetting;
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public partial class MainInterface : Form
    {
        private bool visible;
        private double mutationRate;
        private int populationSize;
        private int eliteCount;
        private int numberOfIterations;


        public MainInterface()
        {
            InitializeComponent();
            populationSize = 6;
            mutationRate = 0.01d;
            eliteCount = 1;
            visible = true;
            numberOfIterations = 5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var currentText = button1.Text;
            button1.Text = "Please Wait";
            button1.Enabled = false;
            SapModel model = new SapModel(@"SapModel", visible, "TestRun");
            model.Initialize(SapUnits.Ton_m_C);
            SapMaterial ST = new SapMaterial(model.SapObjectModel, "ST52/3", MaterialType.STEEL);
            ST.SetDefaultSteel();
            SapSteelSection section1 = new SapSteelSection(model.SapObjectModel, ST, 1.3, 0.45, 0.04, 0.015, 0.04, 0.45, "ST1500x550");
            SapSteelSection section2 = new SapSteelSection(model.SapObjectModel, ST, 0.7, 0.35, 0.04, 0.015, 0.04, 0.35, "ST800x500");
            FactoryOptimizer optimizer = new FactoryOptimizer(model, section1, section2,numberOfIterations, populationSize, eliteCount,richTextBox1);
            optimizer.Start();
            Process.Start(@"SapModel");
            button1.Text = currentText;
            button1.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                visible = false;
            }
            else
            {
                visible = true;
            }
        }

        private void txtBoxPopSize_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtBoxPopSize.Text, out populationSize);
        }

        private void txtBoxMutation_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(txtBoxPopSize.Text, out mutationRate);

        }

        private void txtBoxElite_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtBoxPopSize.Text, out eliteCount);

        }

        private void txtIterations_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtIterations.Text, out numberOfIterations);
        }
    }
}
