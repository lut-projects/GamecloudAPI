using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gamecloud 
{

	public class Gamecloud {

	
		private string SERVER_ADDRESS = "";
		private string authkey = "";

		/// <summary>
		/// Initializes a new instance of the <see cref="Gamecloud.Gamecloud"/> class.
		/// </summary>
		/// <param name="serverAddress">The address of the server to use e.g. http://gamecloud.lut.fi:8888.</param>
		/// <param name="authkey">Your company's authentication key to use.</param>
		public Gamecloud(string serverAddress, string authkey)
		{
			// Initialize the starting values
			this.SERVER_ADDRESS = serverAddress;
			this.authkey = authkey;
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
		/// Sends the data in proper JSON format to the Gamecloud
		/// </summary>
		/// <param name="data">
		/// The properly formated data, done by using the createCall function with proper information.
		/// </param>
		/// <param name="callback">The callback function for results.</param>
		protected void SendData(Hashtable data, Callback callback) 
		{
			// When you pass a Hashtable as the third argument, we assume you want it send as JSON-encoded
			// data.  We'll encode it to JSON for you and set the Content-Type header to application/json
			HTTP.Request theRequest = new HTTP.Request( "post", SERVER_ADDRESS, data );
			theRequest.Send( ( request ) => {
				
				// we provide Object and Array convenience methods that attempt to parse the response as JSON
				// if the response cannot be parsed, we will return null
				// note that if you want to send json that isn't either an object ({...}) or an array ([...])
				// that you should use JSON.JsonDecode directly on the response.Text, Object and Array are
				// only provided for convenience
				Hashtable result = request.response.Object;
				if ( result == null )
				{
					callback("Count not parse JSON response!", null);
					return;
				}

				// Things succeeded, send the result
				callback(null, request.response.Object);
				
			});
		}
	

	}


}
