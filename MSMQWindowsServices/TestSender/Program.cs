using System;
using System.Messaging;

class Program
{
    static void Main()
    {
        string queuePath = @".\private$\MyQueue";

        if (!MessageQueue.Exists(queuePath))
        {
            MessageQueue.Create(queuePath);
            Console.WriteLine("Queue created: " + queuePath);
        }

        using (MessageQueue queue = new MessageQueue(queuePath))
        {
            queue.Send("Hello from C#!");
            Console.WriteLine("Message sent successfully.");
        }

    }
}