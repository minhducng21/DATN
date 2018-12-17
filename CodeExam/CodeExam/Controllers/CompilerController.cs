﻿using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeExam.Models;
using CodeExam.ViewModels;

namespace CodeExam.Controllers
{
    public class CompilerController : Controller
    {
        private CodeWarDbContext db = new CodeWarDbContext();
        private CodeDomProvider GetCurrentProvider()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            return provider;
        }
        static string[] listRef = { "System", "System.Collections.Generic", "Newtonsoft.Json" };

        public ActionResult GenFileCSharp(string source, int taskId)
        {
            var task = db.Tasks.FirstOrDefault(f => f.TaskId == taskId);
            var listTestCases = db.TestCases.Where(w => w.TaskId == task.TaskId);
            List<string> listDataType = new List<string>();
            var listParam = task.Input.Split(';');
            foreach (var item in listParam)
            {
                listDataType.Add((item.Split(':'))[0]);
            }
            string contentFile = "";
            foreach (var item in listRef)
            {
                contentFile += "using " + item + ";";
            }
            contentFile += "namespace CodeWar{ public class CodeWarXDA{" + source;
            contentFile += "public static void Main(string[] args){try{";
            int idx = 0;
            foreach (var item in listTestCases)
            {
                contentFile += "if(args[0] ==\"" + idx.ToString() + "\"){System.Console.WriteLine(JsonConvert.SerializeObject(" + task.TaskName + "(";

                var listTestCase = item.Input.Split(';');
                for (int i = 0; i < listDataType.Count; i++)
                {
                    switch (listDataType[i])
                    {
                        case "arrayofint":
                            contentFile += listTestCase[i].Trim().Replace("[", "new int[] {").Replace("]", "}");
                            break;
                        case "arrayoflong":
                            contentFile += listTestCase[i].Trim().Replace("[", "new long[] {").Replace("]", "}");
                            break;
                        case "arrayofbool":
                            contentFile += listTestCase[i].Trim().Replace("[", "new bool[] {").Replace("]", "}");
                            break;
                        case "arrayoffloat":
                            contentFile += listTestCase[i].Trim().Replace("[", "new float[] {").Replace("]", "}");
                            break;
                        case "arrayofstring":
                            contentFile += listTestCase[i].Trim().Replace("[", "new string[] {").Replace("]", "}");
                            break;
                        case "arrayofchar":
                            contentFile += listTestCase[i].Trim().Replace("[", "new char[] {").Replace("]", "}");
                            break;
                        case "matrixofint":
                            contentFile += "new int[][] " + listTestCase[i].Trim().Substring(1).Replace("[", "new int[] {").Replace("]", "}");
                            break;
                        case "matrixoflong":
                            contentFile += "new long[][] " + listTestCase[i].Trim().Substring(1).Replace("[", "new long[] {").Replace("]", "}");
                            break;
                        case "matrixofbool":
                            contentFile += "new bool[][] " + listTestCase[i].Trim().Substring(1).Replace("[", "new bool[] {").Replace("]", "}");
                            break;
                        case "matrixoffloat":
                            contentFile += "new float[][] " + listTestCase[i].Trim().Substring(1).Replace("[", "new float[] {").Replace("]", "}");
                            break;
                        case "matrixofstring":
                            contentFile += "new string[][] " + listTestCase[i].Trim().Substring(1).Replace("[", "new string[] {").Replace("]", "}");
                            break;
                        case "matrixofchar":
                            contentFile += "new char[][] " + listTestCase[i].Trim().Substring(1).Replace("[", "new char[] {").Replace("]", "}");
                            break;
                        default:
                            contentFile += listTestCase[i].Trim();
                            break;
                    }
                    contentFile += ",";
                }
                contentFile = contentFile.TrimEnd(',');
                contentFile += ")));}";
                idx++;
            }
            contentFile += "}catch(Exception ex){System.Console.WriteLine(ex.StackTrace.ToString());}}}}";
            using (StreamWriter writetext = new StreamWriter(Server.MapPath("~") + "\\SourceCode\\csharp_" + task.TaskId + "_1.cs"))
            {
                writetext.WriteLine(contentFile);
            }
            return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenFileJs(string source, int taskId)
        {
            var task = db.Tasks.FirstOrDefault(f => f.TaskId == taskId);
            var listTestCases = db.TestCases.Where(w => w.TaskId == task.TaskId);
            string contentFile = "";
            contentFile += source.Replace("Console", @"//Console.log").Replace("console", @"//console.log").Replace("alert", @"//alert").Replace("Alert", @"//Alert");
            int idx = 0;
            foreach (var item in listTestCases)
            {
                contentFile += "if(process.argv[2] ==\"" + idx.ToString() + "\"){console.log(JSON.stringify(" + task.TaskName + "(";

                var listTestCase = item.Input.Split(';');
                foreach (var items in listTestCase)
                {
                    contentFile += HttpUtility.UrlDecode(items) + ",";

                }
                contentFile = contentFile.TrimEnd(',');
                contentFile += ")));}";
                idx++;
            }
            using (StreamWriter writetext = new StreamWriter(Server.MapPath("~") + "\\SourceCode\\js_" + task.TaskId + "_" + User.Identity.Name + ".js"))
            {
                writetext.WriteLine(contentFile);
            }
            return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CompileCodeCSharp(int taskId)
        {
            string sourceFile = "csharp_" + taskId + "_1.cs";
            string exeFile = "csharp_" + taskId + "_1.exe";
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            // Configure a CompilerParameters that links System.dll
            // and produces the specified executable file.
            String[] referenceAssemblies = { "System.dll", "Newtonsoft.Json.dll" };
            CompilerParameters cp = new CompilerParameters(referenceAssemblies,
                                                           exeFile, false);
            // Generate an executable rather than a DLL file.
            cp.GenerateExecutable = true;
            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            // Return the results of compilation.
            string errMsg = "";
            bool isSuccess = true;
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                errMsg = "Errors encountered while building " +
                                sourceFile + " into " + cr.PathToAssembly + ": \r\n\n";
                foreach (CompilerError ce in cr.Errors)
                {
                    errMsg += ce.ToString() + "\r\n";
                }
                isSuccess = false;
            }

            return Json(new { status = isSuccess, message = errMsg }, JsonRequestBehavior.AllowGet);
        }
        private ActionResult RunCSharp(int taskId)
        {
            var listTestCase = db.TestCases.Where(w => w.TaskId == taskId).ToList();
            var testCaseCount = listTestCase.Count;
            RunResult runResult = new RunResult();
            for (int i = 0; i < testCaseCount; i++)
            {
                TestCaseResult item = new TestCaseResult();
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "\""+Server.MapPath("~") + "\\SourceCode\\csharp_" + taskId + "_" + User.Identity.Name + ".exe\"",
                        Arguments = i.ToString(),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false,
                        RedirectStandardError = true
                    }
                };
                proc.Start();
                if (!proc.WaitForExit(3000))
                {
                    runResult.errMsg = "Out of time";
                    runResult.isSuccess = false;
                    proc.Kill();
                    break;
                }
                if (!proc.StandardOutput.EndOfStream)
                {
                    item.Result = proc.StandardOutput.ReadLine();
                    item.CompareExpection = item.Result == listTestCase[i].Output;
                    runResult.detail.Add(item);
                }
                if (!proc.StandardError.EndOfStream)
                {
                    runResult.errMsg += proc.StandardError.ReadToEnd();
                    runResult.isSuccess = false;
                    break;
                }
            }
            return Json(runResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RunJS(int taskId)
        {
            var listTestCase = db.TestCases.Where(w => w.TaskId == taskId).ToList();
            var testCaseCount = listTestCase.Count;
            RunResult runResult = new RunResult();
            for (int i = 0; i < testCaseCount; i++)
            {
                TestCaseResult item = new TestCaseResult();
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/C Node \"" + Server.MapPath("~") + "SourceCode\\js_" + taskId + "_" + User.Identity.Name + ".js\" " + i.ToString(),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false,
                        RedirectStandardError = true
                    }
                };
                proc.Start();
                if (!proc.WaitForExit(3000))
                {
                    runResult.errMsg = "Out of time";
                    runResult.isSuccess = false;
                    proc.Kill();
                    break;
                }
                if (!proc.StandardOutput.EndOfStream)
                {
                    item.Result = proc.StandardOutput.ReadLine();
                    item.CompareExpection = item.Result == listTestCase[i].Output;
                    runResult.detail.Add(item);
                }
                if (!proc.StandardError.EndOfStream)
                {
                    runResult.errMsg += proc.StandardError.ReadToEnd();
                    runResult.isSuccess = false;
                    break;
                }
            }
            return Json(runResult, JsonRequestBehavior.AllowGet);
        }
    }
}