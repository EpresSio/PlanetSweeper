using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBuilder : IcosahedronBuilder
{
    public static Geometry ConstuctSphere(Vector3 origo, float radius)
    {
        Geometry icosahedron = ConstuctIcosahedron(origo, radius);
        Geometry sphere1 = Truncate(icosahedron, 1/3f);

        return sphere1;
    }

    private static Geometry Truncate(Geometry icosahedron, float ratio)
    {
        Geometry sphere = new Geometry();
        CalculatePolygonsFromVertices(icosahedron, sphere, ratio);
        return sphere;
    }

    private static void CalculatePolygonsFromVertices(Geometry sourceGeometry, Geometry targetGeometry, float ratio)
    {
        Dictionary<Vector3, HashSet<Vector3>> verticesNeighbourhood = sourceGeometry.CalucalteVerticeNeightbourHood();
    }
}
