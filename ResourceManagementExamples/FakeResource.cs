using System;
using ResourceManagementExamples.Interfaces;

namespace ResourceManagementExamples
{
    public class FakeResource : IResource
    {
        private bool open;

        public void Open()
        {
            if(open)
                throw new Exception("Resource is already open");

            Console.WriteLine("Opening the resource");

            open = true;
        }

        public void Close()
        {
            if (!open)
                throw new Exception("Resource is already closed");

            Console.WriteLine("Closing the resource");

            open = false;
        }

        public string QuerySomeData()
        {
            if (!open)
                throw new Exception("Resource is closed");

            Console.WriteLine("Querying the resource");

            return "Some resource data";
        }
    }
}