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
            var load = Resources.Load<Sprite>(a["Sprite"].ToString());
            if (load)
                card.CardSprite = load;
            else
                card.CardSprite = Resources.Load<Sprite>("Missing");
            _cards.Add(a["Id"].ToString(), card);
        }

        Action<Character> stun = null;
        Action<Character> heal = null;
        Action<Character> breakWeapon =null;

        GetCard("CallDepth").SpecialEffect = stun;
        GetCard("Drowning").SpecialEffect = stun;
        GetCard("Misery3").SpecialEffect = stun;
        GetCard("Misery4").SpecialEffect = stun;
        GetCard("Misery5").SpecialEffect = stun;
        GetCard("CharmingDance").SpecialEffect = stun;
        GetCard("CrystalKnife4").SpecialEffect = heal; //heal self 4
        GetCard("CrystalKnife5").SpecialEffect = heal; //heal self 5
        GetCard("Evicerate").SpecialEffect = heal; //heal self 15
        GetCard("Rend").SpecialEffect = breakWeapon; //Break weapon CrystalKnife




    }

    public static Card GetCard(string name)
    {
        return _cards[name];
    }

}
