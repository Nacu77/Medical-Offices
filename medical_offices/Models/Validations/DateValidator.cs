using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace medical_offices.Models.Validations
{
    public class DateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateStart = (DateTime)validationContext.ObjectInstance;
            return (dateStart > DateTime.Now) ? ValidationResult.Success : new ValidationResult("This is not a correct date!");
        }
    }
}