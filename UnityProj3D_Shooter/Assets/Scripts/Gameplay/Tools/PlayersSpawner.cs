using System.Collections;
using UnityEngine;

public class PlayersSpawner : MonoBehaviour 
{
    private const int SecondsToRespawn = 3;
    private const string PathToPlayersRoot = "/PlayersRoot";
    [SerializeField] private Transform _spawnPoint;
    private Transform _playersRoot;
    public bool isAvailable = true;

    private void Awake()
    {
        _playersRoot = GameObject.Find(PathToPlayersRoot).transform;
    }

    public GameObject Spawn(GameObject playerPrefab) 
    {
        var randX = Random.value;
        var randZ = Random.value;
        var pos = _spawnPoint.position + new Vector3(randX, 0, randZ);
        var obj = Instantiate(playerPrefab, pos, _spawnPoint.rotation, _playersRoot);
        obj.name = playerPrefab.name;
        isAvailable = true;
        return obj;
    }

    public void Respawn(GameObject player)
    {
       StartCoroutine(DelayRespawn(player));
    }


    private IEnumerator DelayRespawn(GameObject player)
    {
        isAvailable = false;
        yield return new WaitForSeconds(SecondsToRespawn);
        Spawn(player);
    }

}
