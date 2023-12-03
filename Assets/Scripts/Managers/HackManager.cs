using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackManager : MonoBehaviour
{
	public static HackManager Instance { get; private set; }
	public static bool HasInfiniteHealth => IsInstanceValid && Instance.hasInfiniteHealth;
	public static bool IsInstanceValid => Instance != null;

	[Header("Debug")]
	[SerializeField, ReadOnly] private bool hasInfiniteHealth;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(Instance.gameObject);
			Instance = this;

			DontDestroyOnLoad(gameObject);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			GameManager.NextLevel();
		}

		if (Input.GetKeyDown(KeyCode.F3))
		{
			hasInfiniteHealth = !hasInfiniteHealth;
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			GameManager.Retry();
		}
	}
}
