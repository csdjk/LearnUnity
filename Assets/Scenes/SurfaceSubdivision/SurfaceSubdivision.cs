using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceSubdivision : MonoBehaviour
{
    public MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        var mesh = meshFilter.mesh;
        var vertices = mesh.vertices;
        Debug.Log(vertices);
        List<Vector3> newVertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < vertices.Length; i += 3)
        {
            Debug.Log(vertices[i].ToString());
            var pos1 = vertices[i];
            var pos2 = vertices[i + 1];
            var pos3 = vertices[i + 2];
            // 
            var newPos1 = (pos1 + pos2) / 2;
            var newPos2 = (pos2 + pos3) / 2;
            var newPos3 = (pos3 + pos1) / 2;

            newVertices.Add(pos1);
            newVertices.Add(newPos1);
            newVertices.Add(newPos3);

            newVertices.Add(newPos1);
            newVertices.Add(pos2);
            newVertices.Add(newPos2);

            newVertices.Add(newPos1);
            newVertices.Add(newPos2);
            newVertices.Add(newPos3);

            newVertices.Add(newPos3);
            newVertices.Add(newPos2);
            newVertices.Add(pos3);
        }
        for (int i = 0; i < newVertices.Count; i++)
        {
            triangles.Add(i);
        }
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
