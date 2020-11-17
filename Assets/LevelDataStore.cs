using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets
{
    static class LevelDataStore
    {
        static LevelDataStore()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if(arg0 != arg1)
            {
                _data = new Dictionary<(Type, string), object>();
            }
        }

        private static IDictionary<(Type, string), object> _data = new Dictionary<(Type, string), object>();

        static T GetOrCreate<T>(string name = "") where T : new()
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
