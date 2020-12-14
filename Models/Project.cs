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
        [Required(ErrorMessage = "Numele proiectului este obligatoriu")]
        [StringLength(40, ErrorMessage = "Numele proiectului nu poate contine mai mult de 30 de caractere")]
        public string Nume { get; set; }
        [StringLength(500, ErrorMessage = "Descrierea proiectului nu trebuie sa depaseasca 500 de caractere")]
        public string Descriere { get; set; }
        public int TeamId { get; set; }
        [Required(ErrorMessage = "Deadline-ul este obligatoriu")]
        public DateTime Deadline { get; set; }
        [Required(ErrorMessage = "Data de inceput a proiectului este obligatorie")]
        public DateTime DataInceput { get; set; }
        //Un proiect apartine unei echipe
        public string UserId { get; set; }

        //un proiect apartine unei echipe
        public virtual Team Team { get; set; }

        //un proiect este creat de catre un organizator
        public virtual ApplicationUser User { get; set; }
    }
}