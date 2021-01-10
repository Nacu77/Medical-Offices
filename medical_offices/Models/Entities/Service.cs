using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace medical_offices.Models.Entities
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }

        // many to many
        public virtual ICollection<MedicalOffice> MedicalOffices { get; set; }
    }
}