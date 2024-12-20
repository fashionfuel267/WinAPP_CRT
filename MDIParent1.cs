﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeFRM employeeFRM = new EmployeeFRM();
            employeeFRM.Show();
            employeeFRM.MdiParent = this;
        }

        private void departmentSummeryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeptSummeryRpt deptSummeryRpt = new DeptSummeryRpt();
            deptSummeryRpt.Show();
            deptSummeryRpt.MdiParent=this;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmpDetailsrpt deptSummeryRpt = new EmpDetailsrpt();
            deptSummeryRpt.Show();
            deptSummeryRpt.MdiParent = this;
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Imgrptfrm deptSummeryRpt = new Imgrptfrm();
            deptSummeryRpt.Show();
            deptSummeryRpt.MdiParent = this;
        }

        private void empImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Imgrptfrm empImage=new Imgrptfrm();
            empImage.Show();
            empImage.MdiParent = this;

        }

        private void subReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SubRptFRM empImage = new SubRptFRM();
            empImage.Show();
            empImage.MdiParent = this;

        }

        private void employeeExpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeExp empImage = new EmployeeExp();
            empImage.Show();
            empImage.MdiParent = this;


        }
    }
}
