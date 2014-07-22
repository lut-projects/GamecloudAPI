using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HTTP;
using Gamecloud;

public class Gamecloud_example : MonoBehaviour {

	private static string DEV_ADDR = "http://";

	//Gamecloud.Gamecloud gameCloud = new Gamecloud.Gamecloud(DEV_ADDR, "NO_AUTH");
	Gamecloud.Gamecloud gamecloud = new Gamecloud.Gamecloud(DEV_ADDR, "as");

	// Use this for initialization
	void Start () 
	{
		// Add the entries here
		gamecloud.AskAchievementsDict.Add("LutTestAchievement1", "gfno7um8erkymn29");
		gamecloud.GiveAchievementsDict.Add("LutTestAchievement1", "nn0mklr14z9i19k9");

		Debug.Log("Started");

		//gamecloud.gainAchievement(gamecloud.GiveAchievementsDict["LutTestAchievement1"], "TestPlayer");
		gamecloud.askAchievement(gamecloud.AskAchievementsDict["LutTestAchievement1"], "TestPlayer", new MethodClass().ExampleMethod);

	}



	public class MethodClass 
	{
		public void ExampleMethod(string error, string message)
		{
			if (error)
			{
				Debug.LogError(error);
			}
			// Okay, no problems found, just continue :-D
			Debug.Log (message);
		}

		public void ASecondMethod(string error, string message)
		{
			if (error)
			{
				Debug.LogError(error);
			}
			// MyFunc() <<-- Do here your stuff
		}

	}
}
