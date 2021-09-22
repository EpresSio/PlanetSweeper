using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShpereGenerator : MonoBehaviour
{
    public TileRenderer tileRenderer;
    // Start is called before the first frame update
    void Start()
    {
        List<GeometryTile> tiles = SphereBuilder.ConstuctIcosahedron(transform.localPosition, 1f);
        tiles.ForEach(delegate(GeometryTile tile)
            {
                TileRenderer rendererInstance = Instantiate(tileRenderer, transform.localPosition, Quaternion.identity);
                rendererInstance.renderTile(tile);
            }
        );
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
