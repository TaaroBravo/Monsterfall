using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiritoBrillo : MonoBehaviour {
	void Update () {
        GetComponent<RectTransform>().Rotate(0, 0, 1);
	}
}
