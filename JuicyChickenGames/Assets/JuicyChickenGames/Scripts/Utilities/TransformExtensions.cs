using System;
using System.Collections.Generic;
using UnityEngine;

namespace JuicyChickenGames
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform children in transform)
            {
                GameObject.Destroy(children.gameObject);
            }
        }

        public static List<TGameObject> RePopulateObjects<TGameObject, TData>
            (
                this Transform container,
                TGameObject prefab,
                List<TData> datas,
                Action<TGameObject, TData> action
            ) where TGameObject : UnityEngine.Object
        {
            container.DestroyChildren();

            return container.PopulateObjects(prefab, datas, action);
        }

        public static List<TGameObject> PopulateObjects<TGameObject, TData>
            (
                this Transform container,
                TGameObject prefab,
                List<TData> datas,
                Action<TGameObject, TData> action
            ) where TGameObject : UnityEngine.Object
        {
            var list = new List<TGameObject>();

            if (datas != null)
            {
                foreach (var decorOption in datas)
                {
                    var newObject = GameObject.Instantiate(prefab, container);
                    action?.Invoke(newObject, decorOption);

                    list.Add(newObject);
                }
            }

            return list;
        }

        public static List<Transform> GetChildrenAsList(this Transform parent)
        {
            List<Transform> returnList = new List<Transform>();

            foreach (Transform child in parent)
            {
                returnList.Add(child);
            }

            return returnList;
        }

        public static Transform FirstChildOrDefault(this Transform parent, Func<Transform, bool> query)
        {
            if (parent.childCount == 0)
            {
                return null;
            }

            Transform result = null;
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (query(child))
                {
                    return child;
                }
                result = FirstChildOrDefault(child, query);

                if (result != null)
                {
                    return result;
                }
            }

            return result;
        }

        public static List<Transform> FindAllChildrenWith(this Transform root, Func<Transform, bool> predicate)
        {
            List<Transform> result = new List<Transform>();
            foreach (Transform child in root)
            {
                if (predicate(child))
                {
                    result.Add(child);
                }

                result.AddRange(FindAllChildrenWith(child, predicate));
            }

            return result;
        }
    }
}
