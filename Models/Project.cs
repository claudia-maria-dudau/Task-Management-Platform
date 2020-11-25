using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task_Management_Platform.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string Nume { get; set; }
        public string Descriere { get; set; }
        public int TeamId { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DataInceput { get; set; }
        //Un proiect apartine unei echipe

        public virtual Team Team { get; set; }
    }
}