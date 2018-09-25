using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public SectionType sectionType;

    public enum SectionType
    {
        Empty,
        Character,
        Random
    }
}
