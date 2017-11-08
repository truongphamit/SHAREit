using System;
using System.ComponentModel.DataAnnotations;

namespace SHAREit.Models
{
    public class User
    {
        [Key]
        public int user_id {get; set;}
        public string username {get; set;}
        public string password {get; set;}
        public string address {get; set;}
        public string phone {get; set;}
        public string email {get; set;}
        public float? lat {get; set;}
        public float? lon {get; set;}
        public DateTime? create_time {set; get;}
        public string role {get; set;}
    }
}