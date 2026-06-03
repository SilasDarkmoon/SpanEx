using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Data;

namespace Generator
{
    public static class MonoCecilExtensions
    {
        internal static MethodReference GetReference(this MethodDefinition method, GenericInstanceType type)
        {
            MethodReference mref = new MethodReference(method.Name, method.ReturnType, type);
            foreach (var par in method.Parameters)
            {
                mref.Parameters.Add(par);
            }
            if (!method.IsStatic)
            {
                mref.HasThis = true;
            }
            return mref;
        }
        internal static MethodReference GetReference(this MethodDefinition method)
        {
            MethodReference mref = new MethodReference(method.Name, method.ReturnType, method.DeclaringType);
            foreach (var par in method.Parameters)
            {
                mref.Parameters.Add(par);
            }
            if (!method.IsStatic)
            {
                mref.HasThis = true;
            }
            return mref;
        }
        internal static MethodReference GetReference(this MethodDefinition method, GenericInstanceType type, ModuleDefinition inModule)
        {
            MethodReference mref = inModule.ImportReference(method);
            mref.DeclaringType = inModule.ImportReference(type);
            if (!method.IsStatic)
            {
                mref.HasThis = true;
            }
            return mref;
        }
        internal static MethodReference GetReference(this MethodDefinition method, ModuleDefinition inModule)
        {
            MethodReference mref = inModule.ImportReference(method);
            if (!method.IsStatic)
            {
                mref.HasThis = true;
            }
            return mref;
        }
        internal static List<MethodDefinition> GetMethods(this TypeDefinition type, string name)
        {
            List<MethodDefinition> list = new List<MethodDefinition>();
            foreach (var method in type.Methods)
            {
                if (method.Name == name)
                {
                    list.Add(method);
                }
            }
            return list;
        }
        internal static MethodDefinition GetMethod(this TypeDefinition type, string name)
        {
            var methods = GetMethods(type, name);
            if (methods.Count > 0)
            {
                return methods[0];
            }
            return null;
        }
        internal static MethodDefinition GetMethod(this TypeDefinition type, string name, int paramCnt)
        {
            foreach (var method in type.Methods)
            {
                if (method.Name == name && method.Parameters.Count == paramCnt)
                {
                    return method;
                }
            }
            return null;
        }
        internal static MethodDefinition GetMethod(this TypeDefinition type, string name, params TypeReference[] pars)
        {
            pars = pars ?? new TypeReference[0];
            foreach (var method in type.Methods)
            {
                if (method.Name == name)
                {
                    if (method.Parameters.Count == pars.Length)
                    {
                        bool match = true;
                        for (int i = 0; i < pars.Length; ++i)
                        {
                            if (pars[i] != method.Parameters[i].ParameterType)
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            return method;
                        }
                    }
                }
            }
            return null;
        }
        internal static FieldDefinition GetField(this TypeDefinition type, string name)
        {
            foreach (var field in type.Fields)
            {
                if (field.Name == name)
                {
                    return field;
                }
            }
            return null;
        }
        internal static PropertyDefinition GetProperty(this TypeDefinition type, string name)
        {
            foreach (var prop in type.Properties)
            {
                if (prop.Name == name)
                {
                    return prop;
                }
            }
            return null;
        }
        internal static TypeDefinition GetNestedType(this TypeDefinition type, string name)
        {
            foreach (var ntype in type.NestedTypes)
            {
                if (ntype.Name == name)
                {
                    return ntype;
                }
            }
            return null;
        }
        internal static void AddRange<T>(this Mono.Collections.Generic.Collection<T> collection, IEnumerable<T> values)
        {
            foreach (var val in values)
            {
                collection.Add(val);
            }
        }
        internal static void AddRange<T>(this Mono.Collections.Generic.Collection<T> collection, params T[] values)
        {
            AddRange(collection, (IEnumerable<T>)values);
        }
        internal static void InsertRange<T>(this Mono.Collections.Generic.Collection<T> collection, int index, IEnumerable<T> values)
        {
            foreach (var val in values)
            {
                collection.Insert(index++, val);
            }
        }
        internal static void InsertRange<T>(this Mono.Collections.Generic.Collection<T> collection, int index, params T[] values)
        {
            InsertRange(collection, index, (IEnumerable<T>)values);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var baseDir = AppContext.BaseDirectory;
            var root = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "../../../../"));
            var srcDll = System.IO.Path.Combine(baseDir, "SpanEx.dll");
            var tar = System.IO.Path.Combine(root, "SpanEx.dll");

            var asm = AssemblyDefinition.ReadAssembly(srcDll);
            var module = asm.MainModule;

            var baseType = module.GetType("Mod.LowLevel.SpanEx");

            static void InjectMethod(MethodDefinition method)
            {
                method.Body.Instructions.Clear();
                method.Body.Variables.Clear();
                var vrv = new VariableDefinition(method.ReturnType);
                method.Body.Variables.Add(vrv);
                var emitter = method.Body.GetILProcessor();
                emitter.Emit(OpCodes.Ldloca, 0);
                emitter.Emit(OpCodes.Ldarga, 0);
                emitter.Emit(OpCodes.Ldind_Ref);
                emitter.Emit(OpCodes.Stind_Ref);
                emitter.Emit(OpCodes.Ldloc_0);
                emitter.Emit(OpCodes.Ret);
            }
            foreach (var method in baseType.GetMethods("ConvertToNormalImp"))
            {
                InjectMethod(method);
            }
            InjectMethod(baseType.GetMethod("ConvertToSpanImp"));
            InjectMethod(baseType.GetMethod("ConvertToReadOnlySpanImp"));

            asm.Write(tar);
            asm.Dispose();

            Console.WriteLine("Injection completed: " + tar);
        }
    }
}
