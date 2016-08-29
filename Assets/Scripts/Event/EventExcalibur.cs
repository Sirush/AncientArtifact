using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventExcalibur : EventBase
{

    public override void InitializeEvent()
    {
        Text = "In front of you a shiny blade is sealed in a rock. It glows as if coming from another world.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Try to get the sword out",
            (c) =>
            {
                var rand = Random.Range(0f, 1f);
                var successText = "The blade sings as it slides out the rock. Freed, in your hands, it feels all powerful. You are crowned by its strength.";

                var chance = .3f;
                if (c.HasTrait("OneHanded"))
                {
                    chance = 1f;
                    successText = "Your unique hand gets the sword out easily. You realise how lucky you are to still have one hand to wield this beautiful blade.";
                }
                if (c.HasTrait("SleepyHead"))
                {
                    chance = 1f;
                    successText =
                        "You would like to sleep now but, as soon as your hand touch the handle, you see a dream greater than all before. A dream of a wonderful place full of joy, far away from here! When you open your eyes le blade rests between your hands. You now have a dream to defend!";
                }
                if (c.HasTrait("Historian"))
                {
                    c.AddCard("Curse1");
                    c.AddCard("Curse1");
                    c.AddCard("Curse1");
                    GameManager.DisplayEventResult("You… You are a fake.");

                    return;
                }
                if (c.HasTrait("Strength"))
                {
                    successText = "Force alone was just enough for this damn sword. You have the blade but something feels wrong. You are Cursed.";
                }

                if (rand > chance)
                {
                    c.SetHealth(-5);
                    GameManager.DisplayEventResult("You can pull all you want it will not come out. But at least you pulled your back.");
                } else
                {
                    if (c.HasTrait("Strength"))
                    {
                        c.AddCard("Curse1");
                        c.AddCard("Curse1");
                    }
                    c.AddItem("Excalibur");
                    GameManager.DisplayEventResult(successText);
                }
            },
            null
        ));
    }
}