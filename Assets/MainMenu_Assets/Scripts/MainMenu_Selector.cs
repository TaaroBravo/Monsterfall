using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Selector : MonoBehaviour {

    MainMenu_Manager MM_Manager;
    public List<Transform> positions;
    public float rotation_speed;
    int currentindex;

    void Start()
    {
        MM_Manager = GetComponent<MainMenu_Manager>();
        currentindex = 0;
        MM_Manager.selector.transform.position = positions[currentindex].position;
    }

    void Update()
    {
        MM_Manager.selector.transform.Rotate(0, 0, rotation_speed);
        if (Input.GetKeyDown(KeyCode.DownArrow)) GoDown();
        if (Input.GetKeyDown(KeyCode.UpArrow)) GoUp();
        if (Input.GetKeyDown(KeyCode.Return) && currentindex == 1) MM_Manager.Play_Transition();
        if (Input.GetKeyDown(KeyCode.Space)) MM_Manager.Finish_Loading();
    }
    void GoDown()
    {
        if (positions[currentindex + 1])
        {
            currentindex++;
            MM_Manager.selector.transform.position = positions[currentindex].position;
            MM_Manager.selector.transform.rotation = positions[currentindex].rotation;

        }
    }
    void GoUp()
    {
        if (positions[currentindex - 1])
        {
            currentindex--;
            MM_Manager.selector.transform.position = positions[currentindex].position;
            MM_Manager.selector.transform.rotation = positions[currentindex].rotation;
        }
    }
}
