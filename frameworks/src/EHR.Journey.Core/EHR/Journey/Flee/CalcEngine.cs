using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Volo.Abp;
using Xamasoft.JsonClassGenerator;

namespace EHR.Journey.Core
{
    public class CalcEngine
    {
        public Dictionary<string, dynamic> Eval(string obj)
        {
            try
            {
                var mscorlib = typeof(Object).Assembly;
                var systemCore = typeof(Enumerable).Assembly;
                ScriptOptions scriptOptions = ScriptOptions.Default;
                scriptOptions = scriptOptions.AddReferences(mscorlib);
                scriptOptions = scriptOptions.AddReferences(systemCore);
                scriptOptions = scriptOptions.AddReferences(typeof(DynamicObject).Assembly);
                scriptOptions = scriptOptions.AddReferences(typeof(Newtonsoft.Json.JsonConvert).Assembly);
                scriptOptions = scriptOptions.AddReferences(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly);
                scriptOptions = scriptOptions.AddReferences(typeof(ExpandoObject).Assembly);
                scriptOptions = scriptOptions.AddReferences(typeof(Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript).Assembly);
                scriptOptions = scriptOptions.AddReferences(typeof(Microsoft.CodeAnalysis.Scripting.Script).Assembly);
                scriptOptions = scriptOptions.AddReferences(typeof(FleeExpression).Assembly);
                scriptOptions = scriptOptions.AddReferences(typeof(CusFun).Assembly);
                scriptOptions = scriptOptions.AddImports("System", "System.Linq", "System.Collections.Generic", "System.Dynamic");
                var jsonClass = GenerateCSharp(obj);
                var code = $$""""
                using Microsoft.CodeAnalysis.Scripting;
                using Newtonsoft.Json;
                using System;
                using System.Collections.Generic;
                using System.Dynamic;
                using System.Linq;
                using System.Text;
                using System.Text.RegularExpressions;
                using System.Threading.Tasks;
                using Flee;
                using EHR.Journey.Core;
                public class MyClass
                {
                   public Dictionary<string, dynamic> DoWork()
                    {

                         Dictionary<string, dynamic> vars = new Dictionary<string, dynamic>();
                         Dictionary<string, string> fos = new Dictionary<string, string>();
                         var myInfo=JsonConvert.DeserializeObject<Root>($$"""{{obj}}""");
                         var fors=myInfo.Fors;
                         foreach(var item in fors){
                            fos.Add(item.FName,item.FValue);
                         }
                         vars.Add("root", myInfo);
                         var res = GetCaculateResult(vars, fos);
                        return res;
                    }
                     private Dictionary<string, dynamic> GetCaculateResult(Dictionary<string, dynamic> vars, Dictionary<string, string> fos)
                    {
                        FleeExpression exp = new FleeExpression(vars, null, null);
                        exp.AddVariables(vars);
                        Dictionary<string, dynamic> listResult = new Dictionary<string, dynamic>();
                        foreach (var item in fos)
                        {
                            listResult.Add(item.Key, exp.Calculate(item.Key, item.Value));


                        }
                        return listResult;
                    }
                {{jsonClass}}
                }
                """";
                var script = CSharpScript.RunAsync(code, scriptOptions).Result;

                var result = script.ContinueWithAsync<Dictionary<string, dynamic>>("new MyClass().DoWork()").Result.ReturnValue;
                return result;

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException($"算薪时失败，具体原因为{ex.Message}");
            }
        }

        private string GenerateCSharp(string jsonText)
        {
            JsonClassGenerator generator = new JsonClassGenerator();
            this.ConfigureGenerator(generator);

            try
            {
                StringBuilder sb = generator.GenerateClasses(jsonText, errorMessage: out String errorMessage);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private IJsonClassGeneratorConfig ConfigureGenerator(IJsonClassGeneratorConfig config)
        {

            config.UseJsonAttributes = false;
            config.UseJsonPropertyName = false;
            return config;

        }

        private Dictionary<string, dynamic> GetCaculateResult(Dictionary<string, dynamic> vars, Dictionary<string, string> fos)
        {
            FleeExpression exp = new FleeExpression(vars, null, null);
            exp.AddVariables(vars);
            Dictionary<string, dynamic> listResult = new Dictionary<string, dynamic>();
            foreach (var item in fos)
            {
                listResult.Add(item.Key, exp.Calculate(item.Key, item.Value));


            }
            return listResult;
        }

    }










}
