using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{

    public string Name;

    public int Health
    {
        get
        {
            return this.Health;
        }
        set
        {
            if (value < 0)
            {
                Shield -= value;
                if (Shield < 0)
                {
                    Health += Shield;
                    Shield = 0;
                }
            }
        }
    }
    public int Shield;
    public int MaxItems;
    public List<Item> Inventory;
    public List<Trait> Traits;
    public List<Card> Deck;


    // Use this for initialization
    void Start()
    {
        Health = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCard(Card card)
    {
        Deck.Add(card);
    }
}
