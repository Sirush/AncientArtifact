using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Character : ScriptableObject
{
    public string Name;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health > MaxHealth)
                _health = MaxHealth;
        }
    }


    public bool IsKid;
    public int MaxHealth = 20;
    public int Shield;
    public int MaxItems = 4;
    public List<Item> Inventory;
    public List<Trait> Traits;
    public List<Card> Deck;
    public int Stunned = 0;
    public int Dodge = 0;

    public Sprite Base, Hair, Mouth, Nose, Eyes;

    [SerializeField] private int _health = 20;


    void Awake()
    {
        Traits = new List<Trait>();
        Inventory = new List<Item>();
        Deck = new List<Card>();
    }

    void Start()
    {
        //RandomizePortrait();
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
        item.User = this;
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
        item.User = this;
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
            item.User = null;
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
        if (Dodge > 0)
        {
            Dodge--;
            return;
        }
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

        if (_health > MaxHealth)
            _health = MaxHealth;

        if (_health <= 0 && IsKid)
        {
            GameManager.KillKid(this);
        }
    }

    public void Death()
    {
        SetHealth(-999);
    }

    public bool HasTrait(string trait)
    {
        return Traits.Contains(GameManager.GetTrait(trait));
    }

    public void CleanDeck()
    {
        Deck.RemoveAll((c) => c.UseNumber <= 0);
    }

    public GameObject RandomizePortrait()
    {
        GameObject hair;
        var rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            rand = Random.Range(0, GameManager.Current.FHair.Count);
            Base = GameManager.Current.FBase;
            Hair = GameManager.Current.FHair[rand];
            hair = GameManager.Current.FBHair[rand];
            RandomizeName(0);
        } else
        {
            rand = Random.Range(0, GameManager.Current.MHair.Count);
            Base = GameManager.Current.MBase;
            Hair = GameManager.Current.MHair[rand];
            hair = GameManager.Current.MBHair[rand];
            RandomizeName(1);
        }

        Eyes = GameManager.Current.Eyes[Random.Range(0, GameManager.Current.Eyes.Count)];
        Nose = GameManager.Current.Nose[Random.Range(0, GameManager.Current.Nose.Count)];
        Mouth = GameManager.Current.Mouth[Random.Range(0, GameManager.Current.Mouth.Count)];

        return hair;
    }

    public GameObject DisplayPortrait()
    {
        var portrait = GameObject.Instantiate(GameManager.Current.PortraitTemplate).transform;

        if (IsKid)
        {
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
        } else
        {
            portrait.FindChildren("Base").GetComponent<Image>().sprite = GameManager.Current.MonsterSprites[UnityEngine.Random.Range(0, GameManager.Current.MonsterSprites.Count)];
            portrait.FindChildren("Hair").gameObject.SetActive(false);
            portrait.FindChildren("Eyes").gameObject.SetActive(false);
            portrait.FindChildren("Nose").gameObject.SetActive(false);
            portrait.FindChildren("Mouth").gameObject.SetActive(false);

            return portrait.gameObject;
        }
    }

    void RandomizeName(int a)
    {
        TextAsset text = (TextAsset) Resources.Load("name" + a, typeof(TextAsset));
        StringReader reader = new StringReader(text.text);
        List<string> list = new List<string>();
        string line;

        while (true)
        {
            line = reader.ReadLine();
            if (line != null)
                list.Add(line);
            else
                break;
        }

        Name = list[UnityEngine.Random.Range(0, list.Count)];
    }

}