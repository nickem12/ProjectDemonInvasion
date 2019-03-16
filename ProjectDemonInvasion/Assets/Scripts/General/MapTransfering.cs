using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransfering : MonoBehaviour {

    public GameObject transferLocation;
    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.transform.position = transferLocation.transform.position;
        }
    }
}
