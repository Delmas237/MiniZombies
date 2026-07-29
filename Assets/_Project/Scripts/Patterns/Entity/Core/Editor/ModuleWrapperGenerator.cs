using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Entity.Editor
{
    public static class ModuleWrapperGenerator
    {
        [MenuItem("Entity/Generate wrappers")]
        private static void Generate()
        {
            int generated = 0;
            int skipped = 0;

            foreach (var monoScript in MonoImporter.GetAllRuntimeMonoScripts())
            {
                Type type = monoScript.GetClass();

                if (type == null)
                    continue;

                if (type.IsAbstract || type.IsInterface || type.IsGenericType)
                    continue;

                if (!typeof(IModule).IsAssignableFrom(type))
                    continue;

                string wrapperName = $"{type.Name}Wrapper";

                string modulePath = AssetDatabase.GetAssetPath(monoScript);
                string directory = Path.GetDirectoryName(modulePath)!;
                string wrapperPath = Path.Combine(directory, wrapperName + ".cs");

                if (File.Exists(wrapperPath))
                {
                    skipped++;
                    continue;
                }

                string code = GenerateWrapper(type, wrapperName);

                File.WriteAllText(wrapperPath, code, Encoding.UTF8);
                generated++;
            }

            AssetDatabase.Refresh();

            Debug.Log($"Wrapper generation completed. Generated: {generated}, skipped: {skipped}");
        }

        private static string GenerateWrapper(Type moduleType, string wrapperName)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(moduleType.Namespace))
            {
                sb.AppendLine($"namespace {moduleType.Namespace}");
                sb.AppendLine("{");
                sb.AppendLine($"    public sealed class {wrapperName} : ModuleWrapper<{moduleType.Name}> {{ }}");
                sb.AppendLine("}");
            }
            else
            {
                sb.AppendLine($"public sealed class {wrapperName} : ModuleWrapper<{moduleType.Name}> {{ }}");
            }

            return sb.ToString();
        }
    }
}