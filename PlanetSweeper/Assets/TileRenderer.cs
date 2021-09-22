using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileRenderer : MonoBehaviour
{
    Mesh mesh;
    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
    }

    public void renderTile(GeometryTile tile) {
        mesh.vertices = tile.Vertices.ToArray();
        mesh.triangles = tile.Triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
