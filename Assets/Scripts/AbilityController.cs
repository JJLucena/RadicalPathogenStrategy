using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [Header("Ability Button")]
    public Button abilityButton; // Botón para activar el modo de selección
    private bool isSelectingLocation = false; // Indica si estamos en modo de selección de ubicación

    [Header("Ability Settings")]
    public bool isBase;
    Vector2 abilityOrigin; // El punto de origen del ataque
    public float attackRange = 5f; // El rango del ataque
    public LayerMask enemyLayers; // Las capas que pueden ser afectadas por el ataque

    GameManager gameManager;

    void Start()
    {
        if (!gameManager)
            gameManager = FindObjectOfType<GameManager>();

        abilityButton.onClick.AddListener(OnAbilityButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        // Si estamos en modo de selección y se hace clic izquierdo
        if (isSelectingLocation && Input.GetMouseButtonDown(0))
        {
            abilityOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (isBase) {
                
            } else {
                // Detectar enemigos en el rango del ataque
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(abilityOrigin, attackRange, enemyLayers);

                // Hacer daño a cada enemigo detectado
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Golpeado " + enemy.name);
                    Destroy(enemy.gameObject);
                }
            }

            // Desactiva el modo de selección
            isSelectingLocation = false;
        }
    }

    public void OnAbilityButtonClicked()
    {
        isSelectingLocation = true;
    }

    void OnDrawGizmosSelected()
    {
        if (abilityOrigin == null)
            return;

        Gizmos.DrawWireSphere(abilityOrigin, attackRange);
    }
}
