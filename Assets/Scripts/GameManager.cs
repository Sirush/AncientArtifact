using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Current;

    public List<Character> Characters;
    public Dictionary<string, Trait> Traits;
    public GameObject EventCanvas, InventoryCanvas;
    public GameObject UIEventText, UIEventButton;
    public GameObject CardTemplate, ItemTemplate, PortraitTemplate;
    public static List<Item> GlobalInventory;
    public static int MaxGlobalItem = 4;

    public static Action OnCloseEvent;

    public List<Sprite> CardNumbers;
    public Sprite SIGN_ATK, SIGN_DEF, SIGN_USE;
    public Sprite FBase, MBase;
    public List<Sprite> FHair, MHair, Eyes, Nose, Mouth;

    public static Dictionary<string, Func<EventBase>> _events;

    public Party ChildParty;

    private float _gameTimer;
    private bool _isTimerPaused = true;
    private float _nextEvent;


    void Awake()
    {
        if (Current == null)
            Current = this;

        GlobalInventory = new List<Item>();
    }

    void Start()
    {
        CardCollection.InitializeCards();
        ItemCollection.InitializeItems();

        AddTraits();
        AddEvents();

        Test();
    }

    void Update()
    {
        if (!_isTimerPaused)
            _gameTimer += Time.deltaTime;

        if (_gameTimer >= _nextEvent)
        {
            RandomEvent();
            _nextEvent += UnityEngine.Random.Range(6f, 12f);
        }
    }

    void AddTraits()
    {
        Traits = new Dictionary<string, Trait>();
        Traits.Add("Strength", new Trait("Strength", "Strong.", Color.green));
        Traits.Add("Agile", new Trait("Agile", "Agile.", Color.green));
        Traits.Add("Lucky", new Trait("Lucky", "Lucky.", Color.green));
        Traits.Add("BigPocket", new Trait("Big Pocket", "+1 item slot.", Color.green));
        Traits.Add("Hoarder", new Trait("Hoarder", "+2 item slot.", Color.green));
        Traits.Add("DimensionalPocket", new Trait("Dimensional Pocket", "A magic pocket. +3 item slot", Color.green));
        Traits.Add("PocketDonator", new Trait("Pocket Donator", "-1 item slot +1 party item slot.", Color.green));
        Traits.Add("KingOfBullies", new Trait("King of Bullies", "You can play all your cards.", Color.green));
        Traits.Add("Tough", new Trait("Tough", "More life.", Color.green));
        Traits.Add("SleepyHead", new Trait("Sleepy Head", "You love dreaming.", Color.green));
        Traits.Add("Historian", new Trait("Historian", "You know everything.", Color.green));

        Traits.Add("Injured", new Trait("Injured", "Bruised.", Color.red));
        Traits["Injured"].Cards.Add("Injured");
        Traits["Injured"].Cards.Add("Injured");
        Traits["Injured"].Cards.Add("Injured");
        Traits.Add("Sick", new Trait("Sick", "You don't feel too good.", Color.red));
        Traits.Add("Unlucky", new Trait("Unlucky", "Unlucky.", Color.red));
        Traits.Add("OneHanded", new Trait("One Handed", "How did you lose it ?", Color.red));
        Traits.Add("WeakSwing", new Trait("Weak Swing", "You fight like a little girl.", Color.red));
        Traits.Add("Asthma", new Trait("Asthma", "It's. hard. to. breath", Color.red));
    }

    void AddEvents()
    {
        _events = new Dictionary<string, Func<EventBase>>();

        _events.Add("Adrenaline", () =>
            {
                var x = new EventAdrenaline();
                return x;
            }
        );

        _events.Add("BottomlessPit", () =>
            {
                var x = new EventBottomlessPit();
                return x;
            }
        );

        _events.Add("DarknessAllAround", () =>
            {
                var x = new EventDarknessAllAround();
                return x;
            }
        );

        _events.Add("Excalibur", () =>
            {
                var x = new EventExcalibur();
                return x;
            }
        );

        _events.Add("Pebbles", () =>
            {
                var x = new EventPebbles();
                return x;
            }
        );

        _events.Add("PocketfulStone", () =>
            {
                var x = new EventPocketfulStone();
                return x;
            }
        );

        _events.Add("Rift", () =>
            {
                var x = new EventRift();
                return x;
            }
        );

        _events.Add("StrangeFruit", () =>
            {
                var x = new EventStrangeFruit();
                return x;
            }
        );

        _events.Add("WoodenBlockade", () =>
            {
                var x = new EventWoodenBlockade();
                return x;
            }
        );

        _events.Add("CompanionRoad", () =>
            {
                var x = new EventCompanionRoad();
                return x;
            }
        );

        _events.Add("CurseLife", () =>
            {
                var x = new EventCurseLife();
                return x;
            }
        );

        _events.Add("PhilosopherDream", () =>
            {
                var x = new EventPhilosopherDream();
                return x;
            }
        );

        _events.Add("PointyHat", () =>
            {
                var x = new EventPointyHat();
                return x;
            }
        );

        _events.Add("IntoDepth", () =>
            {
                var x = new EventIntoDepth();
                return x;
            }
        );
    }

    public static void RandomEvent()
    {
        int rand = UnityEngine.Random.Range(0, _events.Count);
        DoEvent(_events.ElementAt(rand).Value());
    }

    void Test()
    {
        foreach (var c in GetAliveCharacters())
        {
            c.AddItem("Stones");
            c.AddItem("Axe");
            c.AddCard("Fist");
        }
        GameManager.AddItemToGlobalInventory("Stones");
        GameManager.AddItemToGlobalInventory("Saber");
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
            go.GetComponent<Character>().AddCard("Fist");
        }
    }

    public static void DoEvent(EventBase e)
    {
        PauseTimer();

        e.InitializeEvent();
        Current.EventCanvas.SetActive(true);
        Current.EventCanvas.transform.FindChildren("Text").GetComponent<Text>().text = e.Text;

        var optionArea = Current.EventCanvas.transform.FindChildren("OptionArea");

        foreach (var a in e.GetGlobalActions())
        {
            var obj = Instantiate(Current.UIEventButton);
            obj.transform.SetParent(optionArea, false);
            var button = obj.GetComponent<Button>();
            obj.GetComponentInChildren<Text>().text = a.Text;

            var onGlob = a.OnGlobal;
            button.onClick.AddListener(() => onGlob());
        }

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
                } else if (a.OnItem != null)
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
        var optionArea = Current.EventCanvas.transform.FindChildren("OptionArea");
        optionArea.DestroyChildren();

        Current.EventCanvas.transform.FindChildren("Text").GetComponent<Text>().text = text;

        var obj = Instantiate(Current.UIEventButton);
        obj.transform.SetParent(optionArea, false);
        var button = obj.GetComponent<Button>();
        obj.GetComponentInChildren<Text>().text = "Go On";
        button.onClick.AddListener(() =>
        {
            optionArea.DestroyChildren();
            Current.EventCanvas.SetActive(false);
            ResumeTimer();
            button.onClick.RemoveAllListeners();
            if (OnCloseEvent != null)
            {
                OnCloseEvent();
            }
            OnCloseEvent = null;
        });
    }

    public void ShowInventoryScreen()
    {
        PauseTimer();
        Current.InventoryCanvas.SetActive(true);
        Current.InventoryCanvas.GetComponent<InventoryUI>().Refresh();
    }

    public static List<Character> GetAliveCharacters()
    {
        return Current.Characters;
    }

    public static Trait GetTrait(string trait)
    {
        return Current.Traits[trait];
    }

    public static void StartBattle()
    {
        Current.GetComponent<Battle>().StartBattle();
    }

    public static void AddItemToGlobalInventory(string name)
    {
        if (GlobalInventory.Count >= MaxGlobalItem)
            return;
        var item = ItemCollection.GetItem(name).Clone();
        item.InitializeItem();
        GlobalInventory.Add(item);
    }

    public static void AddItemToGlobalInventory(Item item)
    {
        if (GlobalInventory.Count >= MaxGlobalItem)
            return;
        GlobalInventory.Add(item);
    }

    public static void RemoveItem(Item item)
    {
        GlobalInventory.Remove(item);
    }

    public static void MoveItemToCharacterInventory(Character c, Item item)
    {
        c.AddItem(item);
        GlobalInventory.Remove(item);
    }

    public static void KillKid(Character c)
    {
        var kill = Current.Characters.FindIndex(x => x == c);
        Current.Characters.Remove(c);
        Current.ChildParty.Characters[kill].GetComponentInChildren<CharacterMovemement>().enabled = false;
        Current.ChildParty.Characters.RemoveAt(kill);
    }

    public static void ResumeTimer()
    {
        Current._isTimerPaused = false;
        foreach (var c in Current.ChildParty.Characters)
        {
            c.GetComponentInChildren<CharacterMovemement>().enabled = true;
        }
    }

    public static void PauseTimer()
    {
        Current._isTimerPaused = true;
        foreach (var c in Current.ChildParty.Characters)
        {
            c.GetComponentInChildren<CharacterMovemement>().enabled = false;
        }
    }
}