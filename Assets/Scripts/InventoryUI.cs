using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    private List<GameObject> _objs;

    void Awake()
    {
        _objs = new List<GameObject>();
    }

    public void Refresh()
    {
        ClearCards();
        var characters = GameManager.GetAliveCharacters();
        for (int i = 0; i < characters.Count; i++)
        {
            var child = transform.FindChildren("Child" + i);
            var position = child.FindChildren("CardPosition");
            var txt = characters[i].Name + " the ";
            foreach (var trait in characters[i].Traits)
            {
                txt += trait.Name + " | ";
            }
            child.FindChildren("Name").GetComponent<Text>().text =txt;
            var portrait = characters[i].DisplayPortrait();
            portrait.transform.SetParent(child.FindChildren("PortraitPosition"), false);
            _objs.Add(portrait);

            foreach (var item in characters[i].Inventory)
            {
                var a = item.DisplayItem();
                _objs.Add(a);
                CardPreview(a, item);
                a.transform.SetParent(position, false);
            }
        }

        var global = transform.FindChildren("Global").FindChildren("CardPosition");
        foreach (var item in GameManager.GlobalInventory)
        {
            var a = item.DisplayItem();
            _objs.Add(a);
            CardPreview(a, item);
            a.transform.SetParent(global, false);
        }
    }

    void CardPreview(GameObject go, Item item)
    {
        EventTrigger t1 = go.GetComponent<EventTrigger>();
        EventTrigger.Entry e1 = new EventTrigger.Entry();
        e1.eventID = EventTriggerType.PointerClick;

        e1.callback.AddListener((e) =>
        {
            var ip = transform.FindChildren("ItemPreview");
            ip.gameObject.SetActive(true);

            ip.FindChildren("Name").GetComponent<Text>().text = item.Name;
            ip.FindChildren("Description").GetComponent<Text>().text = item.Description;

            ip.FindChildren("Global").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (item.User != null)
                    item.User.MoveItemToGlobalInventory(item);
                CloseCardPreview();
            });

            ip.FindChildren("Cancel").GetComponent<Button>().onClick.AddListener(() => CloseCardPreview());

            ip.FindChildren("Discard").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (item.User != null)
                    item.User.RemoveItem(item);
                else
                    GameManager.RemoveItem(item);
                CloseCardPreview();
            });

            var c = GameManager.GetAliveCharacters();
            Button b = null;
            for (int i = 0; i < 4; i++)
            {
                b = ip.FindChildren("Child" + i).GetComponent<Button>();
                if (i < c.Count && c[i] != item.User)
                {
                    b.enabled = true;
                    b.GetComponentInChildren<Text>().text = "Go to " + c[i].Name + "'s inventory";
                    var chara = c[i];
                    b.onClick.AddListener(() =>
                    {
                        if (item.User == null)
                            GameManager.MoveItemToCharacterInventory(chara, item);
                        else
                        {
                            item.User.MoveItemToGlobalInventory(item);
                            GameManager.MoveItemToCharacterInventory(chara, item);
                        }
                        CloseCardPreview();
                    });
                } else
                {
                    b.enabled = false;
                    b.GetComponentInChildren<Text>().text = "";
                }
            }
        });

        t1.triggers.Add(e1);
    }

    void CloseCardPreview()
    {
        var ip = transform.FindChildren("ItemPreview");
        foreach (var button in ip.GetComponentsInChildren<Button>())
        {
            button.onClick.RemoveAllListeners();
        }
        ip.gameObject.SetActive(false);
        Refresh();
    }

    public void CloseInventory()
    {
        GameManager.ResumeTimer();
        this.gameObject.SetActive(false);
    }

    public void ClearCards()
    {
        foreach (var o in _objs)
        {
            Destroy(o);
        }
        _objs.Clear();
    }
}