/////////////////////////////////////////////////////////////////////////////
//  XMLTestParser.cs -   Parses XML files                                  //
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
 *   This module parses the incoming XML test request file to generate test information.
 *   
 *  
 *   Public Interface
 *   ----------------
 *   XMLTest xmlTest=new XMLTest();
 *   Parser par=new Parser();
 *   void show();
 *   List<XMLTest> parse(XDocument xml);
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  XMLTestParser.cs
 *   - Compiler command: csc XMLTestParser.cs 
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
using System.Xml.Linq;

namespace Loader
{
    public class XMLTest
    {
            public string testName { get; set; }
            public string author { get; set; }
            public DateTime timeStamp { get; set; }
            public String testDriver { get; set; }
            public List<string> testCode { get; set; }
            public void show()
            {
                Console.Write("\n  {0,-12} : {1}", "test name", testName);
                Console.Write("\n  {0,12} : {1}", "author", author);
                Console.Write("\n  {0,12} : {1}", "time stamp", timeStamp);
                Console.Write("\n  {0,12} : {1}", "test driver", testDriver);
                foreach (string library in testCode)
                {
                    Console.Write("\n  {0,12} : {1}", "library", library);
                }
            }
        }

        public class Parser
        {
        public List<XMLTest> parse(XDocument xml)
        {
            List<XMLTest> testList = null;
       
                if (xml == null)
                    return testList;
                string author = xml.Descendants("author").First().Value;
                XElement[] xtests = xml.Descendants("test").ToArray();
                if (xtests != null)
                {
                    testList = new List<XMLTest>();
                    int numTests = xtests.Count();
                    XMLTest test = null;
                    for (int i = 0; i < numTests; ++i)
                    {
                        test = new XMLTest();
                        test.testCode = new List<string>();
                        test.author = author;
                        test.timeStamp = DateTime.Now;
                        test.testName = xtests[i].Attribute("name") != null ? xtests[i].Attribute("name").Value : "No Name";
                        test.testDriver = xtests[i].Element("testDriver").Value;
                        IEnumerable<XElement> xtestCode = xtests[i].Elements("library");
                        //iterates over all library nodes in the xml
                        foreach (var xlibrary in xtestCode)
                        {
                            test.testCode.Add(xlibrary.Value);
                        }
                        //test.show();
                        testList.Add(test);
                    }
                }
            return testList;
            }

        //stub for XML parser
        public static void main(string[] args) {
            Console.WriteLine("A sample XDocument file is passed to the parser");
            XDocument sampleDoc = new XDocument(new XComment("This is a comment"),
                                new XElement("RootNode",new XElement("ChildNode1", "Value1"),
                                new XElement("ChildNode2", "Value2"),new XElement("ChildNode3", "Value"),
                                new XElement("ChildNode4", "Value4")));
            Parser par = new Parser();
            List<XMLTest> list=par.parse(sampleDoc);
            foreach (XMLTest test in list) {
                test.show();
                }
            }
        }
    }

