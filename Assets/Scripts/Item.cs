using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        _currentCards = DefaultCards;
    }

    public void Remove()
    {

    }

}
