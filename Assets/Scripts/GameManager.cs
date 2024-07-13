using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Map
    [Header("TileMap")]
    public int mapSideLenght;

    public GameObject floor;

    public Sprite virusFloor;
    public Sprite virusBubblesFloor;
    public Sprite bodyFloor;

    public List<GameObject> floorTiles = new();

    // Behaviour
    [Header("Behaviour")]
    public List<GameObject> playerBases = new List<GameObject>();
    public List<GameObject> enemyBases = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> playerList = new List<GameObject>();
    //public List<Transform> zonasLibres = new List<Transform>();

    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void GenerateMap() {
        for (int x = 0; x < mapSideLenght; x++)
        {
            for (int y = 0; y < mapSideLenght; y++)
            {
                floorTiles.Add(Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity));
            }
        }
    }

    private void FillListWithTag(List<GameObject> list, string tag) {
        list.Clear();
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsWithTag) {
            list.Add(obj);
        }
    }

    public void AddPlayerBase(GameObject basePlayer) {
        playerBases.Add(basePlayer);
    }

    public void RemovePlayerBase(GameObject basePlayer) {
        playerBases.Remove(basePlayer);
    }

    public void AddEnemyBase(GameObject enemyBase) {
        enemyBases.Add(enemyBase);
    }

    public void RemoveEnemyBase(GameObject enemyBase) {
        enemyBases.Remove(enemyBase);
    }

    public void AddEnemy(GameObject Enemy) {
        enemyList.Add(Enemy);
    }
    public void RemoveEnemy(GameObject Enemy) {
        enemyList.Remove(Enemy);
    }

    public void AddPlayer(GameObject player) {
        playerList.Add(player);
    }
    public void RemovePlayer(GameObject player) {
        playerList.Remove(player);
    }

    /*public void AddZonaLibre(Transform zonaLibre) {
        zonasLibres.Add(zonaLibre);
    }
    public void RemoveZonaLibre(Transform zonaLibre) {
        zonasLibres.Remove(zonaLibre);
    }*/

    public GameObject GetClosestElement(Transform reference, string targetTag) {
        List<GameObject>[] enemyLists = { enemyBases/*, enemyList, zonasLibres*/ };
        List<GameObject>[] playerLists = { playerBases/*, playerList, zonasLibres*/ };

        if (targetTag == "EnemyMinion") {
            return LookingIntoLists(reference, enemyLists);
        } else if (targetTag == "PlayerMinion") {
            return LookingIntoLists(reference, playerLists);
        } else {
            Debug.LogError("TAG no existente");
            return null;
        }
    }

    private GameObject LookingIntoLists(Transform reference, List<GameObject>[] currentList) {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        // Busca el elemento m�s cercano en las listas que corresponden seg�n si es player o enemy
        foreach (var list in currentList) {
            foreach (var element in list) {
                float distance = Vector3.Distance(reference.position, element.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closest = element.gameObject;
                }
            }
        }
        return closest;
    }
}
