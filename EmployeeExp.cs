using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAL;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1
{
    public partial class EmployeeExp : Form
    {
        EmploeeDataAccess dal = new EmploeeDataAccess();
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        SqlDataAdapter adapter;
        SqlDataReader reader;
        public EmployeeExp()
        {
            InitializeComponent();
        }
        public int GETEmpId()
        {
            try
            {
                string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
                string constring = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";

                string sql = "select Top 1  Id from Employee order by Id Desc";
                sqlConnection = new SqlConnection(constring);
                sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.Text;
                var r = sqlCommand.ExecuteScalar();
                int result =int.Parse( r.ToString());
                if (result > 0)
                {

                    return result;
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;
        }
        public int InsertExp(int empId)
        {
            try
            {
                int result = 0;
                string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
                string constring = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";

                sqlConnection = new SqlConnection(constring);

                string sql = "EmployeeExpInsert";
                
                sqlConnection.Open();
                
                foreach(DataGridViewRow row in dataGridViewex.Rows)
                {
                    sqlCommand = new SqlCommand(sql, sqlConnection); 
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (row.IsNewRow) break;
                    sqlCommand.Parameters.AddWithValue("@Com", row.Cells["CompanyName"].Value );
                    sqlCommand.Parameters.AddWithValue("@Exp", row.Cells["ExperienceYear"].Value);
                    sqlCommand.Parameters.AddWithValue("@empId", row.Cells["EmpId"].Value==null?empId : row.Cells["EmpId"].Value);
                      sqlCommand.Parameters.AddWithValue("@expId", row.Cells["Id"].Value == null ? 0 : row.Cells["Id"].Value); 
                    result = sqlCommand.ExecuteNonQuery();
                }
            
                if (result > 0)
                {
                   
                    return result;
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            var dal = new EmploeeDataAccess();
            Employee employee = new Employee();
            employee.Name = this.txtName.Text;
            employee.Salary = double.Parse(this.txtSalary.Text);
            employee.IsActive = true;
            employee.JoiningDate = dtpJD.Value;
            employee.DeptId = int.Parse(cmbDept.SelectedValue.ToString());
          
            employee.ImagePath = this.txtImagePath.Text;
            int result = 0;
            int empId = 0;
            if (txtId.Text.Length > 0)
            {
                employee.Id = int.Parse(txtId.Text);
                result = dal.Update(employee, openFileDialog1);
            }
            else
            {
                result = dal.Insert(employee, openFileDialog1);
                if (result > 0)
                {
                    empId = GETEmpId();
                }
              
            }

            if (result > 0)
            {
                if (InsertExp(empId) > 0)
                {

                    MessageBox.Show("Operation Success");
                   
                    SetGridView();
                    ClearControl();
                }
            }
        }
        private void SetGridView()
        {
            dataGridView1.DataSource = null;
            //Set AutoGenerateColumns False.
            dataGridView1.AutoGenerateColumns = false;

            // Add as many columns as needed

            //Set Columns Count.
            dataGridView1.ColumnCount = 6;

            //Add Columns.
            dataGridView1.Columns[0].Name = "Id";
            dataGridView1.Columns[0].HeaderText = "Id";
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
        private void ClearControl()
        {
            this.txtName.Text = string.Empty;
            this.txtSalary.Text = string.Empty;
            this.dtpJD.Text = string.Empty;
            this.txtId.Text = string.Empty;
            this.cmbDept.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.btnDel.Enabled = false;
            this.btnClear.Enabled = false;
            this.dataGridViewex.DataSource = null;
            this.dataGridViewex.Refresh();
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

        private void EmployeeExp_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbHRDataSet1.Department' table. You can move, or remove it, as needed.
            this.departmentTableAdapter.Fill(this.dbHRDataSet1.Department);
            SetGridView();

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)

        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                var selectedItem = dataGridView1.SelectedRows;

                this.btnSave.Text = "Update";
                this.btnDel.Enabled = true;
                this.btnClear.Enabled = true;
                int id = int.Parse(selectedItem[0].Cells[0].Value.ToString());
                var employee = new EmploeeDataAccess().GetById(id);
                this.txtName.Text = employee.Name;
                this.txtSalary.Text = employee.Salary.ToString();
                this.checkBox1.Checked = employee.IsActive;
                this.dtpJD.Text = employee.JoiningDate.ToString();
                this.txtId.Text = employee.Id.ToString();
                this.cmbDept.SelectedValue = employee.DeptId.ToString();
                this.txtImagePath.Text = employee.ImagePath;
                string imgPath;
                string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));

                pictureBox1.ImageLocation = path + employee.ImagePath;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                this.GetEx(id);
            }
        }
        public void GetEx(int Id)
        {
            Employee emp = new Employee();
            SqlDataAdapter sda = new SqlDataAdapter();
            string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
            string connectionString = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";

            string sql = $"select * from Experience where EmpId={Id}";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sql,sqlConnection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            this.dataGridViewex.AutoGenerateColumns = false;
            this.dataGridViewex.DataSource = null;
            this.dataGridViewex.DataSource = dt;
            this.dataGridViewex.Refresh();
            sqlConnection.Close();
             

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
