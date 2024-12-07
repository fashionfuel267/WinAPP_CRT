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
                imgPath = path + row["Imagepath"].ToString();
                row["Data"] = File.ReadAllBytes(imgPath);
            }
            return dt;
        }

    }
}
