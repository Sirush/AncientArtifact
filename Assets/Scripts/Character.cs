using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public string Name;

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public int Shield;
    public int MaxItems;
    public List<Item> Inventory;
    public List<Trait> Traits;
    public List<Card> Deck;

    [SerializeField] int _health = 5;


    void Awake()
    {
        Traits = new List<Trait>();
        Inventory = new List<Item>();
        Deck = new List<Card>();
    }

    public void AddItem(string name)
    {
        var item = ItemCollection.GetItem(name).Clone();
        item.InitializeItem();
        Inventory.Add(item);
        foreach (var card in item.GetCards())
        {
            Deck.Add(card);
        }
    }

    public void AddCard(string name)
    {
        var card = CardCollection.GetCard(name).Clone();
        Deck.Add(card);
    }

    public void SetHealth(int value)
    {
        if (value < 0)
        {
            Shield += value;
            if (Shield < 0)
            {
                _health += Shield;
            }
            Shield = 0;
        }
        else
        {
            _health = value;
        }
    }

    public void Death()
    {

    }

    public bool HasTrait(string trait)
    {
        return Traits.Contains(GameManager.GetTrait(trait));
    }

    public void CleanDeck()
    {
        Deck.RemoveAll((c) => c.UseNumber <= 0);
    }
}