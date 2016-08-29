using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventBottomlessPit : EventBase
{

    public override void InitializeEvent()
    {
        Text = "They stop just in time after barely falling into an infinite abyss. They can just make out a ladder on the other side of the pit, which they could try to reach to cross it.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Try to reach for the ladder",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);

                var chance = .5f;
                if (c.HasTrait("Agile"))
                    chance += .2f;
                if (c.HasTrait("Injured"))
                    chance -= .2f;
                if (c.HasTrait("Lucky"))
                    chance += .1f;
                if (c.HasTrait("Unlucky"))
                    chance -= .1f;

                if (rand > chance)
                {
                    c.Death();
                    GameManager.DisplayEventResult("<color=blue>" + c.Name +
                                                   "</color> manages barely to set the ladder into motion and the party can carry on. Sadly, he also loses his footing, falls into the botomless pit and <color=red>dies</colors>");
                } else
                {
                    GameManager.DisplayEventResult("<color=blue>" + c.Name + "</color> manages barely to set the ladder into motion and the party can carry on.");
                }
            },
            null
        ));

        _actions.Add(new EventData(
            "Tool",
            "Cross with",
            null,
            (i) =>
            {
                var item = i;
                GameManager.DisplayEventResult("<color=blue>" + item.User.Name + "</color> makes a makeshift bridge with the <color=yellow>" + item.Name +
                                               "</color> and everyone can cross.");
            }
        ));
    }
}