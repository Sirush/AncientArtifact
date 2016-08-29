using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventAdrenaline : EventBase
{

    public override void InitializeEvent()
    {
        Text = "Someone is shivering of excitement.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Me !",
            (c) =>
            {
                c.AddCard("Dodge2");
                GameManager.DisplayEventResult("Good boy !!");
            },
            null
        ));
    }
}
