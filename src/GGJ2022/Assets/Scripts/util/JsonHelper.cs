using System;
using UnityEngine;

namespace util
{
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            Debug.Log(wrapper);
            return wrapper.items;
        }

        public static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T>
            {
                items = array
            };
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            var wrapper = new Wrapper<T>
            {
                items = array
            };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}