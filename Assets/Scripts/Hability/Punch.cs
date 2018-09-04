using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    private void Update()
    {
        transform.position += transform.up * 50 * Time.deltaTime;
    }

    public void Hit(Vector3 dir)
    {
        transform.up = dir;
    }

}
