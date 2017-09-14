/////////////////////////////////////////////////////////////////////////////
//  FileManager.cs- Fetches the names of all xml test requests from        //
//                  the specified directory                                //
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
 *   The FileManager receives the path of the test requests and generates the names of all
 *   available files corresponding to the given file extension.The list of files are used by the client for
 *   test request enqueueing.
 *  
 *   Public Interface
 *   ----------------
 *   FileManager fMan=new FileManager();
 *   string[] getFiles();
 * 
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  FileManager.cs
 *   - Compiler command: csc FileManager.cs 
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

namespace FileManager
{
    public class FileManager
    {
        private string path;

        public string Path {
            get { return path; }
            set { path=value; }
        }

        public string[] getFiles()
        {
            
            //fetches the names of test requests
            string[] fileNames = Directory.GetFiles(Path, "*.xml",SearchOption.AllDirectories);
            if (fileNames.Length == 0) {
                return null;
            }
            else
            return fileNames;
        }


        //stub for filemanager 
        public static void main(string[] args) {
            string requestPath = "..\\..\\..\\Testrequests";
            FileManager fileManager = new FileManager();
            fileManager.Path = requestPath;
            string[] xmlFileNames = fileManager.getFiles();
            int noOfrequests = xmlFileNames.Length;
            for (int i = 0; i < noOfrequests; i++) {
                Console.WriteLine("Request number {0}= ",i,xmlFileNames[i]);
            }
            Console.WriteLine("The total number of test requests are: "+noOfrequests);
        }
    }   
}
