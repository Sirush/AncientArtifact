using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventDarknessAllAround : EventBase
{

    public override void InitializeEvent()
    {
        Text = "The pressure is too great and your brain is running wild.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Global",
            "We are all going to die…",
            null,
            null,
            () =>
            {
                foreach (var c in GameManager.GetAliveCharacters())
                {
                    c.AddCard("Curse1");
                }
                GameManager.DisplayEventResult("You should not utter such words in this place…");
            }
        ));
    }
}
