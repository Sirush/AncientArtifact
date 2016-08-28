using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class Item {

    public string Name;
    public string Description;

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

    public List<Card> GetCards()
    {
        return _currentCards;
    }

    public Item Clone()
    {
        var obj = JsonConvert.SerializeObject(this);
        var item = JsonConvert.DeserializeObject<Item>(obj);

        return item;
    }

    public void Remove()
    {

    }

}
