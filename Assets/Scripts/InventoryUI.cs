using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
            child.FindChildren("Name").GetComponent<Text>().text = characters[i].Name;
            characters[i].DisplayPortrait().transform.SetParent(child.FindChildren("PortraitPosition"), false);

            foreach (var item in characters[i].Inventory)
            {
                item.DisplayItem().transform.SetParent(position, false);
            }
        }

        var global = transform.FindChildren("Global").FindChildren("CardPosition");
        foreach (var item in GameManager.GlobalInventory)
        {
            item.DisplayItem().transform.SetParent(global, false);
        }
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