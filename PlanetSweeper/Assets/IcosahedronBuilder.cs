using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcosahedronBuilder
{
    public static Geometry ConstuctIcosahedron(float radius)
    {
        Geometry icosahedron = new Geometry();
        GeometryTile bottomPentagon = ConstructPentagon(icosahedron, Vector3.zero, radius, 0);
        bottomPentagon.CalucalteVerticeNeightbourHood();
        GeometryTile upperPentagon = ConstructPentagon(icosahedron, Vector3.up * radius, radius, 36);

        AddSpikes(icosahedron, bottomPentagon, upperPentagon, true);
        AddSpikes(icosahedron, upperPentagon, bottomPentagon, false);
        AddHat(icosahedron, upperPentagon, radius, true);
        AddHat(icosahedron, bottomPentagon, radius, false);
        icosahedron.RemoveTile(bottomPentagon);
        icosahedron.RemoveTile(upperPentagon);

        return icosahedron;
    }

    public static GeometryTile ConstructPentagon(Geometry geometry, Vector3 origo, float radius, float rotation)
    {
        return geometry.GenerateTile(() =>
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            vertices.Add(origo);
            Vector3 latestVertice = Quaternion.AngleAxis(rotation, Vector3.up) * Vector3.forward * radius;
            vertices.Add(latestVertice + origo);

            for (int i = 1; i < 5; i++)
            {
                Vector3 vertice = Quaternion.AngleAxis(72, Vector3.up) * latestVertice;
                vertices.Add(vertice + origo);
                latestVertice = vertice;
            }

            for (int i = 1; i < 6; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i == 5 ? 1 : i + 1);
            }

            return (vertices, triangles, new List<int>{0});
        });
    }
    private static void AddHat(Geometry geometry, GeometryTile pentagon, float radius, bool up)
    {
        float pointHeight = Mathf.Sqrt(2 * radius * radius - 2 * radius * Mathf.Cos(Mathf.Deg2Rad * 36));
        Vector3 point = pentagon.Vertices[0] + Vector3.up * pointHeight * (up ? 1 : -1);
        for (int i = 1; i < 6; i++)
        {
            geometry.GenerateTile(() => {
                List<Vector3> vertices = new List<Vector3>();
                List<int> triangles = new List<int>();

                vertices.Add(pentagon.Vertices[i]);
                vertices.Add(point);
                vertices.Add(i == 5 ? pentagon.Vertices[1] : pentagon.Vertices[i + 1]);

                triangles.Add(up ? 2 : 0);
                triangles.Add(1);
                triangles.Add(up ? 0 : 2);

                return (vertices, triangles, new List<int>());
            });
        }
    }

    private static void AddSpikes(Geometry geometry, GeometryTile firstPentagon, GeometryTile secondPentagon, bool forward)
    {
        GeometryTile[] spikes = new GeometryTile[5];
        for (int i = 1; i < 6; i++)
        {
            geometry.GenerateTile(() => {
                List<Vector3> vertices = new List<Vector3>();
                List<int> triangles = new List<int>();

                vertices.Add(firstPentagon.Vertices[i]);
                if (forward)
                {
                    vertices.Add(secondPentagon.Vertices[i]);
                }
                else
                {
                    vertices.Add(i == 5 ? secondPentagon.Vertices[1] : secondPentagon.Vertices[i + 1]);
                }
                vertices.Add(i == 5 ? firstPentagon.Vertices[1] : firstPentagon.Vertices[i + 1]);

                triangles.Add(forward ? 2 : 0);
                triangles.Add(1);
                triangles.Add(forward ? 0 : 2);

                return (vertices, triangles, new List<int>());
            });
        }
    }
}
