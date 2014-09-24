using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gamecloud 
{

	public sealed class Gamecloud {

		private static volatile Gamecloud instance;
		private static object syncRoot = new object();

		private string SERVER_ADDRESS = "";
		private string authkey = "";

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static Gamecloud Instance
		{
			get
			{
				if (instance == null)
				{
					lock(syncRoot)
					{
						if (instance == null)
						{
							instance = new Gamecloud();
						}
					}
				}
				return instance;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Gamecloud.Gamecloud"/> class.
		/// </summary>
		/// <param name="serverAddress">The address of the server to use e.g. http://gamecloud.lut.fi:8888.</param>
		/// <param name="authkey">Your company's authentication key to use.</param>
		private Gamecloud(/*string serverAddress, string authkey*/)
		{
			this.SERVER_ADDRESS = "";
			this.authkey = "";
			// Initialize the starting values
			//this.SERVER_ADDRESS = serverAddress;
			//this.authkey = authkey;
		}

		/// <summary>
		/// Changes the authkey.
		/// </summary>
		/// <param name="authkey">Authkey.</param>
		public void ChangeAuthkey(string authkey)
		{
			this.authkey = authkey;
		}

		/// <summary>
		/// Changes the server address.
		/// </summary>
		/// <param name="serverAddress">Server address.</param>
		public void ChangeServerAddress(string serverAddress)
		{
			this.SERVER_ADDRESS = serverAddress;
		}

		/// <summary>
		/// Delegate for callbacks
		/// </summary>
		/// <param name="error">
		/// Error text received if something goes wrong
		/// </param>
		/// <param name="result">
		/// The resulting JSON Object (as a Hashtable)
		/// </param>
		public delegate void Callback(string error, Hashtable result);

		/// <summary>
		/// Delegate for the GET callbacks
		/// </summary>
		/// <param name="resultText">
		/// Resulting text from the GET
		/// </param>
		public delegate void GetCallback(string resultText);

		//The list of all dictionaries for storing different information
		public Dictionary<string, string> GetItemsDict = new Dictionary<string, string>();
		public Dictionary<string, string> GainItemsDict = new Dictionary<string, string>();
		public Dictionary<string, string> LoseItemsDict = new Dictionary<string, string>();
		public Dictionary<string, string> AskAchievementsDict = new Dictionary<string, string>();
		public Dictionary<string, string> GiveAchievementsDict = new Dictionary<string, string>();
		public Dictionary<string, string> TriggerEventsDict = new Dictionary<string, string>();
		public Dictionary<string, string> HasTriggeredEventsDict = new Dictionary<string, string>();



		
		// CallTypes, that can be used (in some situations, currently unused)
		public enum callTypes {
			GAIN_ITEM,
			ASK_ITEM,
			LOSE_ITEM,
			TRIGGER_EVENT,
			ASK_EVENT,
			GAIN_ACHIEVEMENT,
			ASK_ACHIEVEMENT
		};
		
		/// <summary>
		/// Creates the Gamecloud call for asking data in a proper JSON format
		/// </summary>
		/// <returns>The proper HashTable to send to Gamecloud</returns>
		/// <param name="hash">The hash for the given query</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="characterId">Character identifier.</param>
		protected Hashtable createCall(string hash, string playerId, string characterId) 
		{
			// Create the hash table
			Hashtable data = new Hashtable();
			// Create the callType
			data.Add("callType", "gameDataSave");
			// Add the authkey
			data.Add("authkey", authkey);
			// Add the hash
			data.Add("hash", hash);
			
			// If there is playerId
			if (playerId != null) 
			{
				// Add the player ID
				data.Add("playerId", playerId);
			}
			// If the characterId exists
			if (characterId != null) 
			{
				// Add the character ID
				data.Add("characterId", characterId);
			}
			
			// Then, return the data
			return data;
		}

		/// <summary>
		/// Creates the call for Asking if player has items
		/// </summary>
		/// <param name="hash">ASK hash</param>
		/// <param name="playerId">Player identifier</param>
		/// <param name="characterId">Character identifier</param>
		/// <param name="callback">The callback function for results.</param>
		public void askItems(string hash, string playerId, string characterId, Callback callback)
		{

			// Create the call
			Hashtable data = createCall(hash, playerId, characterId);
			// Send the data to Gamecloud
			SendData(data, callback);
		}

		/// <summary>
		/// Creates the call for giving a gainItem event for the player
		/// </summary>
		/// <param name="hash">gainItem hash</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="characterId">Character identifier.</param>
		/// <param name="callback">The callback function for results.</param>
		public void gainItem(string hash, string playerId, string characterId, Callback callback) 
		{
			// Create the call
			Hashtable data = createCall(hash, playerId, characterId);
			// Send the data to Gamecloud
			SendData(data, callback);
		}

		/// <summary>
		/// Creates the call for giving a loseItem event for the player
		/// </summary>
		/// <param name="hash">loseItem hash.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="characterId">Character identifier.</param>
		/// <param name="callback">The callback function for results.</param>
		public void loseItem(string hash, string playerId, string characterId, Callback callback)
		{
			// Create the call
			Hashtable data = createCall(hash, playerId, characterId);
			// Send the data to Gamecloud
			SendData(data, callback);
		}

		/// <summary>
		/// Creates the triggerEvent call for gamecloud
		/// </summary>
		/// <param name="hash">triggerEvent hash.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">The callback function for results.</param>
		public void triggerEvent(string hash, string playerId, Callback callback)
		{
			// Create the call, no need for characterId so it is null
			Hashtable data = createCall(hash, playerId, null);
			// Send the data to Gamecloud
			SendData(data, callback);
		}

		/// <summary>
		/// Asks if the player in question has received the event in question
		/// </summary>
		/// <param name="hash">askEvent hash.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">The callback function for results.</param>
		public void askEvent(string hash, string playerId, Callback callback)
		{
			// Create the call, no need for characterId so it is null
			Hashtable data = createCall(hash, playerId, null);
			// Send the data to Gamecloud
			SendData(data, callback);
		}

		/// <summary>
		/// Creates the query for giving a gainAchievement event for the player
		/// </summary>
		/// <param name="hash">GainAchievememt hash.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">The callback function for results.</param>
		public void gainAchievement(string hash, string playerId, Callback callback) 
		{
			// Create the call, no need for characterId so it is null
			Hashtable data = createCall(hash, playerId, null);
			// Send the data to Gamecloud
			SendData(data, callback);
		}

		/// <summary>
		/// Asks if the player has received the Achievement in question
		/// </summary>
		/// <param name="hash">ASK Achievement hash.</param>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">The callback function for results.</param>
		public void askAchievement(string hash, string playerId, Callback callback)
		{
			// Create the call, no need for characterId so it is null
			Hashtable data = createCall(hash, playerId, null);
			// Send the data to Gamecloud
			SendData(data, callback);
		}

		/// <summary>
		/// Authenticate the specified username, password, callback and synchronous.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="synchronous">If set to <c>true</c> synchronous.</param>
		public void Authenticate(string username, string password, Callback callback, bool synchronous=false)
		{
			// Create the Login JSOn
			Hashtable loginJson = new Hashtable();
			loginJson.Add("callType", "authenticate");
			loginJson.Add("username", username);
			loginJson.Add("password", password);

			// Then, send it to the server
			SendData(loginJson, callback, synchronous);

		}

		/// <summary>
		/// Gets the games of the user from the server.
		/// </summary>
		/// <param name="creator">Creator.</param>
		/// <param name="authkey">Authkey.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="synchronous">If set to <c>true</c> synchronous.</param>
		public void GetGames(string creator, string authkey, Callback callback, bool synchronous=false)
		{
			// Create a GetGames JSON
			Hashtable getGamesJson = new Hashtable();
			getGamesJson.Add("callType", "getGames");
			getGamesJson.Add("authkey", authkey);
			getGamesJson.Add("creator", creator);

			// Then, make the call
			SendData(getGamesJson, callback, synchronous);
		}

		/// <summary>
		/// Gets the hashes of entry.
		/// </summary>
		/// <param name="authkey">Authkey.</param>
		/// <param name="entryName">Entry name.</param>
		/// <param name="type">Type.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="synchrounous">If set to <c>true</c> synchrounous.</param>
		public void GetHashesOfEntry(string authkey, string entryName, string type, Callback callback, bool synchrounous=false)
		{
			// Create the JSON for getting the hashes
			Hashtable json = new Hashtable();
			json.Add("callType", "getHashes");
			json.Add("authkey", authkey);
			json.Add("name", "ex:" + entryName);
			json.Add("type", type);

			// Then, send it forwards
			SendData(json, callback, synchrounous);
		}

		/// <summary>
		/// Gets the ontologies by game.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="game">Game.</param>
		/// <param name="authkey">Authkey.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="synchronous">If set to <c>true</c> synchronous.</param>
		public void GetOntologiesByGame(string type, string game, string authkey, Callback callback, bool synchronous=false)
		{
			// Create the JSON
			Hashtable json = new Hashtable();

			// Get the types here
			if (type == "Achievement")
			{
				json.Add("callType", "getAchievementsByGame");
			}
			else if (type == "Event")
			{
				json.Add("callType", "getEventsByGame");
			}
			else if (type == "Item")
			{
				json.Add("callType", "getItemsByGame");
			}

			// Next, add the authkey
			json.Add("authkey", authkey);
			// Then, add the creator id
			json.Add("game", "ex:" + game);

			Debug.Log("Sending");
			Debug.Log(JSON.JsonEncode(json));

			// Then, send for processing
			SendData(json, callback, synchronous);
		}

		/// <summary>
		/// Creates the ontology entry.
		/// </summary>
		/// <param name="json">Json.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="synchronous">If set to <c>true</c> synchronous.</param>
		public void CreateOntologyEntry(Hashtable json, Callback callback, bool synchronous=false)
		{
			// Just pass the call onwards
			SendData(json, callback, synchronous);
		}

		/// <summary>
		/// Gets from server.
		/// </summary>
		/// <param name="addressWithQuery">Address with query.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="synchronous">If set to <c>true</c> synchronous.</param>
		public void GetFromServer(string addressWithQuery, GetCallback callback, bool synchronous=false)
		{
			HTTP.Request theRequest = new HTTP.Request("get", addressWithQuery);

			// If synchronous method is requested
			if (synchronous)
			{
				// Add field making it synchronous
				theRequest.synchronous = true;
			}
			// Once we get the result
			theRequest.Send(( request) => {

				// First, check the type of the response
				CheckForStatusCode(request.response);
				// Just return the result text to callback
				callback(request.response.Text);
			});
		}

		/// <summary>
		/// Sends the data in proper JSON format to the Gamecloud
		/// </summary>
		/// <param name="data">
		/// The properly formated data, done by using the createCall function with proper information.
		/// </param>
		/// <param name="callback">The callback function for results.</param>
		/// <param name="synchronous">Whether to do synchronous (for EditorUI) or async call</param>
		protected void SendData(Hashtable data, Callback callback, bool synchronous=false) 
		{
			Debug.Log(JSON.JsonEncode(data));

			// When you pass a Hashtable as the third argument, we assume you want it send as JSON-encoded
			// data.  We'll encode it to JSON for you and set the Content-Type header to application/json
			HTTP.Request theRequest = new HTTP.Request( "post", SERVER_ADDRESS, data );

			// If synchronous method has been requested
			if(synchronous)
			{
				// Add a field making it synchronous
				theRequest.synchronous = true;
			}
			theRequest.Send( ( request ) => {

				// First, check the type of the response
				CheckForStatusCode(request.response);

				// we provide Object and Array convenience methods that attempt to parse the response as JSON
				// if the response cannot be parsed, we will return null
				// note that if you want to send json that isn't either an object ({...}) or an array ([...])
				// that you should use JSON.JsonDecode directly on the response.Text, Object and Array are
				// only provided for convenience
				Hashtable result = request.response.Object;
				if ( result == null )
				{
					callback("Could not parse JSON response!", null);
					return;
				}

				// Things succeeded, send the result
				callback(null, request.response.Object);
				
			});
		}

		/// <summary>
		/// Function for Checking status codes of the received messages
		/// </summary>
		/// <param name="response">Response.</param>
		private void CheckForStatusCode(HTTP.Response response)
		{
			switch(response.status) 
			{
			case 200:
				Debug.Log("200 - Message sent & received succesfully");
				break;
			case 500:
				Debug.LogError("500 - Internal Server Error (something in your JSON was malformed?");
				break;
			default:
				Debug.LogError("Got number, that we have not yet defined in the checker: " + response.status.ToString());
				break;
			}
		}
	

	}


}
