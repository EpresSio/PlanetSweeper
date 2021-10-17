using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBuilder : IcosahedronBuilder
{
    public const double epsilon = 10e-4;

    public static Geometry ConstuctSphere(float radius, int detailLevel)
    {
        Geometry prev = ConstuctIcosahedron(radius);
        Geometry sphere = new Geometry();
        for (int i = 0; i < detailLevel; i++) {
            if (i != 0) {
                prev = Duals(sphere);
            }
            sphere = Truncate(prev, 1f/3f);
        }

        return sphere;
    }

    private static Geometry Duals(Geometry sphere)
    {
        throw new NotImplementedException();
    }

    private static Geometry Truncate(Geometry sourceGeometry, float ratio)
    {
        Geometry targetGeometry = new Geometry();
        Dictionary<Vector3, List<Vector3>> verticesNeighbourhood = sourceGeometry.CalucalteVerticeNeightbourHood();
        Dictionary<VectorPair, Vector3> innerIntersections = new Dictionary<VectorPair, Vector3>();
        foreach (KeyValuePair<Vector3, List<Vector3>> pair in verticesNeighbourhood)
        {
            Vector3 source = pair.Key;
            List<Vector3> neighbours = pair.Value;
            List<Vector3> polygon = new List<Vector3>();

            foreach (Vector3 neighbour in neighbours)
            {
                Vector3 innerIntersect = ratio * (neighbour - source) + source;
                polygon.Add(innerIntersect);
                VectorPair vectorPair = new VectorPair(source, neighbour);
                innerIntersections[vectorPair] = innerIntersect;
            }

            bool hasCentrum;
            Vector3 centrum;
            Vector3 normal;
            findCentrum(out hasCentrum, out centrum, out normal, polygon);
            if (hasCentrum) {
                targetGeometry.GenerateTile(Geometry.PolygonSetup(polygon.Count, centrum, normal, polygon[0]));
            }
        }

        foreach (GeometryTile tile in sourceGeometry.Tiles)
        {
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector3> vertices = tile.Vertices;
            for (int i = 0; i < vertices.Count; i++) {
                VectorPair vectorPairFirst;
                VectorPair vectorPairSecond;
                if (i == vertices.Count - 1) {
                    vectorPairFirst = new VectorPair(vertices[i], vertices[0]);
                    vectorPairSecond = new VectorPair(vertices[0], vertices[i]);
                } else {
                    vectorPairFirst = new VectorPair(vertices[i], vertices[i+1]);
                    vectorPairSecond = new VectorPair(vertices[i+1], vertices[i]);
                }
                newVertices.Add(innerIntersections[vectorPairFirst]);
                newVertices.Add(innerIntersections[vectorPairSecond]);
            }

            bool hasCentrum;
            Vector3 centrum;
            Vector3 normal;
            findCentrum(out hasCentrum, out centrum, out normal, newVertices);
            if ((centrum + normal).magnitude < centrum.magnitude) {
                newVertices.Reverse();
                findCentrum(out hasCentrum, out centrum, out normal, newVertices);
            }
            if (hasCentrum) {
                targetGeometry.GenerateTile(Geometry.PolygonSetup(newVertices.Count, centrum, normal, newVertices[0]));
            }
        }
        return targetGeometry;
    }

    private static void findCentrum(out bool hasCentrum, out Vector3 centrum, out Vector3 normal, List<Vector3> polygon)
    {
        hasCentrum = false;
        centrum = Vector3.zero;
        normal = Vector3.zero;

        if (polygon.Count > 2)
        {
            Vector3 A = polygon[0];
            Vector3 B = polygon[1];
            Vector3 C = polygon[2];

            Vector3 P = (B - A) / 2;
            Vector3 Q = (C - A) / 2;

            normal = Vector3.Cross(P, Q).normalized;

            Vector3 R = Vector3.Cross(P, normal).normalized;
            Vector3 S = Vector3.Cross(Q, normal).normalized;

            Vector3 RxS = Vector3.Cross(R, S);
            Vector3 SxR = -RxS;

            Vector3 Q_PxS = Vector3.Cross(Q - P, S);
            Vector3 P_QxR = Vector3.Cross(P - Q, R);

            if (RxS.magnitude != 0 && Q_PxS.magnitude != 0 && P_QxR.magnitude != 0)
            {
                float t;
                float u;
                if (Mathf.Abs(RxS.x) > epsilon && Mathf.Abs(SxR.x) > epsilon)
                {
                    t = Q_PxS.x / RxS.x;
                    u = P_QxR.x / SxR.x;
                }
                else if (Mathf.Abs(RxS.y) > epsilon && Mathf.Abs(SxR.y) > epsilon)
                {
                    t = Q_PxS.y / RxS.y;
                    u = P_QxR.y / SxR.y;
                }
                else if (Mathf.Abs(RxS.z) > epsilon && Mathf.Abs(SxR.z) > epsilon)
                {
                    t = Q_PxS.z / RxS.z;
                    u = P_QxR.z / SxR.z;
                } else {
                    return;
                }

                Vector3 firstSolution = P + t * R;
                Vector3 secondSolution = Q + u * S;
                if (firstSolution == secondSolution)
                {
                    hasCentrum = true;
                    centrum = A + firstSolution;
                    return;
                } else {
                    Debug.Log((firstSolution - secondSolution).magnitude);
                }
            }
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

        public override int GetHashCode() => first.GetHashCode() + 2 * second.GetHashCode();
        public override bool Equals(object other) => (other as VectorPair)?.first == first && (other as VectorPair)?.second == second;
    }

    public delegate void InterectHandler();
}
