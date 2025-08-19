using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Starter
{
    public class Mod
    {
        private const string MOD_ENTRY = "YourModNamespace.YourEntryPoint";
        private const string MOD_DLL_NAME = "YourMod.dll";
        private const string UPDATER_TYPE = "ZeroOne.Updater.Initializer, ZeroOne.Updater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        public static byte[] bytes;
        public static object assembly;
        public static Type assemblyType;
        public static MethodInfo fileType;
        public static MethodInfo load;
        public static MethodInfo getTypeFromAssembly;
        public static Type mod;

        public static void OnLoad()
        {
            var assemblyClass = Type.GetType("System.Reflection.Assembly");
            load = assemblyClass.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method => method.Name == "Load").Where(method => method.GetParameters()[0].ParameterType.Name == "Byte[]").First();
            getTypeFromAssembly = Type.GetType("System.Reflection.Assembly").GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).Where(method => method.Name == "GetType" && method.GetParameters().Count() == 1 && method.GetParameters()[0].ParameterType.ToString() == "System.String").First();
            fileType = Type.GetType("System.IO.File").GetMethod("ReadAllBytes", BindingFlags.Public | BindingFlags.Static);
            var director = Type.GetType("System.IO.Directory, mscorlib");
            string[] files = (string[])director.GetMethod("GetFiles", new[] { typeof(string) }).Invoke(null, new object[] { ModAPI.Metadata.MetaLocation + "\\Runtime" });
            foreach (var file in files)
            {
                AssemblyLoader.Load(file);
            }
            Type.GetType(UPDATER_TYPE).GetMethod("Init").Invoke(null, new object[] { ModAPI.Metadata.MetaLocation });
            bytes = (byte[])fileType.Invoke(null, new object[] { Path.Combine(ModAPI.Metadata.MetaLocation, MOD_DLL_NAME) });
            assembly = load.Invoke(null, new object[] { bytes });
            mod = (Type)getTypeFromAssembly.Invoke(assembly, new object[] { MOD_ENTRY });
            mod.GetMethod("OnLoad", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { });
        }

        public static void Main()
        {
            mod.GetMethod("Main", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { });
        }

        public static void OnUnload()
        {
            mod.GetMethod("OnUnload", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { });
        }
    }

    public static class AssemblyLoader
    {
        private static Type assemblyType;
        private static MethodInfo getTypeFromAssembly;
        private static MethodInfo fileType;
        private static MethodInfo load;

        static AssemblyLoader()
        {
            assemblyType = Type.GetType("System.Reflection.Assembly");
            getTypeFromAssembly = Type.GetType("System.Reflection.Assembly").GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).Where(method => method.Name == "GetType" && method.GetParameters().Count() == 1 && method.GetParameters()[0].ParameterType.ToString() == "System.String").First();
            fileType = Type.GetType("System.IO.File").GetMethod("ReadAllBytes", BindingFlags.Public | BindingFlags.Static);
            load = assemblyType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method => method.Name == "Load").Where(method => method.GetParameters()[0].ParameterType.Name == "Byte[]").First();
        }

        public static object Load(string path)
        {
            byte[] bytes = (byte[])fileType.Invoke(null, new object[] { path });
            return load.Invoke(null, new object[] { bytes });
        }
    }
}