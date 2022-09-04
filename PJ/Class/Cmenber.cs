using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ.Class
{
    public class Cmenber
    {
        public int MemberID { get; set; }
        public string MemberAccount { get; set; }
        public string MemberPassword { get; set; }
        public string MemberName { get; set; }
        public DateTime BirthDate { get; set; }
        public string MemberPhone { get; set; }
        public string MemberEmail { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public Binary MemberImage { get; set; }
        public string Authority { get; set; }
        public bool isaddornot { get; set; }
        public byte LargePhoto { get; set; }

    }
}
