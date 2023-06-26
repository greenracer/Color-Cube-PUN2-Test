using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Bootstrap : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to Master.");
        SceneManager.LoadScene("MainMenu");
    }
}
