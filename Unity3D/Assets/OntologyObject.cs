using UnityEngine;
using System.Collections;
using System.Reflection;
using Gamecloud;

public class OntologyObject : MonoBehaviour {

	public string Name;
	public string GainHash = "";
	public string LoseHash = "";
	public string AskHash = "";
	public Types Type;

	public GameObject gameObject;
	public GameObject askGameObject;
	public GameObject gainGameObject;
	public GameObject loseGameObject;

	public string askCallback;
	public string gainCallback;
	public string loseCallback;

	private Gamecloud.Gamecloud gamecloud = Gamecloud.Gamecloud.Instance;

	private SessionHandler sessionHandler;

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

		Debug.Log("CallAskFunction() - The type is: " + this.Type.ToString());

		// Switch according to the type
		switch(this.Type)
		{
		case Types.Achievement:
			gamecloud.askAchievement(AskHash, this.sessionHandler.GetPlayerId(), AskCallback);
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
		Debug.Log("CallGainFunction() - The type is: " + this.Type.ToString());
		
		// Switch according to the type
		switch(this.Type)
		{
		case Types.Achievement:
			gamecloud.gainAchievement(GainHash, this.sessionHandler.GetPlayerId(), this.sessionHandler.GetSessionId(), GainCallback);
			break;
		case Types.Event:
			break;
		case Types.Item:
			break;
		default:
			throw new UnityException("CallAskFunction() - Got a strange type that should not exist");
		}
	}

	public void CallLoseFunction()
	{
		Debug.Log("CallLoseFunction() - The type is: " + this.Type.ToString());
		
		// Switch according to the type
		switch(this.Type)
		{
		case Types.Achievement:
			throw new UnityException("CallLoseFunction() called with Achievement. This should not be possible!");
		case Types.Event:
			break;
		case Types.Item:
			break;
		default:
			throw new UnityException("CallAskFunction() - Got a strange type that should not exist");
		}
	}

	private void AskCallback(string error, Hashtable result)
	{
		Debug.Log("Trying to run the selected callback: " + askCallback);
		askGameObject.SendMessage(askCallback, result);
	}

	private void GainCallback(string error, Hashtable result)
	{
		if (error != null)
			Debug.LogError ("Error: " + error);

		Debug.Log("Trying to run the selected callback: " + gainCallback);
		gainGameObject.SendMessage(gainCallback, result);
	}

	private void LoseCallback(string error, Hashtable result)
	{
		Debug.Log("Trying to run the selected callback: " + loseCallback);
		loseGameObject.SendMessage(loseCallback, result);
	}

	// Use this for initialization
	void Start () 
	{
		// Find the session handler
		this.sessionHandler = GameObject.Find("GamecloudManager").GetComponent<SessionHandler>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
