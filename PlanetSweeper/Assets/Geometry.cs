using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry
{
    public delegate (List<Vector3>, List<int>, List<int>) TileSetup();

    List<GeometryTile> _tiles = new List<GeometryTile>();
    Dictionary<Vector3, List<GeometryTile>> _verticeTileDictionary = new Dictionary<Vector3, List<GeometryTile>>();

    public List<GeometryTile> Tiles { get { return _tiles; } }


    public GeometryTile GenerateTile(TileSetup tileSetup)
    {
        GeometryTile geometryTile = new GeometryTile();
        _tiles.Add(geometryTile);

        (List<Vector3>, List<int>, List<int>) setup = tileSetup();
        List<Vector3> vertices = setup.Item1;
        List<int> triangles = setup.Item2;
        List<int> helperVertices = setup.Item3;
        geometryTile.Vertices.AddRange(vertices);
        geometryTile.Triangles.AddRange(triangles);
        geometryTile.HelperVerticeIndexes.AddRange(helperVertices);
        AddVerticesToMap(geometryTile, vertices);
        return geometryTile;
    }

    private void AddVerticesToMap(GeometryTile geometyTile, List<Vector3> vertices)
    {
        foreach (Vector3 vertice in vertices)
        {
            if (!_verticeTileDictionary.ContainsKey(vertice))
            {
                _verticeTileDictionary.Add(vertice, new List<GeometryTile>());
            }
            _verticeTileDictionary[vertice].Add(geometyTile);
        }
    }

    public void RemoveTile(GeometryTile tile)
    {
        _tiles.Remove(tile);
        foreach (Vector3 vertice in tile.Vertices)
        {
            List<GeometryTile> geometryTiles = _verticeTileDictionary[vertice];
            geometryTiles.Remove(tile);
            if (geometryTiles.Count == 0)
            {
                _verticeTileDictionary.Remove(vertice);
            }
        }
    }

    public Dictionary<Vector3, List<Vector3>> CalucalteVerticeNeightbourHood() {
        Dictionary<Vector3, List<Vector3>> neighbourhood = new Dictionary<Vector3, List<Vector3>>();
        foreach (GeometryTile tile in _tiles)
        {
            Dictionary<Vector3, List<Vector3>> tileNeighbourhood = tile.CalucalteVerticeNeightbourHood();
            foreach (KeyValuePair<Vector3, List<Vector3>> currentNeighbourhood in tileNeighbourhood)
            {
                Vector3 vertice = currentNeighbourhood.Key;
                List<Vector3> neighbours = currentNeighbourhood.Value;
                if (!neighbourhood.ContainsKey(vertice)) {
                    neighbourhood.Add(vertice, new List<Vector3>());
                }
                foreach (Vector3 neighbour in neighbours)
                {
                    neighbourhood[vertice].Add(neighbour);
                }
                    
            }
        }
        return neighbourhood;
    }
}
