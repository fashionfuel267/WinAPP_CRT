using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Model
{
    internal class Employee
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public double Salary { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

    }
}
