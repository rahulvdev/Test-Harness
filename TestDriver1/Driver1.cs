/////////////////////////////////////////////////////////////////////////////
//  Driver1.cs-   Tests aspects of TestCode1 and TestCode2                 //
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
 *  The TestDriver1 class implements the ITest_Int interface and defines the test() and getLog() methods.
 *  The test() method creates objects of TestCode1.Calculator and TestCode2.Calculator and checks if the 
 *  arithmetic operation on the operands are valid or not.If a divide by zero exception occurs, the message 
 *  is reported back to the driver and the test fails.If not, the test passes.
 *  
 *   Public Interface
 *   ----------------
 *   Driver1 dr1=new Driver1();
 *   void test();
 *   string getLog();
 * 
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  TestDriver1.cs,TestCode1.cs,TestCode2.cs
 *   - Compiler command: csc TestDriver1.cs,TestCode1.cs,TestCode2.cs
 * 
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 9th October 2016
 *     - first release
 * 
 */
//

using ITest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDriver1
{

    public class Driver1 : ITest_Int
    {
        StringBuilder strBuild = new StringBuilder();
        public string getLog()
        {
            string codeGeneratedLog;
            if (strBuild.ToString().Length == 0)
            {
                codeGeneratedLog = null;
            }
            else
            {
                codeGeneratedLog = strBuild.ToString();
            }
            return codeGeneratedLog;
        }

        public bool test()
        {
            bool testResult = false;
            TestCode1.CalculatorTc1 calcTc1 = new TestCode1.CalculatorTc1();
            //Console.WriteLine("Test on TestCode1 will fail if there is a divide by zero exception");
            testResult = calcTc1.exceptionMonitor('/',5,0);
            TestCode2.CalculatorTc2 calcTc2 = new TestCode2.CalculatorTc2();
            strBuild = calcTc2.calculationFlow('/', 13, 0);
            return testResult;
        }

        //stub for driver
        public static void main(string[] args) {
            Console.WriteLine("Invoking test method from the driver");
            Driver1 d1 = new Driver1();
            d1.test();
            Console.WriteLine("Invoking getLog() method from the driver");
            string log=d1.getLog();
            Console.WriteLine("The log results are as follows: ");
            Console.WriteLine(log);
        }
    }
}
