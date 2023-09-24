using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ConfigLoader", menuName = "Configs/ConfigLoader", order = 1)]
    internal class ConfigLoader: ScriptableObject
    {
        [SerializeField] private List<ScriptableObject> _loadedScriptables;
        [SerializeField] private List<GameObject> _loadedPrefabs;

        public List<ScriptableObject> LoadedScriptables => _loadedScriptables;
        public List<GameObject> LoadedPrefabs => _loadedPrefabs;
    }
}
