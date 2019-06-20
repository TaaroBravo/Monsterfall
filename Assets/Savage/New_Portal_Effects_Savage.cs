using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class New_Portal_Effects_Savage : MonoBehaviour
{
    public VisualEffect Particulas; // VFX ( visual effect )
    public Light Luz; // Point Light
    public Material Material_portal;
    bool Activated;
    bool GoingUp;
    bool GoingDown;
    float timer;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    Activated = true;
        //    GoingUp = true;
        //    Particulas.Play();
        //}
        if (Activated)
        {
            timer += Time.deltaTime;
            if (GoingUp)
            {
                Material_portal.SetFloat("Vector1_EAB94DAD", -1f); // importante
                Luz.range = timer * 200; // importante

                if (timer > 0.1)
                {
                    GoingUp = false;
                    GoingDown = true;
                    timer = 0;
                }
            }
            else if (GoingDown)
            {
                Luz.range = 23 - timer * 23; // importante
                Material_portal.SetFloat("Vector1_EAB94DAD", timer * 6); // importante
                if (timer > 1)
                {
                    GoingDown = false;
                    timer = 0;
                    Luz.range = 0;
                    Material_portal.SetFloat("Vector1_EAB94DAD", 5.08f);
                    Activated = false;
                }
            }
        }
    }
    void Activate()
    {
        Activated = true;
        GoingUp = true;
        Particulas.Play();
    }
}
