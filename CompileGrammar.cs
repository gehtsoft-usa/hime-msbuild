using System;
using System.Collections.Generic;
using Hime.SDK;
using Hime.SDK.Output;

namespace Hime.Build.Task
{
    public class CompileGrammar : Microsoft.Build.Utilities.Task
    {
        public string GrammarFile { get; set; }
        public string GrammarName { get; set; }
        public string Namespace { get; set; }
        public string CodeAccess { get; set; } = "Public";
        public string OutputMode { get; set; } = "Assembly";
        public string OutputPath { get; set; }

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(GrammarFile))
            {
                Log.LogError("The grammar file name is not set");
                return false;
            }

            if (string.IsNullOrEmpty(Namespace))
            {
                Log.LogError("The namespace is not set");
                return false;
            }

            if (string.IsNullOrEmpty(GrammarName))
            {
                Log.LogError("The grammar name is not set");
                return false;
            }

            if (string.IsNullOrEmpty(OutputPath))
            {
                Log.LogError("The output assembly path is not set");
                return false;
            }

            Mode outputMode;
            switch (OutputMode)
            {
                case "Assembly":
                case "assembly":
                    outputMode = Mode.Assembly;
                        break;
                case "Source":
                case "source":
                    outputMode = Mode.Source;
                    break;
                case "SourceAndAssembly":
                case "source-and-assembly":
                    outputMode = Mode.SourceAndAssembly;
                    break;
                default:
                    Log.LogError("The output mode is incorrect, it should be Assembly, Source or SourceAndAssembly");
                    return false;
            }

            Modifier codeAccess;
            switch (CodeAccess)
            {
                case "Public":
                case "public":
                    codeAccess = Modifier.Public;
                    break;
                case "Internal":
                case "internal":
                    codeAccess = Modifier.Public;
                    break;
                default:
                    Log.LogError("The code access mode is incorrect, it should be either Public or Internal");
                    return false;
            }

            CompilationTask task = new CompilationTask();
            task.AddInputFile(GrammarFile);
            task.CodeAccess = codeAccess;
            task.GrammarName = GrammarName;
            task.Mode = outputMode;
            task.OutputPath = OutputPath;
            task.Namespace = Namespace;
            Report report = task.Execute();
            if (report.Errors.Count == 0)
            {
                Log.LogMessage($"Grammar {GrammarName} compiled succesfully");
                return true;
            }

            Log.LogMessage($"Grammar {GrammarName} compilation failed");
            for (int i = 0; i < report.Errors.Count; i++)
            {
                Log.LogError($"Error in {GrammarFile} : {report.Errors[i].ToString()}");
            }
            return false;
        }
    }
}
