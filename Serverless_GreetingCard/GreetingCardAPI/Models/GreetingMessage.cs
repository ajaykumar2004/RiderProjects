namespace GreetingCardAPI.Models
{
    public class GreetingCard
    {
        public string From { get; set; }    
        public string To { get; set; }      
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } 
    }

}
