using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;

    private float _spawnRange = 5f;

    private void Start()
    {
        float yPos = transform.localScale.y / 2;
        Vector3 randomPos = new Vector3(Random.Range(-_spawnRange, _spawnRange), yPos, Random.Range(-_spawnRange, _spawnRange));
        PhotonNetwork.Instantiate(_playerPrefab.name, randomPos, transform.rotation);
    }

}
