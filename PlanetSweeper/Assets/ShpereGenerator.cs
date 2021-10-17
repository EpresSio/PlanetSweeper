using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpereGenerator : MonoBehaviour
{
    public TileRenderer tileRenderer;
    public float radius = 1f;
    public int detailLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        Geometry p = SphereBuilder.ConstuctIcosahedron(radius);
        p.Tiles.ForEach(delegate (GeometryTile tile)
        {
            TileRenderer rendererInstance = Instantiate(tileRenderer, transform.localPosition, Quaternion.identity, tileRenderer.transform.parent);
            rendererInstance.RenderTile(tile);
            rendererInstance.name = rendererInstance.tile.Vertices.Count.ToString();
        }
        );
        Geometry sphere = SphereBuilder.ConstuctSphere(radius, detailLevel);
        sphere.Tiles.ForEach(delegate(GeometryTile tile)
            {
                TileRenderer rendererInstance = Instantiate(tileRenderer, transform.localPosition, Quaternion.identity, tileRenderer.transform.parent);
                rendererInstance.RenderTile(tile);
                rendererInstance.name = rendererInstance.tile.Vertices.Count.ToString();
            }
        );       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
