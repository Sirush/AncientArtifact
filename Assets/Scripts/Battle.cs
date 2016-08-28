using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using System;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public List<Character> Enemies;

    public GameObject BattleCanvas;

    private Queue<Action> _battleStack;
    private List<GameObject> _cardsObject;

    // Use this for initialization
    void Awake()
    {
        _cardsObject = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartBattle()
    {
        BattleCanvas.SetActive(true);
        _battleStack = new Queue<Action>();
        UpdateHealth();
        DrawCards();
    }

    public void PlayTurn()
    {
        while (_battleStack.Count > 0)
            _battleStack.Dequeue()();
        foreach (var o in _cardsObject)
        {
            Destroy(o);
        }
        _cardsObject.Clear();
        DrawCards();
        UpdateHealth();
    }

    void UpdateHealth()
    {
        var allies = GameManager.GetAliveCharacters();
        for (int i = 0; i < allies.Count; i++)
        {
            BattleCanvas.transform.FindChildren("Ally" + i).FindChildren("Health").GetComponent<Text>().text =
                allies[i].Health + " HP";
        }
        for (int i = 0; i < Enemies.Count; i++)
        {
            BattleCanvas.transform.FindChildren("Enemy" + i).FindChildren("Health").GetComponent<Text>().text =
                Enemies[i].Health + " HP";
        }
    }

    public void DrawCards()
    {
        var characters = GameManager.GetAliveCharacters();
        for (int i = 0; i < characters.Count; i++)
        {
            var c = characters[i];
            int rand1 = 0;
            int rand2 = 0;

            while (rand1 == rand2)
            {
                rand1 = UnityEngine.Random.Range(0, c.Deck.Count);
                rand2 = UnityEngine.Random.Range(0, c.Deck.Count);
            }

            var c1 = c.Deck[rand1];
            var c2 = c.Deck[rand2];

            var card1 = c1.DisplayCard();
            var card2 = c2.DisplayCard();

            _cardsObject.Add(card1);
            _cardsObject.Add(card2);

            var cardPosition = BattleCanvas.transform.FindChildren("Ally" + i).FindChildren("CardPosition");
            card1.transform.SetParent(cardPosition, false);
            card2.transform.SetParent(cardPosition, false);

            SetupCard(card1, c1, i);
            SetupCard(card2, c2, i);
        }
    }

    void SetupCard(GameObject go, Card card, int self)
    {
        EventTrigger t1 = go.GetComponent<EventTrigger>();
        EventTrigger.Entry e1 = new EventTrigger.Entry();
        e1.eventID = EventTriggerType.PointerClick;

        e1.callback.AddListener((e) =>
        {
            var allies = GameManager.GetAliveCharacters();

            go.GetComponentInChildren<NicerOutline>().enabled = true;
            if (card.IsUseableOnAllAllies)
                _battleStack.Enqueue(() => card.UseCardOnTarget(allies.ToArray()));
            if (card.IsUseableOnAllEnemies)
                _battleStack.Enqueue(() => card.UseCardOnTarget(Enemies.ToArray()));
            if (card.IsUseableOnAllies)
                for (int j = 0; j < allies.Count; j++)
                {
                    if (j != self)
                    {
                        var target = allies[j];
                        BattleCanvas.transform.FindChildren("Ally" + j)
                            .FindChildren("Attack")
                            .gameObject.GetComponent<Button>()
                            .onClick.AddListener(() =>
                            {
                                _battleStack.Enqueue(() => { card.UseCardOnTarget(target); });
                                ClearButtons();
                            });
                    }
                }
            if (card.IsUseableOnEnemies)
                for (int j = 0; j < Enemies.Count; j++)
                {
                    var enemy = Enemies[j];
                    BattleCanvas.transform.FindChildren("Enemy" + j)
                        .FindChildren("Attack")
                        .gameObject.GetComponent<Button>()
                        .onClick.AddListener(() =>
                        {
                            _battleStack.Enqueue(() => { card.UseCardOnTarget(enemy); });
                            ClearButtons();
                        });
                }
            if (card.IsUseableOnSelf)
            {
                BattleCanvas.transform.FindChildren("Ally" + self)
                    .FindChildren("Attack")
                    .gameObject.GetComponent<Button>()
                    .onClick.AddListener(() => _battleStack.Enqueue(() => card.UseCardOnTarget(allies[self])));
            }
        });

        t1.triggers.Add(e1);
    }

    void ClearButtons()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("AttackButton"))
        {
            go.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}