using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HTTP;
using Gamecloud;

public class Gamecloud_example : MonoBehaviour {

	private static string DEV_ADDR = "";

	//Gamecloud.Gamecloud gameCloud = new Gamecloud.Gamecloud(DEV_ADDR, "NO_AUTH");
	Gamecloud.Gamecloud gamecloud = new Gamecloud.Gamecloud(DEV_ADDR, "as");

	// Use this for initialization
	void Start () 
	{
		// Add the entries here
		gamecloud.AskAchievementsDict.Add("AWorkingAchievement", "wwccpdemnh0ssjor");
		gamecloud.GiveAchievementsDict.Add("AWorkingAchievement", "s33qgs2cs2botj4i");

		Debug.Log("Started");

		gamecloud.gainAchievement(gamecloud.GiveAchievementsDict["AWorkingAchievement"], "TestPlayer");
		gamecloud.askAchievement(gamecloud.AskAchievementsDict["AWorkingAchievement"], "TestPlayer");

	}


}
