using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{

    public static Dictionary<string, int> biomeIDs = new Dictionary<string, int>
    {
        { "Desert", 0 },
        { "Forest", 1 },
        { "Grassland", 2 },
        { "Ice", 3 },
        { "Jungle", 4 },
        { "Mountains", 5 },
        { "Ocean", 6 },
        { "River", 7 },
        { "Tundra", 8 }
    };

    [SerializeField]
    private Collider2D[] _colliders;

    public float height;
    public float moisture;
    public float heat;

    public int biomeID;

    public void InitTile(int biomeID_, float height_, float moisture_, float heat_) {
        biomeID = biomeID_;
        height = height_;
        moisture = moisture_;
        heat = heat_;
        ResizeColliders();
    }

    public void ResizeColliders()
    {
        SpriteRenderer spriteRenderer_ = GetComponent<SpriteRenderer>();
        BoxCollider2D[] colliders_ = GetComponents<BoxCollider2D>();

        foreach (BoxCollider2D collider_It in colliders_) {
            if (spriteRenderer_ != null && collider_It != null)
            {
                collider_It.size = spriteRenderer_.bounds.size;
            }
        }
    }
}
