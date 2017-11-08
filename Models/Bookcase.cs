using System;
using System.ComponentModel.DataAnnotations;

namespace SHAREit.Models
{
    public class Bookcase
    {
        [Key]
        public int bookcase_id {get; set;}
        public DateTime? create_time {get; set;}
        public int user_id {get; set;}
        public int book_id {get; set;}
    }
}