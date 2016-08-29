using UnityEngine;
using System.Collections;

public class TestButton : MonoBehaviour {

    // Use this for initialization
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void CallEvent() {
        //GameManager.RandomEvent();
        GameManager.DoEvent(new EventRift());
    }

    public void StartBattle() {
        GameManager.StartBattle();
    }
}