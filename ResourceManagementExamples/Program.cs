using System;
using System.Threading;
using Castle.DynamicProxy;
using ResourceManagementExamples.Interfaces;

namespace ResourceManagementExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAutomaticManager();
        }

        private static void TestAutomaticManager()
        {
            IResourceUser resourceUser =
                new AutomaticManager(
                    new FakeResource(),
                    TimeSpan.FromMilliseconds(500));

            resourceUser.QuerySomeData();

            Thread.Sleep(400);

            resourceUser.QuerySomeData();

            Thread.Sleep(600);

            resourceUser.QuerySomeData();

            Console.WriteLine("Done");

            Console.ReadLine();
        }

        private static void TestAutomaticManagerAspect()
        {
            var proxyGenerator = new ProxyGenerator();

            var fakeResource = new FakeResource();

            IResourceUser resourceUser =
                (IResourceUser)
                    proxyGenerator.CreateInterfaceProxyWithTarget(
                        typeof(IResourceUser),
                        fakeResource,
                        new AutomaticManagerAspect(fakeResource, TimeSpan.FromMilliseconds(500)));

            resourceUser.QuerySomeData();

            Thread.Sleep(400);

            resourceUser.QuerySomeData();

            Thread.Sleep(600);

            resourceUser.QuerySomeData();

            Console.WriteLine("Done");

            Console.ReadLine();
        }
    }
}
