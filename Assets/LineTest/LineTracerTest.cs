using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTracerTest : MonoBehaviour {

    public GameObject initialpointGO;
    public GameObject endpointGO;
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
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
    }
    void Update()
    {
        //DebugKeys();
        var distance = Vector3.Distance(initialpointGO.transform.position, endpointGO.transform.position);
        //speed = Mathf.Abs(200 - distance);
        var dir = (endpointGO.transform.position - initialpointGO.transform.position).normalized;
        int ammountocreate = Mathf.FloorToInt(distance / separation);
        if (startline)
        {
            for (int i = 0; i < ammountocreate; i++)
            {
                var line = Instantiate(lineMesh);
                spawnedlines.Add(line);
                spawnedlines[i].transform.position = initialpointGO.transform.position + dir * (2 + i) * separation;
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
                line.transform.position = initialpointGO.transform.position + dir * (2 + i) * separation;
            }
        }
        for (int i = 0; i < spawnedlines.Count; i++)
        {
            var actualdistance = Vector3.Distance(endpointGO.transform.position, spawnedlines[i].transform.position);
            var oppositedir = (initialpointGO.transform.position - endpointGO.transform.position).normalized;
            spawnedlines[i].transform.right = dir;
            spawnedlines[i].transform.Rotate(
                initialpointGO.transform.position.z > endpointGO.transform.position.z &&
                Mathf.Abs(initialpointGO.transform.position.z) - Mathf.Abs(endpointGO.transform.position.z) > 40 ?
                new Vector3(100, 0, 0) :
                initialpointGO.transform.position.z < endpointGO.transform.position.z &&
                Mathf.Abs(initialpointGO.transform.position.z) - Mathf.Abs(endpointGO.transform.position.z) > 30 ?
                new Vector3(-100, 0, 0) : new Vector3(0, 0, 0));
            spawnedlines[i].transform.position =
                endpointGO.transform.position + oppositedir * actualdistance
                + dir * speed * Time.deltaTime;
            if (Vector3.Distance(spawnedlines[i].transform.position, endpointGO.transform.position) < 10f ||
                Vector3.Distance(spawnedlines[i].transform.position, initialpointGO.transform.position) < 5f)
            {
                Destroy(spawnedlines[i]);
                spawnedlines.RemoveAt(i);
                break;
            }
        }
    }
    public void Set(GameObject start, GameObject end, int ID)
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
