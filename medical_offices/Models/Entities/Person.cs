﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace medical_offices.Models.Entities
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // one to one
        [ForeignKey("ApplicationUser")]
        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        // many to one
        public virtual ICollection<MedicalOffice> MedicalOffices { get; set; }

        // many to one
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}