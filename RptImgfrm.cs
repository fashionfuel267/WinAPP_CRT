using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace WindowsFormsApp1
{
    public partial class RptImgfrm : Form
    {
        public RptImgfrm()
        {
            InitializeComponent();
        }

        private void RptImgfrm_Load(object sender, EventArgs e)
        {LoadReport();

        }
        private void LoadReport()
        {
            // Define the connection string and SQL query

            string conpath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\App_Data\DbHR.mdf";
            string connectionString = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ={conpath}; Integrated Security = True";

            string sql = "select * from Employee";
            DataSet dataSet = new DataSet();
            string rootd = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
            // Load data into DataSet
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, connection);
                sqlDataAdapter.Fill(dataSet);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                { 
                    row["ImagePath"] = rootd + "\\" + row["ImagePath"].ToString(); 
                }
                
            }
            string reportPath = Path.Combine(rootd, "rptEmpImage.rpt");
            // Load the main report
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(reportPath);
            // Set the main report data source
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                reportDocument.SetDataSource(dataSet.Tables[0]);
                crystalReportViewer1.ReportSource = reportDocument;
                crystalReportViewer1.Refresh();
            }
            else
            {
                MessageBox.Show("No records to display.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
    }
}
