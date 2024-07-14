using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Map
    [Header("TileMap")]
    public int mapSideLenght;

    public GameObject floor;

    public Sprite virusFloor;
    public Sprite virusBubblesFloor;
    public Sprite bodyFloor;

    public List<GameObject> playerFloorTiles = new();
    public List<GameObject> enemyFloorTiles = new();

    // Behaviour
    [Header("Behaviour")]
    public List<GameObject> playerBases = new List<GameObject>();
    public List<GameObject> enemyBases = new List<GameObject>();
    public List<GameObject> enemyMinions = new List<GameObject>();
    public List<GameObject> playerMinions = new List<GameObject>();
    //public List<Transform> zonasLibres = new List<Transform>();

    // Difficulty
    [Header("Timer")]
    public float timer = 0;
    public int phaseLenght;
    public int currentPhase;
    public int lastRunPhase = -1;
    List<int[]> roundSpawns = new();

    [Header("Bases")]
    public GameObject[] playerBasePrefabs;
    public GameObject[] enemyBasePrefabs;

    public float baseSpawnRadius;

    [Header("Minions")]
    public GameObject[] enemyMinionPrefabs;
    public GameObject[] playerMinionPrefabs;

    // Economy
    [Header("Economy")]
    private int coins;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timerText;

    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();

        // Define rounds
        roundSpawns.Add(new int[] { 3, 0, 0, 0 });
        roundSpawns.Add(new int[] { 3, 0, 0, 0 });

        // Render coins text
        coinsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = GetTransformTextTimer();
        currentPhase = (int)timer / phaseLenght;

        if (lastRunPhase < currentPhase)
        {
            for (int i = 0; i < roundSpawns[currentPhase].Length - 1; i++)
            {
                for (int j = 0; j < roundSpawns[currentPhase][i]; j++)
                {
                    SpawnEnemyBase(i);
                }
            }
            lastRunPhase = currentPhase;
        }
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapSideLenght; x++)
        {
            for (int y = 0; y < mapSideLenght; y++)
            {
                playerFloorTiles.Add(Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity));
            }
        }
    }

    private void SpawnEnemyBase(int baseIndex)
    {
        AddEnemyBase(Instantiate(enemyBasePrefabs[baseIndex], new Vector3(Random.Range(0, mapSideLenght) + 0.5f, Random.Range(0, mapSideLenght) + 0.5f, 0f), Quaternion.identity));
    }
    public bool SpawnPlayerBase(int cost, int baseIndex, Vector3 coordinates)
    {

        if (cost <= int.Parse(coinsText.text))
        {
            AddPlayerBase(Instantiate(playerBasePrefabs[baseIndex], coordinates, Quaternion.identity));
            RemoveCoins(cost);
            return true;
        }
        return false;
    }

    public void SpawnMinions(bool isEnemy, int minionAmount, int minionIndex, Vector3 coordinates)
    {

        for (int i = 0; i < minionAmount; i++)
        {
            // Se genera un ángulo aleatorio en radianes
            float angleRad = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 spawnOffset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * Random.Range(3, baseSpawnRadius);
            Vector2 spawnPosition = (Vector2)coordinates + spawnOffset;

            // Se instancia la celda en la posición calculada
            if (spawnPosition.x > 0
            && spawnPosition.y > 0
            && spawnPosition.x < mapSideLenght
            && spawnPosition.y < mapSideLenght)
                AddEnemyMinion(Instantiate((isEnemy ? enemyMinionPrefabs : playerMinionPrefabs)[minionIndex], spawnPosition, Quaternion.identity));
        }
    }

    public void AddCoins(int amount)
    {
        coins = int.Parse(coinsText.text);
        coins += amount;
        coinsText.text = coins.ToString();
    }

    public void RemoveCoins(int amount)
    {
        coins = int.Parse(coinsText.text);
        coins -= amount;
        coinsText.text = coins.ToString();
    }

    // List BASE functions
    public void AddPlayerBase(GameObject basePlayer)
    {
        playerBases.Add(basePlayer);
    }
    public void RemovePlayerBase(GameObject basePlayer)
    {
        playerBases.Remove(basePlayer);
    }

    public void AddEnemyBase(GameObject enemyBase)
    {
        enemyBases.Add(enemyBase);
    }
    public void RemoveEnemyBase(GameObject enemyBase)
    {
        enemyBases.Remove(enemyBase);
    }

    public void AddEnemyMinion(GameObject Enemy)
    {
        enemyMinions.Add(Enemy);
    }
    public void RemoveEnemyMinion(GameObject Enemy)
    {
        enemyMinions.Remove(Enemy);
        AddCoins(10);
    }

    public void AddPlayerMinion(GameObject player)
    {
        playerMinions.Add(player);
    }
    public void RemovePlayerMinion(GameObject player)
    {
        playerMinions.Remove(player);
    }

    // public void AddCoins(int amount)
    // {
    //     coins += amount;
    // }
    /*public void AddZonaLibre(Transform zonaLibre) {
        zonasLibres.Add(zonaLibre);
    }
    public void RemoveZonaLibre(Transform zonaLibre) {
        zonasLibres.Remove(zonaLibre);
    }*/

    public GameObject GetClosestElement(Vector3 reference, string targetTag)
    {
        switch (targetTag)
        {
            case "EnemyMinion":
                if (enemyMinions.Count < 1)
                {
                    return null;
                }
                return LookingIntoLists(reference, enemyMinions);
            case "PlayerMinion":
                if (playerMinions.Count < 1)
                {
                    return null;
                }
                return LookingIntoLists(reference, playerMinions);
            case "EnemyBase":
                if (enemyBases.Count < 1)
                {
                    return null;
                }
                return LookingIntoLists(reference, enemyBases);
            case "PlayerBase":
                if (playerBases.Count < 1)
                {
                    return null;
                }
                return LookingIntoLists(reference, playerBases);
            case "EnemyFloor":
                if (enemyFloorTiles.Count < 1)
                {
                    return null;
                }
                return LookingIntoLists(reference, enemyFloorTiles);
            case "PlayerFloor":
                if (playerFloorTiles.Count < 1)
                {
                    return null;
                }
                return LookingIntoLists(reference, playerFloorTiles);
            default:
                Debug.LogError("TAG no existente");
                return null;
        }
    }

    private GameObject LookingIntoLists(Vector3 reference, List<GameObject> possibleTargets)
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        // Busca el elemento m�s cercano en las listas que corresponden seg�n si es player o enemy

        foreach (var element in possibleTargets)
        {
            float distance = Vector3.Distance(reference, element.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = element.gameObject;
            }
        }

        return closest;
    }

    public string GetTransformTextTimer() {
        int minutos = Mathf.FloorToInt(timer / 60f);
        int segundos = Mathf.FloorToInt(timer % 60f);
        int milisegundos = Mathf.FloorToInt((timer * 1000) % 1000) / 100; // Dividir por 100 para reducir el nº de digitos;

        // Formatear el tiempo en el formato MM:SS:MS
        string tiempoFormateado = string.Format("{0:0}:{1:00}:{2:0}", minutos, segundos, milisegundos);
        return tiempoFormateado;
    }
}
