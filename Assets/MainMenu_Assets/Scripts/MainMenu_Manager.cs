using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenu_Manager : MonoBehaviour
{

    public List<Transform> objectstomove;
    // 0 - Left Section
    // 1 - Right Section
    // 2 - Left Loading
    // 3 - Right Loading
    // 4 - Match Configuration 1 ( Mode )
    // 5 - Match Configuration 2 ( Win Condition )
    // 6 - Match Configuration 3 ( Map Shuffle )
    // 7 - Loading Bar Bundle
    public List<Image> objectstoappear;
    // 0 - Black Background
    // 1 - Loading Text
    // 2 - Loading Text Dot 1
    // 3 - Loading Text Dot 2
    // 4 - Loading Text Dot 3
    public List<float> Timers;
    // 0 - Transition timer 1;
    // 1 - Transition timer 2;
    // 2 - Loop timer;
    // 3 - Exception timer;
    public GameObject selector;
    public GameObject model_background;
    List<Vector3> initialpositions = new List<Vector3>();
    List<Vector3> finalpositions = new List<Vector3>();
    List<bool> Play_Transition_Steps = new List<bool>();
    List<bool> Loading_Loop_Steps = new List<bool>();
    List<bool> Loading_Finish_Steps = new List<bool>();
    bool Play_To_Loading_Transition;
    bool Loading_To_Game_Transition;
    bool Loading_Loop;
    bool Fade_To_Black;

    // Use this for initialization
    void Start()
    {
        Assign_Initials_Positions();
        Assign_Finals_Positions();
        Hide_Loading_Hud();
        for (int i = 0; i < 7; i++) Play_Transition_Steps.Add(true);
        for (int i = 0; i < 7; i++) Loading_Loop_Steps.Add(true);
        for (int i = 0; i < 5; i++) Loading_Finish_Steps.Add(true);
        for (int i = 0; i < 4; i++) Timers.Add(0);
    }
    void Update()
    {
        if (GameObject.FindObjectsOfType<MusicInGame>().Count() > 1)
            Destroy(GameObject.FindObjectsOfType<MusicInGame>().First().gameObject);
        model_background.transform.Rotate(0, 0.5f * Time.deltaTime, 0);
        if (Play_To_Loading_Transition)
        {
            for (int i = 0; i < Play_Transition_Steps.Count; i++)
            {
                if (Play_Transition_Steps[0])
                {
                    Hide(selector, false);
                    Move(objectstomove[0], initialpositions[0], finalpositions[0], 0.8f, 0, 0, Play_To_Loading_Transition);
                    Move(objectstomove[1], initialpositions[1], finalpositions[1], 0.8f, 0, 1, Play_To_Loading_Transition);
                    break;
                }
                else if (Play_Transition_Steps[1])
                {
                    Move(objectstomove[2], finalpositions[2], initialpositions[2], 0.5f, 1, 0, Play_To_Loading_Transition);
                    Move(objectstomove[3], finalpositions[3], initialpositions[3], 0.5f, 1, 1, Play_To_Loading_Transition);
                    break;
                }
                else if (Play_Transition_Steps[2])
                {
                    FadeIn(objectstoappear[1], 0.5f, 2, 0, Play_To_Loading_Transition);
                    break;
                }
                else if (Play_Transition_Steps[3])
                {
                    Move(objectstomove[4], finalpositions[4], initialpositions[4], 0.25f, 3, 0, Play_To_Loading_Transition);
                    break;
                }
                else if (Play_Transition_Steps[4])
                {
                    Move(objectstomove[5], finalpositions[5], initialpositions[5], 0.25f, 4, 0, Play_To_Loading_Transition);
                    break;
                }
                else if (Play_Transition_Steps[5])
                {
                    Move(objectstomove[6], finalpositions[6], initialpositions[6], 0.25f, 5, 0, Play_To_Loading_Transition);
                    break;
                }
                else if (Play_Transition_Steps[6])
                {
                    Loading_Loop = true;
                    Transition_Reset(Play_Transition_Steps, Play_To_Loading_Transition, false);
                    break;
                }
            }
        }
        if (Loading_Loop)
        {
            for (int i = 0; i < Loading_Loop_Steps.Count; i++)
            {
                if (Loading_Loop_Steps[0])
                {
                    FadeIn(objectstoappear[2], 0.25f, 0, 2, Loading_Loop);
                    break;
                }
                else if (Loading_Loop_Steps[1])
                {
                    FadeIn(objectstoappear[3], 0.25f, 1, 2, Loading_Loop);
                    break;
                }
                else if (Loading_Loop_Steps[2])
                {
                    FadeIn(objectstoappear[4], 0.25f, 2, 2, Loading_Loop);
                    break;
                }
                else if (Loading_Loop_Steps[3])
                {
                    FadeOut(objectstoappear[2], 0.25f, 3, 2, Loading_Loop);
                    break;
                }
                else if (Loading_Loop_Steps[4])
                {
                    FadeOut(objectstoappear[3], 0.25f, 4, 2, Loading_Loop);
                    break;
                }
                else if (Loading_Loop_Steps[5])
                {
                    FadeOut(objectstoappear[4], 0.25f, 5, 2, Loading_Loop);
                    break;
                }
                else if (Loading_Loop_Steps[6])
                {
                    Transition_Reset(Loading_Loop_Steps, Loading_Loop, true);
                    break;
                }
            }
        }
        if (Loading_To_Game_Transition)
        {
            for (int i = 0; i < Loading_Finish_Steps.Count; i++)
            {
                if (Loading_Finish_Steps[0])
                {
                    Rotate(objectstomove[7], true, new Vector3(3, 0, 0), 0.5f, 0, 0, Loading_To_Game_Transition);
                    break;
                }
                else if (Loading_Finish_Steps[1])
                {
                    Hide(objectstoappear[1].gameObject, false);
                    Hide(objectstoappear[2].gameObject, false);
                    Hide(objectstoappear[3].gameObject, false);
                    Hide(objectstoappear[4].gameObject, false);
                    Hide(objectstomove[2].gameObject, false);
                    Hide(objectstomove[3].gameObject, true, 1, Loading_To_Game_Transition);
                    break;
                }
                else if (Loading_Finish_Steps[2])
                {
                    List<Transform> Objects_to_rotate = new List<Transform>();
                    Objects_to_rotate.Add(objectstomove[4]);
                    Objects_to_rotate.Add(objectstomove[5]);
                    Objects_to_rotate.Add(objectstomove[6]);
                    RotateMany(Objects_to_rotate, new Vector3(3, 0, 0), 0.5f, 2, 0, Loading_To_Game_Transition);
                    break;
                }
                else if (Loading_Finish_Steps[3])
                {
                    Hide(objectstomove[4].gameObject, false);
                    Hide(objectstomove[5].gameObject, false);
                    Hide(objectstomove[6].gameObject, true, 3, Loading_To_Game_Transition);
                    break;
                }
                else if (Loading_Finish_Steps[4])
                {
                    Transition_Reset(Loading_Finish_Steps, Loading_To_Game_Transition, false);
                    break;
                }
            }
        }
        if (Fade_To_Black)
        {
            Timers[3] += Time.deltaTime;
            objectstoappear[0].color = new Color(objectstoappear[0].color.r, objectstoappear[0].color.g, objectstoappear[0].color.b, 0 + Timers[3] / 5f);
            if (Timers[3] >= 5f) Fade_To_Black = false;
        }
    }
    void Assign_Initials_Positions()
    {
        for (int i = 0; i < objectstomove.Count; i++) initialpositions.Add(objectstomove[i].position);
    }
    void Assign_Finals_Positions()
    {
        for (int i = 0; i < objectstomove.Count; i++)
        {
            if (i == 0) finalpositions.Add(objectstomove[i].position + new Vector3(-1000, 0, 0));
            else if (i == 1) finalpositions.Add(objectstomove[i].position + new Vector3(1000, 0, 0));
            else if (i == 2) finalpositions.Add(objectstomove[i].position + new Vector3(-1000, 0, 0));
            else if (i == 3) finalpositions.Add(objectstomove[i].position + new Vector3(1000, 0, 0));
            else finalpositions.Add(objectstomove[i].position + new Vector3(-1000, 0, 0));
        }
    }
    void Hide_Loading_Hud()
    {
        for (int i = 2; i < objectstomove.Count - 1; i++) objectstomove[i].position = finalpositions[i];
        for (int i = 0; i < objectstoappear.Count; i++) objectstoappear[i].color = new Color(objectstoappear[i].color.r, objectstoappear[i].color.g, objectstoappear[i].color.b, 0);
    }
    void Show(GameObject Object, bool EndStep, int Step_index = 0, bool Transition = false)
    {
        Object.SetActive(true);
        if (EndStep) End_Step(Step_index, 0, Transition);
    }
    void Hide(GameObject Object, bool EndStep, int Step_index = 0, bool Transition = false)
    {
        Object.SetActive(false);
        if (EndStep) End_Step(Step_index, 0, Transition);
    }
    void FadeIn(Image Object, float time, int Step_index, int Timer_Index, bool Transition)
    {
        Timers[Timer_Index] += Time.deltaTime;
        Object.color = new Color(Object.color.r, Object.color.g, Object.color.b, 0 + Timers[Timer_Index] / time);
        if (Timers[Timer_Index] >= time) End_Step(Step_index, Timer_Index, Transition);
    }
    void FadeOut(Image Object, float time, int Step_index, int Timer_Index, bool Transition)
    {
        Timers[Timer_Index] += Time.deltaTime;
        Object.color = new Color(Object.color.r, Object.color.g, Object.color.b, 1 - Timers[Timer_Index] / time);
        if (Timers[Timer_Index] >= time) End_Step(Step_index, Timer_Index, Transition);
    }
    void Wait(float time, int Step_index, int Timer_Index, bool Transition)
    {
        Timers[Timer_Index] += Time.deltaTime;
        if (Timers[Timer_Index] >= time) End_Step(Step_index, Timer_Index, Transition);
    }
    void Move(Transform Object, Vector3 initialposition, Vector3 finalposition, float time, int Step_index, int Timer_Index, bool Transition)
    {
        Timers[Timer_Index] += Time.deltaTime;
        Object.position = Vector3.Lerp(initialposition, finalposition, Timers[Timer_Index] / time);
        if (Timers[Timer_Index] > time) End_Step(Step_index, Timer_Index, Transition);
    }
    void Rotate(Transform Object, bool EndStep, Vector3 rotation_ammount, float time, int Step_index = 0, int Timer_Index = 0, bool Transition = false)
    {
        Timers[Timer_Index] += Time.deltaTime;
        Object.Rotate(rotation_ammount);
        if (Timers[Timer_Index] > time && EndStep) End_Step(Step_index, Timer_Index, Transition);
    }
    void RotateMany(List<Transform> Objects, Vector3 rotation_ammount, float time, int Step_index, int Timer_Index, bool Transition)
    {
        Timers[Timer_Index] += Time.deltaTime;
        for (int i = 0; i < Objects.Count; i++) Objects[i].Rotate(rotation_ammount);
        if (Timers[Timer_Index] > time) End_Step(Step_index, Timer_Index, Transition);
    }
    void Move_and_Rotate(Transform Object, Vector3 initialposition, Vector3 finalposition, float rotation_ammount, float rotate_time, float move_time, int Step_index, int Timer_Index, bool Transition)
    {
        Timers[Timer_Index] += Time.deltaTime;
        Object.position = Vector3.Lerp(initialposition, finalposition, Timers[Timer_Index] / move_time);
        if (Timers[Timer_Index] < rotate_time) Object.Rotate(0, rotation_ammount, 0);
        if (Object.position == finalposition) End_Step(Step_index, Timer_Index, Transition);
    }
    void End_Step(int Step_Index, int Timer_Index, bool Transition)
    {
        if (Transition.Equals(Play_To_Loading_Transition)) Play_Transition_Steps[Step_Index] = false;
        else if (Transition.Equals(Loading_Loop)) Loading_Loop_Steps[Step_Index] = false;
        else if (Transition.Equals(Loading_To_Game_Transition)) Loading_Finish_Steps[Step_Index] = false;
        Timers[Timer_Index] = 0;
    }
    void Transition_Reset(List<bool> Steps, bool trigger, bool restart)
    {
        for (int i = 0; i < Steps.Count; i++) Steps[i] = true;
        if (!restart)
        {
            if (trigger.Equals(Play_To_Loading_Transition)) Play_To_Loading_Transition = false;
            else if (trigger.Equals(Loading_Loop)) Loading_Loop = false;
            else if (trigger.Equals(Loading_To_Game_Transition)) Loading_To_Game_Transition = false;
        }
    }
    public void Play_Transition()
    {
        Play_To_Loading_Transition = true;
        Fade_To_Black = true;
        StartCoroutine(TimeToLoading());
    }

    IEnumerator TimeToLoading()
    {
        yield return new WaitForSeconds(5f);
        Finish_Loading();
    }

    public void Finish_Loading()
    {
        Loading_Loop = false;
        Loading_To_Game_Transition = true;
        StartCoroutine(TimeToCharacterSelection());
    }

    IEnumerator TimeToCharacterSelection()
    {
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(1);
    }
}
