using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventBottomHole : EventBase
{

    public override void InitializeEvent()
    {
        Text = "You find yourself at the bottom of the rift. It’s dark but you see a passage going back up, behind a pillar. Growlings come from this direction.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Global",
            "Go to the passage",
            null,
            null,
            () =>
            {
                GameManager.OnCloseEvent += () => GameManager.StartBattle();
                GameManager.DisplayEventResult("Metal beasts in front of you! You must fight for your life !");
            }
        ));

        _actions.Add(new EventData(
            "Rope",
            "Use",
            null,
            (i) => { GameManager.DisplayEventResult("You escalate the layers of stones that collapsed with a rope. You’re free."); }
        ));

        _actions.Add(new EventData(
            "Excalibur2",
            "Use",
            null,
            (i) =>
            {
                i.User.AddItem("MaskImpostor");
                i.User.RemoveItem(i);
                GameManager.DisplayEventResult(
                    "Something’s wrong. You see symbols on parts of the ground. It’s ancient, and evil ! You ear a voice from beyond : “Fake” ! Your blade darkens and changes into a mask!");
            }
        ));
    }
}