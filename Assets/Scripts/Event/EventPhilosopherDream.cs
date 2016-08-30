using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventPhilosopherDream : EventBase
{

    public override void InitializeEvent()
    {
        Text = "You come across a pedestal on which is put a beautiful red gem. It’s glowing gently of a warm light.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Take it",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                var successText = "It glows gently in your palms. It warms you to your heart.";

                var chance = .6f;
                if (c.HasTrait("Injured") || c.HasTrait("Sick"))
                {
                    chance = 1f;
                    c.Traits.Remove(GameManager.GetTrait("Sick"));
                    c.Traits.Remove(GameManager.GetTrait("Injured"));
                    c.SetHealth(99);
                    successText = "You take the stone and you’re healed !";
                }
                if (c.HasTrait("Lucky"))
                {
                    chance = 1f;
                    successText = "You were lucky this time.";
                }

                if (rand > chance)
                {
                    c.SetHealth(-5);
                    GameManager.DisplayEventResult("The stone burns your hand. You let go and it breaks silently on the ground.");
                } else
                {
                    c.AddItem("Gemstone");
                    GameManager.DisplayEventResult(successText);
                }
            },
            null
        ));
    }
}