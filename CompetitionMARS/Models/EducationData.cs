using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionMARS.Models
{
    public class EducationData
    {
        public string id { get; set; }
        public string CollegeName {  get; set; }
        public string CountryOfCollege { get; set; }
        public string Title {  get; set; }
        public string Degree { get; set; }
        public int YearOfGraduation {  get; set; }
    }
  

    public class CertificationData
    {
        public string id { get; set; }
        public string CertificateOrAward { get; set; }
        public string CertifiedFrom { get; set; }
        public int CertificationYear { get; set; }
    }
}
