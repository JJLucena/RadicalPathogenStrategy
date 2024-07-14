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
    Vector2 attackPoint; // El punto de origen del ataque
    public float attackRange = 5f; // El rango del ataque
    public LayerMask enemyLayers; // Las capas que pueden ser afectadas por el ataque


    void Start()
    {
        abilityButton.onClick.AddListener(OnAbilityButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        // Si estamos en modo de selección y se hace clic izquierdo
        if (isSelectingLocation && Input.GetMouseButtonDown(0))
        {
            // Obtiene la posición del ratón en el mundo 2D
            attackPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Detectar enemigos en el rango del ataque
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayers);

            // Desactiva el modo de selección
            isSelectingLocation = false;

            // Hacer daño a cada enemigo detectado
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Golpeado " + enemy.name);
                Destroy(enemy.gameObject);
            }
        }
    }

    public void OnAbilityButtonClicked()
    {
        isSelectingLocation = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint, attackRange);
    }
}
