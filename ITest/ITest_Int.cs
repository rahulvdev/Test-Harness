/////////////////////////////////////////////////////////////////////////////
//  ITest_Int.cs- Provides an interface to various test drivers            //
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
 *   This module declares the test() and getLog() methods and provides an interface to 
 *   each of the test drivers implementing it
 *  
 *   Public Interface
 *   ----------------
 *   abstract string getLog();
 *   abstract void test();
 * 
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:  ITest_Int.cs
 *   - Compiler command: csc ITest_Int.cs 
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

namespace ITest
{
    public interface ITest_Int
    {
        //Abstract method that is implemented to generate log results
        string getLog();
        //this method runs the test code
        bool test();
        }
    }

