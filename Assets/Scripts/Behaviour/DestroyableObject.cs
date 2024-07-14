using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {
    [SerializeField] GameManager gameManager;

    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] float health;
    [SerializeField] Type typeSelected;

    [Header("Economy")]
    [SerializeField] int reward = 10;

    public enum Type {
        PlayerMinion,
        EnemyMinion,
        PlayerBase,
        EnemyBase
    }

    void Start() {
        health = maxHealth;

        if (!gameManager)
            gameManager = FindObjectOfType<GameManager>();
    }

    void RemoveCurrentElementToList() {
        switch (typeSelected) {
            case Type.PlayerMinion:
                gameManager.RemovePlayerMinion(gameObject);
                break;
            case Type.EnemyMinion:
                gameManager.RemoveEnemyMinion(gameObject);
                break;
            case Type.PlayerBase:
                gameManager.RemovePlayerBase(gameObject);
                break;
            case Type.EnemyBase:
                gameManager.RemoveEnemyBase(gameObject);
                break;
        }
    }

    public void SetDestroyed() {
        /*//Sound Destroy
        AudioManager.instance.PlaySFX("EnemyDestroy");
        //Evento Effect Destroy
        if (onEnemyFXDestroy != null)
            onEnemyFXDestroy(transform.position);
        */
        RemoveCurrentElementToList();
        gameManager.AddCoins(reward);  //Reward
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg) {
        health -= dmg;
        if (health <= 0) {
            SetDestroyed();
        }
    }

    private void OnDestroy() {
        RemoveCurrentElementToList();
    }
}
