using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventPocketfulStone : EventBase
{

    public override void InitializeEvent()
    {
        Text = "Foes right ahead, maybe you could divert their attention or pass silently...";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Pass discreetly.",
            (c) =>
            {
                float rand = Random.Range(0f, 1f);
                float chance = .2f;
                var successText = "You go stealthily in their back.";

                if (c.HasTrait("Agile"))
                {
                    c.AddItem("OldDagger");
                    GameManager.DisplayEventResult("You swiftly go without a sound in their back, without any of them noticing, you even take a little something before leaving.");

                    return;
                }

                if (rand > chance)
                {
                    GameManager.OnCloseEvent += () => GameManager.StartBattle();
                    GameManager.DisplayEventResult("They see you !");
                } else
                {
                    GameManager.DisplayEventResult(successText);
                }
            }
        ));

        _actions.Add(new EventData(
            "Stones",
            "Throw",
            null,
            (i) => { GameManager.DisplayEventResult("You throw a stone in the dark and the noise attract them away. You go quickly in the darkness without being noticed."); }
        ));
    }
}