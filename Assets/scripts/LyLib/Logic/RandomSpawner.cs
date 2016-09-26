using UnityEngine;
using System.Collections;
using LyLib;

public class RandomSpawner : MonoBehaviour {
    public GameObject[] prefabs;
    public float min_period = 1f;
    public float max_period = 3f;
    public bool spawn_one_on_startup = false;
    public Vector3 position_randomize_factor = new Vector3(4, 4, 4);
	// Use this for initialization
	void Start () {
        StartCoroutine(Spawn());
	}
    IEnumerator Spawn()
    {
        if (!spawn_one_on_startup)
        {
            yield return new WaitForSeconds(LM.RandomFloat(min_period, max_period));
        }
        while (true)
        {
            int index = LM.RandomInt(0, prefabs.Length - 1);
            GameObject obj = Instantiate(prefabs[index]);
            obj.transform.position = transform.position + Random.onUnitSphere.Multi(position_randomize_factor);
            obj.transform.rotation = transform.rotation;
            obj.transform.localScale = transform.localScale;
            obj.transform.parent = transform;

            yield return new WaitForSeconds(LM.RandomFloat(min_period, max_period));
        }
    }
}
