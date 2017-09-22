using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		Client.Instance.CreateConnection("127.0.0.1", 10001);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
