using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventCompanionRoad : EventBase
{

    public override void InitializeEvent()
    {
        Text = "Against the wall, a skeleton is still grasping to a shiny shield.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Take it",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                var successText = "The arms of the corpse fall gently to the ground as you take up what was the best companion of a dead man.";

                var chance = .7f;

                if (rand > chance)
                {
                    GameManager.DisplayEventResult("The bones hold desperately to the shield, which breaks before you can take it out of its mortal grasp. Too bad for it. You will continue your adventure without its protection.");
                } else
                {
                    c.AddItem("RadiantShield");
                    GameManager.DisplayEventResult(successText);
                }
            },
            null
        ));

        _actions.Add(new EventData(
            "Shield",
            "New companion for",
            null,
            (i) =>
            {
                i.User.AddItem("RadiantShield");
                GameManager.DisplayEventResult("A new companion for the road.");
            }
        ));
    }
}