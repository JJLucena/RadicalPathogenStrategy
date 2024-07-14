using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public bool isEnemy;
    public int minionIndex;
    public int spawnAmount;

    public int spawnDelay;

    private GameManager gameManager;

    void Start()
    {
        if(!gameManager)
            gameManager = FindObjectOfType<GameManager>();
        // Inicia la corutina de spawn
        StartCoroutine(SpawnCooldown());
    }

    // Corutina que genera un spawn cada 3 segundos
    IEnumerator SpawnCooldown()
    {
        do
        {
            gameManager.SpawnMinions(isEnemy, spawnAmount, minionIndex, transform.position);
            yield return new WaitForSeconds(spawnDelay);
        } while (gameObject.activeSelf);
    }
}