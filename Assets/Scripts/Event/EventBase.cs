using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public struct EventResult
{
    public string Text;
    public Item EventItem;
    public Action<Character> OnCharacter;
    public Action<Item> OnItem;

    public EventResult(string text, Action<Character> onCharacter, Action<Item> onItem, Item item = null)
    {
        Text = text;
        OnCharacter = onCharacter;
        OnItem = onItem;
        EventItem = item;
    }
};

public class EventBase : MonoBehaviour
{

    protected class EventData
    {
        public string Tag;
        public string Text;
        public Action<Character> OnCharacter;
        public Action<Item> OnItem;

        public EventData(string tag, string text, Action<Character> onCharacter, Action<Item> onItem)
        {
            Tag = tag;
            Text = text;
            OnCharacter = onCharacter;
            OnItem = onItem;
        }
    };

    public string Text;

    protected  List<EventData> _actions;

    public virtual void InitializeEvent()
    {

    }

    public List<EventResult> GetPossibleActions(Character character)
    {
        var results = new List<EventResult>();

        foreach (var action in _actions)
        {
            if (action.Tag == "Character")
            {
                results.Add(new EventResult(action.Text, action.OnCharacter, null));
            }
            else
            {
                foreach (var item in character.Inventory)
                {
                    foreach(var tag in item.Tags)
                    {
                        if (tag == action.Tag)
                        {
                            var i = item;
                            results.Add(new EventResult(action.Text + " <color=yellow>" + i.Name + "</color>", null, action.OnItem, i));
                            break;
                        }
                    }
                }
            }
        }

        return results;
    }

}
