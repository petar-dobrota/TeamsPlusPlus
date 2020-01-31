using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web
{
    public class EventCounter
    {
        private readonly int intervalLengthMillis;
        private readonly Func<long> timeProvider;
        private readonly LinkedList<long> EventOccurrences = new LinkedList<long>();
        private readonly object lockObj = new object();

        public EventCounter(int intervalLengthMillis) : this(intervalLengthMillis, DateTimeOffset.Now.ToUnixTimeMilliseconds)
        { }

        public EventCounter(int intervalLengthMillis, Func<long> timeProvider)
        {
            this.intervalLengthMillis = intervalLengthMillis;
            this.timeProvider = timeProvider;
        }

        private void MaintainEventOccurrencesUnsafe(long timeNowMillis)
        {
            long eldestEventTime = timeNowMillis - intervalLengthMillis;

            while(EventOccurrences.Count > 0)
            {
                if (EventOccurrences.First.Value < eldestEventTime)
                {
                    EventOccurrences.RemoveFirst();
                } else
                {
                    break;
                }
            }
        }

        public void SignalEventOccured()
        {
            long timeNowMillis = timeProvider();
            lock(lockObj)
            {
                MaintainEventOccurrencesUnsafe(timeNowMillis);
                EventOccurrences.AddLast(timeNowMillis);
            }
        }

        public int GetNumberOfEventsInInterval() {
            long timeNowMillis = timeProvider();
            lock (lockObj)
            {
                MaintainEventOccurrencesUnsafe(timeNowMillis);
                return EventOccurrences.Count;
            }
        }

    }
}
