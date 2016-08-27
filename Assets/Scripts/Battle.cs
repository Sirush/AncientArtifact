using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle : MonoBehaviour
{

    public List<Character> Enemies;

    public GameObject BattleCanvas;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBattle()
    {
        BattleCanvas.SetActive(true);
        DrawCards();
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
                rand1 = Random.Range(0, c.Deck.Count);
                rand2 = Random.Range(0, c.Deck.Count);
            }

            var card1 = c.Deck[rand1].DisplayCard();
            var card2 = c.Deck[rand2].DisplayCard();

            var cardPosition = BattleCanvas.transform.FindChildren("Card Position " + i);
            card1.transform.SetParent(cardPosition, false);
            card2.transform.SetParent(cardPosition, false);
        }
    }
}
