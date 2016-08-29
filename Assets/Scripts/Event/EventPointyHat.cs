using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventPointyHat : EventBase
{

    public override void InitializeEvent()
    {
        Text = "On the face of a skeleton lying on the ground is a mysterious pointy hat.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Take it",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                var successText = "You feel power rushing in your veins.";

                var chance = .6f;

                if (c.HasTrait("SleepyHead"))
                {
                    successText = "You yawn as your companions put the loot on your head. They’re right. It goes well on you.";
                    chance = 1f;
                } else if (c.HasTrait("Unlucky"))
                    chance = .2f;

                if (rand > chance)
                {
                    GameManager.DisplayEventResult("The hat turns to dust as you try to take it.");
                } else
                {
                    c.AddItem("PointyHat");
                    c.AddCard("Book");
                    c.AddCard("Book2");
                    GameManager.DisplayEventResult(successText);
                }
            },
            null
        ));
    }
}