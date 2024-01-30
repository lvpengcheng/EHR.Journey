﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamasoft.JsonClassGenerator.CodeWriters
{
    public class CSharpCodeWriter : ICodeBuilder
    {
        public string FileExtension
        {
            get { return ".cs"; }
        }

        public string DisplayName
        {
            get { return "C#"; }
        }

        private const string NoRenameAttribute = "[Obfuscation(Feature = \"renaming\", Exclude = true)]";
        private const string NoPruneAttribute = "[Obfuscation(Feature = \"trigger\", Exclude = false)]";

        private static readonly HashSet<string> _reservedKeywords = new HashSet<string>(comparer: StringComparer.Ordinal) {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue",
            "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally",
            "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
            "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public",
            "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct",
            "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
            "virtual", "void", "volatile", "while"
        };

        public bool IsReservedKeyword(string word) => _reservedKeywords.Contains(word ?? string.Empty);

        IReadOnlyCollection<string> ICodeBuilder.ReservedKeywords => _reservedKeywords;

        public string GetTypeName(JsonType type, IJsonClassGeneratorConfig config)
        {
            var arraysAsLists = config.ArrayAsList();

            switch (type.Type)
            {
                case JsonTypeEnum.Anything         : return "object";
                case JsonTypeEnum.Array            : return arraysAsLists ? "List<" + this.GetTypeName(type.InternalType, config) + ">" : this.GetTypeName(type.InternalType, config) + "[]";
                case JsonTypeEnum.Dictionary       : return "Dictionary<string, " + this.GetTypeName(type.InternalType, config) + ">";
                case JsonTypeEnum.Boolean          : return "bool";
                case JsonTypeEnum.Float            : return "double";
                case JsonTypeEnum.Integer          : return "int";
                case JsonTypeEnum.Long             : return "long";
                case JsonTypeEnum.Date             : return "DateTime";
                case JsonTypeEnum.NonConstrained   : return "object";
                case JsonTypeEnum.NullableBoolean  : return "bool?";
                case JsonTypeEnum.NullableFloat    : return "double?";
                case JsonTypeEnum.NullableInteger  : return "int?";
                case JsonTypeEnum.NullableLong     : return "long?";
                case JsonTypeEnum.NullableDate     : return "DateTime?";
                case JsonTypeEnum.NullableSomething: return "object";
                case JsonTypeEnum.Object           : return type.NewAssignedName;
                case JsonTypeEnum.String           : return "string";
                default: throw new NotSupportedException("Unsupported json type: " + type.Type);
            }
        }

        private bool ShouldApplyNoRenamingAttribute(IJsonClassGeneratorConfig config)
        {
            return config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && !config.UsePascalCase;
        }

        private bool ShouldApplyNoPruneAttribute(IJsonClassGeneratorConfig config)
        {
            return config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && config.UseFields;
        }

        public void WriteFileStart(IJsonClassGeneratorConfig config, StringBuilder sw)
        {
            if (config.UseNamespaces)
            {
                // foreach (var line in JsonClassGenerator.FileHeader)
                // {
                //     sw.AppendFormat("// " + line);
                // }

                List<string> namespaces = new List<string>()
                {
                    "System",
                    "System.Collections.Generic"
                };

                if (this.ShouldApplyNoPruneAttribute(config) || this.ShouldApplyNoRenamingAttribute(config))
                {
                    namespaces.Add("System.Reflection");
                }
                if (!config.ExplicitDeserialization && config.UseJsonAttributes)
                {
                    namespaces.Add("Newtonsoft.Json");
                    namespaces.Add("Newtonsoft.Json.Linq");
                }
                if (!config.ExplicitDeserialization && config.UseJsonPropertyName)
                {
                    namespaces.Add("System.Text.Json");
                }
                if (config.ExplicitDeserialization)
                {
                    namespaces.Add("JsonCSharpClassGenerator");
                }
                if (config.SecondaryNamespace != null && config.HasSecondaryClasses && !config.UseNestedClasses)
                {
                    namespaces.Add(config.SecondaryNamespace);
                }

                namespaces.Sort(CompareNamespacesSystemFirst);

                foreach(string ns in namespaces) // NOTE: Using `.Distinct()` after sorting may cause out-of-order results.
                {
                    sw.AppendFormat("using {0};{1}", ns, Environment.NewLine);
                }
            }

            if (config.UseNestedClasses)
            {
                sw.AppendFormat("    {0} class {1}", config.InternalVisibility ? "internal" : "public", config.MainClass);
                sw.AppendLine("    {");
            }
        }

        private static int CompareNamespacesSystemFirst(string x, string y)
        {
            if (x == "System") return -1;
            if (y == "System") return  1;

            if (x.StartsWith("System.", StringComparison.Ordinal))
            {
                if (y.StartsWith("System.", StringComparison.Ordinal))
                {
                    // Both start with "System." - so compare them normally.
                    return StringComparer.Ordinal.Compare(x,y);
                }
                else
                {
                    // Only `x` starts with "System", so `x` should always come first (i.e. `x < y` or `y > x`).
                    return -1;
                }
            }
            else
            {
                // Only `y` starts with "System", so `y` should always come first (i.e. `x > y` or `y < x`).
                if (y.StartsWith("System.", StringComparison.Ordinal))
                {
                    return 1;
                }
                else
                {
                    // Neither are "System." namespaces - so compare them normally.
                    return StringComparer.Ordinal.Compare(x,y);
                }
            }
        }

        public void WriteFileEnd(IJsonClassGeneratorConfig config, StringBuilder sw)
        {
            if (config.UseNestedClasses)
            {
                sw.AppendLine("    }");
            }
        }

        public void WriteDeserializationComment(IJsonClassGeneratorConfig config, StringBuilder sw)
        {
            if (config.UseJsonPropertyName)
            {
                //sw.AppendLine("// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);");
            }
            else
            {
                //sw.AppendLine("// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); ");
            }
        }

        public void WriteNamespaceStart(IJsonClassGeneratorConfig config, StringBuilder sw, bool root)
        {
            sw.AppendLine();
            sw.AppendFormat("namespace {0}", root && !config.UseNestedClasses ? config.Namespace : (config.SecondaryNamespace ?? config.Namespace));
            sw.AppendLine("{");
            sw.AppendLine();
        }

        public void WriteNamespaceEnd(IJsonClassGeneratorConfig config, StringBuilder sw, bool root)
        {
            sw.AppendLine("}");
        }

        private static string GetTypeIndent(IJsonClassGeneratorConfig config, bool typeIsRoot)
        {
            if (config.UseNestedClasses)
            {
                if (typeIsRoot)
                {
                    return "    "; // 4x
                }
                else
                {
                    return "        "; // 8x
                }
            }
            else
            {
                return "    "; // 4x
            }
        }

        public void WriteClass(IJsonClassGeneratorConfig config, StringBuilder sw, JsonType type)
        {
            string indentTypes   = GetTypeIndent(config, type.IsRoot);
            string indentMembers = indentTypes   + "    ";
            string indentBodies  = indentMembers + "    ";

            const string visibility = "public";

            var className = type.AssignedName;
            sw.AppendFormat(indentTypes + "{0} class {1}{2}", visibility, className, Environment.NewLine);
            sw.AppendLine  (indentTypes + "{");

#if CAN_SUPRESS
            var shouldSuppressWarning = config.InternalVisibility && !config.UseProperties && !config.ExplicitDeserialization;
            if (shouldSuppressWarning)
            {
                sw.AppendFormat("#pragma warning disable 0649");
                if (!config.UsePascalCase) sw.AppendLine();
            }
            if (config.ExplicitDeserialization)
            {
                if (config.UseProperties) WriteClassWithPropertiesExplicitDeserialization(sw, type, prefix);
                else WriteClassWithFieldsExplicitDeserialization(sw, type, prefix);
            }
            else
#endif
            {
                if (config.ImmutableClasses)
                {
                    this.WriteClassConstructor(config, sw, type, indentMembers: indentMembers, indentBodies: indentBodies);
                }

                this.WriteClassMembers(config, sw, type, indentMembers);
            }
#if CAN_SUPPRESS
            if (shouldSuppressWarning)
            {
                sw.WriteLine();
                sw.WriteLine("#pragma warning restore 0649");
                sw.WriteLine();
            }
#endif

            if ((!config.UseNestedClasses) || (config.UseNestedClasses && !type.IsRoot))
            {
                sw.AppendLine(indentTypes + "}");
            }

            sw.AppendLine();
        }

        /// <summary>Converts an identifier from JSON into a C#-safe PascalCase identifier.</summary>
        private string GetCSharpPascalCaseName(string name)
        {
            // Check if property is a reserved keyword
            if (this.IsReservedKeyword(name)) name = "@" + name;

            // Check if property name starts with number
            if (!string.IsNullOrEmpty(name) && char.IsDigit(name[0])) name = "_" + name;

            return name;
        }

        /// <summary>Converts a camelCase identifier from JSON into a C#-safe camelCase identifier.</summary>
        private string GetCSharpCamelCaseName(string camelCaseFromJson)
        {
            if (String.IsNullOrEmpty(camelCaseFromJson)) throw new ArgumentException(message: "Value cannot be null or empty.", paramName: nameof(camelCaseFromJson));

            string name = camelCaseFromJson;

            //

            if (name.Length >= 3)
            {
                if (Char.IsUpper(name[0]) && Char.IsUpper(name[1]) && Char.IsLower(name[2]))
                {
                    // "ABc" --> "abc" // this may be wrong in some cases, if the first two letters are a 2-letter acronym, like "IO".
                    name = name.Substring(startIndex: 0, length: 2).ToLowerInvariant() + name.Substring(startIndex: 2);
                }
                else if (Char.IsUpper(name[0]))
                {
                    // "Abc" --> "abc"
                    // "AbC" --> "abC"
                    name = Char.ToLower(name[0]) + name.Substring(startIndex: 1);
                }
            }
            else if (name.Length == 2)
            {
                if (Char.IsUpper(name[0]))
                {
                    // "AB" --> "ab"
                    // "Ab" --> "ab"
                    name = name.ToLowerInvariant();
                }
            }
            else // Implicit: name.Length == 1
            {
                // "A" --> "a"
                name = name.ToLowerInvariant();
            }

            if      (!Char.IsLetter(name[0]))      name = "_" + name;
            else if (this.IsReservedKeyword(name)) name = "@" + name;

            return name;
        }

        public void WriteClassMembers(IJsonClassGeneratorConfig config, StringBuilder sw, JsonType type, string indentMembers)
        {
            bool first = true;
            foreach (FieldInfo field in type.Fields)
            {
                string classPropertyName = this.GetCSharpPascalCaseName(field.MemberName);
                string propertyAttribute = config.GetCSharpJsonAttributeCode(field);

                if( !first && (propertyAttribute.Length > 0 || config.ExamplesInDocumentation) )
                {
                    // If rendering examples/XML comments - or property attributes - then add a newline before the property for readability's sake (except if it's the first property in the class)
                    sw.AppendLine();
                }

                if (config.ExamplesInDocumentation)
                {
                    sw.AppendFormat(indentMembers + "/// <summary>");
                    sw.AppendFormat(indentMembers + "/// Examples: " + field.GetExamplesText());
                    sw.AppendFormat(indentMembers + "/// </summary>");
                    sw.AppendLine();
                }

                if (propertyAttribute.Length > 0)
                {
                    sw.Append(indentMembers);
                    sw.AppendLine(propertyAttribute);
                }

                if (config.UseFields)
                {
                    sw.AppendFormat(indentMembers + "public {0}{1} {2};{3}", config.ImmutableClasses ? "readonly " : "", field.Type.GetTypeName(), classPropertyName, Environment.NewLine);
                }
                else if (config.ImmutableClasses)
                {
                    if(field.Type.Type == JsonTypeEnum.Array)
                    {
                        sw.AppendFormat(indentMembers + "public IReadOnlyList<{0}> {1} {{ get; }}{2}", this.GetTypeName(field.Type.InternalType, config), classPropertyName, Environment.NewLine);
                    }
                    else
                    {
                        sw.AppendFormat(indentMembers + "public {0} {1} {{ get; }}{2}", field.Type.GetTypeName(), classPropertyName, Environment.NewLine);
                    }
                }
                else
                {
                    var getterSetterPart = "{ get; set; }";
                    if (config.NoSettersForCollections 
                        &&  (field.Type.IsCollectionType() 
                            && (config.ArrayAsList() && field.Type.Type == JsonTypeEnum.Array))) getterSetterPart = "{ get; } = new " + field.Type.GetTypeName() + "();";
                    sw.AppendFormat(indentMembers + "public {0} {1} {2}{3}", field.Type.GetTypeName(), classPropertyName, getterSetterPart, Environment.NewLine);
                }

                first = false;
            }

        }

        private void WriteClassConstructor(IJsonClassGeneratorConfig config, StringBuilder sw, JsonType type, string indentMembers, string indentBodies)
        {
            // Write an empty constructor on a single-line:
            if(type.Fields.Count == 0)
            {
                sw.AppendFormat(indentMembers + "public {0}() {{}}{1}", type.AssignedName, Environment.NewLine);
                return;
            }

            // Constructor signature:
            {
                if(config.UseJsonAttributes)
                {
                    sw.AppendLine(indentMembers + "[JsonConstructor]");
                }

                sw.AppendFormat(indentMembers + "public {0}({1}", type.AssignedName, Environment.NewLine);
                
                FieldInfo lastField = type.Fields[type.Fields.Count-1];

                foreach (FieldInfo field in type.Fields)
                {
                    // Writes something like: `[JsonProperty("foobar")] string foobar,`

                    string ctorParameterName = this.GetCSharpCamelCaseName(field.MemberName);

                    bool isLast = Object.ReferenceEquals(field, lastField);
                    string comma = isLast ? "" : ",";
                    
                    //

                    sw.Append(indentBodies);

                    string attribute = config.GetCSharpJsonAttributeCode(field);
                    if(attribute.Length > 0)
                    {
                        sw.Append(attribute);
                        sw.Append(' ');
                    }

                    sw.AppendFormat("{0} {1}{2}{3}", /*0:*/ field.Type.GetTypeName(), /*1:*/ ctorParameterName, /*2:*/ comma, /*3:*/ Environment.NewLine);
                }
            }

            sw.AppendLine(indentMembers + ")");

            // Constructor body:
            sw.AppendLine(indentMembers + "{");

            foreach (FieldInfo field in type.Fields)
            {
                string ctorParameterName = this.GetCSharpCamelCaseName(field.MemberName);
                string classPropertyName = this.GetCSharpPascalCaseName(field.MemberName);

                sw.AppendFormat(indentBodies + "this.{0} = {1};{2}", /*0:*/ classPropertyName, /*1:*/ ctorParameterName, /*2:*/ Environment.NewLine);
            }

            sw.AppendLine(indentMembers + "}");
            sw.AppendLine();
        }

    }
}
