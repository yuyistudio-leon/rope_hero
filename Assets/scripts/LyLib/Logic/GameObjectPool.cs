using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
 * GameObject对象池，按照类型保存已经创建好的对象。
 * 重用对象的时候会使用SendMessage发送Reuse消息。
 */
public class GameObjectPool {
    public Transform free_root;
    Dictionary<Type, Queue<GameObject>> type_queues = new Dictionary<Type, Queue<GameObject>>();
    /*
     * GetFree失败后，需要调用者去创建GameObject。
     */
    public GameObject GetFree(Type type)
    {
        Debug.Log("trying " + type);
        if (!type_queues.ContainsKey(type))
        {
            Debug.Log("failed");
            return null;
        }
        Queue<GameObject> queue_free = type_queues[type];
        if (queue_free.Count > 0)
        {
            queue_free.Peek().SetActive(true);
            queue_free.Peek().SendMessage("Reuse", SendMessageOptions.RequireReceiver);
            return queue_free.Dequeue();
        }
        return null;
    }
    /*
     * 添加一个Free对象
     */
    public void AddFree(GameObject free_obj)
    {
        if (free_obj == null) return;
        free_obj.SetActive(false);
        free_obj.transform.parent = free_root;
        Queue<GameObject> queue_free;
        if (type_queues.ContainsKey(free_obj.GetType()))
        {
            queue_free = type_queues[free_obj.GetType()];;
        } 
        else
        {
            queue_free = new Queue<GameObject>();
            type_queues[free_obj.GetType()] = queue_free;
        }
        Debug.Log("add " + free_obj.GetType());
        queue_free.Enqueue(free_obj);
    }
    public void Clear()
    {
        type_queues.Clear();
    }
}
