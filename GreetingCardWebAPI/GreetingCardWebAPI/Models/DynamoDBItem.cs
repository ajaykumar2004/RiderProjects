using Amazon.DynamoDBv2.DataModel;
using System;
namespace GreetingCardWebAPI.Models
{
    [DynamoDBTable("GreetingCards")]
    public class DynamoDBItem
    {
        [DynamoDBHashKey("To")]  
        public string To { get; set; }

        [DynamoDBRangeKey("ReceivedAt")]  
        public string ReceivedAt { get; set; }

        [DynamoDBProperty("From")]
        public string From { get; set; }

        [DynamoDBProperty("Message")]
        public string Message { get; set; }

        [DynamoDBProperty("CreatedAt")]
        public string CreatedAt { get; set; }  
    }
}