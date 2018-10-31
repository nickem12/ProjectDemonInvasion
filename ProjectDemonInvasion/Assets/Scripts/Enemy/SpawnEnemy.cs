using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public float distanceNeeded = 5;
    private GameObject gameManager;
    public List<GameObject> enemies = new List<GameObject>();
    private GameObject player;
    private float timer;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.GetComponent<CombatManager>().fieldEnemies = 0;
        timer = 30f;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            timer = 30f;
            if (gameManager.GetComponent<CombatManager>().fieldEnemies < 20)
            {
                if (Vector3.Distance(this.transform.position, player.transform.position) >= distanceNeeded)
                {
                    int rng = Random.Range(0, enemies.Count);
                    Instantiate(enemies[rng], this.transform.position, Quaternion.identity);
                    gameManager.GetComponent<CombatManager>().fieldEnemies++;
                }
            }
        }
	}
}
