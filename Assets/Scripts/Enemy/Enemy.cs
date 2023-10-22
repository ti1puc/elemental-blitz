using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region Fields
	//[Header("Settings")]
	[Header("References")]
	[SerializeField] protected Transform enemyVisual;
	[SerializeField, ReadOnly] protected EnemySpawner parentEnemySpawner;
	[SerializeField, ReadOnly] protected PowerupDrop powerupDrop;
	[SerializeField, ReadOnly] protected HealthController healthController;
	[SerializeField, ReadOnly] protected ShootBase shootBase;
	[Header("Debug")]
	[SerializeField, ReadOnly] protected bool isHitable;
	[SerializeField, ReadOnly] protected Vector2 moveRangeX;
	[SerializeField, ReadOnly] protected Vector2 moveRangeZ;
	[SerializeField, ReadOnly] protected bool hasInitialized;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	protected virtual void Awake()
	{
		powerupDrop = GetComponent<PowerupDrop>();
		healthController = GetComponent<HealthController>();
		shootBase = GetComponent<ShootBase>();

		// desabilita o tiro até que o inimigo seja hitable
		shootBase.DisableShoot = true;
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		// os inimigos primeiro aparecem na tela, aí rola um feedback mostrando que é possivel atirar nele
		// até esse feedback visual aparecer o inimigo não pode ser hitable
		if (isHitable == false) return;

		// checar se tomou dano aqui

		// diminuir o dano que tomou, o segundo parametro é a funcao q vai chamar se a vida chegar em 0, nesse caso é pra destruir o inimigo
		healthController.TakeDamage(0, PlayerDestroyEnemy);
	}
	#endregion

	public void InitializeEnemy(Vector2 moveRangeX, Vector2 moveRangeZ, EnemySpawner enemySpawner)
	{
		// usa o this. porque o nome das variaveis é igual
		this.moveRangeX = moveRangeX;
		this.moveRangeZ = moveRangeZ;
		parentEnemySpawner = enemySpawner;

		hasInitialized = true;
		AfterInitialization();
	}

	/// <summary>
	/// chamar essa funcao se for destruido pelo Player, por que ela tem o codigo de spawnar powerup
	/// </summary>
	[Button]	
	public void PlayerDestroyEnemy()
	{
		if (powerupDrop)
			powerupDrop.TrySpawnPowerup();

		DestroyEnemy();
	}

	[Button]
	public void DestroyEnemy()
	{
		transform.rotation = Quaternion.identity; // reseta a rotaçao quando é despawnado
		UnMakeEnemyHitable();

		if (parentEnemySpawner)
			parentEnemySpawner.DecreaseEnemyCount();

		ObjectPoolManager.DespawnGameObject(gameObject);
	}

	/// <summary>
	/// usar essa funcao nas classes herdadas ao invés de Awake, Start ou OnEnable
	/// </summary>
	protected virtual void AfterInitialization() { }

	/// <summary>
	/// usar essa funcao pra marcar quando o inimigo pode ser acertado e começar a atirar tbm
	/// </summary>
	protected virtual void MakeEnemyHitable()
	{
		if (isHitable) return; // pra evitar chamar a funcao varias vezes

		isHitable = true;
		shootBase.DisableShoot = false;
	}

	/// <summary>
	/// usar essa funcao pra desmarcar quando o inimigo pode ser acertado e parar os tiros tbm
	/// </summary>
	protected virtual void UnMakeEnemyHitable()
	{
		if (isHitable == false) return; // pra evitar chamar a funcao varias vezes

		isHitable = false;
		shootBase.DisableShoot = true;
	}
}
