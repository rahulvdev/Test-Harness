/////////////////////////////////////////////////////////////////////////////
//  Loader.cs -   Loads appropriate drivers and test code modules          //
//                into the appdomain                                       //
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
 *   The Loader receives the XML test request in the form of a string variant.
 *   It parses this string variant back to its orginal type.i.e.XDocument.
 *   The loader loads appropriate drivers and test code assemblies into the childAppdomain and 
 *   initiates testing.At every stage the loader uses the general and test log objects to save execution 
 *   states into log files that may be referred to after testing.
 *   
 *  
 *   Public Interface
 *   ----------------
 *   Loader ldr=new Loader();
 *   void loadTests(string xmlDoc, Logger testlogs, Logger generalLogs);
 *   ITest.ITest_Int loadDriver(XMLTest test,Logger genLogs,Logger testLogs);
 *   void run(ITest.ITest_Int testDriver, string nameOfDriver,Logger genLogs,Logger testLogs);
 *   void parseResultCheck(List<XMLTest> testList,Logger generalLogs,Logger testlogs);
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  Loader.cs,XMLTestParser.cs,TestDriver1.cs,TestDriver2.cs,TestCode1.cs,TestCode2.cs,TestCode3.cs,Logger.cs
 *   - Compiler command: csc Loader.cs,XMLTestParser.cs,TestDriver1.cs,TestDriver2.cs,TestCode1.cs,TestCode2.cs,TestCode3.cs,Logger.cs 
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using util;

namespace Loader
{
    public class Loader:MarshalByRefObject
    {
        private string testPath = "../../../Repository/";
        private string testDriverLogPath = "../../../LogResults/Testlogs/";

        //property accessors for test path and test log path
        public string TestPath { get; set; }
        public string TestDriverLogPath { get; set; }

        public void loadTests(string xmlDoc, Logger testlogs, Logger generalLogs)
        {
            testlogs.log("Test drivers and test code repository path is :" + testPath);
            generalLogs.log("Test drivers and test code repository path is :" + testPath);
            generalLogs.log("Loader is executing in " + AppDomain.CurrentDomain.FriendlyName);
            XDocument testRequest;
            List<XMLTest> testList = null;
            try
            {
                if (xmlDoc == null)
                {
                    testlogs.log("The test request is empty");
                    generalLogs.log("The test request is empty");
                }
                //Parsing string vaiant of XML file back to XDocument
                else
                {
                    testlogs.log("Parsing string variant of XML file back to XDocument object");
                    testRequest = XDocument.Parse(xmlDoc);
                    Parser parser = new Parser();
                    //initiating XML parse
                    //parse method returns a list of all the tests available in the given test request
                    testList = parser.parse(testRequest);
                    parseResultCheck(testList, generalLogs, testlogs);
                }
            }
            catch (Exception ex)
            {
                generalLogs.log("Exception occured while parsing into XDocument type");
                testlogs.log("Exception occured while parsing into XDocument type");
            }
        }
            
        public ITest.ITest_Int loadDriver(XMLTest test,Logger genLogs,Logger testLogs)
        {
            ITest.ITest_Int testDriver = null;
            try
            {
                Assembly assem = null;
                Type[] types = null;
                List<string> testCode = test.testCode;
                //iterates over each libraries required for the test case
                foreach (string tCode in testCode)
                {
                    try
                    {
                        // loads test code assembly
                        assem = Assembly.LoadFrom(testPath + tCode);
                        genLogs.log(tCode+" "+"loaded");
                        testLogs.log(tCode+" "+"loaded");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Test code could not be found in repository");
                        genLogs.log("Test code could not be found in repository");
                    }
                }
                try
                {
                    // loads the test driver asssembly
                    assem = Assembly.LoadFrom(testPath + test.testDriver);
                    genLogs.log(test.testDriver + " " + "loaded");
                    testLogs.log(test.testDriver + " " + "loaded");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to load driver from repository");
                    genLogs.log("Unable to load driver from repository");
                }
                types = assem.GetExportedTypes();
                //Iterates through each of the types until driver is encountered
                foreach (Type t in types)
                {
                    // checks if the type derives from ITest.ITest_Int
                    if (t.IsClass && typeof(ITest.ITest_Int).IsAssignableFrom(t))
                        // create instance of test driver
                        testDriver = (ITest.ITest_Int)Activator.CreateInstance(t);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Loader");
                genLogs.log("Exception in loader");
                testLogs.log("Exception in loader");
                testDriver = null;
            }
            return testDriver;
        }

        public void run(ITest.ITest_Int testDriver, string nameOfDriver,Logger genLogs,Logger testLogs)
        {
            try
            {
                Console.Write(" Executing test code invoked by test driver   {0}   in Domain   {1} ", nameOfDriver, AppDomain.CurrentDomain.FriendlyName);
                genLogs.log("Executing test code invoked by test driver " + nameOfDriver + "in" + AppDomain.CurrentDomain.FriendlyName);
                testLogs.log("Executing test code invoked by test driver" + nameOfDriver + "in" + AppDomain.CurrentDomain.FriendlyName);
                if (testDriver.test() == true)
                {
                    Console.WriteLine("\n  test passed");
                    genLogs.log("test passed");
                    testLogs.log("test passed");
                }
                else
                {
                    Console.WriteLine("\n  test failed");
                    genLogs.log("test failed");
                    testLogs.log("test failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("Exception occured in Driver {0}", nameOfDriver);
                genLogs.log("Exception occured in " + nameOfDriver);
                testLogs.log("Exception occured in " + nameOfDriver);
            }
        }

        public void parseResultCheck(List<XMLTest> testList,Logger generalLogs,Logger testlogs) {
            //check if list is empty
            //If not,loop through each test and recover driver and test code details
            if ((testList != null) && (testList.Count != 0))
            {
                testlogs.nameOfFile = testDriverLogPath + testList[0].author + "_" + System.Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".txt";
                //iterating over each of the tests
                for (int i = 0; i < testList.Count; i++)
                {
                    testlogs.log("Author :" + testList[i].author);
                    testlogs.log("Test Name : " + testList[i].testName);
                    testlogs.log("Test Time : " + testList[i].timeStamp.ToString());
                    //loading tests and returning handle to driver object
                    ITest.ITest_Int driver = loadDriver(testList[i], generalLogs, testlogs);
                    if (driver != null)
                    {
                        run(driver, testList[i].testDriver, generalLogs, testlogs);
                        string logFromTestImpl = driver.getLog();
                        if (logFromTestImpl != null)
                        {
                            testlogs.log("Logs from test number " + (i+1).ToString());
                            testlogs.log(logFromTestImpl);
                            testlogs.log("End of logs from test number " + (i+1).ToString());
                        }
                        else
                        {
                            testlogs.log("No logs were generated from the test");
                        }
                    }
                    else
                    {
                        Console.WriteLine("driver does not exist");
                        testlogs.log("Driver does not exist");
                    }
                }
            }
            else
                generalLogs.log("There are no tests at the moment");
        }


        //stub for loader
        public static void main(string[] args) {
            Console.WriteLine("Initiating loader operation");
            XDocument sampleDoc = new XDocument(new XComment("This is a comment"),
                               new XElement("RootNode", new XElement("ChildNode1", "Value1"),
                               new XElement("ChildNode2", "Value2"), new XElement("ChildNode3", "Value"),
                               new XElement("ChildNode4", "Value4")));
            string doc_ = sampleDoc.ToString();
            Loader ldr = new Loader();
            Logger genLogs = new Logger("../../../ LogResults / GeneralLogs / thLogs_" + DateTime.Now.ToString().Replace(" / ", " - ").Replace(":", " - ") + ".txt");
            Logger testExecLog = new Logger();
            ldr.loadTests(doc_, genLogs, testExecLog);
            }
    }
}
