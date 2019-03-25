using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour {

    public Vector3 CenterShakePosition;
    public bool Shaking;
    public float ShakeRange;

	void FixedUpdate ()
    {
        transform.Rotate(0, 0, 1);
        if (Shaking && gameObject.activeSelf)
        {
            transform.position = CenterShakePosition + new Vector3
                (
                    Random.Range(-ShakeRange, ShakeRange),
                    Random.Range(-ShakeRange, ShakeRange),
                    Random.Range(-ShakeRange, ShakeRange)
                );
        }
    }
}
