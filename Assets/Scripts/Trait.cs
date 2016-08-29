using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Trait
{

    public string Name;
    public string Description;
    public List<string> Cards = new List<string>();
    public Action<Character> Effect;

    private Color _color;


    public Trait(string name, string desc, Color color)
    {
        Name = name;
        Description = desc;
        _color = color;
    }

    public void AddToCharacter(Character c)
    {
        if (Effect != null)
            Effect(c);
        foreach (var card in Cards)
        {
            c.AddCard(card);
        }
        c.Traits.Add(this);
    }
}