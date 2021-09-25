using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBuilder : IcosahedronBuilder
{
    public static Geometry ConstuctSphere(Vector3 origo, float radius)
    {
        Geometry icosahedron = ConstuctIcosahedron(origo, radius);

        return new Geometry();
    }
}
