using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Current;

    public List<Character> Characters;
    public Dictionary<string, Trait> Traits;
    public GameObject EventCanvas;
    public GameObject UIEventText, UIEventButton;
    public GameObject CardTemplate;


    void Start()
    {
        if (Current == null)
            Current = this;

        CardCollection.InitializeCards();
        ItemCollection.InitializeItems();

        Traits = new Dictionary<string, Trait>();
        Traits.Add("Injured", new Trait("Injured", "Missing a leg", Color.red));

        Test();
    }

    void Test()
    {
        foreach (var c in GetAliveCharacters())
        {
            c.AddItem("Axe");
            c.AddItem("Katana");
            c.AddItem("Pickaxe");
            c.AddCard("Fist");
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
        }
        GetComponent<Battle>().StartBattle();
    }

    public static void DoEvent(EventBase e)
    {
        e.InitializeEvent();
        Current.EventCanvas.SetActive(true);
        Current.EventCanvas.transform.FindChildren("Text").GetComponent<Text>().text = e.Text;

        var optionArea = Current.EventCanvas.transform.FindChildren("OptionArea");
        foreach (var c in GetAliveCharacters())
        {
            var name = Instantiate(Current.UIEventText);
            name.transform.SetParent(optionArea, false);
            name.GetComponent<Text>().text = c.Name;

            var actions = e.GetPossibleActions(c);
            foreach (var a in actions)
            {
                var obj = Instantiate(Current.UIEventButton);
                obj.transform.SetParent(optionArea, false);
                var button = obj.GetComponent<Button>();
                obj.GetComponentInChildren<Text>().text = a.Text;

                if (a.OnCharacter != null)
                {
                    var onChar = a.OnCharacter;
                    var child = c;
                    button.onClick.AddListener(() => onChar(child));
                }
                else
                {
                    var onItem = a.OnItem;
                    var item = a.EventItem;
                    button.onClick.AddListener(() => onItem(item));
                }
            }
        }
    }

    public static void DisplayEventResult(string text)
    {
        Debug.Log(text);
        var optionArea = Current.EventCanvas.transform.FindChildren("OptionArea");
        optionArea.DestroyChildren();

        Current.EventCanvas.transform.FindChildren("Text").GetComponent<Text>().text = text;
    }

    public static List<Character> GetAliveCharacters()
    {
        return Current.Characters;
    }

    public static Trait GetTrait(string trait)
    {
        return Current.Traits[trait];
    }
}