using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Journey.Core
{
    using CodingSeb.ExpressionEvaluator;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using global::System.Dynamic;

    public static class CusFun
    {

        public static string GetAll(string objectJson)
        {
            return objectJson;
        }

        public static Object Evaluate(string fun)
        {
            ExpressionEvaluator evaluator = new ExpressionEvaluator();
            var obj = evaluator.Evaluate(fun);
            return obj;


        }

        public static object Test2(object ob)
        {
            var jsonstring = JsonConvert.SerializeObject(ob);
            var list = JsonConvert.DeserializeObject<List<dynamic>>(jsonstring);

            return list;
        }



        public static object Test1(object ob)
        {
            var jsonstring = JsonConvert.SerializeObject(ob);
            var items = JsonConvert.DeserializeObject<List<dynamic>>(jsonstring);
            var filter = "p=>p.jiaqishichang>1";
            var mscorlib = typeof(Object).Assembly;
            var systemCore = typeof(Enumerable).Assembly;
            ScriptOptions scriptOptions = ScriptOptions.Default;
            scriptOptions = scriptOptions.AddReferences(mscorlib);
            scriptOptions = scriptOptions.AddReferences(systemCore);
            scriptOptions = scriptOptions.AddReferences(typeof(DynamicObject).Assembly);
            scriptOptions = scriptOptions.AddReferences(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly);
            scriptOptions = scriptOptions.AddReferences(typeof(ExpandoObject).Assembly);
            scriptOptions = scriptOptions.AddImports("System", "System.Linq", "System.Collections.Generic", "System.Dynamic");
            var script = CSharpScript.Create($@"var result = new List<dynamic>();
result=Args.Where({filter}).ToList();
return result; ", scriptOptions, globalsType: typeof(Globals));

            // run it at twice on the user values that were received before
            // also you can reuse an array, but anyway
            var res = script.RunAsync(new Globals { Args = items }).Result.ReturnValue;
            return res;
        }








    }

    public class Globals
    {
        public List<dynamic> Args;
    }

    public class CollectionFilterer
    {
        public async Task<IEnumerable<T>> Filter<T>(IEnumerable<T> items, string filter)
        {
            //var discountFilter = "album => album.Quantity > 0";
            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly);

            Func<T, bool> filterExpression = await CSharpScript.EvaluateAsync<Func<T, bool>>(filter, options);

            var filteredItems = items.Where(filterExpression);
            return filteredItems;
        }
    }
}
