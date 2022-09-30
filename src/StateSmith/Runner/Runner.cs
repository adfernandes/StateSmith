using StateSmith.output.C99BalancedCoder1;
using StateSmith.output.UserConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateSmith.Compiling;
using System.IO;
using StateSmith.Input.Expansions;
using StateSmith.Input;
using StateSmith.compiler.Visitors;
using StateSmith.Input.PlantUML;
using StateSmith.compiler;

#nullable enable

namespace StateSmith.Runner
{
    /// <summary>
    /// Builds a single state machine
    /// </summary>
    public class SmRunner
    {
        RunnerSettings settings;
        Statemachine sm = new("non_null_dummy");
        Compiler compiler = new();
        ExceptionPrinter exceptionPrinter;

        protected HashSet<string> PlantUmlFileExtensions = new() { ".pu", ".puml", ".plantuml" };
        protected HashSet<string> YedFileExtensions = new() { ".graphml" };


        protected void OutputStageMessage(string message)
        {
            // todo add logger functionality
            Console.WriteLine("StateSmith Runner - " + message);
        }

        public SmRunner(RunnerSettings settings)
        {
            this.settings = settings;
            exceptionPrinter = settings.exceptionPrinter;
        }

        protected void RunInner()
        {
            RunCompiler();
            RunRest();
        }

        public void Run()
        {
            try
            {
                System.Console.WriteLine();
                RunInner();
                OutputStageMessage("finished normally.");
            }
            catch (System.Exception e)
            {
                if (settings.propagateExceptions)
                {
                    throw;
                }

                Environment.ExitCode = -1; // lets calling process know that code gen failed

                exceptionPrinter.PrintException(e);
                DumpErrorDetailsToFile(e);
                OutputStageMessage("finished with failure.");
            }

            System.Console.WriteLine();
        }

        private void DumpErrorDetailsToFile(Exception e)
        {
            var errorDetailFilePath = settings.diagramFile + ".err.txt";
            //errorDetailFilePath = Path.GetFullPath(errorDetailFilePath); // if you want the full path but with resolved "../../"
            errorDetailFilePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), errorDetailFilePath);
            exceptionPrinter.DumpExceptionDetails(e, errorDetailFilePath);
            Console.Error.WriteLine("Additional exception detail dumped to file: " + errorDetailFilePath);
        }

        protected void RunRest()
        {
            FindStateMachine();

            CodeGenContext codeGenContext = new(sm, settings.renderConfig);
            settings.mangler.SetStateMachine(sm);
            codeGenContext.mangler = settings.mangler;
            codeGenContext.style = settings.style;

            ConfigReader reader = new ConfigReader(codeGenContext.expander, expansionVarsPath: "self->vars.");
            reader.ReadObject(settings.renderConfig);

            CBuilder cBuilder = new(codeGenContext);
            cBuilder.Generate();

            CHeaderBuilder cHeaderBuilder = new(codeGenContext);
            cHeaderBuilder.Generate();

            string hFileContents = codeGenContext.hFileSb.ToString();
            string cFileContents = codeGenContext.cFileSb.ToString();

            File.WriteAllText($"{settings.outputDirectory}{settings.mangler.HFileName}", hFileContents);
            File.WriteAllText($"{settings.outputDirectory}{settings.mangler.CFileName}", cFileContents);
        }

        private void FindStateMachine()
        {
            if (settings.stateMachineName != null)
            {
                var action = () => { sm = (Statemachine)compiler.GetVertex(settings.stateMachineName); };
                action.RunOrWrapException((e) => new ArgumentException($"Couldn't find state machine in diagram with name `{settings.stateMachineName}`.", e));
            }
            else
            {
                var action = () => { sm = compiler.rootVertices.OfType<Statemachine>().Single(); };
                action.RunOrWrapException((e) => new ArgumentException($"State machine name not specified. Expected diagram to have find 1 Statemachine node at root level. Instead, found {compiler.rootVertices.OfType<Statemachine>().Count()}.", e));
            }

            OutputStageMessage($"Generating code for state machine `{sm.Name}`.");
        }

        private void OutputCompilingDiagramMessage()
        {
            string filePath = Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory + "../../../..", settings.diagramFile);
            filePath = filePath.Replace('\\', '/');
            OutputStageMessage($"Compiling file: `{filePath}` "
                + ((settings.stateMachineName == null) ? "(no state machine name specified)" : $"with target state machine name: `{settings.stateMachineName}`")
                + "."
            );
        }



        private void RunCompiler()
        {
            OutputCompilingDiagramMessage();
            CompileFile();
            compiler.SetupRoots();
            compiler.SupportParentAlias();
            compiler.Validate();

            compiler.SimplifyInitialStates();
            compiler.SupportEntryExitPoints();
            compiler.Validate();
            compiler.DefaultToDoEventIfNoTrigger();
            compiler.FinalizeTrees();
            compiler.Validate();
        }

        private void CompileFile()
        {
            string diagramFile = settings.diagramFile;
            var fileExtension = Path.GetExtension(diagramFile);

            if (InputFileIsYedFormat(fileExtension))
            {
                compiler.CompileYedFile(settings.diagramFile);
            }
            else if (InputFileIsPlantUML(fileExtension))
            {
                PlantUMLToNodesEdges translator = new();
                translator.ParseDiagramFile(settings.diagramFile);

                if (translator.HasError())
                {
                    string reasons = Compiler.ParserErrorsToReasonStrings(translator.GetErrors(), separator: "\n  - ");
                    throw new FormatException("PlantUML input failed parsing. Reason(s):\n  - " + reasons);
                }

                compiler.CompileDiagramNodesEdges(new List<DiagramNode> { translator.Root }, translator.Edges);
            }
            else
            {
                throw new ArgumentException($"Unsupported file extension `{fileExtension}`. \n  - yEd supports: {string.Join(", ", YedFileExtensions)}\n  - PlantUML supports: {string.Join(", ", PlantUmlFileExtensions)}");
            }
        }


        private bool InputFileIsPlantUML(string fileExtension)
        {
            return PlantUmlFileExtensions.Contains(fileExtension);
        }

        private bool InputFileIsYedFormat(string fileExtension)
        {
            return YedFileExtensions.Contains(fileExtension);
        }
    }
}
