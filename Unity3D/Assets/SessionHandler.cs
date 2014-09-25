using UnityEngine;
using System.Collections;
using System;

public class SessionHandler : MonoBehaviour {

	public string GamecloudAddress;

	private Gamecloud.Gamecloud gamecloud = Gamecloud.Gamecloud.Instance;
	public string SessionId;
	public string PlayerId;

	// Use this for initialization
	void Start () 
	{
		// Set the gamecloud address to be correct one
		this.gamecloud.ChangeServerAddress(GamecloudAddress);
		// Start the session
		StartSession();
		// Create a fake player ID, if one doesn't exist
		if(this.PlayerId == null)
		{
			this.PlayerId = "ex:testPlayer";
		}
	}

	/// <summary>
	/// Sets the gamecloud address.
	/// </summary>
	/// <param name="address">Address.</param>
	public void SetGamecloudAddress(string address)
	{
		GamecloudAddress = address;
	}

	public void SetPlayerId(string playerId)
	{
		this.PlayerId = playerId;
	}

	public string GetPlayerId()
	{
		return this.PlayerId;
	}

	public string GetSessionId()
	{
		// Return the session ID
		return this.SessionId;
	}

	public void StartSession()
	{
		// Create a unique session id
		this.SessionId = this.CreateSessionId();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Creates the session identifier.
	/// </summary>
	/// <returns>The session identifier string.</returns>
	private string CreateSessionId()
	{
		// Create the random session
		string sessionId = "Session" + GetISOTimeInUTC() + KeyGenerator.GetUniqueKey(10);
		// And return the results
		return sessionId;
	}
	
	/// <summary>
	/// Gets the ISO time in UT.
	/// </summary>
	/// <returns>The ISO time in UT.</returns>
	private string GetISOTimeInUTC()
	{
		return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
	}
}
