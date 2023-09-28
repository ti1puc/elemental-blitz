using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton é um design pattern usado pra ter acesso global à um GameObject. Pra criar um novo Singleton basta extender dessa classe.
/// </summary>
public class SingletonManager : MonoBehaviour
{
	public static SingletonManager Instance { get; private set; }

	[Header("Settings")]
	[SerializeField] protected bool dontDestroyOnLoad = true;

	protected virtual void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}

		if (dontDestroyOnLoad)
			DontDestroyOnLoad(gameObject);
	}
}
