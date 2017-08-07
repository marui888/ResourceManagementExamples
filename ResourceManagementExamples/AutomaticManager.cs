using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceManagementExamples.Interfaces;

namespace ResourceManagementExamples
{
    public class AutomaticManager : IResourceUser
    {
        private readonly IResource resource;

        private readonly TimeSpan maximumIdleTimeBeforeClosing;

        private readonly object lockObject = new object();

        private bool isOpen;

        private Stopwatch lastRequestStopwatch = new Stopwatch();

        private long requestNumber;

        public AutomaticManager(
            IResource resource,
            TimeSpan maximumIdleTimeBeforeClosing)
        {
            this.resource = resource;
            this.maximumIdleTimeBeforeClosing = maximumIdleTimeBeforeClosing;
        }

        public string QuerySomeData()
        {
            lock (lockObject)
            {
                if (!isOpen)
                {
                    resource.Open();

                    isOpen = true;
                }

                try
                {
                    return resource.QuerySomeData();
                }
                finally
                {
                    lastRequestStopwatch.Restart();

                    requestNumber++;

                    if (requestNumber == 1)
                    {
                        ScheduleResourceClosing();
                    }
                }
            }
        }

        private async void ScheduleResourceClosing()
        {
            var delay = maximumIdleTimeBeforeClosing;

            while (true)
            {
                await Task.Delay(delay).ConfigureAwait(false);

                lock (lockObject)
                {
                    delay = maximumIdleTimeBeforeClosing - lastRequestStopwatch.Elapsed;

                    if (requestNumber == 1 || delay <= TimeSpan.Zero)
                    {
                        requestNumber = 0;

                        isOpen = false;

                        lastRequestStopwatch.Reset();

                        resource.Close();

                        return;
                    }
                }
            }
        }
    }
}
