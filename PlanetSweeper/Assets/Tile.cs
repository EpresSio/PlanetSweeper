using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryTile
{
    private List<Vector3> _vertices;
    public List<Vector3> Vertices { get; set; } 
    private List<int> _triangles;
    public List<int> Triangles { get; set; }
}
