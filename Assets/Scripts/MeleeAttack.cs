using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Transform attackPoint; // El punto de origen del ataque (puede ser un hijo del jugador)
    public float attackRange = 0.5f; // El rango del ataque
    public LayerMask enemyLayers; // Las capas que pueden ser afectadas por el ataque

    public void Attack(float damage)
    {
        //Hacer animacion
        //---------------------------------------------


        // Detectar enemigos en el rango del ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Hacer daño a cada enemigo detectado
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Golpeado " + enemy.name);
            DestroyableObject destroyableObject = enemy.GetComponent<DestroyableObject>();
            if (!destroyableObject)
                destroyableObject = enemy.GetComponentInParent<DestroyableObject>();

            destroyableObject.TakeDamage(damage);
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