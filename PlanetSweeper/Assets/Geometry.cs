using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour
{
    List<GeometryTile> _tiles = new List<GeometryTile>();
    Dictionary<Vector3, List<GeometryTile>> _verticeTileDictionary = new Dictionary<Vector3, List<GeometryTile>>();

    public void AddVertice(Vector3 vertice, GeometryTile tile)
    {
        _tiles.Add(tile);
        tile.Vertices.Add(vertice);
        if (!_verticeTileDictionary.ContainsKey(vertice))
        {
            _verticeTileDictionary.Add(vertice, new List<GeometryTile>());
        }
        _verticeTileDictionary[vertice].Add(tile);
    }
}
