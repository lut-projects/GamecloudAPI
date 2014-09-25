using UnityEngine;
using System.Collections;
using System.Reflection;
using Gamecloud;

public class OntologyObject : MonoBehaviour {

	public string Name;
	public string GainHash;
	public string LoseHash;
	public string AskHash;
	public Types Type;

	public GameObject gameObject;
	public GameObject askGameObject;
	public GameObject gainGameObject;
	public GameObject loseGameObject;

	public string askCallback;
	public string gainCallback;
	public string loseCallback;

	private Gamecloud.Gamecloud gamecloud = Gamecloud.Gamecloud.Instance;

	public void DefineOntology(Types type, string name, string askHash, string gainHash, string loseHash)
	{
		this.Type = type;
		this.Name = name;
		this.AskHash = askHash;
		this.GainHash = gainHash;
		this.LoseHash = loseHash;
	}
	
	public void CallAskFunction()
	{
		// Switch according to the type
		switch(Type)
		{
		case Types.Achievement:
			//gamecloud.askAchievement(AskHash, "ex:testPlayer44", AskCallback);
			break;
		case Types.Event:
			break;
		case Types.Item:
			break;
		default:
			throw new UnityException("CallAskFunction() - Got a strange type that should not exist");
		}

	}

	public void CallGainFunction()
	{

	}

	public void CallLoseFunction()
	{

	}

	private void AskCallback(string error, Hashtable result)
	{
		Debug.Log("Trying to run the selected callback: " + askCallback);
		askGameObject.SendMessage(askCallback, new Hashtable());
	}

	private void GainCallback(string error, Hashtable result)
	{
		Debug.Log("Trying to run the selected callback: " + gainCallback);
		askGameObject.SendMessage(gainCallback, new Hashtable());
	}

	private void LoseCallback(string error, Hashtable result)
	{
		Debug.Log("Trying to run the selected callback: " + loseCallback);
		askGameObject.SendMessage(loseCallback, new Hashtable());
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
