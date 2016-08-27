using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            _cards.Add(a["Id"].ToString(), card);
        }


    }

    public static Card GetCard(string name)
    {
        return _cards[name];
    }

}
