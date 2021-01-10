using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace medical_offices.Models.Entities
{
    public class MedicalOffice
    {
        public int MedicalOfficeId { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }

        // one to one
        public virtual Address Address { get; set; }

        // one to many
        public virtual Person Person { get; set; }

        // many to one
        public virtual ICollection<Appointment> Appointments { get; set; }

        // many to many
        public virtual ICollection<Service> Services { get; set; }

        // checkbox list
        [NotMapped]
        public List<CheckBoxViewModel> ServicesList { get; set; }
    }
}