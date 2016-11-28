using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Data_ConnectionList : MonoBehaviour {

	public static string playerNick ="";
	public static short contrID = -1000;

	public static List<short> listController = new List<short> ();

	public static List<string> listPlayer = new List<string>();

	public static List<NetworkConnection> listConnection = new List<NetworkConnection>();

	public static void WritePlayer(NetworkConnection tempdata, string player){
		if (!listConnection.Contains (tempdata)) {
			listConnection.Add (tempdata);
			listPlayer.Add (player);
			short contr_id = (short)listConnection.IndexOf (tempdata);
			listController.Add (contr_id);
		}
	}

	public static void GetPlayer(NetworkConnection playercon){
		int pl_id = listConnection.IndexOf (playercon);
		playerNick = listPlayer [pl_id];
		contrID = listController[pl_id];
	}

	public static void DisconnectPlayer(NetworkConnection conn){
		if (listConnection.Contains (conn)) {
			int pl_id = listConnection.IndexOf (conn);
			listConnection.RemoveAt (pl_id);
			listPlayer.RemoveAt (pl_id);
			listController.RemoveAt (pl_id);
		}
	}

	public static void Clear(){
		contrID = -1000;
		playerNick = "";
	}
}
