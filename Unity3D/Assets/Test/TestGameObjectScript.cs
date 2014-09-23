using UnityEngine;
using System.Collections;

public class TestGameObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AskFunction(string error, Hashtable result)
	{
		Debug.Log ("Ask Function() called!");
	}

	public void GainFunction(string error, Hashtable result)
	{
		Debug.Log ("Gain Function() called!");
	}
}
