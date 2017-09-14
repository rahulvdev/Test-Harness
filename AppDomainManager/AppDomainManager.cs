/////////////////////////////////////////////////////////////////////////////
//  AppDomainManager.cs -   Creates a child appDomain                      //
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
 *   The AppDomain manager creates a child appdomain for the given specifications and returns
 *   a handle to the appDomain back to the loader. 
 *   
 *  
 *   Public Interface
 *   ----------------
 *   AppDomainManager adm=new AppDomainManager();
 *   AppDomain domainCreator();
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  AppDomainManager.cs
 *   - Compiler command: csc AppDomainManager.cs 
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
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppDomainManager
{
    public class AppDomainManager
    {
        public AppDomain domainCreator()
        {
            AppDomain main = AppDomain.CurrentDomain;
            Console.Write("\n  Starting in AppDomain {0}\n", main.FriendlyName);

            // Create application domain setup information for new AppDomain
            AppDomainSetup domaininfo = new AppDomainSetup();
            // defines search path for assemblies
            domaininfo.ApplicationBase = "file:///" + System.Environment.CurrentDirectory;

            //Create evidence for the new AppDomain from evidence of current
            Evidence adevidence = AppDomain.CurrentDomain.Evidence;

            // Create Child AppDomain
            AppDomain ad= AppDomain.CreateDomain("ChildDomain", adevidence, domaininfo);
            return ad;
        }

        //stub for AppdomainManager
        public static void main(string[] args) {
            Console.WriteLine("Initiating child AppDomain creation");
            AppDomainManager man = new AppDomainManager();
            AppDomain ad=man.domainCreator();
            Console.WriteLine("The friendly name of the child appdomain is :");
            Console.WriteLine(ad.FriendlyName);
        }
    }
}

