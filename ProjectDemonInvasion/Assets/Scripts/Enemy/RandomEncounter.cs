using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour {

    private GameObject player;
    private Vector3 startPosition;
    [SerializeField]
    private float distanceNeeded;
    private float distance;
    private float rng;
    private GameObject gameManager;
    public List<string> enemyNames = new List<string>();
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        startPosition = player.transform.position;
        distanceNeeded = 5;
	}
	
	// Update is called once per frame
	void Update () {
        distance = Vector3.Distance(startPosition, player.transform.position);
        if(distance>=distanceNeeded)
        {
            SpawnEnemy();
            startPosition = player.transform.position;
        }
	}

    private void SpawnEnemy()
    {
        rng = Random.Range(0.1f, 10f);
        if (rng > 7.1f) return;

        gameManager.GetComponent<CombatManager>().StartCombat(enemyNames[Random.Range(0, enemyNames.Count)], WeaknessType.NORMAL);
    }
}
