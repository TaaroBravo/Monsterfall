using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    bool checker;
    float timer;
    public bool oscilator;
    public float limit;
    public float growth;
    public float rotation_speed;
    void Update()
    {
        if (!oscilator)
        {
            transform.Rotate(0, 0, rotation_speed);
            timer = timer > limit ? 0 : timer += Time.deltaTime;
            checker = timer > limit ? !checker : checker;
            transform.localScale =
                timer < limit && checker ? transform.localScale + new Vector3(growth, growth, growth) :
                transform.localScale - new Vector3(growth, growth, growth);
        }
        else 
        {
            timer = timer > limit ? 0 : timer += Time.deltaTime;
            checker = timer > limit ? !checker : checker;
            transform.Rotate(0,timer < limit && checker ? rotation_speed : -rotation_speed, 0);     
        }
    }
}
