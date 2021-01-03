﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task_Management_Platform.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Titlul task-ului este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Titlul nu poate avea mai mult de 100 de caractre.")]
        public string Title { get; set; }

        public string Description { get; set; }
        public string UserId { get; set; }
        public string UserId2 { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Data de inceput este obligatorie.")]
        public DateTime DataStart { get; set; }

        [Required(ErrorMessage = "Data de final este obligatorie.")]
        public DateTime DataFin { get; set; }
        public int TeamId { get; set; }


        //foreign key
        //un task apartine unei echipe
        public virtual Team Team { get; set; }

        //un task poate avea unul sau mai multe comentarii
        public virtual ICollection<Comment> Comments { get; set; }

        internal static Task FromResult(int v)
        {
            throw new NotImplementedException();
        }
        //Este asignat unui membru
        public virtual ApplicationUser User2 { get; set; }

        //un task este creat de catre un organizator
        public virtual ApplicationUser User { get; set; }
    }
}