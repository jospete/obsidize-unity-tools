using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Obsidize.ExpressExport.Editor
{
    [CustomEditor(typeof(ExpressExportSettings))]
	[CanEditMultipleObjects]
    public class ExpressExportSettingsEditor : UnityEditor.Editor
    {

		private static readonly bool smokeTest = false;
		private static readonly bool verbose = false;

		struct UnityPackageJson
		{
			public string name;
			public string displayName;
			public string version;
			public string description;
		}

		private string[] GetAssetsArray(ExpressExportSettings settings)
		{

			var settingsAssetPath = AssetDatabase.GetAssetPath(settings);
			var settingsAssetFolderPath = Path.GetDirectoryName(settingsAssetPath);
			var result = settings.Paths.Select(v => Path.Combine(settingsAssetFolderPath, v)).ToArray();

			if (verbose)
			{
				foreach (var item in result)
				{
					Debug.Log("Added asset export path: " + item);
				}
			}

			return result;
		}

		private ExportPackageOptions GetExportOptions(ExpressExportSettings settings)
		{

			var result = ExportPackageOptions.Default;

			if (settings.RecurseFolders)
			{
				result |= ExportPackageOptions.Recurse;
			}

			if (settings.IncludeDependencies)
			{
				result |= ExportPackageOptions.IncludeDependencies;
			}

			return result;
		}

		private string GetExportedAssetPath(ExpressExportSettings settings)
		{

			var settingsAssetPath = AssetDatabase.GetAssetPath(settings);
			var folderPath = Path.GetDirectoryName(settingsAssetPath);
			var folderName = new DirectoryInfo(folderPath).Name;
			var packageJsonPath = Path.Combine(folderPath, "package.json");
			var packageJsonText = File.ReadAllText(packageJsonPath);
			var packageData = JsonUtility.FromJson<UnityPackageJson>(packageJsonText);
			var replacerTable = new Dictionary<string, string>();

			replacerTable["folderName"] = folderName;
			replacerTable["packageName"] = packageData.name;
			replacerTable["packageVersion"] = packageData.version;

			var exportPathTemplate = settings.ExportedAssetPath;
			var exportPath = exportPathTemplate;

			foreach (var pair in replacerTable)
			{
				exportPath = exportPath.Replace($"{{{{{pair.Key}}}}}", pair.Value);
			}

			if (verbose)
			{
				Debug.Log("Constructed export path: " + exportPathTemplate + " -> " + exportPath);
			}

			return exportPath;
		}

		private void ExportPackage(ExpressExportSettings settings)
		{

			var assetPaths = GetAssetsArray(settings);
			var exportPath = GetExportedAssetPath(settings);
			var exportOptions = GetExportOptions(settings);
			
			if (smokeTest)
			{
				Debug.LogWarning("smoke test active - skipping export to " + exportPath);
				return;
			}

			AssetDatabase.ExportPackage(assetPaths, exportPath, exportOptions);
			AssetDatabase.Refresh();

			Debug.Log("Exported asset bundle to " + exportPath);
		}

		public override void OnInspectorGUI()
		{

			base.OnInspectorGUI();

			var settings = target as ExpressExportSettings;

			if (settings == null)
			{
				return;
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			if (GUILayout.Button("Export"))
			{
				ExportPackage(settings);
			}
		}
	}
}
