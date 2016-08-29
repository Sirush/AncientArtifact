using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventCurseLife : EventBase
{

    public override void InitializeEvent()
    {
        Text = "A beautiful pair of white shoes lies before you.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Take it",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                var successText = "The leather is smooth under your sole. You feel quicker. But there may be a price to pay.";

                var chance = .5f;

                if (c.HasTrait("Unlucky"))
                    chance = .3f;

                if (rand > chance)
                {
                    c.AddCard("Curse1");
                    c.AddCard("Curse1");
                    c.AddCard("Curse1");
                    c.AddCard("Curse1");
                    GameManager.DisplayEventResult(
                        "As you close on, you see they are in a poor state. You touch them to see if you can salvage them. You can’t. And you shouldn’t have touched this thing.");
                } else
                {
                    c.AddItem("WhiteShoes");
                    c.AddCard("Curse1");
                    c.AddCard("Curse1");
                    GameManager.DisplayEventResult(successText);
                }
            },
            null
        ));
    }
}