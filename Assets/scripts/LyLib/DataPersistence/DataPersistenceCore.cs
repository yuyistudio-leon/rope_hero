using UnityEngine;
using System.Runtime.Serialization;
using System;
using System.Security.Permissions;
using System.Collections.Generic;


namespace LyLib
{
    /*
     * 用户接口
     */
    [Serializable]
    public class Vector3Serializable
    {
        // transform
        float x, y, z;
        public Vector3Serializable()
        {

        }
        public Vector3Serializable(Vector3 v)
        {
            value = v;
        }
        public Vector3 value
        {
            get
            {
                return new Vector3(x, y, z);
            }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }
    }

    [Serializable]
    public class QuaternionSerializable
    {
        float x, y, z, w;
        public QuaternionSerializable()
        {

        }
        public QuaternionSerializable(Quaternion v)
        {
            value = v;
        }
        public Quaternion value
        {
            get
            {
                return new Quaternion(x, y, z, w);
            }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
                w = value.w;
            }
        }
    }
    [Serializable]
    public class TransformSerializable
    {
        // transform
        public Vector3Serializable position = new Vector3Serializable();
        public Vector3Serializable scale = new Vector3Serializable();
        public QuaternionSerializable quaternion = new QuaternionSerializable();
        public void SaveFrom(Transform transform)
        {
            position.value = transform.localPosition;
            scale.value = transform.localScale;
            quaternion.value = transform.localRotation;
        }
        public void LoadTo(Transform game_object)
        {
            game_object.transform.localPosition = position.value;
            game_object.transform.localScale = scale.value;
            game_object.transform.localRotation = quaternion.value;
        }
    }

    [Serializable]
    public class PersistenceInfo
    {
        Dictionary<string, object> info = new Dictionary<string, object>();
        public override string ToString()
        {
            string res = "[PInfo:";
            foreach (var pair in info)
            {
                res += "(" + pair.Key + "," + pair.Value + "),";
            }
            return res;
        }
        //索引
        public object this[string index]
        {
            get
            {
                return info[index];
            }
            set
            {
                info[index] = value;
            }
        }

        // for convenience
        public T Get<T>(string key)
        {
            return (T)info[key];
        }
    }
    [Serializable]
    public class BasicGameObjectInfo
    {
        public List<PersistenceInfo> components_info = new List<PersistenceInfo>();
    }
    [Serializable]
    public class GameObjectInfo : BasicGameObjectInfo
    {
        public TransformSerializable transform = new TransformSerializable();
        public List<GameObjectInfo> children_info = new List<GameObjectInfo>();
    }
    [Serializable]
    public class UIObjectInfo : BasicGameObjectInfo
    {
        public List<UIObjectInfo> children_info = new List<UIObjectInfo>();
        public int slot_index = 0;
    }

    [Serializable]
    public class TopUIObjectInfo : UIObjectInfo
    {
        public string prefab_name;
    }
    [Serializable]
    public class TopGameObjectInfo : GameObjectInfo
    {
        public string prefab_name;
    }
    public interface IDataPersistence
    {
        void OnSave(PersistenceInfo info);
        void OnLoad(PersistenceInfo info);
    }
    /*
     * 持久化Transform的所有子GameObject。
     */
    public static class DataPersistence
    {
        public delegate string PrefabNameRetriever(Transform game_object);
        public delegate GameObject PrefabSpawner(string prefab_name);

        public static PrefabNameRetriever prefab_name_retriever;
        public static PrefabSpawner prefab_spawner;

        /*
         * 保存root层级下的所有game objects。
         */
        public static bool Save(Transform root, string filename)
        {
            List<TopGameObjectInfo> objects_info = new List<TopGameObjectInfo>();

            for (int i_child = 0; i_child < root.childCount; ++i_child)
            {
                var child = root.GetChild(i_child);
                var game_object_info = new TopGameObjectInfo();

                // save children info
                if (child.childCount > 0)
                {
                    SaveRecursively(child, game_object_info);
                }

                // save prefab_name info
                game_object_info.prefab_name = prefab_name_retriever(child);

                objects_info.Add(game_object_info);
            }

            FileUtility.Serialize(filename, objects_info);
            return true;
        }
        /*
         * 保存一个UI的信息。
         */
        public static GameObject LoadUI(TopUIObjectInfo item)
        {
            var item_game_object = prefab_spawner(item.prefab_name);
            var coms = item_game_object.GetComponents<IDataPersistence>();
            for (int i_com = 0; i_com < coms.Length; ++i_com)
            {
                coms[i_com].OnLoad(item.components_info[i_com]);
            }
            return item_game_object;
        }
        public static TopUIObjectInfo SaveUI(GameObject game_object)
        {
            var game_object_info = new TopUIObjectInfo();

            // save children info
            if (game_object.transform.childCount > 0)
            {
                SaveUIRecursively(game_object.transform, game_object_info);
            }

            // save prefab_name info
            game_object_info.prefab_name = prefab_name_retriever(game_object.transform);
            return game_object_info;
        }
        static bool SaveUIRecursively(Transform root, UIObjectInfo game_object_info)
        {
            // save IDataPersistence components info
            var coms = root.GetComponents<IDataPersistence>();
            foreach (var com in coms)
            {
                var pinfo = new PersistenceInfo();
                com.OnSave(pinfo);
                game_object_info.components_info.Add(pinfo);
            }

            // save recursively
            for (int i_child = 0; i_child < root.childCount; ++i_child)
            {
                var child = root.GetChild(i_child);
                var child_info = new UIObjectInfo();
                SaveUIRecursively(child, child_info);

                game_object_info.children_info.Add(child_info);
            }
            return true;
        }
        /*
         * 保存一个game object的信息。
         */
        public static TopGameObjectInfo Save(GameObject game_object)
        {
            var game_object_info = new TopGameObjectInfo();

            // save children info
            if (game_object.transform.childCount > 0)
            {
                SaveRecursively(game_object.transform, game_object_info);
            }

            // save prefab_name info
            if (prefab_name_retriever == null)
            {
                Debug.LogError("prefab_name_retriever fn isn't set");
                return null;
            }
            game_object_info.prefab_name = prefab_name_retriever(game_object.transform);
            return game_object_info;
        }
        public static bool SaveToFile(object info, string filename)
        {
            try
            {
                FileUtility.Serialize(filename, info);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        static bool SaveRecursively(Transform root, GameObjectInfo game_object_info)
        {
            // save IDataPersistence components info
            var coms = root.GetComponents<IDataPersistence>();
            foreach (var com in coms)
            {
                var pinfo = new PersistenceInfo();
                com.OnSave(pinfo);
                game_object_info.components_info.Add(pinfo);
            }

            // save transform component's info
            game_object_info.transform.SaveFrom(root);

            // save recursively
            for (int i_child = 0; i_child < root.childCount; ++i_child)
            {
                var child = root.GetChild(i_child);
                var child_info = new GameObjectInfo();
                SaveRecursively(child, child_info);

                game_object_info.children_info.Add(child_info);
            }
            return true;
        }
        /*
         * 载入文件内容并挂到root层级下。
         */
        public static List<TopGameObjectInfo> Load(Transform root, string filename, Callback<GameObject> each_game_object_fn = null)
        {
            List<TopGameObjectInfo> objects_info = null;
            try
            {
                objects_info = FileUtility.Deserialize(filename) as List<TopGameObjectInfo>;
            }
            catch (Exception)
            {
                return null;
            }
            if (objects_info == null)
            {
                //Debug.LogError("failed to load from file: " + filename);
                return null;
            }
            LoadFromObjectsInfo(root, objects_info, each_game_object_fn);
            return objects_info;
        }
        public static List<TopGameObjectInfo> LoadFromObjectsInfo(Transform root, List<TopGameObjectInfo> objects_info, Callback<GameObject> each_game_object_fn = null)
        {
            foreach (var object_info in objects_info)
            {
                var game_object = LoadFromObjectInfo(object_info);
                game_object.transform.SetParent(root, true);
                if (each_game_object_fn != null)
                {
                    each_game_object_fn(game_object);
                }
            }
            return objects_info;
        }
        public static GameObject LoadFromObjectInfo(TopGameObjectInfo object_info)
        {
            if (object_info == null)
            {
                return null;
            }
            var game_object = prefab_spawner(object_info.prefab_name);
            LoadRecursively(game_object.transform, object_info);
            return game_object;
        }
        static bool LoadRecursively(Transform root, GameObjectInfo game_object_info)
        {
            // load transform component's info
            game_object_info.transform.LoadTo(root);

            // load IDataPersistence components info
            var coms = root.GetComponents<IDataPersistence>();
            for (int i_com = 0; i_com < coms.Length; ++i_com)
            {
                var com_info = game_object_info.components_info[i_com];
                var com = coms[i_com];
                com.OnLoad(com_info);
            }

            // load recursively
            for (int i_child = 0; i_child < game_object_info.children_info.Count; ++i_child)
            {
                var child_info = game_object_info.children_info[i_child];
                var child = root.GetChild(i_child);
                LoadRecursively(child, child_info);
            }
            return true;
        }



        public static void LoadUIFromObjectsInfo(Transform root, List<TopUIObjectInfo> objects_info, Callback<GameObject> each_game_object_fn = null)
        {
            foreach (var object_info in objects_info)
            {
                var game_object = prefab_spawner(object_info.prefab_name);
                LoadUIRecursively(game_object.transform, object_info);
                game_object.transform.SetParent(root, false);
                if (each_game_object_fn != null)
                {
                    each_game_object_fn(game_object);
                }
            }
        }
        static bool LoadUIRecursively(Transform root, UIObjectInfo game_object_info)
        {
            // load IDataPersistence components info
            var coms = root.GetComponents<IDataPersistence>();
            for (int i_com = 0; i_com < coms.Length; ++i_com)
            {
                var com_info = game_object_info.components_info[i_com];
                var com = coms[i_com];
                com.OnLoad(com_info);
            }

            // load recursively
            for (int i_child = 0; i_child < game_object_info.children_info.Count; ++i_child)
            {
                var child_info = game_object_info.children_info[i_child];
                var child = root.GetChild(i_child);
                LoadUIRecursively(child, child_info);
            }
            return true;
        }
    }

}



