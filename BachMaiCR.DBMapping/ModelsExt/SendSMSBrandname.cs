using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachMaiCR.DBMapping.ModelsExt
{
    public class SendSMSBrandname
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string Phone { get; set; }

        public string Contents { get; set; }

        public string Types { get; set; }
    }
}
