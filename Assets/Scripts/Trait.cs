using UnityEngine;
using System.Collections;

[System.Serializable]
public class Trait {

    public string Name;
    public string Description;

    private Color _color;


    public Trait(string name, string desc, Color color)
    {
        Name = name;
        Description = desc;
        _color = color;
    }
}
