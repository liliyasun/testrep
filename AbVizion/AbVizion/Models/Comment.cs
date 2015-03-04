using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbVizion.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int TaskId { get; set; }
        
        public virtual Task Task { get; set; }
        
        public virtual string Author { get; set; }
    }
}