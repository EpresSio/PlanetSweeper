using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpereGenerator : MonoBehaviour
{
    public TileRenderer tileRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Geometry icosahedron = SphereBuilder.ConstuctSphere(transform.localPosition, 1f);
        icosahedron.Tiles.ForEach(delegate(GeometryTile tile)
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
