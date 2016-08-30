using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventTreasure : EventBase
{

    public override void InitializeEvent()
    {
        Text = "You find a treasure chest.";

        _actions = new List<EventData>();

        _actions.Add(new EventData(
            "Character",
            "Open it",
            (c) =>
            {
                Item item;

                List<string> chest = new List<string>() {
                    "Gladius",
                    "Rlank",
                    "Rope",
                    "FluffyBoots",
                    "JadeAmulet",
                    "OldDagger",
                    "Katana",
                    "Axe",
                    "Saber",
                    "CrystalKnife",
                    "StaffOfMisery",
                    "Shield",
                    "Spade",
                    "Twig",
                    "ThrowKnives",
                    "Bardiche",
                    "Hammer",
                    "HSword",
                    "Guns"
                };

                int rand = UnityEngine.Random.Range(0, chest.Count);

                c.AddItem(chest[rand]);
                GameManager.DisplayEventResult("You find a shiny <color=yellow>" + chest[rand] + "</color>");
            },
            null
        ));
    }
}
