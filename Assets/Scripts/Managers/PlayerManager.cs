using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[Header("Debug")]
	[SerializeField, ReadOnly] private HealthController playerLife;
	[SerializeField, ReadOnly] private PlayerCollision playerCollision;
	[SerializeField, ReadOnly] private ElementManager playerElement;
	[SerializeField, ReadOnly] private ShieldPowerUp playerShield;

	public static PlayerManager Instance { get; private set; }
	public static GameObject Player => Instance.gameObject;
	public static HealthController PlayerLife => Instance.playerLife;
	public static PlayerCollision PlayerCollision => Instance.playerCollision;
	public static ElementManager PlayerElement => Instance.playerElement;
	public static ShieldPowerUp PlayerShield => Instance.playerShield;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(Instance.gameObject);
			Instance = this;
		}

		playerLife = GetComponent<HealthController>();
		playerCollision = GetComponentInChildren<PlayerCollision>();
		playerElement = GetComponent<ElementManager>();
		playerShield = GetComponentInChildren<ShieldPowerUp>(true);
	}
}
