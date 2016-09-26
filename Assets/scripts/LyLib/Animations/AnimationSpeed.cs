using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationSpeed : MonoBehaviour {
    [SerializeField]
    public string[] speed;

    public Animation anim;

    Dictionary<string, float> anims_speed = new Dictionary<string, float>();
 
	// Use this for initialization
	void Start () {
        if (anim == null)
        {
            anim = GetComponent<Animation>();
        }
        foreach (var s in speed)
        {
            var f = s.Split();
            if (f.Length != 2)
            {
                Debug.LogError(transform.parent.name + "." + name + " animation speed componet error");
                return;
            }
            var anim_name = f[0];
            var anim_speed = float.Parse(f[1]);
            anims_speed[anim_name] = anim_speed;
            anim[f[0]].speed = anim_speed;
        }
	}
    public void ResetSpeed(string name)
    {
        anim[name].speed = anims_speed[name];
    }
}
