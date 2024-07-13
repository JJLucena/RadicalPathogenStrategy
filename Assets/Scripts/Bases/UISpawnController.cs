using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISpawnController : MonoBehaviour
{
    public GameObject spawnerPrefab; // Prefab del objeto a crear
    public Button spawnButton; // Botón para activar el modo de selección
    private bool isSelectingLocation = false; // Indica si estamos en modo de selección de ubicación

    void Start()
    {
        // Asigna el método OnSpawnButtonClicked al evento onClick del botón
        spawnButton.onClick.AddListener(OnSpawnButtonClicked);
    }

    void Update()
    {
        // Si estamos en modo de selección y se hace clic izquierdo
        if (isSelectingLocation && Input.GetMouseButtonDown(0))
        {
            // Obtiene la posición del ratón en el mundo 2D
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Crea el objeto en la posición del ratón
            Instantiate(spawnerPrefab, spawnPosition, Quaternion.identity);

            // Desactiva el modo de selección
            isSelectingLocation = false;
        }
    }

    // Activa el modo de selección
    public void OnSpawnButtonClicked()
    {
        isSelectingLocation = true;
    }
}