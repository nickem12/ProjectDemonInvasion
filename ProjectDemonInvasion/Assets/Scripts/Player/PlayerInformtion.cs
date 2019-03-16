using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformtion : MonoBehaviour {

    public List<Card> inventory = new List<Card>();
    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> played = new List<Card>();
    public List<Card> reward = new List<Card>();

    public GameObject inventoryHandler;
    public GameObject[] cardsForHand;
    public int maxDeckCards;
    public int maxInventoryCards;
    int deckIndex = 0;

    public int targetIndex;
    public int comboLength;
    bool inCombat;
	// Use this for initialization
	void Awake () {

        InitialCreateDeck();
        maxDeckCards = 20;

        comboLength = 3;
        targetIndex = 0;
        inCombat = false;

	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown("space"))
        //{
        //    Card c = new Card("Sword", "Weapon", "Normal", 1, 7, 0, 0);
        //    inventory.Add(c);
        //    Debug.Log("Added Card.");
        //    inventoryHandler.GetComponent<ScrollScript>().GenerateInventoryList();
        //    inventoryHandler.GetComponent<ScrollScript>().GenerateDeckList();
        //}
    }

    void InitialCreateDeck()
    {
        Card c = new Card("Sword", "Weapon", "Normal", 1, 7, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Dagger", "Weapon", "Normal", 1, 5, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Katana", "Weapon", "Normal", 1, 9, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Sword", "Weapon", "Normal", 2, 12, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Dagger", "Weapon", "Normal", 2, 10, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Katana", "Weapon", "Normal", 2, 14, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Sword", "Weapon", "Normal", 3, 17, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Dagger", "Weapon", "Normal", 3, 15, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Katana", "Weapon", "Normal", 3, 19, 0, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Armour", "Armour", "Normal", 1, 0, 5, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Armour", "Armour", "Normal", 1, 0, 5, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Armour", "Armour", "Normal", 2, 0, 10, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Armour", "Armour", "Normal", 2, 0, 10, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Armour", "Armour", "Normal", 3, 0, 15, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Armour", "Armour", "Normal", 3, 0, 15, 0);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("MinorPotion", "Item", "Normal", 1, 0, 0, 5);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Ale", "Item", "Normal", 1, 0, 0, 5);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Ham", "Item", "Normal", 2, 0, 0, 10);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("Cake", "Item", "Normal", 2, 0, 0, 10);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);

        c = new Card("MajorPotion", "Item", "Normal", 3, 0, 0, 15);
        c.inDeck = true;
        inventory.Add(c);
        deck.Add(c);
    }

    public void RemoveCardFromDeck(Card c)
    {
        deck.Remove(c);
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i] == c)
            {
                inventory[i].inDeck = false;
                return;
            }
        }
    }

    public void RemoveCardFromDeck(string cardName, string element, string comboNum)
    {
        int i = 0;
        int combo = int.Parse(comboNum);

        for(i = 0;i<deck.Count;i++)
        {
            if(deck[i].cardName == cardName && deck[i].element == element && deck[i].comboNum == combo)
            {
                deck.Remove(deck[i]);
                break;
            }
        }
        for(i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].cardName == cardName && inventory[i].element == element && inventory[i].comboNum == combo)
            {
                inventory[i].inDeck = false;
                break;
            }
        }
        inventoryHandler.GetComponent<ScrollScript>().GenerateInventoryList();
        inventoryHandler.GetComponent<ScrollScript>().GenerateDeckList();
    }

    public void AddCardToDeck(string cardName,string element,string comboNum)
    {
        if(deck.Count<maxDeckCards)
        {
            int combo = int.Parse(comboNum);

            for (int i = 0; i < inventory.Count; i++)
            {
                if(!inventory[i].inDeck)
                {
                    if (inventory[i].cardName == cardName && inventory[i].element == element && inventory[i].comboNum == combo)
                    {
                        inventory[i].inDeck = true;
                        deck.Add(inventory[i]);
                        break;
                    }
                }
            }
            inventoryHandler.GetComponent<ScrollScript>().GenerateInventoryList();
            inventoryHandler.GetComponent<ScrollScript>().GenerateDeckList();
        }
        else
        {
            Debug.Log("Max amount of cards reached");
            return;
        }
    }

    public void StartCombat()
    {
        ShuffleDeck();
        for (int i = 0; i < 5; i++)
        {
            hand.Add(deck[i]);
        }
        deckIndex = 5;
        DisplayHand();
    }

    public void ShuffleDeck()
    {
        for(int i = 0; i < deck.Count - 1; i++)
        {
            int rng = Random.Range(i, deck.Count);
            Card tmp = deck[i];
            deck[i] = deck[rng];
            deck[rng] = tmp;
        }
    }

    public void OnPlayCard(int index)
    {
        if(played.Count < comboLength)
        {
            played.Add(hand[index]);
            hand.RemoveAt(index);
            hand.Add(deck[deckIndex]);
            deckIndex++;
            if(deckIndex>=deck.Count)
            {
                deckIndex = 0;
            }
            DisplayHand();
        }

    }

    public void DisplayHand()
    {
        
        for(int w = 0; w < 5; w++)
        {
            string element = hand[w].element;
            string cardType = hand[w].cardName;
           
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < cardsForHand[0].transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.transform.childCount; j++)
                {
                    cardsForHand[w].transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject.SetActive(false);
                }
                cardsForHand[w].transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            for(int a = 0;a<5;a++)
            {
                cardsForHand[w].transform.GetChild(1).gameObject.transform.GetChild(a).gameObject.SetActive(false);
            }
            cardsForHand[w].transform.GetChild(0).gameObject.transform.Find(element).gameObject.SetActive(true);
            cardsForHand[w].transform.GetChild(0).gameObject.transform.Find(element).gameObject.transform.Find(cardType).gameObject.SetActive(true);
            cardsForHand[w].transform.GetChild(1).gameObject.transform.GetChild(hand[w].comboNum - 1).gameObject.SetActive(true);
        }
    }
}
