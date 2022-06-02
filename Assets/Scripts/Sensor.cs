using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class Sensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 1;
    public int segments = 10;
    public Color meshColor = Color.magenta;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> objectsInSight = new List<GameObject>();

    private Collider[] _colliders = new Collider[50];
    private Mesh _mesh;
    private int _count;
    private float _scanInterval;
    private float _scanTimer;

    private void Start()
    {
        _scanInterval = 1.0f / scanFrequency;
        
    }

    private void Update()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer < 0)
        {
            _scanTimer += _scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position, distance, _colliders, layers,
            QueryTriggerInteraction.Collide);
        
        objectsInSight.Clear();
        for (int i = 0; i < _count; i++)
        {
            GameObject obj = _colliders[i].gameObject;
            if (IsInSight(obj))
            {
                objectsInSight.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject target)
    {
        Vector3 origin = transform.position;
        Vector3 dest = target.transform.position;
        Vector3 dir = dest - origin;

        if (gameObject == target)
        {
            return false;
        }
        
        if (dir.y < 0 || dir.y > height)
        {
            return false;
        }

        dir.y = 0;
        float deltaAngle = Vector3.Angle(dir, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }


        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int numTriangles = (segments * 4) + 2 + 2;
        int numVerticies = numTriangles * 3;
        Vector3[] verticies = new Vector3[numVerticies];
        int[] triangles = new int[numVerticies];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;
        
        // left side
        verticies[vert++] = bottomCenter;
        verticies[vert++] = bottomLeft;
        verticies[vert++] = topLeft;
        
        verticies[vert++] = topLeft;
        verticies[vert++] = topCenter;
        verticies[vert++] = bottomCenter;
        
        // right side
        verticies[vert++] = bottomCenter;
        verticies[vert++] = topCenter;
        verticies[vert++] = topRight;
        
        verticies[vert++] = topRight;
        verticies[vert++] = bottomRight;
        verticies[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;
            
            
            // far side
            verticies[vert++] = bottomLeft;
            verticies[vert++] = bottomRight;
            verticies[vert++] = topRight;
        
            verticies[vert++] = topRight;
            verticies[vert++] = topLeft;
            verticies[vert++] = bottomLeft;
        
            // top
            verticies[vert++] = topCenter;
            verticies[vert++] = topLeft;
            verticies[vert++] = topRight;
        
            // bottom
            verticies[vert++] = bottomCenter;
            verticies[vert++] = bottomRight;
            verticies[vert++] = bottomLeft;
            
            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVerticies; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        
        return mesh;
    }

    private void OnValidate()
    {
        _mesh = CreateWedgeMesh();
        _scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if(!_mesh) return;
        Gizmos.color = meshColor;
        Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < _count; i++)
        {
            Gizmos.DrawSphere(_colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.cyan;
        foreach (var obj in objectsInSight)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
