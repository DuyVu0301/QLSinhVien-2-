using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class FacultyService
    {
        public List<Faculty> GetAll()
        {
            Model1 sinhVienDB = new Model1();
            return sinhVienDB.Faculties.ToList();
        }
    }
}
