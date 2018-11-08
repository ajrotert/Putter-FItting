using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PutterFitting
{
    public partial class PutterFittingSoftware : Form
    {
        public string[] data = new string[9];
        public int[] importance = new int[9];
        public string[,] listItems = new string[9, 4] {
        {"Long", "Short", "Not Applicable", ""},
        {"Right Handed, Right Eye", "Right Handed, Left Eye", "Left Handed, Left Eye", "Left Handed, Right Eye"},
        {"Arcing Path", "Straight Back Straight Through", "", ""},
        {"Struggles with Alignment", "Alignment is Okay", "", ""},
        {"Greather than 6ft 6in", "Greater than 6ft", "Less than 6ft", "Less than 5ft 5in"},
        {"Wrist bend", "No Wrist Bend", "Not Applicable" ,""},
        {"Standard Size Grip", "Larger Grip", "Not Applicable", ""},
        {"Softer Feel", "Harder Feel", "Not Applicable", ""},
        {"", "", "", "" } };
        public string[] listNames = new string[9] {
        "Common Distance Miss",
        "Dominant Eye",
        "Swing Path",
        "Alignment",
        "Height",
        "Putter Head Movement",
        "Grip Perefrence",
        "Feel",
        ""};
        public int counter = 0;
        public PutterFittingSoftware()
        {
            InitializeComponent();
        }
        private void PathList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }//selection box

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }//big box thing

        private void PutterTitle_Click(object sender, EventArgs e)
        {

        }//putter title

        private void Path_Click(object sender, EventArgs e)
        {

        }//selection box title

        private void label2_Click(object sender, EventArgs e)
        {

        }//importance title

        private void SwingPathImportance_TextChanged(object sender, EventArgs e)
        {

        }//importance box

        private void label3_Click(object sender, EventArgs e)
        {

        }//1-5 label 

        private void NextButton_Click(object sender, EventArgs e)
        {
            optionsList.Items.Clear();
            //optionsList.Items.Add
            data[counter] = optionsList.Text;
            importance[counter] = Convert.ToInt32(importanceBox.Text);
            optionsTitle.Text = listNames[counter];
            for (int a = 0; a < 4; a++)
                optionsList.Items.Add(listItems[counter, a]);
            counter++;
            importanceBox.Text = "5";
            if (counter == 9)
            {
                Controls.Remove(optionsList);
                Controls.Remove(NextButton);
                Controls.Remove(optionsTitle);
                Controls.Remove(importanceBox);
                Controls.Remove(oneToFiveLabel);
                Controls.Remove(importanceTitle);
                start();
            }

        }//next button
        public void start()
        {
            //Fitting person1 = new FItting(data);
        }
    }
    
}


/*
        Common miss {"Left", "Right", "Not Applicable"};
        Common miss {"Long", "Short", "Not Applicable"};
        Dominant Eye {"Right Handed, Right Eye", "Right Handed, Left Eye", "Left Handed, Left Eye", "Left Handed, Right Eye"};
        Swing path {"Arcing Path", "Straight Back Straight Through"};
        Alignment {"Struggles with Alignment", "Alignment is Okay"};
        Height {"Greather than 6ft 6in", "Greater than 6ft", "Less than 6ft", "Less than 5ft 5in"};
        head movement {"Wrist bend", "No Wrist Bend", "Not Applicable"};
        Grip perefrence {"Standard Size Grip", "Larger Grip", "Not Applicable"};
        Feel {"Softer Feel", "Harder Feel", "Not Applicable"};

Alignment-alignment
Toe hang-common miss, and swing path
Offset-eye
Head shapes-alignment
Loft x
Length - height
Lie Angle x
Weight - common miss
Grip Style/Size - head movement, prefernce
Face Material/Texture - feel
 */
