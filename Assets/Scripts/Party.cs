using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Party : MonoBehaviour
{

    public List<GameObject> Characters;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    Camera.main.transform.SetParent(Characters[0].transform, false);
	}
}
