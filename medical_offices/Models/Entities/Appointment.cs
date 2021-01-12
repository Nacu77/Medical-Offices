using medical_offices.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace medical_offices.Models.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [DateValidator]
        public DateTime Date { get; set; }

        // one to many
        public virtual MedicalOffice MedicalOffice { get; set; }

        // one to many
        public virtual Person Person { get; set; }
    }
}