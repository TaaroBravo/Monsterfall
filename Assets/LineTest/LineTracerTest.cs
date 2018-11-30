using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTracerTest : MonoBehaviour {

    public Transform initialpointGO;
    public Transform endpointGO;
    public GameObject lineMesh;
    public List<GameObject> spawnedlines = new List<GameObject>();
    public float separation;
    public Material arrowmat;
    public float speed;
    public bool startline;
    public bool updateline;
    List<Color> PColors = new List<Color>();

    private void Awake()
    {
        Color outcolor;
        separation = 2f;
        speed = 15;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
    }
    void Update()
    {
        //DebugKeys();
        Debug.Log("Debug 1");
        if (!initialpointGO) return;
        var distance = Vector3.Distance(initialpointGO.transform.position, endpointGO.transform.position);
        //speed = Mathf.Abs(200 - distance);
        var dir = (endpointGO.transform.position - initialpointGO.transform.position).normalized;
        int ammountocreate = Mathf.FloorToInt(distance / separation);
        if (startline)
        {
        Debug.Log("Debug 2");
            for (int i = 0; i < ammountocreate; i++)
            {
                var line = Instantiate(lineMesh);
                spawnedlines.Add(line);
                spawnedlines[i].transform.position = initialpointGO.transform.position + dir * i * separation;
            }
            startline = false;
            updateline = true;
        }
        if (updateline && spawnedlines.Count < ammountocreate)
        {
            for (int i = 0; i < 1; i++)
            {
                var line = Instantiate(lineMesh);
                spawnedlines.Add(line);
                line.transform.position = initialpointGO.transform.position + dir * (1+i) * separation;
            }
        }
        for (int i = 0; i < spawnedlines.Count; i++)
        {
            var actualdistance = Vector3.Distance(endpointGO.transform.position, spawnedlines[i].transform.position);
            var oppositedir = (initialpointGO.transform.position - endpointGO.transform.position).normalized;
            spawnedlines[i].transform.right = dir;
            spawnedlines[i].transform.Rotate(-100, 0, 0);
            //spawnedlines[i].transform.Rotate(
            //    initialpointGO.transform.position.x > endpointGO.transform.position.x &&
            //    Mathf.Abs(initialpointGO.transform.position.x) - Mathf.Abs(endpointGO.transform.position.x) > 4 ?
            //    new Vector3(100, 0, 0) :
            //    initialpointGO.transform.position.x < endpointGO.transform.position.x &&
            //    Mathf.Abs(initialpointGO.transform.position.x) - Mathf.Abs(endpointGO.transform.position.x) > 4 ?
            //    new Vector3(-100, 0, 0) : new Vector3(0, 0, 0));
            spawnedlines[i].transform.position =
                endpointGO.transform.position + oppositedir * actualdistance
                + dir * speed * Time.deltaTime;
            if (Vector3.Distance(spawnedlines[i].transform.position, endpointGO.transform.position) < 1f
                || Vector3.Distance(spawnedlines[i].transform.position, initialpointGO.transform.position) < 0.1f)
            {
                Destroy(spawnedlines[i]);
                spawnedlines.RemoveAt(i);
                break;
            }
        }
    }
    public void Set(Transform start, Transform end, int ID)
    {
        initialpointGO = start;
        endpointGO = end;
        arrowmat.SetColor("_Tinte", PColors[ID]);
        startline = true;
    }
    //void DebugKeys()
    //{
    //    if (Input.GetKeyDown(KeyCode.D)) Set(initialpointGO, endpointGO, Random.Range(0, PColors.Count));
    //}
}
