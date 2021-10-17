using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBuilder : IcosahedronBuilder
{
    public static Geometry ConstuctSphere(float radius)
    {
        Geometry icosahedron = ConstuctIcosahedron(radius);
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
        Dictionary<Vector3, List<Vector3>> verticesNeighbourhood = sourceGeometry.CalucalteVerticeNeightbourHood();
        Dictionary<VectorPair, List<Vector3>> innerIntersections = new Dictionary<VectorPair, List<Vector3>>();
        foreach (KeyValuePair<Vector3, List<Vector3>> pair in verticesNeighbourhood)
        {
            Vector3 source = pair.Key;
            List<Vector3> neighbours = pair.Value;
            List<Vector3> polygon = new List<Vector3>();

            foreach (Vector3 neighbour in neighbours)
            {
                Vector3 intersect = (1f / 3f) * (neighbour - source) + source;
                polygon.Add(intersect);
                VectorPair vectorPair = new VectorPair(source, neighbour);
                if (!innerIntersections.ContainsKey(vectorPair))
                {
                    innerIntersections.Add(vectorPair, new List<Vector3>());
                }
                innerIntersections[vectorPair].Add(intersect);
            }

            if (polygon.Count > 2) {
                Vector3 A = polygon[0];
                Vector3 B = polygon[1];
                Vector3 C = polygon[2];

                Vector3 P = (B - A)/2;
                Vector3 Q = (C - A)/2;

                Vector3 e = Vector3.Cross(P, Q).normalized;

                Vector3 R = Vector3.Cross(P, e).normalized;
                Vector3 S = Vector3.Cross(Q, e).normalized;

                Vector3 RxS = Vector3.Cross(R, S);
                Vector3 SxR = -RxS;

                Vector3 Q_PxS = Vector3.Cross(Q - P, S);
                Vector3 P_QxR = Vector3.Cross(P - Q, R);

                if (RxS.magnitude != 0 && Q_PxS.magnitude != 0 && P_QxR.magnitude != 0) {
                    float t;
                    float u;
                    if (RxS.x != 0) {
                        t = Q_PxS.x/RxS.x;
                        u = P_QxR.x/SxR.x;
                    } else if (RxS.y != 0) {
                        t = Q_PxS.y/RxS.y;
                        u = P_QxR.y/SxR.y;
                    } else {
                        t = Q_PxS.z/RxS.z;
                        u = P_QxR.z/SxR.z;
                    }

                    Debug.Log(P + t * R == Q + u * S);
                    if (P + t * R == Q + u * S) {
                        Vector3 intersect = A + P + t * R;
                        targetGeometry.GenerateTile(Geometry.PolygonSetup(polygon.Count, intersect, e, polygon[0]));
                        
                    }
                }
            }
        }

        foreach (GeometryTile tile in sourceGeometry.Tiles)
        {
            List<Vector3> vertices = tile.Vertices;
            // todo hexagons
        }
    }

    public class VectorPair
    {
        public Vector3 first;
        public Vector3 second;

        public VectorPair(Vector3 first, Vector3 second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
