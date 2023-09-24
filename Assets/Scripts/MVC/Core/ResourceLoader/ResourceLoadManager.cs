using System.Collections.Generic;
using UnityEngine;
using Configs;

namespace Core.ResourceLoader
{
    internal static class ResourceLoadManager
    {
        private static ConfigLoader _mainLoadObject;


        public static void Init(ConfigLoader mainLoadObject)
        {
            _mainLoadObject = mainLoadObject;
        }

        public static T GetConfig<T>(string name = "") where T : Object
        {
            List<ScriptableObject> scriptableObjects = _mainLoadObject.LoadedScriptables.
                FindAll(element => element.GetType() == typeof(T));

            return scriptableObjects.Count > 1 ? scriptableObjects.
                Find(element => element.name == name) as T : scriptableObjects[0] as T;
        }

        public static T GetPrefabComponentOrGameObject<T>(string name) where T : Object
        {
            GameObject element = _mainLoadObject.LoadedPrefabs.
                Find(element => element.gameObject != null && element.name == name);

            var gameobject = element.gameObject;
            var component = gameobject.GetComponent<T>();
            return component == null ? gameobject as T : component;
        }
    }
}
