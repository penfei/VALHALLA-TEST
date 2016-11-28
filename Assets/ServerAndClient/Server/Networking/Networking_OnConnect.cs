using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Networking_OnConnect : MonoBehaviour
{
	public static NetworkConnection tempConn = new NetworkConnection ();
	public static List<NetworkConnection> onlineConn = new List<NetworkConnection> ();
	public static List<int> onlineAcc = new List<int> ();

	public static string log = "";
	public static string pass = "";

	void Awake ()
	{
		SQL_FindLogPass.OnNoAccount += NoAccount;
		SQL_FindLogPass.OnNoChar += NoCharecter;
		SQL_FindLogPass.OnCharDataSet += DataSet;
	}

	static public void LoginGet (NetworkMessage netms)
	{
		Message_Sr.LoginSendMess_Sr Aut = netms.ReadMessage<Message_Sr.LoginSendMess_Sr> ();
		log = Aut.log;
		pass = Aut.pass;
		tempConn = netms.conn;
		SQL_FindLogPass.CheckLP ();
		Clear ();
	}

	static public void NoAccount ()
	{
		NetworkWriter wr = new NetworkWriter ();
		wr.StartMessage (Networking_msgType_Sr.NoAccount);
		wr.Write (0);
		wr.FinishMessage ();
		tempConn.SendWriter (wr, 0);
		tempConn = null;
	}

	static public void NoCharecter ()
	{
		Message_Sr.NoChar_Sr no = new Message_Sr.NoChar_Sr ();
		no.nochar = "nochar";
		tempConn.Send (Networking_msgType_Sr.NoChar, no);
		tempConn = null;
	}

	static public void DataSet ()
	{
		Message_Sr.CharData data = new Message_Sr.CharData ();
		tempConn.Send (Networking_msgType_Sr.PlayerDataGet, data);
		SQL_FindLogPass.ClearData ();
		tempConn = null;
	}

	public static void AddOnlineAccount (int accID)
	{
		onlineConn.Add (tempConn);
		onlineAcc.Add (accID);
	}

	public static void AccountUsed ()
	{
		NetworkWriter wr = new NetworkWriter ();
		wr.StartMessage (Networking_msgType_Sr.AccountOnline);
		wr.Write ("Account currently used.");
		wr.FinishMessage ();
		tempConn.SendWriter (wr, 0);
	}

	static void Clear ()
	{
		log = "";
		pass = "";
	}


}
