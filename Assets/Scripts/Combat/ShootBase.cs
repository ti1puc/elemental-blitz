using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class ShootBase : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
	[SerializeField] protected float shootInterval;
	[Header("References")]
	[SerializeField] protected Transform spawnPosition;
	[SerializeField] protected List<GameObject> bulletPrefabs = new List<GameObject>();
	[Header("Debug")]
	[SerializeField, ReadOnly] private float shootCooldown = 0;
	[SerializeField, ReadOnly] private bool canShoot = true;


    #endregion

    #region Properties
    #endregion

    #region Unity Messages


    protected virtual void Update()
	{
		// only count cooldown if you cant shoot
		if (canShoot) return;

		shootCooldown -= Time.deltaTime;
		if (shootCooldown <= 0)
		{
			canShoot = true;
			shootCooldown = shootInterval;
		}
	}
	#endregion

	#region Public Methods
	

	public void Shoot(int index = 0)
	{
		

        // do not shoot if you cant
        if (!canShoot) return;


		// find and spawn bullet
		GameObject bullet = bulletPrefabs[index];
		ObjectPoolManager.SpawnGameObject(bullet, spawnPosition.position, spawnPosition.rotation);


		
        canShoot = false;
	}


    #endregion

    #region Private Methods
    #endregion
}
