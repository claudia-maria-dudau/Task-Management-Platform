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
        public string UserId { get; set; }

        //un proiect apartine unei echipe
        public virtual Team Team { get; set; }

        //un proiect este creat de catre un organizator
        public virtual ApplicationUser Organizator { get; set; }
    }
}