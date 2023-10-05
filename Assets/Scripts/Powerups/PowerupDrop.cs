using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupDrop : MonoBehaviour
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private List<GameObject> powerupsPrefabs = new List<GameObject>();
    [SerializeField, Range(0, 100)] private float dropChance;
    //[Header("References")]
    //[Header("Debug")]

    #endregion

    #region Properties
    #endregion

    #region Unity Messages
    #endregion

    #region Public Methods
    [Button]
	public void TrySpawnPowerup()
    {
        int random = Random.Range(0, 101); // 101 pq random.Range usando int é exclusivo. vira 0-100
        if (random < dropChance)
			SpawnRandomPowerup();
    }
    #endregion

    #region Private Methods
	private void SpawnRandomPowerup()
	{
        int random = Random.Range(0, powerupsPrefabs.Count-1);
		SpawnPowerup(random);
	}

	private void SpawnPowerup(int index = 0)
	{
		//if (index >= powerupsPrefabs.Count) // n vai achar o pooler pq o index requisitado é maior, por isso return
		//return;

		ObjectPoolManager.SpawnGameObject(powerupsPrefabs[index]);

	}
    #endregion
}
