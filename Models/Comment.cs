using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task_Management_Platform.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Continutul comentariului este obligatoriu")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        //membrii isipot sterge/edita doar comentariile proprii
        public string UserId { get; set; }  
        public DateTime DataAdaug { get; set; }

        public int TaskId { get; set; }

        //foreign key
        //un comntariu este asociat unui task
        public virtual Task Task { get; set; }

        //un comentariu este creat de catre un membru
        public virtual ApplicationUser User { get; set; }
    }
}