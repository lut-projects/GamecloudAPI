using UnityEngine;
using System.Collections;
using UnityEditor;

public class AskFromOtherGame : MonoBehaviour {

    public string AskHash;
    public Types Type;
    public GameObject askGameObject;
    public string askCallbackFunctionName;

    private Gamecloud.Gamecloud gamecloud = Gamecloud.Gamecloud.Instance;
    private SessionHandler sessionHandler;

    /// <summary>
    /// The Callback function to use
    /// </summary>
    /// <param name="error">Error message if something goes wrong</param>
    /// <param name="result">The resulting JSON as a HashTable</param>
    private void AskCallback(string error, Hashtable result)
    {
        Debug.Log("Trying to run the selected callback: " + askCallbackFunctionName);
        askGameObject.SendMessage(askCallbackFunctionName, result);
    }

    /// <summary>
    /// Functions Calling the ASK
    /// </summary>
    public void CallAsk()
    {
        // Check that every value is valid
        CheckValidity();

        

        // Switch according to the type
        switch (this.Type)
        {
            case Types.Achievement:
                gamecloud.askAchievement(AskHash, this.GetSessionHandler().GetPlayerId(), AskCallback);
                break;
            case Types.Event:
                gamecloud.askEvent(AskHash, this.GetSessionHandler().GetPlayerId(), AskCallback);
                break;
            case Types.Item:
                gamecloud.askItems(AskHash, GetSessionHandler().GetPlayerId(), GetSessionHandler().GetCharacterId(), AskCallback);
                break;
            default:
                throw new UnityException("CallAskFunction() - Got a strange type that should not exist");
        }
    }

    /// <summary>
    /// Gets the Session Handler
    /// </summary>
    /// <returns></returns>
    private SessionHandler GetSessionHandler()
    {
        //Debug.Log("GetSessionHandler() - Session is currently: " + this.sessionHandler.ToString());

        // Check, if this.sessionHandler is empty
        if (this.sessionHandler == null)
        {
            Debug.Log("SessionHandler is currently null");
            // Then, find it
            this.sessionHandler = GameObject.Find("GamecloudManager").GetComponent<SessionHandler>();
            Debug.Log("SessionHandler found: " + this.sessionHandler);
        }

        // And return the session handler
        return this.sessionHandler;
    }

    /// <summary>
    /// Checks for function validity
    /// </summary>
    private void CheckValidity()
    {
        // Check that neither AskHash or Type are null
        if (AskHash == null)
        {
            throw new UnityException("AskFromOtherGame.CallAsk() - AskHash is null!");
        }
        if (AskHash == "")
        {
            throw new UnityException("AskFromOtherGame.CallAsk() - AskHash is empty!");
        }
        if (Type == null)
        {
            throw new UnityException("AskFromOtherGame.CallAsk() -Type of the ASK is not defined!");
        }
    }
}
