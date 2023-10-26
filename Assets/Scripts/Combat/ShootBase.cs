using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class ShootBase : MonoBehaviour
{
    [Header("Settings")]
	[SerializeField] protected float shootInterval;
	[SerializeField] protected bool useSpawnScale;
	[Header("Bullets")]
	[SerializeField] protected GameObject lightningBulletPrefab;
	[SerializeField] protected GameObject waterBulletPrefab;
	[SerializeField] protected GameObject fireBulletPrefab;
	[Header("References")]
	[SerializeField] protected Transform spawnPosition;
	[Header("Debug")]
	[SerializeField, ReadOnly] protected ElementManager elementManager;
	[SerializeField, ReadOnly] private float shootCooldown = 0;
	[SerializeField, ReadOnly] private bool canShoot = false;

	public bool DisableShoot { get; set; }
	public float ShootCooldown
	{
		get => shootCooldown;
		set => shootCooldown = value;
	}

	private void Awake()
	{
		// tenta achar o ElementManager caso nao foi settado no Inspector
		elementManager = GetComponent<ElementManager>();
		if (elementManager == null)
			elementManager = GetComponentInParent<ElementManager>();

		shootCooldown = shootInterval;
		canShoot = false;
	}

	protected virtual void Update()
	{
		// nao atirar se estiver disabilitado por outra classe
		if (DisableShoot) return;

		// only count cooldown if you cant shoot
		if (canShoot) return;

		shootCooldown -= Time.deltaTime;
		if (shootCooldown <= 0)
		{
			canShoot = true;
			shootCooldown = shootInterval;
		}
	}

	public void Shoot()
	{
        // do not shoot if you cant
        if (!canShoot) return;
		if (elementManager == null) return;

		// get element from ShootBase element manager
		Element elementIndex = elementManager.CurrentElement;

		GameObject bulletPrefab = null;
		switch (elementIndex)
		{
			case Element.Lightning:
				bulletPrefab = lightningBulletPrefab;
				break;
			case Element.Water:
				bulletPrefab = waterBulletPrefab;
				break;
			case Element.Fire:
				bulletPrefab = fireBulletPrefab;
				break;
			default:
				// erro
				break;
		}

		if (bulletPrefab == null) return;

		// find and spawn bullet
		GameObject bullet = ObjectPoolManager.SpawnGameObject(bulletPrefab, spawnPosition.position, spawnPosition.rotation);

		if (useSpawnScale)
			bullet.transform.localScale = spawnPosition.localScale;
		
        canShoot = false;
	}
}
