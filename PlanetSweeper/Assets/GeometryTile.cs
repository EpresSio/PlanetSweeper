using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryTile
{
    private List<Vector3> _vertices = new List<Vector3>();
    public List<Vector3> Vertices { get { return _vertices; } } 
    private List<int> _triangles = new List<int>();
    public List<int> Triangles { get { return _triangles; } }

    public Dictionary<Vector3, List<Vector3>> CalucalteVertexNightbourHood()
    {
        Dictionary<Vector3, List<Vector3>> neighborHood = new Dictionary<Vector3, List<Vector3>>();
        int count = _triangles.Count;
        for (int i = 0; i < count; i++)
        {
            Vector3 currentVertex = _vertices[_triangles[i]];
            if (!neighborHood.ContainsKey(currentVertex))
            {
                neighborHood.Add(currentVertex, new List<Vector3>());
            }

            if (count == 1)
            {
                break;
            }

            List<Vector3> currentNeighbours = neighborHood[currentVertex];

            if (i == 0)
            {
                currentNeighbours.Add(_vertices[_triangles[count - 1]]);
                currentNeighbours.Add(_vertices[_triangles[i + 1]]);
            } else if (i == count - 1)
            {
                currentNeighbours.Add(_vertices[_triangles[i - 1]]);
                currentNeighbours.Add(_vertices[_triangles[0]]);
            } else
            {
                currentNeighbours.Add(_vertices[_triangles[i + 1]]);
                currentNeighbours.Add(_vertices[_triangles[i - 1]]);
            }
        }
        return neighborHood;
    }
}
