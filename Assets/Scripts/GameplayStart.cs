using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStart : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Start()
    {
        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        player.SetActive(true);
    }
}
