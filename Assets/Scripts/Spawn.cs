using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    void Start()
    {
        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(Random.Range(-21f, -18f), 0.073f, Random.Range(-8.8f, -1f)), Quaternion.identity);
    }
}
