using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventStrangeFruit : EventBase
{

    public override void InitializeEvent()
    {
        Text = "On the wall a vine grows. On it, a fruit with a strange metal piece. Someone strapped a string around it.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Pick it",
            (c) =>
            {
                c.AddItem("Grenade");
                GameManager.DisplayEventResult("It seems useful.");
            },
            null
        ));
    }
}
