using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.DAL
{
    internal class EmploeeDataAccess
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        SqlDataAdapter adapter;
        SqlDataReader reader;

        public int Insert(Employee employee ,OpenFileDialog openFileDialog )
        {
            try
            {
//                string constring = ConfigurationManager.ConnectionStrings["WindowsFormsApp1.Properties.Settings.DbHRConnectionString"].ConnectionString;

                string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
                string constring = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";

                string sql = "";

                string filename = System.IO.Path.GetFileName(openFileDialog.FileName);
                string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\Images\" + filename;
                if (int.Parse("0" + employee.Id) > 0)
                {
                    //sql = $"update  Employee set EmployeeName='{txtName.Text}',Designation='{txtDesignation.Text}',JoiningDate='{DateTime.Parse(DTPJD.Text)}',Salary={txtSalary.Text} where Id={lblId.Text}";
                }
                else
                {
                    employee.ImagePath = @"\Images\" + filename;

                    sql = $"Insert into Employee(EmployeeName,JoinindDate,Salary,ImagePAth,IsActive,DeptId)values('{employee.Name}'," +
                                $"'{employee.JoiningDate}',{employee.Salary},'{employee.ImagePath}','{employee.IsActive}',{employee.DeptId}      )";
                }

                sqlConnection = new SqlConnection(constring);
                sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    System.IO.File.Copy(openFileDialog.FileName, path);

                    // SetGridView();

                    //loadEmployee();
                    // ClearControl();
                    return result;
                }
                sqlConnection.Close();
            }
            catch (Exception ex) {
                return 0;
            }
            return 0;
        }
        public int Update(Employee employee, OpenFileDialog openFileDialog)
        {
            try
            {
                //  string constring = ConfigurationManager.ConnectionStrings["WindowsFormsApp1.Properties.Settings.DbHRConnectionString"].ConnectionString;

                string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
                string constring = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";

                string sql = "";
                string fileName = openFileDialog.FileName;
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                //}
                //    if (openFileDialog.CheckFileExists)
                //{
                    string filename = System.IO.Path.GetFileName(openFileDialog.FileName);
                    string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\Images\" + filename;
                   
                    if(File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    System.IO.File.Copy(openFileDialog.FileName, path);
                    employee.ImagePath = @"\Images\" + filename;
                }
                employee.ImagePath = employee.ImagePath;
                    sql = $"update Employee set EmployeeName='{employee.Name}',JoinindDate='{employee.JoiningDate}',Salary={employee.Salary}," +
                    $"ImagePAth='{employee.ImagePath}',IsActive='{employee.IsActive}',DeptId={employee.DeptId} where Id={employee.Id}";
                sqlConnection = new SqlConnection(constring);
                sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
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
        public int Delete(int Id)
        {
            try
            {
                //                string constring = ConfigurationManager.ConnectionStrings["WindowsFormsApp1.Properties.Settings.DbHRConnectionString"].ConnectionString;

                string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
                string constring = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";
                string sql = "";
                sql = $"delete  from Employee where Id={Id} ";

                sqlConnection = new SqlConnection(constring);
                sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
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
        public DataTable LoadData()
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
            string connectionString = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";
            string sql = $"select * from Employee ";
            sqlConnection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, sqlConnection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            // dataGridView1.DataSource = dt;

            //Add a new Byte[] Column.
            dt.Columns.Add("Data", Type.GetType("System.Byte[]"));

            //Convert all Images to Byte[] and copy to DataTable.
            string imgPath;
            string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
            // imgPath = Path.Combine(path, row.Cells[2].Value.ToString());

            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["Imagepath"].ToString()))
                {
                    imgPath = path + row["Imagepath"].ToString();
                    if (File.Exists(imgPath)) {
                        row["Data"] = File.ReadAllBytes(imgPath);
                    }
                    
                }
            }
            return dt;
        }
        public Employee GetById(int Id)
        {
            Employee emp = new Employee();
            SqlDataAdapter sda = new SqlDataAdapter();
            string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
            string connectionString = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";

            string sql = $"select * from Employee where Id={Id}";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            sqlCommand = new SqlCommand(sql, sqlConnection);
            reader= sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
               
                while (reader.Read())
                {
                    emp.Id =int.Parse( reader["Id"].ToString());
                    emp.Name = reader["EmployeeName"].ToString();
                    emp.Salary =double.Parse( reader["Salary"].ToString());
                    emp.IsActive =bool.Parse( reader["Isactive"].ToString());
                    emp.ImagePath = reader["ImagePath"].ToString();
                    emp.JoiningDate =DateTime.Parse( reader["JoinindDate"].ToString());
                    emp.DeptId = int.Parse(reader["DeptId"].ToString());
                }
            }
            //Convert all Images to Byte[] and copy to DataTable.
           sqlConnection.Close();
            return emp;
          
        }

       

    }
}
