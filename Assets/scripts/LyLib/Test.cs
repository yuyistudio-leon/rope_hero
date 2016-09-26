using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        (new Tween.Float()).From(0).To(6)
            .OnUpdate(UpdatePosition)
            .Loop()
            .After(

            (new Tween.V3())
            .From(() => transform.position)
            .To(() => transform.position + new Vector3(5, 3, 2))
            .OnUpdate(UpdatePositionV3)
            // call all V3-specified functions before calling any functions of its base-type
            .After(

                (new Tween.V3())
                .From(() => transform.position)
                .To(() => transform.position + new Vector3(3,-3,-3))
                .OnUpdate(UpdatePositionV3)
                .After(

                (new Tween.V3())
                .From(() => transform.position)
                .To(transform.position)
                .OnUpdate(UpdatePositionV3)
                .OnComplete(()=>Debug.Log("all animations done"))

                )

            )

            ).Start();
	}
	void UpdatePosition(float v){
        transform.position = new Vector3(0,0,v);
    }
    void UpdatePositionV3(Vector3 v)
    {
        transform.position = v;
    }
}
