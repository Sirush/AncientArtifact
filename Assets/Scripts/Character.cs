using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Character : MonoBehaviour
{
    public string Name;

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public int Shield;
    public int MaxItems = 4;
    public List<Item> Inventory;
    public List<Trait> Traits;
    public List<Card> Deck;

    public Sprite Base, Hair, Mouth, Nose, Eyes;

    [SerializeField] int _health = 5;


    void Awake()
    {
        Traits = new List<Trait>();
        Inventory = new List<Item>();
        Deck = new List<Card>();
    }

    void Start()
    {
        RandomizePortrait();
    }

    public void AddItem(string name)
    {
        if (Inventory.Count >= MaxItems)
        {
            GameManager.AddItemToGlobalInventory(name);
            return;
        }
        var item = ItemCollection.GetItem(name).Clone();
        item.InitializeItem();
        Inventory.Add(item);
        foreach (var card in item.GetCards())
        {
            Deck.Add(card);
        }
    }

    public void AddItem(Item item)
    {
        if (Inventory.Count >= MaxItems)
        {
            GameManager.AddItemToGlobalInventory(item);
            return;
        }
        Inventory.Add(item);
        foreach (var card in item.GetCards())
        {
            Deck.Add(card);
        }
    }

    public void MoveItemToGlobalInventory(Item item)
    {
        if (GameManager.GlobalInventory.Count < GameManager.MaxGlobalItem)
        {
            GameManager.AddItemToGlobalInventory(item);
            RemoveItem(item);
        }
    }

    public void RemoveItem(Item item)
    {
        foreach (var card in item.GetCards())
        {
            Deck.Remove(card);
        }
        Inventory.Remove(item);
    }

    public void AddCard(string name)
    {
        var card = CardCollection.GetCard(name).Clone();
        card.User = this;
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
        } else
        {
            _health = value;
        }
    }

    public void Death()
    {}

    public bool HasTrait(string trait)
    {
        return Traits.Contains(GameManager.GetTrait(trait));
    }

    public void CleanDeck()
    {
        Deck.RemoveAll((c) => c.UseNumber <= 0);
    }

    public void RandomizePortrait()
    {
        var rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            Base = GameManager.Current.FBase;
            Hair = GameManager.Current.FHair[Random.Range(0, GameManager.Current.FHair.Count)];
        } else
        {
            Base = GameManager.Current.MBase;
            Hair = GameManager.Current.MHair[Random.Range(0, GameManager.Current.MHair.Count)];
        }

        Eyes = GameManager.Current.Eyes[Random.Range(0, GameManager.Current.Eyes.Count)];
        Nose = GameManager.Current.Nose[Random.Range(0, GameManager.Current.Nose.Count)];
        Mouth = GameManager.Current.Mouth[Random.Range(0, GameManager.Current.Mouth.Count)];
    }

    public GameObject DisplayPortrait()
    {
        var portrait = GameObject.Instantiate(GameManager.Current.PortraitTemplate).transform;

        portrait.FindChildren("Base").GetComponent<Image>().sprite = Base;
        portrait.FindChildren("Hair").GetComponent<Image>().sprite = Hair;
        portrait.FindChildren("Eyes").GetComponent<Image>().sprite = Eyes;
        portrait.FindChildren("Nose").GetComponent<Image>().sprite = Nose;
        portrait.FindChildren("Mouth").GetComponent<Image>().sprite = Mouth;

        var shadow = Instantiate(portrait);
        foreach (var outline in shadow.GetComponentsInChildren<BestFitOutline>())
        {
            outline.enabled = true;
        }
        portrait.SetParent(shadow);
        portrait.FindChildren("Glow").gameObject.SetActive(false);
        portrait.FindChildren("Background").gameObject.SetActive(false);


        return shadow.gameObject;
    }
}