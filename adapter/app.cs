using System;
using System.Threading;

namespace AdapterExplanation
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new ConsoleLogger();
            ILogger AwsAdaptedLogger = new EventsTrackerAdapter(new AwsEventsTracker());

            logger.Send("Teste");
            AwsAdaptedLogger.Send("Teste");

            Console.WriteLine("finished...");
        }
    }

    interface ILogger 
    {
        void Send(string log);
    }

    class ConsoleLogger: ILogger 
    {
        public void Send(string log)
        {
            Console.WriteLine("ConsoleLogger: "+log);
        }
    }

    // imagine that the class and interface below is part of 
    // a third party library and you can't change this
    interface IEventsTracker 
    {
        void TrackEvent(string type, string eventDetais);
    }

    class AwsEventsTracker : IEventsTracker
    {
        public void TrackEvent(string type, string eventDetais)
        {
            Console.WriteLine("AwsEventsTracker: "+type+":"+eventDetais);
        }
    }

    // then, to use AwsEventsTracker as a ILogger, you will need a adapter
    class EventsTrackerAdapter : ILogger
    {
        private IEventsTracker EventsTracker;

        public EventsTrackerAdapter(IEventsTracker eventsTracker)
        {
            this.EventsTracker = eventsTracker;
        }

        public void Send(string log)
        {
            this.EventsTracker.TrackEvent("log", log);
        }
    }
}