using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryTile
{
    private List<Vector3> _vertices = new List<Vector3>();
    public List<Vector3> Vertices { get { return _vertices; } } 
    private List<int> _triangles = new List<int>();
    public List<int> Triangles { get { return _triangles; } }
    private List<int> _helperVerticeIndexes = new List<int>();
    public List<int> HelperVerticeIndexes { get { return _helperVerticeIndexes; } }
    public List<Vector3> GeometyVertices {
        get {
            if (_helperVerticeIndexes.Count == 0) {
                return _vertices;
            }
            List<Vector3> geometryVertices = new List<Vector3>(_vertices);
            foreach (int index in _helperVerticeIndexes)
            {
                geometryVertices.RemoveAt(index);
            }
            return geometryVertices;
        }
    }

    public Dictionary<Vector3, HashSet<Vector3>> CalucalteVerticeNeightbourHood()
    {
        Dictionary<Vector3, HashSet<Vector3>> neighborHood = new Dictionary<Vector3, HashSet<Vector3>>();
        int count = _triangles.Count;
        if (count == 1)
        {
            return neighborHood;
        }

        List<List<int>> triangles = new List<List<int>>();
        for (int i = 0; i < count; i += 3)
        {
            int first = _triangles[i];
            int second = _triangles[i + 1];
            int third = _triangles[i + 2];

            List<int> triangle = new List<int>();
            triangles.Add(triangle);

            triangle.Add(first);
            triangle.Add(second);
            triangle.Add(third);
        }

        foreach (List<int> triangle in triangles)
        {
            for (int i = 0; i < triangle.Count; i++)
            {
                int current = triangle[i];
                if (_helperVerticeIndexes.Contains(current)) {
                    continue;
                }
                Vector3 currentVertex = _vertices[current];
                if (!neighborHood.ContainsKey(currentVertex))
                {
                    neighborHood.Add(currentVertex, new HashSet<Vector3>());
                }

                int next = -1;
                int prev = -1;
                if (i == 0) {
                    next = triangle[1];
                    prev = triangle[2];
                } else if (i == 1) {
                    next = triangle[2];
                    prev = triangle[0];
                } else {
                    next = triangle[0];
                    prev = triangle[1];
                }

                if (next > -1 && !_helperVerticeIndexes.Contains(next)) {
                    neighborHood[currentVertex].Add(_vertices[next]);
                }
                if (prev > -1 && !_helperVerticeIndexes.Contains(prev)) {
                    neighborHood[currentVertex].Add(_vertices[prev]);
                }
            }
        }
        
        return neighborHood;
    }
}
