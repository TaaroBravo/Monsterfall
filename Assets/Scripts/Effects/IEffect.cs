using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect {

    void Effect(PlayerController player);

    float GetMaxTimer();

    float GetDelayTimer();

}
