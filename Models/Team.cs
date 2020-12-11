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
        public int TeamId {get;set;}
        public string Nume { get; set; }
        public DateTime DataInscriere { get; set; }
        public int ProjectId { get; set; }
        //O echipa poate are mai multi membri
        //public virtual ICollection<Member> Members { get; set; }
        //O echipa poate dezvolta mai multe taskuri
        public virtual ICollection<Task> Tasks { get; set; }
        //O echipa poate dezvolta mai multe proiecte
        public virtual ICollection<Project> Projects { get; set; }
    }
}