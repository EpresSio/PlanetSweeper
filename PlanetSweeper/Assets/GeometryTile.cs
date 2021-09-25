using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryTile
{
    private List<Vector3> _vertices = new List<Vector3>();
    public List<Vector3> Vertices { get; } 
    private List<int> _triangles = new List<int>();
    public List<int> Triangles { get; }
}
