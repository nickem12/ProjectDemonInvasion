using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour {

    private GameObject gameManager;
    public string enemy;
    public WeaknessType type;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameManager.GetComponent<CombatManager>().StartCombat(enemy, type);
            Destroy(this.gameObject);
        }
    }
}
