using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbVizion.Models
{
    public class Task
    {
        public Task()
        {
            Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

         [Required]
        public string Description { get; set; }
        [Required]
        public string Complexity { get; set; }

        public string ApplicationUserId { get; set; }
        
        [JsonIgnore]        
        public virtual ICollection<Comment> Comments { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}