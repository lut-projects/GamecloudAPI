using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HTTP;
using Gamecloud;

public class Gamecloud_example : MonoBehaviour {

	private static string DEV_ADDR = "http://";

	//Gamecloud.Gamecloud gameCloud = new Gamecloud.Gamecloud(DEV_ADDR, "NO_AUTH");
	Gamecloud.Gamecloud gamecloud = Gamecloud.Gamecloud.Instance; // new Gamecloud.Gamecloud(DEV_ADDR, "as");

	// Use this for initialization
	void Start () 
	{
		// Set the gamecloud address and authkey
		gamecloud.ChangeServerAddress(DEV_ADDR);
		gamecloud.ChangeAuthkey("NO_AUTH");

		// Add the entries here
		gamecloud.AskAchievementsDict.Add("LutTestAchievement1", "gfno7um8erkymn29");
		gamecloud.GiveAchievementsDict.Add("LutTestAchievement1", "nn0mklr14z9i19k9");

		Debug.Log("Started");

		//gamecloud.gainAchievement(gamecloud.GiveAchievementsDict["LutTestAchievement1"], "TestPlayer");
		// Example of querying for player having an achievement WITH callback
		gamecloud.askAchievement(gamecloud.AskAchievementsDict["LutTestAchievement1"], "TestPlayer", new MethodClass().ExampleMethod);

	}


	/// <summary>
	/// An example of method class that is used to instatiate callbacks.
	/// Create your own methods here and instantiate them, whenever you want to add your own
	/// functionality to the system.
	/// You should use JSON.JsonEncode to read the results as a JSON string
	/// </summary>
	/// 
	///
	public class MethodClass 
	{
		/// <summary>
		/// Example of using the methods
		/// </summary>
		/// <param name="error">Error message that is received if something goes wrong.</param>
		/// <param name="result">The result as a JSON object when things work out.</param>
		public void ExampleMethod(string error, Hashtable result)
		{
			if (error != null)
			{
				Debug.LogError(error);
			}

			// To encode the message into a JSON string
			Debug.Log (JSON.JsonEncode(result));
			// And to read a specific JSON value from the received JSON Object (=Hashtable)
			Debug.Log ("Amount of entries found:" + result["count"].ToString());

		}

		public void ASecondMethod(string error, Hashtable result)
		{
			if (error != null)
			{
				Debug.LogError(error);
			}
			// MyFunc() <<-- Do here your stuff
		}

	}
}
