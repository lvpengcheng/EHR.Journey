using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodingSeb.ExpressionEvaluator;
using Flee.PublicTypes;

namespace EHR.Journey.Core
{
    /// <summary>
    /// 表达式计算引擎
    /// </summary>
    public class FleeExpression : IDisposable
    {
        /// <summary>
        /// 全局静态参数
        /// </summary>
        protected readonly string GLOBAL_VARIABLE_FORMAT = "_{0}";
        /// <summary>
        /// 全局动态方法
        /// </summary>
        protected readonly string GLOBAL_DYNAMIC_METHOD_FORMAT = "_func{0}";

        /// <summary>
        /// 表达式上下文
        /// </summary>
        protected ExpressionContext _context;
        /// <summary>
        /// 变量集合
        /// </summary>
        protected VariableCollection _vars;

        /// <summary>
        /// 函数集合
        /// </summary>
        protected ExpressionImports _method;


        /// <summary>
        /// 内置方程式列表
        /// </summary>
        private Dictionary<string, IGenericExpression<dynamic>> _initGenericFormulae;

        public enum OperatorVariableType
        {
            /// <summary>
            /// 默认不处理
            /// </summary>
            None = 0,
            /// <summary>
            /// 清理项目
            /// </summary>
            Clear = 1,
            /// <summary>
            /// 强制设置为数据类型默认值
            /// </summary>
            SetDefault = 2
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FleeExpression()
        {
            _context = new ExpressionContext();
            _vars = _context.Variables;
            _method = _context.Imports;
            _method.ImportBuiltinTypes();
            _initGenericFormulae = new Dictionary<string, IGenericExpression<dynamic>>();
            _method.AddType(typeof(DateTime), "DateTime");
            _method.AddType(typeof(Convert));
            _method.AddType(typeof(Regex));
            _method.AddType(typeof(Math), "Math");
            _method.AddType(typeof(String), "String");
            _method.AddType(typeof(CusFun), "f");
            //_method.AddType(typeof(CustomFunctions), "f");
            _method.AddType(typeof(List<>));
            _method.AddType(typeof(ExpressionEvaluator));
            _method.AddType(typeof(ExpandoObject));

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="globalVars">全局变量字典</param>
        /// <param name="globalFuncs">全局方法字典</param>
        /// <param name="methodTypes">函数字典</param>
        public FleeExpression(Dictionary<string, dynamic> globalVars, Dictionary<string, dynamic> globalFuncs, Dictionary<string, Type> methodTypes)
        {
            _context = new ExpressionContext();
            _vars = _context.Variables;
            _method = _context.Imports;
            _method.ImportBuiltinTypes();
            _initGenericFormulae = new Dictionary<string, IGenericExpression<dynamic>>();
            methodTypes = new Dictionary<string, Type>();
            methodTypes.Add("DateTime", typeof(DateTime));
            methodTypes.Add("Math", typeof(Math));
            methodTypes.Add("String", typeof(String));
            methodTypes.Add("f", typeof(CusFun));
            methodTypes.Add("Convert", typeof(Convert));
            methodTypes.Add("Regex", typeof(Regex));
            AddGlobalVariables(globalVars);
            AddGlobalMethods(globalFuncs);
            AddStaticMethods(methodTypes);
        }

        /// <summary>
        /// 新增全局共享变量(租户内共享数据)
        /// </summary>
        /// <param name="vars">全局变量字典</param>
        /// <param name="isForceRefresh">操作模式</param>
        public void AddGlobalVariables(Dictionary<string, dynamic> vars, OperatorVariableType operatorType = OperatorVariableType.None)
        {
            if (operatorType == OperatorVariableType.Clear) // 清理项目
            {
                var removeKeys = _vars.Where(d => d.Key.StartsWith(GLOBAL_VARIABLE_FORMAT.AsFormat(""))).Select(d => d.Key);
                foreach (var key in removeKeys)
                {
                    _vars.Remove(key);
                }
            }
            else if (operatorType == OperatorVariableType.SetDefault) // 强制设定默认值
            {
                var setDefaultKeys = _vars.Where(d => d.Key.StartsWith(GLOBAL_VARIABLE_FORMAT.AsFormat(""))).Select(d => d.Key);

                foreach (var key in setDefaultKeys)
                {
                    _vars[key] = default;
                }
            }
            if (vars != null && vars.Any())
            {
                foreach (var var in vars)
                {
                    if (!_vars.ContainsKey(GLOBAL_VARIABLE_FORMAT.AsFormat(var.Key)))
                    {
                        _vars.Add(GLOBAL_VARIABLE_FORMAT.AsFormat(var.Key), var.Value);
                    }
                    else
                    {
                        _vars[GLOBAL_VARIABLE_FORMAT.AsFormat(var.Key)] = var.Value;
                    }
                }
            }
        }

        /// <summary>
        /// 新增全局共享方法变量(租户内共享数据)
        /// </summary>
        /// <param name="methods">全局变量字典</param>
        /// <param name="isForceRefresh">操作模式</param>
        public void AddGlobalMethods(Dictionary<string, dynamic> methods, OperatorVariableType operatorType = OperatorVariableType.None)
        {
            if (operatorType == OperatorVariableType.Clear) // 清理项目
            {
                var removeKeys = _vars.Where(d => d.Key.StartsWith(GLOBAL_DYNAMIC_METHOD_FORMAT.AsFormat(""))).Select(d => d.Key);
                foreach (var key in removeKeys)
                {
                    _vars.Remove(key);
                }
            }
            else if (operatorType == OperatorVariableType.SetDefault) // 强制设定默认值
            {
                var setDefaultKeys = _vars.Where(d => d.Key.StartsWith(GLOBAL_DYNAMIC_METHOD_FORMAT.AsFormat(""))).Select(d => d.Key);

                foreach (var key in setDefaultKeys)
                {
                    _vars[key] = default;
                }
            }
            if (methods != null && methods.Any())
            {
                foreach (var var in methods)
                {
                    if (!_vars.ContainsKey(GLOBAL_DYNAMIC_METHOD_FORMAT.AsFormat(var.Key)))
                    {
                        _vars.Add(GLOBAL_DYNAMIC_METHOD_FORMAT.AsFormat(var.Key), var.Value);
                    }
                    else
                    {
                        _vars[GLOBAL_DYNAMIC_METHOD_FORMAT.AsFormat(var.Key)] = var.Value;
                    }
                }
            }
        }

        /// <summary>
        /// 获取全局变量列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, dynamic> GetGlobalVariables()
        {
            return _vars.Where(d => d.Key.StartsWith(GLOBAL_VARIABLE_FORMAT.AsFormat(""))).ToDictionary(d => d.Key, d => d.Value);
        }

        /// <summary>
        /// 获取全局变量列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, dynamic> GetUserVariables()
        {
            return _vars.Where(d => !d.Key.StartsWith(GLOBAL_VARIABLE_FORMAT.AsFormat(""))).ToDictionary(d => d.Key, d => d.Value);
        }

        /// <summary>
        /// 添加个人变量
        /// </summary>
        /// <param name="vars">变量字典</param>
        /// <param name="operatorType">操作类型</param>
        public void AddVariables(Dictionary<string, dynamic> vars, OperatorVariableType operatorType = OperatorVariableType.None)
        {
            if (operatorType == OperatorVariableType.Clear) // 清理项目
            {
                var removeKeys = _vars.Where(d => !d.Key.StartsWith(GLOBAL_VARIABLE_FORMAT.AsFormat(""))).Select(d => d.Key);
                foreach (var key in removeKeys)
                {
                    _vars.Remove(key);
                }
            }
            if (operatorType == OperatorVariableType.SetDefault) // 强制设定默认值
            {
                var setDefaultKeys = _vars.Where(d => !d.Key.StartsWith(GLOBAL_VARIABLE_FORMAT.AsFormat(""))).Select(d => d.Key);
                foreach (var key in setDefaultKeys)
                {
                    _vars[key] = default;
                }
            }
            if (vars != null && vars.Any())
            {
                foreach (var var in vars)
                {
                    if (!_vars.ContainsKey(var.Key))
                    {
                        _vars.Add(var.Key, var.Value);
                    }
                    else
                    {
                        _vars[var.Key] = var.Value;
                    }
                }
            }
        }

        /// <summary>
        /// 添加函数
        /// </summary>
        /// <param name="methodTypes">函数类字典</param>
        public void AddStaticMethods(Dictionary<string, Type> methodTypes)
        {
            if (methodTypes != null && methodTypes.Any())
            {
                foreach (var method in methodTypes)
                {
                    _method.AddType(method.Value, method.Key);
                }
            }
        }

        /// <summary>
        /// 新增内置变量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void AddVariable(string key, dynamic value)
        {
            if (_vars != null)
            {
                if (_vars.ContainsKey(key))
                    _vars[key] = value;
                else
                    _vars.Add(key, value);
            }
            else
            {
                _vars = _context.Variables;
                _vars.Add(key, value);
            }
        }

        /// <summary>
        /// 计算方程式(指定方程式)
        /// </summary>
        /// <typeparam name="T">泛型变量</typeparam>
        /// <param name="vars">变量集合</param>
        /// <param name="formulaCode">指定方程式编号</param>
        /// <param name="formulaText">指定方程式</param>
        /// <returns></returns>
        public dynamic CalculateSingle(Dictionary<string, dynamic> vars, string formulaCode, string formulaText)
        {
            AddVariables(vars);
            if (_initGenericFormulae.Any() && _initGenericFormulae.ContainsKey(formulaCode))
            {
                return _initGenericFormulae[formulaCode].Evaluate();
            }
            var e = _context.CompileGeneric<dynamic>(formulaText);
            _initGenericFormulae[formulaCode] = e;
            return e.Evaluate();
        }

        /// <summary>
        /// 计算方程式(指定方程式)
        /// </summary>
        /// <typeparam name="T">泛型变量</typeparam>
        /// <param name="formulaText">指定方程式</param>
        /// <returns></returns>
        public T CalculateSingle<T>(string formulaText)
        {
            if (_vars != null && _vars.Any())
            {
                var e = _context.CompileGeneric<T>(formulaText);
                return e.Evaluate();
            }
            return default;
        }

        /// <summary>
        /// 计算方程式(自动将计算结果作为变量添加到内置变量中)
        /// </summary>
        /// <typeparam name="T">泛型变量</typeparam>
        /// <param name="formulaText">指定方程式</param>
        /// <returns></returns>
        public T Calculate<T>(string formulaCode, string formulaText)
        {
            if (_initGenericFormulae.Any() && _initGenericFormulae.ContainsKey(formulaCode))
            {
                var resultInit = _initGenericFormulae[formulaCode].Evaluate();
                AddVariable(formulaCode, resultInit);
                return (T)resultInit;
            }
            var e = _context.CompileGeneric<dynamic>(formulaText);
            _initGenericFormulae[formulaCode] = e;
            var result = e.Evaluate();
            AddVariable(formulaCode, result);
            return (T)result;
        }

        public dynamic Calculate()
        {
            return null;
        }

        /// <summary>
        /// 计算方程式(自动将计算结果作为变量添加到内置变量中)
        /// </summary>
        /// <typeparam name="T">泛型变量</typeparam>
        /// <param name="formulaText">指定方程式</param>
        /// <returns></returns>
        public dynamic Calculate(string formulaCode, string formulaText)
        {
            if (_initGenericFormulae.Any() && _initGenericFormulae.ContainsKey(formulaCode))
            {
                var resultInit = _initGenericFormulae[formulaCode].Evaluate();
                AddVariable(formulaCode, resultInit);
                return resultInit;
            }
            var e = _context.CompileGeneric<dynamic>(formulaText);
            _initGenericFormulae[formulaCode] = e;
            var result = e.Evaluate();
            AddVariable(formulaCode, result);
            return result;
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        public void Dispose()
        {
            _context = null; _vars = null; _method = null;
            _initGenericFormulae = null;
        }
    }
}
