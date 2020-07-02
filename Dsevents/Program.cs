using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Dsevents
{
    class Program
    {
        public static void Main(string[] args)
        {
            string input = GetInput();
            JsonModel jsonModel = JsonSerializer.Deserialize<JsonModel>(input);
            WorkingModel model = jsonModel.GetWorkingModel();

            if (args[0] == "past")
            {
                Dictionary<string, ISet<string>> past = model.GetPast(GetEventIDs(args));
                WriteEventIDsDictionary("past", past);
            }
            else if (args[0] == "future")
            {
                Dictionary<string, ISet<string>> future = model.GetFuture(GetEventIDs(args));
                WriteEventIDsDictionary("future", future);
            }
            else if (args[0] == "concurrent")
            {
                Dictionary<string, ISet<string>> concurrent = model.GetConcurrent(GetEventIDs(args));
                WriteEventIDsDictionary("concurrent", concurrent);
            }
        }

        private static string GetInput()
        {
            string input = "";
            while (true)
            {
                string line = Console.ReadLine();
                if (line != null)
                {
                    input += line;
                }
                else
                {
                    return input;
                }
            }
        }

        private static List<string> GetEventIDs(string[] args)
        {
            List<string> eventIDs = new List<string>(args);
            eventIDs.RemoveAt(0);
            return eventIDs;
        }

        private static void WriteEventIDsDictionary(string title, Dictionary<string, ISet<string>> eventIDsDictionary)
        {
            Console.WriteLine(title + ":");
            foreach (var eventIDs in eventIDsDictionary)
            {
                Console.WriteLine("  " + eventIDs.Key + ":");
                foreach (string eventID in eventIDs.Value)
                {
                    Console.WriteLine("    " + eventID);
                }
            }
        }

        /*private static Dictionary<string, List<Event>> GetProcesses(JsonModel model)
        {
            var processes = new Dictionary<string, List<Event>>();
            foreach (Process process in model.Processes)
            {
                processes.Add(process.ID, new List<Event>());
            }

            while (true)
            {
                bool added = false;
                foreach (Event e in model.Events)
                {
                    if (processes[e.ProcessID].Count + 1 == e.Seq)
                    {
                        processes[e.ProcessID].Add(e);
                        added = true;
                    }
                }

                if (!added)
                {
                    return processes;
                }
            }
        }

        private static void WritePast(Dictionary<string, List<Event>> processes, List<Channel> channels, string[] args)
        {
            Console.WriteLine("past:");
            for (int i = 1; i < args.Length; i++)
            {
                Event analyzableEvent = GetEvent(args[i], processes);
                ISet<string> past = new SortedSet<string>();
                FindPast(analyzableEvent, processes, channels, past);

                Console.WriteLine("  " + analyzableEvent.ID + ":");
                foreach (string pastEvent in past)
                {
                    Console.WriteLine("    " + pastEvent);
                }
            }
        }

        private static void WriteFuture(Dictionary<string, List<Event>> processes, List<Channel> channels, string[] args)
        {
            Console.WriteLine("future:");
            for (int i = 1; i < args.Length; i++)
            {
                Event analyzableEvent = GetEvent(args[i], processes);
                ISet<string> future = new SortedSet<string>();
                FindFuture(analyzableEvent, processes, channels, future);

                Console.WriteLine("  " + analyzableEvent.ID + ":");
                foreach (string futureEvent in future)
                {
                    Console.WriteLine("    " + futureEvent);
                }
            }
        }

        private static void WriteConcurrent(Dictionary<string, List<Event>> processes, List<Channel> channels, string[] args)
        {
            Console.WriteLine("concurrent:");
            for (int i = 1; i < args.Length; i++)
            {
                Event analyzableEvent = GetEvent(args[i], processes);

                ISet<string> past = new SortedSet<string>();
                FindPast(analyzableEvent, processes, channels, past);

                ISet<string> future = new SortedSet<string>();
                FindFuture(analyzableEvent, processes, channels, future);

                Console.WriteLine("  " + analyzableEvent.ID + ":");
                foreach (var process in processes)
                {
                    foreach (Event e in process.Value)
                    {
                        if (e.ID != analyzableEvent.ID && !past.Contains(e.ID) && !future.Contains(e.ID))
                        {
                            Console.WriteLine("    " + e.ID);
                        }
                    }
                }
            }
        }

        private static Event GetEvent(string eventID, Dictionary<string, List<Event>> processes)
        {
            foreach (var process in processes)
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

        private static void FindPast(Event analyzableEvent, Dictionary<string, List<Event>> processes, List<Channel> channels, ISet<string> past)
        {
            if (analyzableEvent.Seq > 1)
            {
                Event pastEvent = processes[analyzableEvent.ProcessID][analyzableEvent.Seq - 2];
                if (!past.Contains(pastEvent.ID))
                {
                    past.Add(pastEvent.ID);
                    FindPast(pastEvent, processes, channels, past);
                }
            }

            if (analyzableEvent.ChannelID != null)
            {
                Channel channel = channels.Find(c => c.ID == analyzableEvent.ChannelID);
                if (channel.To == analyzableEvent.ProcessID)
                {
                    foreach (Event e in processes[channel.From])
                    {
                        if (e.ChannelID == channel.ID && !past.Contains(e.ID))
                        {
                            past.Add(e.ID);
                            FindPast(e, processes, channels, past);
                        }
                    }
                }
            }
        }

        private static void FindFuture(Event analyzableEvent, Dictionary<string, List<Event>> processes, List<Channel> channels, ISet<string> future)
        {
            if (analyzableEvent.Seq < processes[analyzableEvent.ProcessID].Count)
            {
                Event futureEvent = processes[analyzableEvent.ProcessID][analyzableEvent.Seq];
                if (!future.Contains(futureEvent.ID))
                {
                    future.Add(futureEvent.ID);
                    FindFuture(futureEvent, processes, channels, future);
                }
            }

            if (analyzableEvent.ChannelID != null)
            {
                Channel channel = channels.Find(c => c.ID == analyzableEvent.ChannelID);
                if (channel.From == analyzableEvent.ProcessID)
                {
                    foreach (Event e in processes[channel.To])
                    {
                        if (e.ChannelID == channel.ID && !future.Contains(e.ID))
                        {
                            future.Add(e.ID);
                            FindFuture(e, processes, channels, future);
                        }
                    }
                }
            }
        }*/
    }
}
