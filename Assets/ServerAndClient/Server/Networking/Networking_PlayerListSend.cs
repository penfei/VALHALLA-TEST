using UnityEngine;
using UnityEngine.Networking;

public class Networking_PlayerListSend : MonoBehaviour
{
    //Send player packets to 15 players
    static int playersSends = 0;
    static int playersSend;
    //This value will be responsible for the amount of data sent to the players at a time
    static int packetPlayer = 15;
    static int lastPlayer;
    static bool allPlayers;

    //Send items data
    static int itemsSends = 0;
    static int itemsSend;
    //This value will be responsible for the amount of data sent to the players at a time
    static int packetItem = 15;
    static int lastItems;
    static bool allItems;

    public static void OnPlayerList(NetworkConnection con)
    {
        playersSend = packetPlayer;
        playersSends = 0;
        if (ListScene.playerList.Count == 1)
        {
            NetworkWriter writer = new NetworkWriter();
            writer.StartMessage(Networking_msgType_Sr.PlayerList);
            writer.Write(playersSends);
            writer.FinishMessage();
            con.SendWriter(writer, 1);
        }

        while (!allPlayers)
        {
            if (playersSends != ListScene.playerListCount)
            {
                lastPlayer = ListScene.playerListCount - playersSends;

                NetworkWriter writer = new NetworkWriter();
                writer.StartMessage(Networking_msgType_Sr.PlayerList);

                if (lastPlayer < playersSend)
                {
                    allPlayers = true;
                    playersSend = lastPlayer;
                    writer.Write(playersSend);
                }
                else
                {
                    writer.Write(playersSend);
                }
                for (int k = 0; k < playersSend; k++)
                {
                    Data_PlayerFile_Sr data = ListScene.GetPlayerData(playersSends);
                    if (data)
                    {
                        writer.Write(true);
                        writer.Write(data.sceneID);
                        writer.Write(data.charID);
                        writer.Write(data.rang);
                        writer.Write(data.nick);
                        writer.Write(data.title);
                        writer.Write(ListScene.objPlayers[playersSends].transform.position);
                        writer.Write(ListScene.objPlayers[playersSends].transform.rotation);
                        writer.Write(ListScene.playerMoveScr[playersSends].death);
                    }
                    else
                    {
                        writer.Write(false);
                    }
                    playersSends++;
                }
                writer.FinishMessage();
                con.SendWriter(writer, 0);
            }
            else
            {
                allPlayers = true;
            }
        }
        allPlayers = false;
    }

    public static void SendItems(NetworkConnection con, int index)
    {
        itemsSend = packetItem;
        itemsSends = 0;
        if (ListScene.itemListCount > 0)
        {
            while (!allItems)
            {
                if (itemsSends != ListScene.itemListCount)
                {
                    lastItems = ListScene.itemListCount - itemsSends;

                    NetworkWriter writer = new NetworkWriter();
                    writer.StartMessage(Networking_msgType_Sr.ItemList);

                    if (lastItems < itemsSend)
                    {
                        allItems = true;
                        itemsSend = lastItems;
                        writer.Write(itemsSend);
                        writer.Write(true);
                    }
                    else
                    {
                        writer.Write(itemsSend);
                        writer.Write(false);
                    }
                    for (int k = 0; k < itemsSend; k++)
                    {
                        Player_Item_Sr data = ListScene.GetItem(itemsSends);
                        if (data != null)
                        {
                            writer.Write(true);
                            writer.Write(data.index);
                            writer.Write((int)data.ItemType);

                            switch (data.ItemType)
                            {
                                case ItemType_Sr.weapon:
                                    writer.Write((int)data.WeaponType);
                                    Player_Weapon_Sr weap = data.gameObject.GetComponent<Player_Weapon_Sr>();
                                    writer.Write(weap.Dropped);
                                    if (weap.Dropped)
                                    {
                                        writer.Write(data.transform.position);
                                        writer.Write(data.transform.rotation);
                                    }
                                    else
                                    {
                                        writer.Write(weap.ShoterIndex);
                                        if(weap.ShoterIndex == index)
                                        {
                                            writer.Write(true);
                                            writer.Write(weap.maxAmmo);
                                        }
                                        else
                                        {
                                            writer.Write(false);
                                        }
                                    }
                                    break;
                            }

                        }
                        else
                        {
                            writer.Write(false);
                        }
                        itemsSends++;
                    }
                    writer.FinishMessage();
                    con.SendWriter(writer, 0);
                }
                else
                {
                    allItems = true;
                }
            }
        }
        else
        {
            NetworkWriter writer = new NetworkWriter();
            writer.StartMessage(Networking_msgType_Sr.ItemList);
            writer.Write(0);
            writer.Write(true);
            writer.FinishMessage();
            con.SendWriter(writer, 0);
        }
        allItems = false;
    }
}

