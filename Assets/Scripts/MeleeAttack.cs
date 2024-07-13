using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Transform attackPoint; // El punto de origen del ataque (puede ser un hijo del jugador)
    public float attackRange = 0.5f; // El rango del ataque
    public LayerMask enemyLayers; // Las capas que pueden ser afectadas por el ataque

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Puedes cambiar esto por el input que prefieras
        {
            Attack();
        }
    }

    void Attack()
    {
        // Detectar enemigos en el rango del ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Hacer daño a cada enemigo detectado
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Golpeado " + enemy.name);
        }
    }

    // Método para dibujar el rango de ataque en el editor de Unity
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}