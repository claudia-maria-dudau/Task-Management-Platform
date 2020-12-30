using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task_Management_Platform.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Numele echipei este obligatoriu")]
        public string Nume { get; set; }
        public DateTime DataInscriere { get; set; }
 
        public int ProjectId { get; set; }

        public string UserId { get; set; }
        
        //o echipa este creata de catre un organizator
        public virtual ApplicationUser User { get; set; }

        //o echipa poate are mai multi membri
        public virtual ICollection<ApplicationUser> Users { get; set; }
        
        //o echipa poate dezvolta mai multe taskuri
        public virtual ICollection<Task> Tasks { get; set; }
        
        //o echipa poate dezvolta mai multe proiecte
        public virtual ICollection<Project> Projects { get; set; }
    }
}