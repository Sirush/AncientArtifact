using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventWoodenBlockade : EventBase
{

    public override void InitializeEvent()
    {
        Text = "The party moves forward when they find a wooden barricade standing in their way. It seems pretty heavy and unstable. They will have to move it somehow to go forward.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Push with bare hands",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                Debug.Log(rand);
                if (rand < .5f)
                {
                    c.Traits.Add(GameManager.GetTrait("Injured"));
                    c.Health -= 1;
                    GameManager.DisplayEventResult("<color=blue>" + c.Name + "</color> successfully pushed the wood away but a big chunk fell on him. He is now <color=red>injured</color>");
                }
                else
                {
                    GameManager.DisplayEventResult("<color=blue>" + c.Name + "</color> successfully pushed the wood away and the team can now move forward.");
                }
            },
            null
        ));

        _actions.Add(new EventData(
            "Sharp",
            "Cut through with",
            null,
            (i) =>
            {
                var item = i;
                GameManager.DisplayEventResult("<color=blue>" + item.User.Name + "</color> successfully cut through the barricade with his trusty <color=yellow>" + item.Name +"</color>. But sadly, the <color=yellow>" + item.Name + "</color> <color=red>broke</color>");
                i.Remove();
            }
        ));
    }
}
