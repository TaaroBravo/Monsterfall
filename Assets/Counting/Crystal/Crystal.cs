﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour {

	void Update () {
        transform.Rotate(0, 0, 10f * Time.deltaTime);
	}
}
