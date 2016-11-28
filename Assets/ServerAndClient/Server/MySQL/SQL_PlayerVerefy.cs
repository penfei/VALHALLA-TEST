using UnityEngine;
using MySql.Data.MySqlClient;

public class SQL_PlayerVerefy
{
	public static int sceneID = 0;
	public static Data_PlayerFile_Sr player;
	private static string AcId;
	private static string playername;


	private static MySqlCommand Linq = SQL_sqlConnect.Linq;

	//Check player data, if data confirm, send player data and return true
	static public bool CheckLP ()
	{
		bool check = false;
		Linq.CommandText = "SELECT id, AccountName, PasswordAc FROM accountlist WHERE AccountName='" + Networking_OnPlayerSelect.log + "' AND PasswordAc='" + Networking_OnPlayerSelect.pass + "'";
		MySqlDataReader Reader = Linq.ExecuteReader ();
		try {
			Reader.Read ();
			AcId = Reader.GetString (0);
			Reader.Close ();

			Linq.CommandText = "SELECT PlayerName, scene_ID, MaxHP, PlayerScores, x, y, z, zombie, zombie_mutant, zombie_strong, title, rang FROM charecter WHERE account_id='" + AcId + "' AND PlayerName='" + Networking_OnPlayerSelect.plname + "'";
			MySqlDataReader Reader2 = Linq.ExecuteReader ();
			try {
				Reader2.Read ();
                int i = 0;
				playername = Reader2.GetString (i++);
				sceneID = int.Parse (Reader2.GetString (i++));
				player = ScriptableObject.CreateInstance<Data_PlayerFile_Sr> ();
				player.login = Networking_OnPlayerSelect.log;
				player.password = Networking_OnPlayerSelect.pass;
				player.nick = playername;
				player.sceneID = sceneID;
                player.HPMax = int.Parse (Reader2.GetString (i++));
                player.PlayerScores = int.Parse (Reader2.GetString (i++));
				float x = float.Parse (Reader2.GetString (i++));
				float y = float.Parse (Reader2.GetString (i++));
				float z = float.Parse (Reader2.GetString (i++));
                player.zombie = int.Parse(Reader2.GetString(i++));
                player.zombieMutant = int.Parse(Reader2.GetString(i++));
                player.zombieStrong = int.Parse(Reader2.GetString(i++));
                player.title = Reader2.GetString(i++);
                player.rang = int.Parse(Reader2.GetString(i++));
                player.position.Set (x, y, z);
				Reader2.Close ();
				check = true;
			} catch (MySqlException ex) {
				Debug.Log (ex.ErrorCode + ex.Message);
			}
			Reader2.Close ();
		} catch (MySqlException ex) {
			Debug.Log (ex.ErrorCode + ex.Message);
		}
		ListScene.playerGet = player;
		player = null;
		Reader.Close ();
		return check;
	}
	
}
