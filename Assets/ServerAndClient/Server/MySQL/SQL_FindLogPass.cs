using UnityEngine;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

//Associated with the database and retrieves data of players, as well as putting a flag "online" if the account is used

public class SQL_FindLogPass
{
	public delegate void ServerReply ();

	public static event ServerReply OnNoAccount;
	public static event ServerReply OnCharDataSet;
	public static event ServerReply OnNoChar;

	private static MySqlCommand Linq = SQL_sqlConnect.Linq;

	public static List<Data_PlayerFile_Sr> players = new List<Data_PlayerFile_Sr> ();

	public static Data_PlayerFile_Sr player;

	public static void CheckLP ()
	{
		Linq.CommandText = "SELECT id FROM accountlist WHERE AccountName='" + Networking_OnConnect.log + "' AND PasswordAc='" + Networking_OnConnect.pass + "'";
		MySqlDataReader Reader = Linq.ExecuteReader ();
		try {
			Reader.Read ();
			string AccId = Reader.GetString (0);
			Reader.Close ();
			if (!Networking_OnConnect.onlineAcc.Contains (int.Parse (AccId))) {
				Networking_OnConnect.AddOnlineAccount (int.Parse (AccId));
				CharDataGet (AccId);
				Networking_OnConnect.DataSet ();
			} else if (SQL_sqlConnect.testMode) {
				CharDataGet (AccId);
			} else {
				Networking_OnConnect.AccountUsed ();
			}
		} catch (MySqlException ex) {
			Reader.Close ();
			Debug.Log (ex.ErrorCode + ex.Message);
			Networking_OnConnect.NoAccount ();
		}
		Reader.Close ();
	}

	public static void CharDataGet (string AccId)
	{
		Linq.CommandText = "SELECT PlayerName, scene_ID, MaxHP, PlayerScores, char_id FROM charecter WHERE account_id = '" + AccId + "'";
		MySqlDataReader Reader = Linq.ExecuteReader ();
		try {

			while (Reader.Read ()) {
                int i = 0;
				player = ScriptableObject.CreateInstance<Data_PlayerFile_Sr> ();
				player.nick = Reader.GetString (i++);
				player.sceneID = int.Parse (Reader.GetString (i++));
				player.HPMax = int.Parse (Reader.GetString (i++));
				player.PlayerScores = int.Parse (Reader.GetString (i++));
				player.charID = int.Parse (Reader.GetString (i++));
				try {
					players.Add (player);
				} catch (InvalidExpressionException ex) {
					Debug.Log (ex.HelpLink + ex.Message + ex.Source);
				}
			}
			Reader.Close ();
		} catch (MySqlException ex) {
			Reader.Close ();
			Debug.Log (ex.ErrorCode + ex.Message);
			Networking_OnConnect.NoCharecter ();
		}
	}

	public static void ClearData ()
	{
		players.Clear ();
		player = null;
	}
}
