using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackManager : MonoBehaviour
{
	public static HackManager Instance { get; private set; }

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
	}
}
