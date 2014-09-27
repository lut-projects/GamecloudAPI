using UnityEngine;
using System.Collections;

public class TestGameObjectScript : MonoBehaviour {

    private bool tested = false;

	// Use this for initialization
	void Start () {


	}

    void Awake()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
	    if(tested == false)
        {
            TestAsk();
            tested = true;
        }
	}

	public void AskFunction(Hashtable result)
	{
		Debug.Log ("Ask Function() called!");
		Debug.Log (JSON.JsonEncode(result).ToString());
	}

	public void GainFunction(Hashtable result)
	{
		Debug.Log ("Gain Function() called!");
		Debug.Log (JSON.JsonEncode(result).ToString());
	}
	public void LoseFunction(Hashtable result)
	{
		Debug.Log("Lose Function() called!");
		Debug.Log (JSON.JsonEncode(result).ToString());
	}


	public void TestAsk()
	{
        OntologyObject script = GameObject.Find("GainMaailmanMahtavinHienoSaavutusAchievement").GetComponent<OntologyObject>();
        script.CallAskFunction();
	}

	public void TestGain()
	{
        OntologyObject script = GameObject.Find("GainMaailmanMahtavinHienoSaavutusAchievement").GetComponent<OntologyObject>();
        script.CallGainFunction();
	}
}
