using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesManager : MonoBehaviour {
    public List<GameObject> playerBases = new List<GameObject>();
    public List<GameObject> EnemyBases = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> playerList = new List<GameObject>();
    //public List<Transform> zonasLibres = new List<Transform>();

    public static EntitiesManager Instance;

    //Singleton
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        // Busca y llena las listas con los objetos de la escena usando tags
        FillListWithTag(playerBases, "PlayerBase");
        FillListWithTag(EnemyBases, "EnemyBase");
        FillListWithTag(enemyList, "EnemyMinion");
        FillListWithTag(playerList, "PlayerMinion");
        //FillListWithTag(zonasLibres, "ZonaLibre");
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
        EnemyBases.Add(enemyBase);
    }

    public void RemoveEnemyBase(GameObject enemyBase) {
        EnemyBases.Remove(enemyBase);
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
        List<GameObject>[] enemyLists = { EnemyBases/*, enemyList, zonasLibres*/ };
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

        // Busca el elemento más cercano en las listas que corresponden según si es player o enemy
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
