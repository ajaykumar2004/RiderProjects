using System;
using System.Diagnostics;
using System.Messaging;
using System.ServiceProcess;
using System.Timers;

namespace MsmqListenerService
{
    public class ListenerService : ServiceBase
    {
        private Timer _timer;
        private const string QueuePath = @".\private$\MyQueue";
        private const string EventSource = "MSMQListenerService";
        private const string EventLogName = "Application";

        public ListenerService()
        {
            ServiceName = "MSMQListenerService";
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, EventLogName);
            }
        }

        protected override void OnStart(string[] args)
        {
            LogEvent("Service started.");
            _timer = new Timer(5000); // Check every 5 seconds
            _timer.Elapsed += ProcessQueueMessages;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer?.Stop();
            _timer?.Dispose();
            LogEvent("Service stopped.");
        }

        private void ProcessQueueMessages(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!MessageQueue.Exists(QueuePath))
                {
                    LogEvent("MSMQ Queue does not exist: " + QueuePath);
                    return;
                }

                using (var queue = new MessageQueue(QueuePath))
                {
                    queue.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
                    var message = queue.Receive(TimeSpan.FromSeconds(1));
                    if (message != null)
                    {
                        LogEvent("Received Message: " + message.Body.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogEvent("Error: " + ex.Message);
            }
        }

        private void LogEvent(string message)
        {
            EventLog.WriteEntry(EventSource, message, EventLogEntryType.Information);
        }
    }
}
