namespace SHAREit.Models
{
    public class Respone 
    {
        public Respone (int code, string msg, object data) {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
        public int code {get; set;}
        public string msg {get; set;}
        public object data {get; set;}   
    }        
}