using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public float distanceNeeded = 5;
    public List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> enemyList = new List<GameObject>();
    private GameObject gameManager;
    public List<GameObject> enemies = new List<GameObject>();
    private GameObject player;
    private float timer;
    private bool encouterOn = false;
	
    // Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.GetComponent<CombatManager>().fieldEnemies = 0;
        timer = 30f;
	}
	
	// Update is called once per frame
	void Update () {

        if (encouterOn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 30f;
                if (gameManager.GetComponent<CombatManager>().fieldEnemies < 20)
                {
                    int spawnrng = Random.Range(0, spawnPoints.Count);
                    if(Vector2.Distance(spawnPoints[spawnrng].position,player.transform.position)>distanceNeeded)
                    {
                        int rng = Random.Range(0, enemies.Count);
                        GameObject enemy = Instantiate(enemies[rng], spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
                        enemyList.Add(enemy);
                        gameManager.GetComponent<CombatManager>().fieldEnemies++;
                    }
                }
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        encouterOn = true;
        int i = 0;
        foreach(Transform s in spawnPoints)
        {
            int rng = Random.Range(0, enemies.Count);
            GameObject enemy = Instantiate(enemies[rng], spawnPoints[i].position, Quaternion.identity);
            enemyList.Add(enemy);
            gameManager.GetComponent<CombatManager>().fieldEnemies++;
            i++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        encouterOn = false;
        for(int i = enemies.Count-1 ; i >= 0 ; i--)
        {
            Destroy(enemyList[i]);
        }
    }
}
