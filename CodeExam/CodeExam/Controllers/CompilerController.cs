using Newtonsoft.Json;
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
        static string[] listRef = { "System", "System.Collections.Generic", "Newtonsoft.Json" };
        public ActionResult GenerateTemplateCode(int taskId, string language)
        {
            string source = "";
            var userId = Constant.Constant.GetUserIdByIdentity(User.Identity.Name);
            var leaderBoardItem = db.LeaderBoards.FirstOrDefault(f => f.TaskId == taskId && f.UserId == userId);
            if (leaderBoardItem != null)
            {
                source = leaderBoardItem.SourceCode;
            }
            else
            {
                var itemTask = db.Tasks.FirstOrDefault(f => f.TaskId == taskId);
                if (language == "csharp")
                {
                    source += "static ";
                    switch (itemTask.OutputType)
                    {
                        case "integer":
                            source += "int";
                            break;
                        case "arrayofint":
                            source += "int[]";
                            break;
                        case "arrayoflong":
                            source += "long[]";
                            break;
                        case "arrayofbool":
                            source += "bool[]";
                            break;
                        case "arrayoffloat":
                            source += "float[]";
                            break;
                        case "arrayofstring":
                            source += "string[]";
                            break;
                        case "arrayofchar":
                            source += "char[]";
                            break;
                        case "matrixofint":
                            source += "int[][]";
                            break;
                        case "matrixoflong":
                            source += "long[][]";
                            break;
                        case "matrixofbool":
                            source += "bool[][]";
                            break;
                        case "matrixoffloat":
                            source += "float[][]";
                            break;
                        case "matrixofstring":
                            source += "stirng[][]";
                            break;
                        case "matrixofchar":
                            source += "char[][]";
                            break;
                        default:
                            break;
                    }
                    source += " " + itemTask.TaskName + "(";
                    foreach (var item in itemTask.Input.TrimEnd(';').Split(';'))
                    {
                        switch (item.Split(':')[1])
                        {
                            case "integer":
                                source += "int";
                                break;
                            case "arrayofint":
                                source += "int[]";
                                break;
                            case "arrayoflong":
                                source += "long[]";
                                break;
                            case "arrayofbool":
                                source += "bool[]";
                                break;
                            case "arrayoffloat":
                                source += "float[]";
                                break;
                            case "arrayofstring":
                                source += "string[]";
                                break;
                            case "arrayofchar":
                                source += "char[]";
                                break;
                            case "matrixofint":
                                source += "int[][]";
                                break;
                            case "matrixoflong":
                                source += "long[][]";
                                break;
                            case "matrixofbool":
                                source += "bool[][]";
                                break;
                            case "matrixoffloat":
                                source += "float[][]";
                                break;
                            case "matrixofstring":
                                source += "stirng[][]";
                                break;
                            case "matrixofchar":
                                source += "char[][]";
                                break;
                            default:
                                break;
                        }
                        source += " " + item.Split(':')[0] + ",";
                    }
                    source = source.TrimEnd(',');
                    source += ")\n{\n\n}";
                }
                else
                {
                    source += "function " + itemTask.TaskName + "(";
                    foreach (var item in itemTask.Input.TrimEnd(';').Split(';'))
                    {
                        source += item.Split(':').FirstOrDefault() + ",";
                    }
                    source = source.TrimEnd(',');
                    source += ")";
                    source += "{\n\n}";
                }
            }
            return Json(source, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenFileAndRun(string source, int taskId, string language, bool isSubmit = false)
        {
            if (language == "csharp")
            {
                return GenFileCsharp(source, taskId, isSubmit);
            }
            else
            {
                return GenFileJs(source, taskId, isSubmit);
            }
        }
        public ActionResult GenFileCsharp(string source, int taskId, bool isSubmit)
        {
            try
            {
                var task = db.Tasks.FirstOrDefault(f => f.TaskId == taskId);
                var listTestCases = db.TestCases.Where(w => w.TaskId == task.TaskId);
                List<string> listDataType = new List<string>();
                var listParam = task.Input.TrimEnd(';').Split(';');
                foreach (var item in listParam)
                {
                    listDataType.Add((item.Split(':'))[1]);
                }
                string contentFile = "";
                contentFile += "namespace CodeWar{";

                foreach (var item in listRef)
                {
                    contentFile += "using " + item + ";";
                }
                contentFile += "public class CodeWarXDA{" + source;
                contentFile += "public static void Main(string[] args){try{";
                int idx = 0;
                foreach (var item in listTestCases)
                {
                    contentFile += "if(args[0] ==\"" + idx.ToString() + "\"){System.Console.WriteLine(JsonConvert.SerializeObject(" + task.TaskName + "(";

                    var listTestCase = item.Input.TrimEnd(';').Split(';');
                    for (int i = 0; i < listDataType.Count; i++)
                    {
                        switch (listDataType[i])
                        {
                            case "arrayofint":
                                contentFile += HttpUtility.UrlDecode(listTestCase[i].Trim().Replace("[", "new int[] {").Replace("]", "}"));
                                break;
                            case "arrayoflong":
                                contentFile += HttpUtility.UrlDecode(listTestCase[i].Trim().Replace("[", "new long[] {").Replace("]", "}"));
                                break;
                            case "arrayofbool":
                                contentFile += HttpUtility.UrlDecode(listTestCase[i].Trim().Replace("[", "new bool[] {").Replace("]", "}"));
                                break;
                            case "arrayoffloat":
                                contentFile += HttpUtility.UrlDecode(listTestCase[i].Trim().Replace("[", "new float[] {").Replace("]", "}"));
                                break;
                            case "arrayofstring":
                                contentFile += HttpUtility.UrlDecode(listTestCase[i].Trim().Replace("[", "new string[] {").Replace("]", "}"));
                                break;
                            case "arrayofchar":
                                contentFile += HttpUtility.UrlDecode(listTestCase[i].Trim().Replace("[", "new char[] {").Replace("]", "}"));
                                break;
                            case "matrixofint":
                                contentFile += "new int[][] " + HttpUtility.UrlDecode(listTestCase[i].Trim().Substring(1).Replace("[", "new int[] {").Replace("]", "}"));
                                break;
                            case "matrixoflong":
                                contentFile += "new long[][] " + HttpUtility.UrlDecode(listTestCase[i].Trim().Substring(1).Replace("[", "new long[] {").Replace("]", "}"));
                                break;
                            case "matrixofbool":
                                contentFile += "new bool[][] " + HttpUtility.UrlDecode(listTestCase[i].Trim().Substring(1).Replace("[", "new bool[] {").Replace("]", "}"));
                                break;
                            case "matrixoffloat":
                                contentFile += "new float[][] " + HttpUtility.UrlDecode(listTestCase[i].Trim().Substring(1).Replace("[", "new float[] {").Replace("]", "}"));
                                break;
                            case "matrixofstring":
                                contentFile += "new string[][] " + HttpUtility.UrlDecode(listTestCase[i].Trim().Substring(1).Replace("[", "new string[] {").Replace("]", "}"));
                                break;
                            case "matrixofchar":
                                contentFile += "new char[][] " + HttpUtility.UrlDecode(listTestCase[i].Trim().Substring(1).Replace("[", "new char[] {").Replace("]", "}"));
                                break;
                            default:
                                contentFile += HttpUtility.UrlDecode(listTestCase[i].Trim());
                                break;
                        }
                        contentFile += ",";
                    }
                    contentFile = contentFile.TrimEnd(',');
                    contentFile += ")));}";
                    idx++;
                }
                contentFile += "}catch(Exception ex){System.Console.WriteLine(ex.StackTrace.ToString());}}}}";
                using (StreamWriter writetext = new StreamWriter("csharp_" + task.TaskId + "_" + Constant.Constant.GetUserIdByIdentity(User.Identity.Name) + ".cs"))
                {
                    writetext.WriteLine(contentFile);
                }
                return CompileCodeCSharp(taskId, source, isSubmit);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GenFileJs(string source, int taskId, bool isSubmit)
        {
            try
            {
                var task = db.Tasks.FirstOrDefault(f => f.TaskId == taskId);
                var listTestCases = db.TestCases.Where(w => w.TaskId == task.TaskId);
                string contentFile = "";
                contentFile += source.Replace("Console", @"//Console.log").Replace("console", @"//console.log").Replace("alert", @"//alert").Replace("Alert", @"//Alert");
                int idx = 0;
                foreach (var item in listTestCases)
                {
                    contentFile += "if(process.argv[2] ==\"" + idx.ToString() + "\"){console.log(JSON.stringify(" + task.TaskName + "(";

                    var listTestCase = item.Input.TrimEnd(';').Split(';');
                    foreach (var items in listTestCase)
                    {
                        contentFile += HttpUtility.UrlDecode(items) + ",";

                    }
                    contentFile = contentFile.TrimEnd(',');
                    contentFile += ")));}";
                    idx++;
                }
                using (StreamWriter writetext = new StreamWriter("js_" + task.TaskId + "_" + Constant.Constant.GetUserIdByIdentity(User.Identity.Name) + ".js"))
                {
                    writetext.WriteLine(contentFile);
                }
                return Run(taskId, "js", source, isSubmit);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CompileCodeCSharp(int taskId, string source, bool isSubmit)
        {
            string sourceFile = "csharp_" + taskId + "_" + Constant.Constant.GetUserIdByIdentity(User.Identity.Name) + ".cs";
            string exeFile = "csharp_" + taskId + "_" + Constant.Constant.GetUserIdByIdentity(User.Identity.Name) + ".exe";
            CodeDomProvider provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
            // Configure a CompilerParameters that links System.dll
            // and produces the specified executable file.
            String[] referenceAssemblies = { "System.dll" };
            CompilerParameters cp = new CompilerParameters(referenceAssemblies,
                                                           exeFile, false);
            cp.ReferencedAssemblies.Add("Newtonsoft.Json.dll");
            // Generate an executable rather than a DLL file.
            cp.GenerateExecutable = true;
            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            // Return the results of compilation.
            RunResult runResult = new RunResult();
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                runResult.errMsg = "Errors encountered while building " +
                                sourceFile + " into " + cr.PathToAssembly + ": \r\n\n";
                foreach (CompilerError ce in cr.Errors)
                {
                    runResult.errMsg += ce.ToString() + "\r\n";
                }
                runResult.isSuccess = false;
            }
            if (runResult.isSuccess)
            {
                return Run(taskId, "csharp", source, isSubmit);
            }
            else
            {
                return Json(runResult, JsonRequestBehavior.AllowGet);
            }
        }
        private ActionResult Run(int taskId, string language, string source, bool isSubmit)
        {
            var listTestCase = db.TestCases.Where(w => w.TaskId == taskId).ToList();
            int totalTestCase = isSubmit ? listTestCase.Count : listTestCase.Count / 2;
            RunResult runResult = new RunResult();
            runResult.totalTestCase = totalTestCase;
            int success = 0;
            for (int i = 0; i < totalTestCase; i++)
            {
                TestCaseResult item = new TestCaseResult();
                var proc = new Process();
                if (language == "csharp")
                {
                    proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "csharp_" + taskId + "_" + Constant.Constant.GetUserIdByIdentity(User.Identity.Name) + ".exe",
                            Arguments = i.ToString(),
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = false,
                            RedirectStandardError = true
                        }
                    };
                }
                else if (language == "js")
                {
                    proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = "/C Node js_" + taskId + "_" + Constant.Constant.GetUserIdByIdentity(User.Identity.Name) + ".js " + i.ToString(),
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = false,
                            RedirectStandardError = true
                        }
                    };
                }
                proc.Start();
                if (!proc.WaitForExit(4000))
                {
                    runResult.errMsg = "Out of time";
                    runResult.isSuccess = false;
                    proc.Kill();
                    break;
                }
                if (!proc.StandardOutput.EndOfStream)
                {
                    if ((!isSubmit && i < totalTestCase) || (isSubmit && i < totalTestCase / 2))
                    {
                        item.Result = proc.StandardOutput.ReadLine();
                    }
                    item.CompareExpection = item.Result == listTestCase[i].Output;
                    if (item.CompareExpection)
                    {
                        success++;
                    }
                    runResult.detail.Add(item);
                }
                if (!proc.StandardError.EndOfStream)
                {
                    runResult.errMsg = proc.StandardError.ReadToEnd();
                    runResult.isSuccess = false;
                    break;
                }
            }
            if (runResult.isSuccess)
            {
                if (isSubmit)
                {
                    int point = 0;
                    switch (runResult.totalTestCase - success)
                    {
                        case 0:
                            point = runResult.totalPoint;
                            break;
                        case 1:
                            point = runResult.totalPoint * 3 / 4;
                            break;
                        case 2:
                            point = runResult.totalPoint / 2;
                            break;
                        case 3:
                            point = runResult.totalPoint / 4;
                            break;
                        default:
                            point = 0;
                            break;
                    }
                    runResult.successPoint = point;
                }
                runResult.successTestCase = success;
            }
            return Json(runResult, JsonRequestBehavior.AllowGet);
        }
    }
}