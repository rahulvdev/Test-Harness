/////////////////////////////////////////////////////////////////////////////
//  Client.cs -   Fetches test requests and enqueues them                  //
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
 *   The client module makes use of a file manager to fetch test requests from the specified directory.
 *   The test requests from the file manager are stored in a list and enqueued sequentially into a Blocking Queue.
 *   The operational flow in the client is represented in a log file.
 *   
 *  
 *   Public Interface
 *   ----------------
 *   Client cl=new Client();
 *   void getTestRequests();
 *   void queueRequests(string[] testRequests);
 *   static void main();
 * 
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  Client.cs,FileManager.cs,BlockingQueue.cs,TestExecutive.cs
 *   - Compiler command: csc Client.cs,FileManager.cs,BlockingQueue.cs,TestExecutive.cs 
 * 
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 9th October 2016
 *     - first release
 * 
 */
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarnessApp
{
    using FileManager;
    using SWTools;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using util;

    class Client
    {
        //Logger object for storing general logs pertaining to the flow of operation
         Logger genLogs = new Logger("../../../LogResults/GeneralLogs/thLogs_" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".txt");
        public void getTestRequests()
        {
            try
            {
                Console.WriteLine("Fetching test requests");
                Console.WriteLine(Directory.GetCurrentDirectory());
                genLogs.log("Fetching test requests");
                //relative path of test requests
                string xmlRequestpath = "../../../Testrequests";
                Console.WriteLine("Test requests reside in {0}", xmlRequestpath);
                genLogs.log("Test requests reside in the following path :" + xmlRequestpath);
                //instantiating file manager
                FileManager manager = new FileManager();
                //set test request path
                manager.Path = xmlRequestpath;
                //storing test request names in arrays
                Console.WriteLine("Fetching XML file names");
                genLogs.log("Fetching XML file names");
                string[] testRequests = manager.getFiles();
                if (testRequests == null)
                {
                    Console.WriteLine("There are currently no test requests");
                    genLogs.log("There are currently no test requests");
                }
                else
                {
                    Console.WriteLine("There are currently {0} test requests", testRequests.Length);
                    genLogs.log("There are currently" + " " + testRequests.Length.ToString() + " " + "tests");
                }
                Console.WriteLine("The test requests that were found in path {0} are:", xmlRequestpath);
                genLogs.log("The test requests that were found in path" + " " + xmlRequestpath + " " + "are");
                for (int i = 0; i < testRequests.Length; i++)
                {
                    Console.WriteLine("i." + " " + testRequests[i]);
                    genLogs.log("i" + " " + testRequests[i]);
                }
                Client client = new Client();
                //sending test requests to queue member function
                client.queueRequests(testRequests);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in client");
            }
        }
    
        public void queueRequests(string[] testRequests)
        {
            XDocument doc_;
            BlockingQueue<XDocument> bqueue = new BlockingQueue<XDocument>();
            FileStream stream = null; ;
            Console.WriteLine("Creating XML document objects");
            genLogs.log("Creating XDocument objects");
            foreach (string request in testRequests)
            {
                try
                {
                    //establishing a stream for the XML test request file
                    stream = new FileStream(request, FileMode.Open);
                    doc_ = XDocument.Load(stream);
                    genLogs.log(doc_.ToString());
                    Console.WriteLine();
                    //Enqueueing of test requests
                    /*The blocking queue does not allow dequeue operation during enqueueing process
                     * or until the queue is full
                     */
                    //The thread that is loading tests holds a lock lock to the queue
                    //The lock is released only when enqueuing is completed
                    bqueue.enQ(doc_);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine("Stream could not be established");
                    genLogs.log("Stream could not be established");
                }
                catch (XmlException ex)
                {
                    Console.WriteLine("Improper XML phrasing");
                    genLogs.log("XML exception due to improper XML phraseing");
                }
                finally
                {
                    stream.Close();
                }
            }
            TestExecutive tEx = new TestExecutive();
            tEx.initiateTestOperation(bqueue,genLogs);

        }
        //stub for client
        public static void Main(string[] args)
        {
            Console.WriteLine("Initiating client request");
            Client cl = new Client();
            cl.getTestRequests();
            Console.Read();
        }
    }
}
