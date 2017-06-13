using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Plugins.Editor
{
    public class PluginPackageBuilder
    {
        /// <summary>
        /// The key package path
        /// </summary>
        public const string KeyPackagePath = "package_path";

        /// <summary>
        /// Builds the Appmetr package. Path to package will be received from command line args.
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        public static void BuildPackage()
        {
            var args = GetCommandLineArgs();
            var errorBuilder = new StringBuilder();

            var packagePath = TryGetArgsValue(KeyPackagePath, args, errorBuilder);

            if (errorBuilder.Length != 0)
            {
                throw new Exception(errorBuilder.ToString());
            }

            BuildPackage(AssetPaths, packagePath);
        }

        /// <summary>
        /// Builds the Appmetr package with showing SaveFilePanel
        /// </summary>
        [MenuItem("Assets/Build Appmetr package")]
        public static void BuildPackageDefault()
        {
            string packagePath =
                EditorUtility.SaveFilePanel("Save Appmetr package", Application.dataPath, "Appmetr", "unitypackage");
            BuildPackage(AssetPaths, packagePath);
        }

        /// <summary>
        /// Builds the package.
        /// </summary>
        /// <param name="assetPathNames">The asset path names.</param>
        /// <param name="packagePath">The package path.</param>
        public static void BuildPackage(string[] assetPathNames, string packagePath)
        {
            var outputDirectory = Path.GetDirectoryName(packagePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Debug.Log("Trying create output dir: " + outputDirectory);
                Directory.CreateDirectory(outputDirectory);
            }
            else if (File.Exists(packagePath))
            {
                File.Delete(packagePath);
            }
            IEnumerable<string> resolveWildPatterns = new List<string>();
            assetPathNames = assetPathNames.Where(path =>
            {
                var fileName = Path.GetFileName(path);
                if (string.IsNullOrEmpty(fileName) || !fileName.Contains("*") && !fileName.Contains("?")) return true;
                resolveWildPatterns =
                    resolveWildPatterns.Concat(Directory.GetFiles(Path.GetDirectoryName(path) ?? path, fileName));
                return false;
            }).ToArray().Concat(resolveWildPatterns).ToArray();

            AssetDatabase.ExportPackage(assetPathNames, packagePath,
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        }

        /// <summary>
        /// Gets the command line arguments.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetCommandLineArgs()
        {
            var args = Environment.GetCommandLineArgs();
            var argsDictionary = new Dictionary<string, string>();
            var argsLength = args.Length;

            for (var i = 0; i < argsLength; i++)
            {
                var key = args[i];
                var iNext = i + 1;
                var value = (iNext < argsLength) ? args[iNext] : null;

                if (key.StartsWith("-"))
                {
                    key = key.Substring(1);
                }

                if (string.IsNullOrEmpty(value) || value.StartsWith("-"))
                {
                    value = null;
                }
                else
                {
                    i++;
                }
                argsDictionary.Add(key, value);
            }

            return argsDictionary;
        }

        /// <summary>
        /// Tries the get arguments value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="errorBuilder">The error builder.</param>
        /// <returns></returns>
        public static string TryGetArgsValue(string key, Dictionary<string, string> args, StringBuilder errorBuilder)
        {
            string value;
            if (!args.TryGetValue(key, out value) || string.IsNullOrEmpty(value))
            {
                errorBuilder.AppendLine(string.Format(
                    "Missed args: '{0}' key not found in command line args or it's value is empty", key));
            }
            return value;
        }

        private static readonly string[] AssetPaths =
        {
            "Assets/Plugins/Appmetr",
            "Assets/Plugins/iOS",
            "Assets/Plugins/x86/Appmetr*",
            "Assets/Plugins/Android/appmetr-*"
        };
    }
}