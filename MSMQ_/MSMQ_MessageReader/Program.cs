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
            Message msg = queue.Receive();
            msg.Formatter = new XmlMessageFormatter(new string[] { "System.String" });
            Console.WriteLine("Received: " + msg.Body);
        }
    }
}