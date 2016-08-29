using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventPebbles : EventBase
{

    public override void InitializeEvent()
    {
        Text = "You find some pebbles.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Take some",
            (c) =>
            {
                c.AddItem("Stones");
                c.AddItem("Stones");
                c.AddItem("Stones");
                c.AddItem("Stones");
                GameManager.DisplayEventResult("You fill your pockets with <color=yellow>stones</color>");
            },
            null
        ));
    }
}
