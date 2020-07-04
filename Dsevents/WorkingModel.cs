using System;
using System.Collections.Generic;
using System.Text;

namespace Dsevents
{
    class WorkingModel
    {
        public Dictionary<string, Channel> Channels { get; set; } = new Dictionary<string, Channel>();
        public Dictionary<string, List<Event>> Processes { get; set; } = new Dictionary<string, List<Event>>();

        public ISet<string> GetPast(string eventID)
        {
            Event analyzableEvent = GetEvent(eventID);

            ISet<string> past = new SortedSet<string>();
            FindPast(analyzableEvent, past);

            return past;
        }

        public ISet<string> GetFuture(string eventID)
        {
            Event analyzableEvent = GetEvent(eventID);

            ISet<string> future = new SortedSet<string>();
            FindFuture(analyzableEvent, future);

            return future;
        }

        public ISet<string> GetConcurrent(string eventID)
        {
            Event analyzableEvent = GetEvent(eventID);

            ISet<string> past = new SortedSet<string>();
            FindPast(analyzableEvent, past);

            ISet<string> future = new SortedSet<string>();
            FindFuture(analyzableEvent, future);

            ISet<string> concurrent = new SortedSet<string>();
            foreach (var process in Processes)
            {
                foreach (Event e in process.Value)
                {
                    if (e.ID != analyzableEvent.ID && !past.Contains(e.ID) && !future.Contains(e.ID))
                    {
                        concurrent.Add(e.ID);
                    }
                }
            }

            return concurrent;
        }

        private Event GetEvent(string eventID)
        {
            foreach (var process in Processes)
            {
                foreach (Event e in process.Value)
                {
                    if (e.ID == eventID)
                    {
                        return e;
                    }
                }
            }
            return null;
        }

        private void FindPast(Event analyzableEvent, ISet<string> past)
        {
            if (analyzableEvent.Seq > 1)
            {
                Event pastEvent = Processes[analyzableEvent.ProcessID][analyzableEvent.Seq - 2];
                if (!past.Contains(pastEvent.ID))
                {
                    past.Add(pastEvent.ID);
                    FindPast(pastEvent, past);
                }
            }

            if (analyzableEvent.ChannelID != null)
            {
                Channel channel = Channels[analyzableEvent.ChannelID];
                if (channel.To == analyzableEvent.ProcessID)
                {
                    foreach (Event e in Processes[channel.From])
                    {
                        if (e.ChannelID == channel.ID && !past.Contains(e.ID))
                        {
                            past.Add(e.ID);
                            FindPast(e, past);
                        }
                    }
                }
            }
        }

        private void FindFuture(Event analyzableEvent, ISet<string> future)
        {
            if (analyzableEvent.Seq < Processes[analyzableEvent.ProcessID].Count)
            {
                Event futureEvent = Processes[analyzableEvent.ProcessID][analyzableEvent.Seq];
                if (!future.Contains(futureEvent.ID))
                {
                    future.Add(futureEvent.ID);
                    FindFuture(futureEvent, future);
                }
            }

            if (analyzableEvent.ChannelID != null)
            {
                Channel channel = Channels[analyzableEvent.ChannelID];
                if (channel.From == analyzableEvent.ProcessID)
                {
                    foreach (Event e in Processes[channel.To])
                    {
                        if (e.ChannelID == channel.ID && !future.Contains(e.ID))
                        {
                            future.Add(e.ID);
                            FindFuture(e, future);
                        }
                    }
                }
            }
        }
    }
}
