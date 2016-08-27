using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Trait {

    public string Name;
    public string Description;
    public List<Card> Cards = new List<Card>();

    private Color _color;


    public Trait(string name, string desc, Color color)
    {
        Name = name;
        Description = desc;
        _color = color;
    }

    public void AddToCharacter(Character c)
    {
        foreach(var card in Cards)
        {
            c.AddCard(card);
            c.Traits.Add(this);
        }
    }
}
