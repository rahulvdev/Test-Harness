/////////////////////////////////////////////////////////////////////////////
//  Logger.cs-   Writes log messages to text files                        //
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
 *   The main motivation of the logger is for writing test harness and test code execution details 
 *   into corresponding text files.
 *  
 *   Public Interface
 *   ----------------
 *   Logger logger=new Logger();
 *   void log(String text);
 * 
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  Logger.cs
 *   - Compiler command: csc Logger.cs
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public class Logger : MarshalByRefObject
    {
        public string nameOfFile { get; set; }

        public Logger(String name)
        {
            nameOfFile = name;
        }
        //empty constructor
        //for test logs, file name will be available after parsing XML
        public Logger() {
        }

        // Appending text to the file during every call to the method
        public void log(String text)
        {
            if (nameOfFile != null)
            {
                //System.IO.StreamWriter(string path,bool append) variant
                using (StreamWriter file = new StreamWriter(nameOfFile, true))
                {
                    file.WriteLine(DateTime.Now + " : " + text);
                }
            }
        }

        //stub for logger
        public static void main(string[] args) {
            Console.WriteLine("Creating Logger instance");
            Logger lgr = new Logger("../../../ LogResults / GeneralLogs / thLogs_" + DateTime.Now.ToString().Replace(" / ", " - ").Replace(":", " - ") + ".txt");
            lgr.log("Testing logger module");
        }
    }
}
