using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileRenderer : MonoBehaviour
{
    public GeometryTile tile;
    Mesh mesh;
    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
    }

    public void RenderTile(GeometryTile tile) {
        this.tile = tile;
        mesh.vertices = tile.Vertices.ToArray();
        mesh.triangles = tile.Triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
