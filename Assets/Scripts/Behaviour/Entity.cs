using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour {

    [Header("Attack")]
    [SerializeField] float damage = 10f;
    [SerializeField] float attackCooldownTime = 1f;
    [SerializeField] float cooldownAttack = 2f;

    [Header("Radious & Movement")]
    [SerializeField] float visionRadius = 5f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float cooldownTarget = 2f;

    public string targetTag;

    private GameObject target;
    private Rigidbody2D rb;
    private bool canAttack = true;
    private Coroutine canAttackCorrutine;

    private enum State {
        GetTarget,
        Move,
        Attack,
        Cooldown
    }
    private State currentState;
        
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        currentState = State.GetTarget;
    }

    void Update() {
        switch (currentState) {
            case State.GetTarget:
                GetTarget();
                break;
            case State.Move:
                Move();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Cooldown:
                StartCoroutine(Cooldown());
                break;
        }
        ContiniousRadiusDetection();
    }

    void ContiniousRadiusDetection() {
        if (targetTag == "EnemyMinion") {
            if (!target.CompareTag("PlayerMinion") || !target.CompareTag("PlayerBase"))
                RadiusDetection();
        }
        else if (targetTag == "PlayerMinion") {
            if (!target.CompareTag("EnemyMinion") || !target.CompareTag("EnemyBase"))
                RadiusDetection();
        }
        /*if (target != null && !target.CompareTag(targetTag)) {
            RadiusDetection();
        }*/
    }
    void RadiusDetection() {
        // Detect target within vision radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionRadius);
        foreach (var hit in hits) {
            if (hit.CompareTag(targetTag)) {
                target = hit.gameObject;
                currentState = State.Move;
            }
        }
    }

    void GetTarget() {
        if (target == null) {
            RadiusDetection();

            // If no player detected, get the closest target from the list
            target = EntitiesManager.Instance.GetClosestElement(this.transform, targetTag);
        }

        if (target != null) {
            currentState = State.Move;
        }
    }

    void Move() {
        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        if (distanceToTarget <= attackRange) {
            rb.velocity = Vector2.zero; // Stop movement
            currentState = State.Attack;
        }
        else {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    void Attack() {
        if (target.gameObject == null) {
            target = null;
            currentState = State.GetTarget;
            return;
        }

        if (canAttack) {
            //Detiene e inicia la corrutina que permite atacar con un cooldown
            if (canAttackCorrutine != null)
                StopCoroutine(canAttackCorrutine);
            StartCoroutine(CooldownAttack());

            // Animaci�n o efecto de sonido para el ataque
            
            // Suponiendo que el objetivo tiene un script con un m�todo 'TakeDamage'
            target.GetComponent<DestroyableObject>().TakeDamage(damage);

        }

        // Verifica si el objetivo sigue estando en el rango de ataque
        if (Vector2.Distance(transform.position, target.transform.position) > attackRange) {
            currentState = State.Move;
        }
    }
    IEnumerator CooldownAttack() {
        canAttack = false;
        yield return new WaitForSeconds(cooldownAttack);
        canAttack = true;
    }
    IEnumerator Cooldown() {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(cooldownTarget);
        currentState = State.GetTarget;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
            // Dibujar el radio de visi�n en el editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionRadius);
            // Dibujar el rango de ataque en el editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    #endif
}
