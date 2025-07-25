using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.RunTime
{
    public abstract class ResettableScriptableObject : ScriptableObject, IResettable
    {
#if UNITY_EDITOR
        private string _initialJson = string.Empty;
#else
        private string _initialJsonKey => $"{name}_initialJson";
#endif

        public void SaveInitialState()
        {
#if UNITY_EDITOR
            _initialJson = EditorJsonUtility.ToJson(this);
#elif UNITY_IOS
            string json = JsonUtility.ToJson(this);
            ES3.Save(_initialJsonKey, json);
#endif
        }

        public void ResetToInitialState()
        {
#if UNITY_EDITOR
            EditorJsonUtility.FromJsonOverwrite(_initialJson, this);
#elif UNITY_IOS
            if (ES3.KeyExists(_initialJsonKey))
            {
                string json = ES3.Load<string>(_initialJsonKey);
                JsonUtility.FromJsonOverwrite(json, this);
            }
#endif
        }
    }
}