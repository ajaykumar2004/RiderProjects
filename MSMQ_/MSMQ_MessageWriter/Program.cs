using System;
using System.Messaging;

class Program
{
    static void Main()
    {
        string queuePath = @".\private$\TestQueue";

        if (!MessageQueue.Exists(queuePath))
        {
            Console.WriteLine("Queue does not exist.");
            return;
        }

        using (MessageQueue queue = new MessageQueue(queuePath))
        {
            queue.Send("Hello, MSMQ!", "Test Message");
            Console.WriteLine("Message sent to MSMQ.");
        }
    }
}