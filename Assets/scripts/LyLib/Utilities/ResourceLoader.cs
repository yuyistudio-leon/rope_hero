using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceLoader
{
    protected GameObject[] item_prefabs;
    protected GameObject[] object_prefabs;

    Dictionary<string, GameObject> name2items;
    Dictionary<string, GameObject> name2objects;

    /*
     * path Resources目录下的目录，例如“a/b/c”
     * name2obj 名称到prefab的转换表
     * prefabs prefabs列表
     */
    public void LoadPrefabs(string path, ref Dictionary<string, GameObject> name2obj, ref GameObject[] prefabs)
    {
        prefabs = Resources.LoadAll<GameObject>(path);

        if (name2obj == null)
        {
            name2obj = new Dictionary<string, GameObject>();
        }
        foreach (var prefab in prefabs)
        {
            name2obj[prefab.name] = prefab;
        }
    }
}