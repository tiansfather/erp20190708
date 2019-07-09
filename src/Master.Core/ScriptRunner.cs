using Master.Module;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Master
{
    public class ScriptRunner
    {
        public static async Task<object> EvaluateScript(string script, ScriptOptions option = null, object globals = null, Type globalsType = null)
        {
            var op = option ?? ScriptOptions.Default;
            op = op.AddImports(new string[] { "System", "System.Math" });
            op = op.AddReferences(typeof(ModuleInfo).Assembly);
            var result = await CSharpScript.EvaluateAsync(script, op, globals, globalsType);
            return result;
        }
        public static async Task<T> EvaluateScript<T>(string script, ScriptOptions option = null, object globals = null, Type globalsType = null)
        {
            var op = option ?? ScriptOptions.Default;
            op = op.AddImports(new string[] { "System", "System.Math" });
            op = op.AddReferences(typeof(ModuleInfo).Assembly);
            var result = await CSharpScript.EvaluateAsync<T>(script, op, globals, globalsType);
            return result;
        }

        public static Type CompileType(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var type = CompileType("GenericGenerator", syntaxTree);

            return type;
        }


        private static Type CompileType(string originalClassName, SyntaxTree syntaxTree)
        {
           
            // 指定编译选项。
            var assemblyName = $"{originalClassName}.g";
            var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree },
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(GetGlobalReferences())
                //.AddReferences(
                //    // 这算是偷懒了吗？我把 .NET Core 运行时用到的那些引用都加入到引用了。
                //    // 加入引用是必要的，不然连 object 类型都是没有的，肯定编译不通过。
                //    AppDomain.CurrentDomain.GetAssemblies().Select(x => MetadataReference.CreateFromFile(x.Location)))
                    ;

            // 编译到内存流中。
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    //return assembly.GetTypes().First(x => x.Name == originalClassName);
                    return assembly.GetTypes()[0];
                }
                throw new CompilationErrorException($"编译错误{string.Join(',',result.Diagnostics.Select(o=>o.ToString()))}",result.Diagnostics);
            }
        }

        private static IEnumerable<MetadataReference> GetGlobalReferences()
        {
            //var assemblies=AppDomain.CurrentDomain.GetAssemblies().Where(o => o.GetName().Name.StartsWith("Master")).ToList();
            var assemblies=AppDomain.CurrentDomain.GetAssemblies();
            //assemblies.Add(typeof(object).Assembly);
            //var assemblies = new Assembly[]
            //    {
            //        typeof(ModuleInfo).Assembly,
            ///*Used for the GeneratedCode attribute*/
            ////typeof(System.CodeDom.Compiler.CodeCompiler).Assembly,              //System.CodeDom.Compiler
            //};
            var refs = new List<MetadataReference>();
            foreach(var asm in assemblies)
            {
                try
                {
                    refs.Add(MetadataReference.CreateFromFile(asm.Location));
                }
                catch
                {

                }
            }
            //var refs = from a in assemblies
            //           select MetadataReference.CreateFromFile(a.Location);

            //var returnList = refs.ToList();

            //The location of the .NET assemblies
           // var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            /* 
                * Adding some necessary .NET assemblies
                * These assemblies couldn't be loaded correctly via the same construction as above,
                * in specific the System.Runtime.
                */
            //returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")));
            //returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")));
            //returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")));
            //returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")));

            return refs;
        }
    }
}
