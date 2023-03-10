using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineOfSight : MonoBehaviour
{
    #region variables
    #region Edittable
    // Colour of Line Of Sight Visualization
    [SerializeField]
    Color sensorColour = Color.green;

    // Line Of Sight Visualization Variables
    [SerializeField]
    float angle = 30;
    [SerializeField]
    float distance = 10;
    [SerializeField]
    float height = 1.0f;

    // Scan for Objects frequency
    [SerializeField]
    int scanFrequency = 30;
    // Layer(s) of Target
    [SerializeField]
    LayerMask targetLayers;
    // Layer(s) that the target cannot be seen through.
    [SerializeField]
    LayerMask blockLayers;
    #endregion

    // List of Objects currently in Line of Sight.
    List<GameObject> objs = new List<GameObject>();
    // List of Colliders in the surrounding sphere.
    Collider[] colliders = new Collider[25];
    // 
    int count;
    float scanInterval;
    float scanTimer;
    Mesh Sensor;
    #endregion
    public List<GameObject> Objs { get => objs; }

    private void Awake()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }
    public bool InSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        if (direction.y < 0 || direction.y > height)
            return false;

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
            return false;

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, blockLayers))
            return false;
        return true;
    }
    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, targetLayers, QueryTriggerInteraction.Collide);

        objs.Clear();
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (InSight(obj))
                objs.Add(obj);
        }
    }

    Mesh CreateSensorMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 4;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int index = 0;

        // Left Side
        vertices[index++] = bottomCenter;
        vertices[index++] = bottomLeft;
        vertices[index++] = topLeft;

        vertices[index++] = topLeft;
        vertices[index++] = topCenter;
        vertices[index++] = bottomCenter;

        // Right Side
        vertices[index++] = bottomCenter;
        vertices[index++] = topCenter;
        vertices[index++] = topRight;

        vertices[index++] = topRight;
        vertices[index++] = bottomRight;
        vertices[index++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            // Far Side
            vertices[index++] = bottomLeft;
            vertices[index++] = bottomRight;
            vertices[index++] = topRight;

            vertices[index++] = topRight;
            vertices[index++] = topLeft;
            vertices[index++] = bottomLeft;
            // Top
            vertices[index++] = topCenter;
            vertices[index++] = topLeft;
            vertices[index++] = topRight;

            // Bottom
            vertices[index++] = bottomCenter;
            vertices[index++] = bottomRight;
            vertices[index++] = bottomLeft;

            currentAngle += deltaAngle;
        }
        
        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
    private void OnValidate()
    {
        Sensor = CreateSensorMesh();
    }
    private void OnDrawGizmos()
    {
        if (Sensor)
        {
            Gizmos.color = sensorColour;
            Gizmos.DrawMesh(Sensor, transform.position, transform.rotation);
        }

     /*   Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; ++i)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 1.0f);
        }

        Gizmos.color = sensorColour;*/
        foreach (var Object in objs)
        {
            Gizmos.DrawSphere(Object.transform.position, 1.0f);
        }
    }
}
