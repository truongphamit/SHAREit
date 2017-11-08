using System;
using System.ComponentModel.DataAnnotations;

namespace SHAREit.Models
{
    public class Borrow
    {
        [Key]
        public int borrow_id {get; set;}
        public DateTime? borrow_date {get; set;}
        public DateTime? return_date {get; set;}
        public string status {get; set;}
        public int user_id_borrow {get; set;}
        public int bookcase_id {get; set;}
    }
}