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
                ISet<string> past = model.GetPast(args[1]);
                WriteEventIDs(past);
            }
            else if (args[0] == "future")
            {
                ISet<string> future = model.GetFuture(args[1]);
                WriteEventIDs(future);
            }
            else if (args[0] == "concurrent")
            {
                ISet<string> concurrent = model.GetConcurrent(args[1]);
                WriteEventIDs(concurrent);
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

        private static void WriteEventIDs(ISet<string> eventIDs)
        {
            foreach (string eventID in eventIDs)
            {
                Console.Write(eventID + " ");
            }
            Console.WriteLine();
        }
    }
}
