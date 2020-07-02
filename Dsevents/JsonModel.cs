using System;
using System.Collections.Generic;
using System.Text;

namespace Dsevents
{
    class JsonModel
    {
        public List<Process> Processes { get; set; }
        public List<Channel> Channels { get; set; }
        public List<Message> Messages { get; set; }
        public List<Event> Events { get; set; }

        public WorkingModel GetWorkingModel()
        {
            WorkingModel workingModel = new WorkingModel();

            foreach (Channel channel in Channels)
            {
                workingModel.Channels.Add(channel.ID, channel);
            }

            foreach (Process process in Processes)
            {
                workingModel.Processes.Add(process.ID, new List<Event>());
            }

            while (true)
            {
                bool added = false;
                foreach (Event e in Events)
                {
                    if (workingModel.Processes[e.ProcessID].Count + 1 == e.Seq)
                    {
                        workingModel.Processes[e.ProcessID].Add(e);
                        added = true;
                    }
                }

                if (!added)
                {
                    return workingModel;
                }
            }
        }
    }
}
