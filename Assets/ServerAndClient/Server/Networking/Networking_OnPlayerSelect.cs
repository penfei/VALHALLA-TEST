using UnityEngine;
using UnityEngine.Networking;

public class Networking_OnPlayerSelect : MonoBehaviour
{
	public static string log = "";
	public static string pass = "";
	public static string plname = "";

	//If a player has selected a character check his data and send him to his character data
	static public void PlayerSelect (NetworkMessage netmsg)
	{
		Message_Sr.PlayerSelected_Sr selected = netmsg.ReadMessage<Message_Sr.PlayerSelected_Sr> ();
		log = selected.Login;
		pass = selected.Password;
		plname = selected.playerNick;

		if (SQL_PlayerVerefy.CheckLP ()) {
			SQL_PlayerVerefy.sceneID = 0;
            int index = PlayerLoad(netmsg.conn);
            Networking_PlayerListSend.OnPlayerList (netmsg.conn);
            Networking_PlayerListSend.SendItems(netmsg.conn, index);
            Clear ();
		} else {
			Clear ();
			netmsg.conn.Disconnect ();
		}
	}

    //Add new player on map
    static int PlayerLoad(NetworkConnection con)
    {
        GameObject go = (GameObject)Instantiate((GameObject)Resources.Load("Player"), ListScene.playerGet.position, Quaternion.identity);
        go.name = ListScene.playerGet.nick;
        Player_MovePlayer move = go.GetComponent<Player_MovePlayer>();
        move.playerCon = con;
        ListScene.WritePlayer(con, ListScene.playerGet, go);

        Message_Sr.NewPlayerOnScene_Sr newPL = new Message_Sr.NewPlayerOnScene_Sr();
        newPL.player = ListScene.GetPlayerData(move.index);
        Networking_Server.SendMSG(Networking_msgType_Sr.NewPlayerConnnectOnScene, newPL);

        Player_Weapon_Sr weap = Networking_Server.InstantiateWeapon(weaponType_Sr.GaussGun);
        if (weap)
        {
            weap.TakeWeapon(move.weapon, move.index);
        }

        return move.index;
    }

    static void Clear ()
	{
		log = "";
		pass = "";
		plname = "";
	}

}
