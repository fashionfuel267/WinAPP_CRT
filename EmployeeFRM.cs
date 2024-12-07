using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1
{
    public partial class EmployeeFRM : Form
    {
        EmploeeDataAccess dal = new EmploeeDataAccess();
        public EmployeeFRM()
        {
            InitializeComponent();
        }
        private void SetGridView()
        {
            //Set AutoGenerateColumns False.
            dataGridView1.AutoGenerateColumns = false;

            // Add as many columns as needed

            //Set Columns Count.
            dataGridView1.ColumnCount = 4;

            //Add Columns.
            dataGridView1.Columns[0].Name = "Id";
            dataGridView1.Columns[0].HeaderText = "Image Id";
            dataGridView1.Columns[0].DataPropertyName = "Id";

            dataGridView1.Columns[1].HeaderText = "Name";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[1].DataPropertyName = "EmployeeName";

            dataGridView1.Columns[2].HeaderText = "Salary";
            dataGridView1.Columns[2].Name = "Salary";
            dataGridView1.Columns[2].DataPropertyName = "Salary";
            //Add a Image Column to the DataGridView at the third position.
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = "Image";
            imageColumn.DataPropertyName = "Data";
            imageColumn.HeaderText = "Image";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.Columns.Insert(3, imageColumn);
            dataGridView1.RowTemplate.Height = 90;
            dataGridView1.Columns[3].Width = 100;

            //Bind DataGridView.
            dataGridView1.DataSource = dal.LoadData();

        }
        private void EmployeeFRM_Load(object sender, EventArgs e)
        {
           
            // TODO: This line of code loads data into the 'dbHRDataSet.Department' table. You can move, or remove it, as needed.
            this.departmentTableAdapter.Fill(this.dbHRDataSet.Department);
            SetGridView();
        }

        private void btnImageload_Click(object sender, EventArgs e)
        {
            // To where your opendialog box get starting location.My initial directory location is desktop.
            openFileDialog1.InitialDirectory = "C://Download";
            //Your opendialog box title name.
            openFileDialog1.Title = "Select image to be upload.";
            //which type image format you want to upload in database. just add them.
            openFileDialog1.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            //FilterIndex property represents the index of the filter currently selected in the file dialog box.
            openFileDialog1.FilterIndex = 1;
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialog1.CheckFileExists)
                    {
                        string path = System.IO.Path.GetFullPath(openFileDialog1.FileName);

                        pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload image.");
                }
            }
            catch (Exception ex)
            {
                //it will give if file is already exits..
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var dal= new EmploeeDataAccess();
            Employee employee = new Employee();
            employee.Name = this.txtName.Text;
            employee.Salary =double.Parse   ( this.txtSalary.Text);
            employee.IsActive = true;
            employee.JoiningDate = dtpJD.Value;
            employee.DeptId=int.Parse( cmbDept.SelectedValue.ToString());
           if( dal.Insert(employee,openFileDialog1)>0)
            {
                MessageBox.Show("save Success");
                //dataGridView1.DataSource = dal.LoadData();
                SetGridView();
            }
        }
    }
}
