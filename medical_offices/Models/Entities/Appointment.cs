using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace medical_offices.Models.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }

        // one to many
        public virtual MedicalOffice MedicalOffice { get; set; }

        // one to many
        public virtual Person Person { get; set; }
    }
}