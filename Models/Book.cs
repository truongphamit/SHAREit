using System.ComponentModel.DataAnnotations;

namespace SHAREit.Models
{
    public class Book
    {
        [Key]
        public int book_id {get; set;}
        public string sku {get; set;}
        public string title {get; set;}
        public string sub_description {get; set;}
        public string description {get; set;}
        public string author {get; set;}
        public string company {get; set;}
        public string image {get; set;}
    }
}