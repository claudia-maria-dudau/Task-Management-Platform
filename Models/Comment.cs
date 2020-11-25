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

        public DateTime DataAdaug { get; set; }

        public int TaskId { get; set; }

        //foreign key
        //un comntariu este asociat unui task
        public virtual Task1 Task { get; set; }
    }
}