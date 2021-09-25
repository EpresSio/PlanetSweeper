using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBuilder
{
    public static List<GeometryTile> ConstuctIcosahedron(Vector3 origo, float radius)
    {
        List<GeometryTile> tiles = new List<GeometryTile>();
        GeometryTile bottomPentagon = ConstructPentagon(origo, radius, 0);
        GeometryTile upperPentagon = ConstructPentagon(origo + Vector3.up * radius, radius, 36);
        // tiles.Add(bottomPentagon);
        // tiles.Add(upperPentagon);
        tiles.AddRange(GetSpikes(bottomPentagon, upperPentagon, true));
        tiles.AddRange(GetSpikes(upperPentagon, bottomPentagon, false));
        tiles.AddRange(GetHat(upperPentagon, radius, true));
        tiles.AddRange(GetHat(bottomPentagon, radius, false));
        return tiles;
    }

    private static GeometryTile[] GetHat(GeometryTile pentagon, float radius, bool up)
    {
        GeometryTile[] hatPieces = new GeometryTile[5];
        float pointHeight = Mathf.Sqrt(2 * radius * radius - 2 * radius * Mathf.Cos(Mathf.Deg2Rad * 36));
        Debug.Log(pointHeight);
        Vector3 point = pentagon.Vertices[0] + Vector3.up * pointHeight * (up ? 1 : -1);
        for (int i = 1; i < 6; i++)
        {
            GeometryTile piece = new GeometryTile();
            piece.Vertices.Add(pentagon.Vertices[i]);
            piece.Vertices.Add(point);
            piece.Vertices.Add(i == 5 ? pentagon.Vertices[1] : pentagon.Vertices[i + 1]);

            piece.Triangles.Add(up ? 2 : 0);
            piece.Triangles.Add(1);
            piece.Triangles.Add(up ? 0 : 2);
            hatPieces[i-1] = piece;
        }
        return hatPieces;
    }

    public static GeometryTile ConstructPentagon(Vector3 origo, float radius, float rotation)
    {
        GeometryTile tile = new GeometryTile();
        tile.Vertices.Add(origo);
        Vector3 latestVertice = Quaternion.AngleAxis(rotation, Vector3.up) * Vector3.forward * radius;
        tile.Vertices.Add(latestVertice + origo);
        
        for (int i = 1; i < 5; i++)
        {
            Vector3 vertice = Quaternion.AngleAxis(72, Vector3.up) * latestVertice;
            tile.Vertices.Add(vertice + origo);
            latestVertice = vertice;
        }

        List<int> triangles = tile.Triangles;
        for (int i = 1; i < 6; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i == 5 ? 1 : i+1);
        }
        return tile;
    }

    public static GeometryTile[] GetSpikes(GeometryTile firstPentagon, GeometryTile secondPentagon, bool forward) {
        GeometryTile[] spikes = new GeometryTile[5];
        for (int i = 1; i < 6; i++)
        {
            GeometryTile spike = new GeometryTile();
            spike.Vertices.Add(firstPentagon.Vertices[i]);
            if (forward) {
                spike.Vertices.Add(secondPentagon.Vertices[i]);
            } else {
                spike.Vertices.Add(i == 5 ? secondPentagon.Vertices[1] : secondPentagon.Vertices[i+1]);
            }
            spike.Vertices.Add(i == 5 ? firstPentagon.Vertices[1] : firstPentagon.Vertices[i+1]);

            spike.Triangles.Add(forward ? 2 : 0);
            spike.Triangles.Add(1);
            spike.Triangles.Add(forward ? 0 : 2);
            spikes[i-1] = spike;
        }

        return spikes; 
    }
}
