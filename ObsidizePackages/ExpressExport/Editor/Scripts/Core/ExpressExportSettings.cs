using System.Collections.Generic;
using UnityEngine;

namespace Obsidize.ExpressExport.Editor
{
    [CreateAssetMenu]
    public class ExpressExportSettings : ScriptableObject
    {

        [SerializeField] private bool _recurseFolders = true;
        [SerializeField] private bool _includeDependencies = true;
        [SerializeField] private string _exportedAssetPath = "{{packageName}}.unitypackage";
        [SerializeField] private List<string> _paths = new List<string>();

        public bool RecurseFolders => _recurseFolders;
        public bool IncludeDependencies => _includeDependencies;
        public string ExportedAssetPath => _exportedAssetPath;
        public List<string> Paths => _paths;
    }
}
