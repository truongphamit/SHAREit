using System;
using System.ComponentModel.DataAnnotations;

namespace SHAREit.Models
{
    public class Rate
    {
        [Key]
        public int rate_id {get; set;}
        public string review {get; set;}
        public int star {get; set;}
        public DateTime? create_time {get; set;}
        public int user_id {get; set;}
        public int book_id {get; set;}
    }
}