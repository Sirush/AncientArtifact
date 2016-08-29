using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

[System.Serializable]
public class Card
{
    public string Name, Description;
    [JsonIgnore]public Sprite CardSprite;

    public Character User;

    [JsonProperty("Uses")] public int UseNumber = 1;
    public int Attack, Shield;

    [JsonProperty("Dodge")] public bool IsDodge;

    [JsonProperty("UseSelf")] public bool IsUseableOnSelf;
    [JsonProperty("UseAllies")] public bool IsUseableOnAllies;
    [JsonProperty("UseEnemies")] public bool IsUseableOnEnemies;
    [JsonProperty("UseAllAllies")] public bool IsUseableOnAllAllies;
    [JsonProperty("UseAllEnemies")] public bool IsUseableOnAllEnemies;

    [JsonIgnore] public Action<Character, Character> SpecialEffect; //User, target

    public void UseCardOnTarget(params Character[] targets)
    {
        foreach (var target in targets)
        {
            if (Attack != 0)
                target.SetHealth(-Attack);
            if (Shield != 0)
                target.Shield += Shield;
            if (IsDodge)
                target.Dodge++;
            if (SpecialEffect != null)
                SpecialEffect(User, target);
        }
        UseNumber -= 1;
    }

    public GameObject DisplayCard()
    {
        var card = GameObject.Instantiate(GameManager.Current.CardTemplate).transform;
        //card.FindChildren("Name").GetComponent<Text>().text = Name;
        if (CardSprite != null)
            card.FindChildren("Image").GetComponent<Image>().sprite = CardSprite;
        card.FindChildren("Uses").GetComponent<Text>().text = UseNumber < 100 ? UseNumber.ToString() : "∞";
        if (Attack > 0)
        {
            card.FindChildren("CardSign").GetComponent<Image>().sprite = GameManager.Current.SIGN_ATK;
            card.FindChildren("Atk").GetComponent<Text>().text = Attack.ToString();
        } else
        {
            card.FindChildren("CardSign").GetComponent<Image>().sprite = GameManager.Current.SIGN_DEF;
            card.FindChildren("Atk").GetComponent<Text>().text = Shield.ToString();
        }

        return card.gameObject;
    }


    public Card Clone()
    {
        var obj = JsonConvert.SerializeObject(this);
        var card = JsonConvert.DeserializeObject<Card>(obj);
        card.SpecialEffect = SpecialEffect;
        card.CardSprite = CardSprite;

        return card;
    }
}