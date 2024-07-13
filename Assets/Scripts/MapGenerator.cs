using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("TileMap")]
    public int mapSideLenght;

    public GameObject floor;

    public Sprite virusFloor;
    public Sprite virusBubblesFloor;
    public Sprite bodyFloor;

    List<GameObject> floorTiles = new();

    void Start()
    {
        for (int x = 0; x < mapSideLenght; x++)
        {
            for (int y = 0; y < mapSideLenght; y++)
            {
                floorTiles.Add(Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity));
            }
        }
    }
}
