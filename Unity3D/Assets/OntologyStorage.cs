using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OntologyStorage : MonoBehaviour {

	/// Types of different storages existing
	public enum OntologyStorageType
	{
		GetItems,
		GainItems,
		LoseItems,
		AskAchievements,
		GiveAchievements,
		TriggerEvents,
		HasTriggeredEvents
	}

	//The list of all dictionaries for storing different information
	public Dictionary<string, string> GetItemsDict = new Dictionary<string, string>();
	public Dictionary<string, string> GainItemsDict = new Dictionary<string, string>();
	public Dictionary<string, string> LoseItemsDict = new Dictionary<string, string>();
	public Dictionary<string, string> AskAchievementsDict = new Dictionary<string, string>();
	public Dictionary<string, string> GiveAchievementsDict = new Dictionary<string, string>();
	public Dictionary<string, string> TriggerEventsDict = new Dictionary<string, string>();
	public Dictionary<string, string> HasTriggeredEventsDict = new Dictionary<string, string>();

	/// <summary>
	/// Adds to storage.
	/// </summary>
	/// <param name="storageType">Storage type.</param>
	/// <param name="name">Name.</param>
	/// <param name="hash">Hash.</param>
	public void AddToStorage(OntologyStorageType storageType, string name, string hash)
	{
		GetStorage(storageType).Add(name, hash);
		/*
		switch(storageType)
		{
		case OntologyStorageType.GetItems:
			GetItemsDict.Add(name, hash);
			break;
		case OntologyStorageType.GainItems:
			GainItemsDict.Add(name, hash);
			break;
		case OntologyStorageType.LoseItems:
			LoseItemsDict.Add(name, hash);
			break;
		case OntologyStorageType.AskAchievements:
			AskAchievementsDict.Add(name, hash);
			break;
		case OntologyStorageType.GiveAchievements:
			GiveAchievementsDict.Add(name, hash);
			break;
		case OntologyStorageType.TriggerEvents:
			TriggerEventsDict.Add(name, hash);
			break;
		case OntologyStorageType.HasTriggeredEvents:
			HasTriggeredEventsDict.Add(name, hash);
			break;
		default:
			throw new UnityException("OntologyStorageType was not recognized: " + storageType.ToString());
			break;
		} // End of switch

		*/
	}

	/// <summary>
	/// Gets the hash from storage.
	/// </summary>
	/// <returns>The hash from storage.</returns>
	/// <param name="storageType">Storage type.</param>
	/// <param name="name">Name.</param>
	public string GetHashFromStorage(OntologyStorageType storageType, string name)
	{
		return GetStorage(storageType)[name];
	}

	/// <summary>
	/// Gets the storage.
	/// </summary>
	/// <returns>The storage.</returns>
	/// <param name="storageType">Storage type.</param>
	public Dictionary<string, string> GetStorage(OntologyStorageType storageType)
	{
		switch(storageType)
		{
		case OntologyStorageType.GetItems:
			return GetItemsDict;
		case OntologyStorageType.GainItems:
			return GainItemsDict;
		case OntologyStorageType.LoseItems:
			return LoseItemsDict;
		case OntologyStorageType.AskAchievements:
			return AskAchievementsDict;
		case OntologyStorageType.GiveAchievements:
			return GiveAchievementsDict;
		case OntologyStorageType.TriggerEvents:
			return TriggerEventsDict;
		case OntologyStorageType.HasTriggeredEvents:
			return HasTriggeredEventsDict;
		default:
			throw new UnityException("OntologyStorageType was not recognized: " + storageType.ToString());
		} // End of switch
	}
}
