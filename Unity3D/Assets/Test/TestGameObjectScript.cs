using UnityEngine;
using System.Collections;

public class TestGameObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TestAsk();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AskFunction(Hashtable result)
	{
		Debug.Log ("Ask Function() called!");
	}

	public void GainFunction(Hashtable result)
	{
		Debug.Log ("Gain Function() called!");
	}
	public void LoseFunction(Hashtable result)
	{
		Debug.Log("Lose Function() called!");
	}

	public void TestAsk()
	{
		OntologyObject script = GameObject.Find("GainFirstUnityTestAchievement").GetComponent<OntologyObject>();
		script.CallAskFunction();
	}
}
