using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface ITheseRepo
    {
        public void ShowPdf(int these_id);
        public String GetNomEncadrantFromTheseId(int TheseId);

    }
}
