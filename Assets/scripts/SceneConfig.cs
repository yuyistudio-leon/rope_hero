using UnityEngine;
using System.Collections;

public class SceneConfig : MonoBehaviour {
    public float hero_angle_speed = 120;
    public Transform finish_point;
    public bool use_light = false;
    public Color global_lighting = Color.white;
    public float global_lighting_strength = 0.75f;
    public string level_tip = "tap to start";

    Transform targets;
    int child_index = -1;
    void Awake()
    {
        RenderSettings.ambientLight = global_lighting;
        RenderSettings.ambientIntensity = global_lighting_strength;
        targets = transform.FindChild("TargetPoints");
    }
    void Start()
    {
        if (finish_point == null)
        {
            finish_point = transform.FindChild("FinishPoint");
        }
    }
    public void Reset()
    {
        child_index = -1;
    }
    public Vector3 NextTarget()
    {
        child_index ++;
        if (child_index < targets.childCount)
        {
            return targets.GetChild(child_index).transform.position;
        }
        return Vector3.zero;
    }
}
