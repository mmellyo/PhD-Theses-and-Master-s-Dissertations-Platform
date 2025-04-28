using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IReportsRepo
    {
        public void ReportThesis(int Thesis_Id, string description);
        public void ReportComment(int CommentId, int? Userid);
        public void ReportAccount(int Account_Id, string description);

    }
}
