using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager Instance { get; private set; }
	public static HealthController PlayerLife { get; private set; }
	public static GameObject Player => Instance.gameObject;

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

		PlayerLife = GetComponent<HealthController>();
	}
}
