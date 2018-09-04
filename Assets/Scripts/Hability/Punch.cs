using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    Vector3 _dir;

    private void Update()
    {
        transform.position += _dir.normalized * 50 * Time.deltaTime;
    }

    public void Hit(Vector3 dir)
    {
        _dir = dir;
        transform.up = -dir;
    }

}
