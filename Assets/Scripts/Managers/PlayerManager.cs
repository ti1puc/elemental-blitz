using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool dontDestroyOnLoad = true;

	public static PlayerManager Player { get; private set; }
	public static HealthController PlayerLife { get; private set; }

	private void Awake()
	{
		if (Player == null)
		{
			Player = this;

			if (dontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);
		}
		else if (Player != this)
		{
			Destroy(Player.gameObject);
			Player = this;

			if (dontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);
		}

		PlayerLife = GetComponent<HealthController>();
	}
}
