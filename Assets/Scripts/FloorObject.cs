using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FloorObject : MonoBehaviour
{
    public bool isInfected;
    public Sprite[] sprites;

    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    List<Collider2D> troopsColliders = new();

    void Awake()
    {
        if(!gameManager)
            gameManager = FindObjectOfType<GameManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isInfected){
            if (spriteRenderer.sprite != sprites[1] && spriteRenderer.sprite != sprites[2])
                if (Random.Range(0, 10) == 1){
                    spriteRenderer.sprite = sprites[2];
                } else {
                    spriteRenderer.sprite = sprites[1];
                }
        } else {
            if (spriteRenderer.sprite != sprites[0])
                spriteRenderer.sprite = sprites[0];
        }

        // Check troops for infection
        int[] troopsCount = new int[4];

        boxCollider.OverlapCollider(new ContactFilter2D().NoFilter(), troopsColliders);
        foreach (var collider in troopsColliders){
            switch (collider.gameObject.tag){
                case "EnemyMinion":
                    troopsCount[0] += 1;
                break;
                case "PlayerMinion":
                    troopsCount[1] += 1;
                break;
                case "EnemyBase":
                    troopsCount[2] += 1;
                break;
                case "PlayerBase":
                    troopsCount[3] += 1;
                break;
            }
        }

        if (troopsCount[2] != 0 || troopsCount[3] != 0){
            if (troopsCount[2] > troopsCount[3]){
                isInfected = true;
            } else {
                isInfected = false;
            }
        } else if(troopsCount[0] != 0 || troopsCount[1] != 0) {
            if (troopsCount[0] > troopsCount[1]){
                isInfected = true;
            } else {
                isInfected = false;
            }
        }
    }
}
