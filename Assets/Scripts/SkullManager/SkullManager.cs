using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullManager : MonoBehaviour {

    public List<Skull> skulls; // scripts de las calaveras

    public void ActivateSkulls(int ammount) //se le pasa la cantidad de kills que obtuvo el player
    {
        for (int i = 0; i < skulls.Count; i++)
        {
            if (!skulls[i].normalstate && ammount > 0)
            {
                skulls[i].Activate();
                ammount--;
            }
        }
    }
    public void NormalizeSkulls() // pone las calaveras amarillas como normales
    {
        for (int i = 0; i < skulls.Count; i++)
        {
            if (skulls[i].yellowstate)
                skulls[i].Normalize();
        }
    }

    //void DebugKeys()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q)) ActivateSkulls(2);
    //    if (Input.GetKeyDown(KeyCode.W)) NormalizeSkulls();
    //}
}
