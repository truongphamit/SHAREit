namespace SHAREit.Models
{
    public class UserRequestModel 
    {
        public string address {get; set;}
        public string phone {get; set;}
        public string email {get; set;}
        public float? lat {get; set;}
        public float? lon {get; set;}
    }
}