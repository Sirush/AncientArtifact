using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventIntoDepth : EventBase
{

    public override void InitializeEvent()
    {
        Text = "A strange feeling comes to you. A powerful pressure puts you to the ground. You feel like underwater.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Ask for mercy",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                var successText = "You see something! A huge mask of iron against a stone. You run for it and as soon as you touch it the pressure disappears..";

                var chance = .2f;
                if (c.HasTrait("Lucky"))
                {
                    chance = 1f;
                }
                if (c.HasTrait("Strength"))
                {
                    chance = 1f;
                    successText = "You resist the pressure! It takes you all you have but you succeed. As the pressure fades away you see against a stone an enormous mask or maybe helm.";
                }

                if (rand > chance)
                {
                    foreach (var a in GameManager.GetAliveCharacters())
                    {
                        a.SetHealth(-5);
                    }
                    GameManager.GetTrait("Injured").AddToCharacter(c);
                    GameManager.DisplayEventResult("You implore for it to stop, but the pressure only goes up. It shatters your bones.");
                } else
                {
                    c.AddItem("MaskDepth");
                    GameManager.DisplayEventResult(successText);
                }
            },
            null
        ));

        _actions.Add(new EventData(
            "MaskDepth",
            "Use",
            null,
            (i) =>
            {
                i.User.AddItem("Depth");
                GameManager.DisplayEventResult(
                    "Another one. Another one to conquer, another one to convert. To welcome a new brother into the Depth. Such pressure is nothing for you, from the binocles of the mask you see the new one. You take it and your companions are free of its power. Now, it is for you to pass on.");
            }
        ));
    }
}