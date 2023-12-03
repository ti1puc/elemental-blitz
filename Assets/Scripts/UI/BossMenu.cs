using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMenu : MonoBehaviour
{
	[SerializeField] private Laser laser;

	private void OnEnable()
	{
		StartCoroutine(WaitToShoot());
	}

	private void OnDisable()
	{
		if (laser != null)
		{
			laser.StopLaser();
		}
	}

	private IEnumerator WaitToShoot()
	{
		yield return new WaitForSeconds(0.1f);
		laser.ChargeLaserToShoot();
	}
}
