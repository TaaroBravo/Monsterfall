using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CreatorRays : MonoBehaviour
{
    public static CreatorRays Instance { get; private set; }

    public GameObject cristal;

    public Material material;
    public Transform[] rays;
    public PlayerController[] players;

    Vector3[] vertices = new Vector3[10];
    Vector2[] uv = new Vector2[10];
    int[] triangles = new int[8];

    bool activeMode;
    int countOfRays = 1;

    Vector3 lowPos;
    public Vector3 highPos;
    public float speed = 2f;
    public float duration = 5f;

    private float _timeToDeath;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        vertices = new Vector3[10];
        uv = new Vector2[10];
        triangles = new int[] {

            0,1,2,
            2,3,0,
            3,4,0,
            4,5,0,
            5,6,0,
            6,7,0,
            7,8,0,
            8,9,0
        };
        lowPos = cristal.transform.position - new Vector3(0, 0.25f, 0);
        highPos = lowPos + new Vector3(0, 0.5f, 0);
        StartCoroutine(StartCoroutine());
        CalculateTimeToDeath();
        StartCoroutine(WaitToSuddenDeath(_timeToDeath));
    }

    void CalculateTimeToDeath()
    {
        countOfRays = 2;
        if (players.Count() == 4)
            _timeToDeath = 55f;
        else if(players.Count() == 3)
            _timeToDeath = 40f;
        else if (players.Count() == 2)
            _timeToDeath = 25f;
    }

    public void SetPlayers(PlayerController[] _players)
    {
        players = _players;
    }

    private void Update()
    {
        MoveRays();
        if (activeMode)
        {
            cristal.SetActive(true);
            var activeRays = rays.Take(countOfRays);

            foreach (var ray in activeRays)
                CalculatePoints(ray);
            foreach (var ray in GameObject.FindGameObjectsWithTag("CristalRay").Skip(countOfRays))
                Destroy(ray);
        }
        else
        {
            cristal.SetActive(false);
            foreach (var cristalRay in GameObject.FindGameObjectsWithTag("CristalRay"))
                Destroy(cristalRay);
        }

        #region Inputs
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (activeMode) activeMode = false;
            else activeMode = true;
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    countOfRays = 1;
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    countOfRays = 2;
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //    countOfRays = 3;
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //    countOfRays = 4;
        #endregion
    }

    IEnumerator StartCoroutine()
    {
        while (true)
        {
            yield return RepeatLerp(lowPos, highPos, duration);
            yield return RepeatLerp(highPos, lowPos, duration);
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0f;
        float rate = (1f / time) * speed;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            cristal.transform.position = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    IEnumerator WaitToSuddenDeath(float x)
    {
        while (true)
        {
            yield return new WaitForSeconds(x);
            //cristal.SetActive(true);
            activeMode = true;
            countOfRays = 0;
            yield return ChangeScale(cristal.transform.Find("Cristal").localScale, new Vector3(0.3290589f, 0.3290589f, 0.239385f),1f);
            //yield return new WaitForSeconds(1f);
            countOfRays = 2;
            break;
        }
    }

    public IEnumerator ChangeScale(Vector3 a, Vector3 b, float time)
    {
        float i = 0f;
        float rate = (1f / time);
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            cristal.transform.Find("Cristal").localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    void CreateRaysObject(List<Tuple<float, float>> positions, Transform reference)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(positions[i].Item1, positions[i].Item2);
            uv[i] = new Vector3(positions[i].Item1, positions[i].Item2);
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        GameObject currentMesh = GameObject.Find("Mesh " + reference.name);
        if (!currentMesh)
        {
            GameObject rayObject = new GameObject("Mesh " + reference.name, typeof(MeshFilter), typeof(MeshRenderer), typeof(DynamicCristal));

            rayObject.GetComponent<DynamicCristal>().cristal = reference;
            rayObject.GetComponent<DynamicCristal>().players = players;
            rayObject.GetComponent<MeshFilter>().mesh = mesh;
            rayObject.GetComponent<MeshRenderer>().material = material;
            rayObject.tag = "CristalRay";
            rayObject.GetComponent<DynamicCristal>().CalculateCollisions(positions);
        }
        else
        {
            currentMesh.GetComponent<MeshFilter>().mesh = mesh;
            currentMesh.GetComponent<MeshRenderer>().material = material;
            currentMesh.GetComponent<DynamicCristal>().CalculateCollisions(positions);
        }

    }

    void CalculatePoints(Transform reference)
    {
        List<Tuple<float, float>> positions = new List<Tuple<float, float>>();

        positions.Add(Tuple.Create(reference.position.x, reference.position.y));

        #region Vectors
        Vector3 v0 = (reference.up + reference.transform.right).normalized;
        Vector3 v1 = (reference.up + v0).normalized;
        Vector3 v2 = (reference.up + v1).normalized;
        Vector3 v3 = (reference.up + v2).normalized;
        Vector3 v4 = (reference.up + v3).normalized;
        Vector3 v5 = (reference.up + v4).normalized;
        Vector3 v6 = (v4 + v3).normalized;
        Vector3 v7 = (v3 + v2).normalized;
        Vector3 v8 = (v7 + v3).normalized;
        Vector3 v9 = (v7 + v2).normalized;
        #endregion

        #region Layers
        var layerMaskIgnore1 = 1 << 8;
        var layerMaskIgnore2 = 1 << 9;
        var layerMaskIgnore3 = 1 << 13;
        var layerMaskIgnore4 = 1 << 18;
        var layerMaskIgnore5 = 1 << 20;
        var layerMask = layerMaskIgnore1 | layerMaskIgnore2 | layerMaskIgnore3 | layerMaskIgnore4 | layerMaskIgnore5;
        layerMask = ~layerMask;
        #endregion

        #region Raycast
        RaycastHit info;

        if (Physics.Raycast(reference.position, reference.up, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v5, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v4, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v6, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v3, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v8, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v7, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v9, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.position, v2, out info, 1000, layerMask))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        #endregion

        CreateRaysObject(positions, reference);
    }

    void MoveRays()
    {
        foreach (var ray in rays)
            ray.Rotate(0, 0, -20 * Time.deltaTime);
    }

    #region OnDrawGizmos
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + reference.transform.up * 50);
    //    //Gizmos.DrawLine(reference.transform.position, reference.transform.position + reference.transform.right * 50);

    //    Vector3 v0 = (reference.transform.up + reference.transform.right).normalized;
    //    Vector3 v1 = (reference.transform.up + v0).normalized;
    //    Vector3 v2 = (reference.transform.up + v1).normalized;
    //    Vector3 v3 = (reference.transform.up + v2).normalized;
    //    Vector3 v4 = (reference.transform.up + v3).normalized;
    //    Vector3 v5 = (reference.transform.up + v4).normalized;
    //    Vector3 v6 = (v4 + v3).normalized;
    //    Vector3 v7 = (v3 + v2).normalized;
    //    Vector3 v8 = (v7 + v3).normalized;
    //    Vector3 v9 = (v7 + v2).normalized;

    //    //Gizmos.DrawLine(reference.transform.position, reference.transform.position + v0 * 50);
    //    //Gizmos.DrawLine(reference.transform.position, reference.transform.position + v1 * 50);


    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v2 * 50);
    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v3 * 50);
    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v4 * 50);
    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v5 * 50);
    //    //Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v6 * 50);
    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v7 * 50);
    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v8 * 50);
    //    Gizmos.DrawLine(reference.transform.position, reference.transform.position + v9 * 50);


    //}
    #endregion

}
