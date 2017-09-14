/////////////////////////////////////////////////////////////////////////////
//  TestExecutive.cs -   Initiates test harness operation,childAppDomain   //
//                       creation and assembly loading                     //
//                                                                         //
//  ver 1.0                                                                //
//  Language:     C#, VS 2015                                              //
//  Platform:     Windows 10,                                              //
//  Application:  Test Harness App                                         //
//  Author:       Rahul Vijaydev                                           //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Module Operations
 *   -----------------
 *   The Test Executive is the engine behind test harness operation.It initiates the creation 
 *   of a child Appdomain for the given test request.The test Executive injects the loader and ITest 
 *   interfaces into the appDomain and creates a proxy loader object using which the XML request
 *   is passed.Since XDocument objects are not serializable we pass them in the form of a string and
 *   parse them back into their respective type later.
 *   
 *   
 *  
 *   Public Interface
 *   ----------------
 *   TestExecutive testEx=new TestExecutive();
 *   void initiateTestOperation(BlockingQueue<XDocument> queue,Logger genLog);
 *   StringBuilder getLogResults(String logFileName)
 * 
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  TestExecutive.cs,Client.cs,AppDomainManager.cs,BlockingQueue.cs,Loader.cs,Logger.cs
 *   - Compiler command: csc TestExecutive.cs,Client.cs,AppDomainManager.cs,BlockingQueue.cs,Loader.cs,Logger.csS
 * 
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 9th October 2016
 *     - first release
 * 
 */
//

using SWTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using util;

namespace TestHarnessApp
{
    class TestExecutive
    {
        public void initiateTestOperation(BlockingQueue<XDocument> queue, Logger genLog)
        {
            AppDomainManager.AppDomainManager aDomManager = new AppDomainManager.AppDomainManager();
            AppDomain ad = null;
            try
            {
                while (queue.size() != 0)
                {
                    //one XML request is dequeued from the blocking Queue per iteration of the loop
                    Console.WriteLine("Dequeueing XML request");
                    genLog.log("Dequeuing XMl request");
                    XDocument doc = queue.deQ();
                    Console.WriteLine(doc);
                    //creation of child appDomain
                    ad = aDomManager.domainCreator();
                    Console.WriteLine("Child AppDomain succesfully created");
                    genLog.log("Child AppDomain succesfully created");
                    Assembly assembly = ad.Load("Loader");
                    Console.WriteLine("Loader injected into the child appDomain");
                    genLog.log("Loader is injected into the child appDomain");
                    Console.Write("\n\n");
                    //injecting ITest into the childAppDomain
                    Console.WriteLine("Injecting Itest into the child appDomain");
                    genLog.log("Injecting ITest into the child child appDomain");
                    ad.Load("ITest");
                    Console.Write("\n\n");
                    //generating handle to Loader
                    Console.WriteLine("Generating handle to loader");
                    genLog.log("Generating handle to loader");
                    ObjectHandle oh = ad.CreateInstance("Loader", "Loader.Loader");
                    // unwrap creates proxy to ChildDomain
                    object obj = oh.Unwrap();
                    Console.WriteLine("Unwrapping Loader and generating reference to loader");
                    genLog.log("Unwrapping Loader and generating reference to loader");
                    Loader.Loader load = (Loader.Loader)obj;
                    Logger testLogs = new Logger();
                    //XDocument objects are sent in the form of a string since thay are not serializable
                    genLog.log("XDocument objects are not serializable.They are sent in the form of a string to loadTests method in the Loader");
                    load.loadTests(doc.ToString(), testLogs, genLog);
                    Console.WriteLine("Getting Test logs from File");
                    Console.WriteLine(getLogResults(testLogs.nameOfFile).ToString());
                    Console.Write("\n  {0}", obj);
                    // unloading ChildDomain
                    AppDomain.Unload(ad);
                    Console.Write("\n\n");
                }
            }
            catch (Exception except)
            {
                Console.Write("\n  {0}\n\n", except.Message);
                genLog.log("Exception while initiating test operation");
                genLog.log(except.Message);
            }
        }

        //for independant recovery of log results
        public StringBuilder getLogResults(String logFileName)
        {
            StringBuilder logBuilder = new StringBuilder();
            try
            {
                // Create an instance of StreamReader to read from a file.
                using (StreamReader sr = new StreamReader(logFileName))
                {
                    string logLine;
                    // Keep checking the stream to see if there are more lines
                    while ((logLine = sr.ReadLine()) != null)
                    {
                        logBuilder.Append(logLine);
                        logBuilder.Append(Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not establish stream");
            }
            return logBuilder;
        }

        //stub for test executive
        public static void main(string[] args)
        {
            Console.WriteLine("Initiating test harness operation");
            XDocument sampleDoc1 = new XDocument(new XComment("This is a comment"),
                                new XElement("RootNode", new XElement("ChildNode1", "Value1"),
                                new XElement("ChildNode2", "Value2"), new XElement("ChildNode3", "Value"),
                                new XElement("ChildNode4", "Value4")));
            XDocument sampleDoc2 = new XDocument(new XComment("This is a comment"),
                                new XElement("RootNode", new XElement("Football clubs", "Leagues"),
                                new XElement("La Liga", "Fc Barcelona"), new XElement("EPL", "Liverpool"),
                                new XElement("Italian Seria A", "AC Milan")));
            BlockingQueue<XDocument> bqueue = new BlockingQueue<XDocument>();
            bqueue.enQ(sampleDoc1);
            bqueue.enQ(sampleDoc2);
            Logger genLogger = new Logger("../../../ LogResults / GeneralLogs / thLogs_" + DateTime.Now.ToString().Replace(" / ", " - ").Replace(":", " - ") + ".txt");
            TestExecutive tex = new TestExecutive();
            tex.initiateTestOperation(bqueue, genLogger);
            StringBuilder logRes = tex.getLogResults("../../../ LogResults / GeneralLogs / thLogs_" + DateTime.Now.ToString().Replace(" / ", " - ").Replace(":", " - ") + ".txt");
            Console.WriteLine("Here are your requested logs");
            Console.WriteLine(logRes);
        }
    }
}
