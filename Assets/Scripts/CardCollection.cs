using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public static class CardCollection {

    private static Dictionary<string, Card> _cards;

    public static void InitializeCards()
    {
        _cards = new Dictionary<string, Card>();

        var data = Resources.Load("cards") as TextAsset;
        JArray arr = JArray.Parse(data.text);
        foreach (var a in arr)
        {
            Card card = JsonConvert.DeserializeObject<Card>(a.ToString());
            var load = Resources.Load<Sprite>("Sprites/" + a["Sprite"].ToString());
            if (load)
                card.CardSprite = load;
            else
                card.CardSprite = Resources.Load<Sprite>("Sprites/Missing");
            _cards.Add(a["Id"].ToString(), card);
        }

        Action<Character, Character> stun = (user, target) => target.Stunned++;

        GetCard("CallDepth").SpecialEffect = stun;
        GetCard("Drowning").SpecialEffect = stun;
        GetCard("Misery3").SpecialEffect = stun;
        GetCard("Misery4").SpecialEffect = stun;
        GetCard("Misery5").SpecialEffect = stun;
        GetCard("CharmingDance").SpecialEffect = stun;
        GetCard("SweetDream").SpecialEffect = stun;

        GetCard("CrystalKnife4").SpecialEffect = (user, target) => user.SetHealth(4); //heal self 4
        GetCard("CrystalKnife5").SpecialEffect = (user, target) => user.SetHealth(5); //heal self 5
        GetCard("Evicerate").SpecialEffect = (user, target) => user.SetHealth(15); //heal self 15
        GetCard("Rend").SpecialEffect = (user, target) => user.RemoveItem(user.Inventory.Find(c => c.Name == "CrystalKnife")); //Break weapon CrystalKnife
    }

    public static Card GetCard(string name)
    {
        //Debug.Log(name);
        return _cards[name];
    }

}
