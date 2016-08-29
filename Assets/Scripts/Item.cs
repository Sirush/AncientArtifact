using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;

[System.Serializable]
public class Item {

    public string Name;
    public string Description;
    [JsonIgnore] public Sprite ItemSprite;

    public Character User;

    public List<string> Tags;
    public List<Card> DefaultCards;
    
    private List<Card> _currentCards;

    public void InitializeItem()
    {
        _currentCards = new List<Card>();

        foreach (var card in DefaultCards)
        {
            _currentCards.Add(card.Clone());
        }
    }

    public GameObject DisplayItem()
    {
        var item = GameObject.Instantiate(GameManager.Current.ItemTemplate).transform;
        //card.FindChildren("Name").GetComponent<Text>().text = Name;
        if (ItemSprite != null)
            item.FindChildren("Image").GetComponent<Image>().sprite = ItemSprite;

        return item.gameObject;
    }


    public List<Card> GetCards()
    {
        return _currentCards;
    }

    public Item Clone()
    {
        var obj = JsonConvert.SerializeObject(this);
        var item = JsonConvert.DeserializeObject<Item>(obj);
        item.ItemSprite = ItemSprite;

        return item;
    }

    public void Remove()
    {

    }

}
