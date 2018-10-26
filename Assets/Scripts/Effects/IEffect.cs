using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect {

    void Effect(PlayerController player);

    void DisableEffect(PlayerController player);

    float GetMaxTimer();

    float GetDelayTimer();

}
