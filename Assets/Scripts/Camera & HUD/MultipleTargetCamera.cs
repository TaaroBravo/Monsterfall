using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();

    public Vector3 offset;
    public float smoothTime = 0.5f;

    public float minZoom = 100f;
    public float maxZoom = 65f;
    public float zoomLimiter = 50f;

    public float posLimitX;
    public float negLimitX;
    public float posLimitY;
    public float negLimitY;

    private Vector3 velocity;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        GameManager.Instance.OnSpawnCharacters += x => SetPlayers(x);
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        for (int i = 0; i < targets.Count; i++)
            if (targets[i] == null)
                targets.Remove(targets[i]);

        Move();
        Zoom();
        BoundsOfCamera();
    }

    void BoundsOfCamera()
    {
        if (transform.position.x > posLimitX)
            transform.position = new Vector3(posLimitX, transform.position.y, transform.position.z);

        if (transform.position.x < negLimitX)
            transform.position = new Vector3(negLimitX, transform.position.y, transform.position.z);

        if (transform.position.y > posLimitY)
            transform.position = new Vector3(transform.position.x, posLimitY, transform.position.z);

        if (transform.position.y < negLimitY)
            transform.position = new Vector3(transform.position.x, negLimitY, transform.position.z);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
            bounds.Encapsulate(targets[i].position);

        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
            bounds.Encapsulate(targets[i].position);

        return bounds.center;
    }

    void SetPlayers(List<PlayerController> heroes)
    {
        targets.Clear();
        foreach (var hero in heroes)
            targets.Add(hero.transform);
    }

}
