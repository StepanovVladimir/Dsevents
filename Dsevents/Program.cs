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
    }
}
