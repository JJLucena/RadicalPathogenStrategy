using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    public GameObject cellPrefab; // Célula a instanciar
    public float spawnRadius = 8; // Radio de la zona de spawn
    public int maxCells = 1; // Máximo de células a instanciar

    void Start()
    {
        // Inicia la corutina de spawn
        StartCoroutine(SpawnCooldown());
    }

    // Método que genera un spawn en una posición aleatoria
    void Spawn()
    {
        for (int i = 0; i < maxCells; i++)
        {
            // Se genera un ángulo aleatorio en radianes
            float angleRad = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            // Se calcula un desplazamiento basado en el ángulo y el radio
            Vector2 spawnOffset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * Random.Range(3, spawnRadius);

            // Se calcula la posición del spawn relativa a la posición del spawner
            Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;

            // Se instancia la celda en la posición calculada
            Instantiate(cellPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Corutina que genera un spawn cada 3 segundos
    IEnumerator SpawnCooldown()
    {
        do
        {
            yield return new WaitForSeconds(3);
            Spawn();
        } while (gameObject.activeSelf);
    }

}