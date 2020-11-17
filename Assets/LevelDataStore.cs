using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    static class LevelDataStore
    {
        private static string _previousScene;

        [RuntimeInitializeOnLoadMethod()]
        static void Init()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            _previousScene = SceneManager.GetActiveScene().name;
        }

        private static void SceneManager_activeSceneChanged(Scene _, Scene newScene)
        {
            if(newScene.name != _previousScene)
            {
                _data = new Dictionary<(Type, string), object>();
                _previousScene = newScene.name;
            }
        }

        private static IDictionary<(Type, string), object> _data = new Dictionary<(Type, string), object>();

        public static T GetOrCreate<T>(string name = "") where T : new()
        {
            if(_data.TryGetValue((typeof(T), name), out var value))
            {
                return (T)value;
            }

            var created = new T();
            _data[(typeof(T), name)] = created;
            return created;
        }
    }
}
