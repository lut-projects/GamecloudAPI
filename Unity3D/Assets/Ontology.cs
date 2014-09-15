using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamecloud;



public class Ontology : MonoBehaviour 
{
	public Types Type;
	public string ActionType;
	public string ActionName;
	public string ActionHash;
	public string Hash;

    public List<string> Listing;
    OntologyStorage ont = new OntologyStorage();

    public void RefreshListing()
    {
        // Empty the listing
        Listing = new List<string>();      

        ont.AddToStorage(OntologyStorage.OntologyStorageType.AskAchievements, "AskTestAchievement", "asigng");
        ont.AddToStorage(OntologyStorage.OntologyStorageType.AskAchievements, "ASecondTester", "oinhfdoihfd");

        // Add to the Listing all the achievements
        foreach (string achievement in ont.GetStorage(OntologyStorage.OntologyStorageType.AskAchievements).Keys)
        {
            // Do stuff
            Listing.Add(achievement);
        }
        //Listing.Add(ont.GetHashFromStorage(OntologyStorage.OntologyStorageType.AskAchievements, "AskTestAchievement"));



    }

    public void SetActionName(string name)
    {
        // Set the name
        this.ActionName = name;
        // When setting the name, retrieve the proper hash as well
        RetrieveActionHash();
    }

    private void RetrieveActionHash()
    {
        try
        {
            this.ActionHash = ont.GetHashFromStorage(OntologyStorage.OntologyStorageType.AskAchievements, this.ActionName);
        }
        catch (KeyNotFoundException e)
        {
            // Just ignore;
        }
    }


}
