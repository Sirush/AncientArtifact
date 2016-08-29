using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventRift : EventBase
{

    public override void InitializeEvent()
    {
        Text = "A great dark chasm opens under your feet, as the whole damn place is shaking.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Jump !",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                var successText = "Close enough, you barely made it before the crevice opens and swallows a whole section of the ground.";

                var chance = .2f;
                if (c.HasTrait("Agile"))
                {
                    chance = .8f;
                    successText = "You jump from unstable ground to unstable ground but you finally made it !";
                }
                if (c.HasTrait("Lucky"))
                {
                    chance = .8f;
                    successText = "You were lucky this time.";
                }

                if (rand > chance)
                {
                    foreach (var a in GameManager.GetAliveCharacters())
                    {
                        a.SetHealth(-5);
                    }
                    GameManager.GetTrait("Injured").AddToCharacter(c);
                    GameManager.OnCloseEvent += () => GameManager.DoEvent(new EventBottomHole());
                    GameManager.DisplayEventResult("The ground collapses under you and you hurt yourselves badly in the fall.");
                } else
                {
                    GameManager.DisplayEventResult(successText);
                }
            },
            null
        ));
    }
}