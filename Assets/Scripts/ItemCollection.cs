using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class ItemCollection {

    private static Dictionary<string, Item> _items;

    public static void InitializeItems()
    {
        _items = new Dictionary<string, Item>();

        var data = Resources.Load("items") as TextAsset;
        JArray arr = JArray.Parse(data.text);
        foreach (var a in arr)
        {
            Item item = JsonConvert.DeserializeObject<Item>(a.ToString());
            item.DefaultCards = new List<Card>();
            foreach (var c in a["Cards"].ToObject<List<string>>())
            {
                item.DefaultCards.Add(CardCollection.GetCard(c));
            }
            _items.Add(a["Id"].ToString(), item);
        }


    }

    public static Item GetItem(string name)
    {
        return _items[name];
    }

}
