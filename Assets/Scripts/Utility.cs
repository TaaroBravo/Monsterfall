using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class Utility {

    public static T Log<T>(T elem, string message = "")
    {
        Debug.Log(elem + message);
        return elem;
    }

    public static IEnumerable<T> ChildrenWithComponent<T>(this Transform parent)
    {
        return parent.OfType<Transform>().Select(x => x.GetComponent<T>()).Where(x => x != null);
    }

}
