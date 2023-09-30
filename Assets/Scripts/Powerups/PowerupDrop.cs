using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupDrop : PoolGroup
{
    #region Fields
    [Header("Settings")]
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
        int random = Random.Range(0, 101); // 101 pq random.Range usando int � exclusivo. vira 0-100
        if (random < dropChance)
			SpawnRandomPowerup();
    }
    #endregion

    #region Private Methods
	private void SpawnRandomPowerup()
	{
        int random = Random.Range(0, ObjectPoolers.Count-1);
		SpawnPowerup(random);
	}

	private void SpawnPowerup(int index = 0)
	{
		if (index >= ObjectPoolers.Count) // n vai achar o pooler pq o index requisitado � maior, por isso return
			return;

		FindObjectPooler(index).SpawnPoolableObject();
	}
    #endregion
}
