using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using System;
using UnityEngine.UI;

public class Battle : MonoBehaviour {
    public List<Character> Enemies;

    public GameObject BattleCanvas;

    private Queue<Action> _battleStack;
    private List<GameObject> _cardsObject;
    private List<int> _alreadyPlayed;
    private int _currentTurn; //0 = player 1 = enemy
    private List<List<Card>> _enemiesCards;

    // Use this for initialization
    void Awake()
    {
        _cardsObject = new List<GameObject>();
        _alreadyPlayed = new List<int>();
        _enemiesCards = new List<List<Card>>();
    }


    public void StartBattle()
    {
        GameManager.PauseTimer();
        BattleCanvas.SetActive(true);
        _battleStack = new Queue<Action>();
        UpdateHealth();
        DrawCards();
    }

    public void EndBattle()
    {
        GameManager.ResumeTimer();
        BattleCanvas.SetActive(false);
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
        foreach (var c in GameManager.GetAliveCharacters())
        {
            c.CleanDeck();
        }
        foreach (var c in Enemies)
        {
            c.CleanDeck();
        }

        UpdateHealth();
        _alreadyPlayed.Clear();
        _enemiesCards.Clear();


        if (_currentTurn == 1)
        {
            _currentTurn = 0;
            DrawCards();
        } else
        {
            _currentTurn = 1;
            DrawCards();
            StartCoroutine("EnemyTurn");
        }

        List<Character> toremove = new List<Character>();
        foreach (var e in Enemies)
        {
            if (e.Health <= 0)
                toremove.Add(e);
        }
        foreach (var e in toremove)
        {
            Enemies.Remove(e);
        }
        if (Enemies.Count <= 0)
            EndBattle();

    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(.5f);

        var allies = GameManager.GetAliveCharacters();

        //For each enemy
        for (int i = 0; i < Enemies.Count; i++)
        {
            //Pick randomly one of two cards
            int rand = UnityEngine.Random.Range(0, 2);
            var card = _enemiesCards[i][rand];

            if (Enemies[i].Stunned > 0)
            {
                Enemies[i].Stunned--;
                continue;
            }

            var go = BattleCanvas.transform.FindChildren("Enemy" + i).FindChildren("CardPosition").GetChild(rand).gameObject;
            LeanTween.scale(go, new Vector3(.85f, .85f), .5f).setLoopPingPong().setEase(LeanTweenType.easeOutCirc);
            go.GetComponentInChildren<NicerOutline>().enabled = true;

            if (card.IsUseableOnAllAllies)
            {
                _battleStack.Enqueue(() => card.UseCardOnTarget(Enemies.ToArray()));
            }
            if (card.IsUseableOnAllEnemies)
            {
                _battleStack.Enqueue(() => card.UseCardOnTarget(allies.ToArray()));
            }
            if (card.IsUseableOnEnemies)
                _battleStack.Enqueue(() => { card.UseCardOnTarget(allies[RandomExcludeSelf(i, allies.Count)]); });

            else if (card.IsUseableOnAllies)
                _battleStack.Enqueue(() => { card.UseCardOnTarget(Enemies[RandomExcludeSelf(i, Enemies.Count)]); });
            else if (card.IsUseableOnSelf)
                _battleStack.Enqueue(() => { card.UseCardOnTarget(Enemies[i]); });

            yield return new WaitForSeconds(1f);
        }

        PlayTurn();
    }

    int RandomExcludeSelf(int self, int max)
    {
        int rand = 0;

        do
        {
            rand = UnityEngine.Random.Range(0, max);
        } while (rand == self);
        return rand;
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
        List<Character> characters;

        if (_currentTurn == 0)
            characters = GameManager.GetAliveCharacters();
        else
            characters = Enemies;

        for (int i = 0; i < characters.Count; i++)
        {
            var c = characters[i];

            if (c.Stunned > 0)
            {
                c.Stunned--;
                continue;
            }

            int rand1 = 0;
            int rand2 = 0;

            while (rand1 == rand2)
            {
                rand1 = UnityEngine.Random.Range(0, c.Deck.Count);
                rand2 = UnityEngine.Random.Range(0, c.Deck.Count);
            }

            var c1 = c.Deck[rand1];
            var c2 = c.Deck[rand2];

            if (_currentTurn == 1)
            {
                _enemiesCards.Add(new List<Card>());
                _enemiesCards[i].Add(c1);
                _enemiesCards[i].Add(c2);
            }

            var card1 = c1.DisplayCard();
            var card2 = c2.DisplayCard();

            //Tweening Sliding effect
            var pos = card1.transform.localPosition.y;
            if (_currentTurn == 0)
            {
                card1.transform.localPosition -= new Vector3(0, 300);
                card2.transform.localPosition -= new Vector3(0, 300);
            } else
            {
                card1.transform.localPosition += new Vector3(0, 300);
                card2.transform.localPosition += new Vector3(0, 300);
            }
            LeanTween.moveLocalY(card1, pos, UnityEngine.Random.Range(.4f, .8f)).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocalY(card2, pos, UnityEngine.Random.Range(.4f, .8f)).setEase(LeanTweenType.easeOutCubic);

            _cardsObject.Add(card1);
            _cardsObject.Add(card2);

            Transform cardPosition;
            if (_currentTurn == 0)
                cardPosition = BattleCanvas.transform.FindChildren("Ally" + i).FindChildren("CardPosition");
            else
                cardPosition = BattleCanvas.transform.FindChildren("Enemy" + i).FindChildren("CardPosition");
            card1.transform.SetParent(cardPosition, false);
            card2.transform.SetParent(cardPosition, false);

            if (_currentTurn == 0)
            {
                SetupCard(card1, c1, i);
                SetupCard(card2, c2, i);
            }
        }
    }

    void SetupCard(GameObject go, Card card, int self)
    {
        EventTrigger t1 = go.GetComponent<EventTrigger>();
        EventTrigger.Entry e1 = new EventTrigger.Entry();
        e1.eventID = EventTriggerType.PointerClick;

        e1.callback.AddListener((e) =>
        {
            DisableCardSelectAll();

            LeanTween.scale(go, new Vector3(.85f, .85f), .5f).setLoopPingPong().setEase(LeanTweenType.easeOutCirc);

            var allies = GameManager.GetAliveCharacters();

            go.GetComponentInChildren<NicerOutline>().enabled = true;
            if (card.IsUseableOnAllAllies)
            {
                PlayCard(self);
                _battleStack.Enqueue(() => card.UseCardOnTarget(allies.ToArray()));
            }
            if (card.IsUseableOnAllEnemies)
            {
                PlayCard(self);
                _battleStack.Enqueue(() => card.UseCardOnTarget(Enemies.ToArray()));
            }
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
                                PlayCard(self);
                                _battleStack.Enqueue(() => { card.UseCardOnTarget(target); });
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
                            PlayCard(self);
                            _battleStack.Enqueue(() => { card.UseCardOnTarget(enemy); });
                        });
                }
            if (card.IsUseableOnSelf)
            {
                BattleCanvas.transform.FindChildren("Ally" + self)
                    .FindChildren("Attack")
                    .gameObject.GetComponent<Button>()
                    .onClick.AddListener(() =>
                    {
                        PlayCard(self);
                        _battleStack.Enqueue(() => card.UseCardOnTarget(allies[self]));
                    });
            }
        });

        t1.triggers.Add(e1);
    }

    void PlayCard(int who)
    {
        var allies = GameManager.GetAliveCharacters();
        _alreadyPlayed.Add(who);
        DisableCardSelect(who);
        for (int i = 0; i < allies.Count; i++)
        {
            if (!_alreadyPlayed.Contains(i))
                EnableCardSelect(i);
        }
        ClearButtons();
    }

    void DisableCardSelectAll()
    {
        for (int i = 0; i < GameManager.GetAliveCharacters().Count; i++)
            DisableCardSelect(i);
    }

    void DisableCardSelect(int who)
    {
        foreach (Transform card in BattleCanvas.transform.FindChildren("Ally" + who).FindChildren("CardPosition"))
        {
            card.GetComponent<EventTrigger>().enabled = false;
        }
    }

    void EnableCardSelect(int who)
    {
        foreach (Transform card in BattleCanvas.transform.FindChildren("Ally" + who).FindChildren("CardPosition"))
        {
            card.GetComponent<EventTrigger>().enabled = true;
        }
    }

    void ClearButtons()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("AttackButton"))
        {
            go.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}