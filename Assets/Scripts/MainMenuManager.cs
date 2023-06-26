using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true,
            
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Level");
        Debug.Log($"Connected to Room {PhotonNetwork.CurrentRoom.Name}");
    }
}
