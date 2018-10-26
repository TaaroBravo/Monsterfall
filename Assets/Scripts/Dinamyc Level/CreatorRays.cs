using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreatorRays : MonoBehaviour
{
    public Material material;
    public GameObject reference;

    Vector3[] vertices = new Vector3[4];
    Vector2[] uv = new Vector2[4];
    int[] triangles = new int[6];

    void Start()
    {
        vertices = new Vector3[4];
        uv = new Vector2[4];
        triangles = new int[6];
        CalculatePoints();
    }

    void CreateRaysObject(List<Tuple<float, float>> positions)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(positions[i].Item1, positions[i].Item2);
            uv[i] = new Vector3(positions[i].Item1, positions[i].Item2);
        }

        for (int i = 0; i < triangles.Length; i++)
            triangles[i] = i;

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GameObject rayObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

        rayObject.GetComponent<MeshFilter>().mesh = mesh;
        rayObject.GetComponent<MeshRenderer>().material = material;

    }

    void CalculatePoints()
    {
        List<Tuple<float, float>> positions = new List<Tuple<float, float>>();

        positions.Add(Tuple.Create(reference.transform.position.x, reference.transform.position.y));
        positions.Add(Tuple.Create(reference.transform.position.x, reference.transform.position.y));

        RaycastHit info;
        if (Physics.Raycast(reference.transform.position, reference.transform.up, out info, 1000))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        if (Physics.Raycast(reference.transform.position, reference.transform.right, out info, 1000))
            positions.Add(Tuple.Create(info.point.x, info.point.y));

        CreateRaysObject(positions);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(reference.transform.position, reference.transform.up * 1000);
        Gizmos.DrawLine(reference.transform.position, reference.transform.right * 1000);
    }

}
