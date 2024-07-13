using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] float health;
    [SerializeField] Type typeSelected;

    public enum Type {
        PlayerMinion,
        EnemyMinion,
        PlayerBase,
        EnemyBase
    }

    void Start() {
        health = maxHealth;
    }

    void RemoveCurrentElementToList() {
        switch (typeSelected) {
            case Type.PlayerMinion:
                EntitiesManager.Instance.RemovePlayer(this.gameObject);
                break;
            case Type.EnemyMinion:
                EntitiesManager.Instance.RemoveEnemy(this.gameObject);
                break;
            case Type.PlayerBase:
                EntitiesManager.Instance.RemovePlayerBase(this.gameObject);
                break;
            case Type.EnemyBase:
                EntitiesManager.Instance.RemoveEnemyBase(this.gameObject);
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
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg) {
        health -= dmg;
        if (health <= 0) {
            SetDestroyed();
        }
    }
}
