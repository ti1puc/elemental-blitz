using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool resetShootCooldownOnHitable = true;
	[SerializeField] private int scoreToGive = 10;
	[Header("Hitable VFX")]
	[SerializeField] private Color defaultWhiteTint = Color.white;
	[SerializeField] private Color tintWhenNotHitable = Color.grey;
	[SerializeField] private float colorChangeSpeed = 1f;
	[SerializeField] private float colorChangeDuration = 1f;
	[SerializeField] private MeshRenderer[] meshRenderers;
	[Header("References")]
	[SerializeField] protected Transform enemyVisual;
	[SerializeField] protected Animator animator;
	[SerializeField, ReadOnly] protected EnemySpawner parentEnemySpawner;
	[SerializeField, ReadOnly] protected PowerupDrop powerupDrop;
	[SerializeField, ReadOnly] protected HealthController healthController;
	[SerializeField, ReadOnly] protected ShootBase[] shootBases;
	[Header("Debug")]
	[SerializeField, ReadOnly] protected bool isHitable;
	[SerializeField, ReadOnly] protected Vector2 moveRangeX;
	[SerializeField, ReadOnly] protected Vector2 moveRangeZ;
	[SerializeField, ReadOnly] protected bool hasInitialized;

	// usar isso aqui permite mudar as settings de um material sem mudar pra todos
	// se n usar isso teria q criar um material pra cada sprite
	private MaterialPropertyBlock materialPropertyBlock;

	private MaterialPropertyBlock MaterialPropertyBlock
	{
		get
		{
			if (materialPropertyBlock == null)
				materialPropertyBlock = new MaterialPropertyBlock();
			return materialPropertyBlock;
		}
	}
	public bool IsHitable => isHitable;
	public int ScoreToGive => scoreToGive;

	#region Unity Messages
	protected virtual void Awake()
	{
		powerupDrop = GetComponent<PowerupDrop>();
		healthController = GetComponent<HealthController>();
		shootBases = GetComponents<ShootBase>();
		if (shootBases.Length <= 0)
			shootBases = GetComponentsInChildren<ShootBase>();
	}
	#endregion

	public void InitializeEnemy(Vector2 moveRangeX, Vector2 moveRangeZ, EnemySpawner enemySpawner)
	{
		// usa o this. porque o nome das variaveis é igual
		this.moveRangeX = moveRangeX;
		this.moveRangeZ = moveRangeZ;
		parentEnemySpawner = enemySpawner;

		// desabilita o tiro até que o inimigo seja hitable
		foreach (var shootBase in shootBases)
			shootBase.DisableShoot = true;

		MaterialPropertyBlock.SetColor("_Tint", tintWhenNotHitable);
		foreach (var meshRenderer in meshRenderers)
			meshRenderer.SetPropertyBlock(MaterialPropertyBlock);

		hasInitialized = true;
		AfterInitialization();
	}

	/// <summary>
	/// chamar essa funcao se for destruido pelo Player, por que ela tem o codigo de spawnar powerup e de ganahr ponto
	/// </summary>
	[Button]	
	public void PlayerDestroyEnemy()
	{
		if (powerupDrop)
			powerupDrop.TrySpawnPowerup();

		GameManager.IncreaseScore(ScoreToGive);
        AudioManager.Instance.PlaySFXPowerUp("snd_Explosion01");
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
		EnableShoot();

		if (resetShootCooldownOnHitable)
		{
			foreach (var shootBase in shootBases)
				shootBase.ShootCooldown = 0;
		}

		ChangeToColor(tintWhenNotHitable, defaultWhiteTint);
	}

	/// <summary>
	/// usar essa funcao pra desmarcar quando o inimigo pode ser acertado e parar os tiros tbm
	/// </summary>
	protected virtual void UnMakeEnemyHitable()
	{
		if (isHitable == false) return; // pra evitar chamar a funcao varias vezes

		isHitable = false;
		DisableShoot();

		ChangeToColor(defaultWhiteTint, tintWhenNotHitable);
	}

	/// <summary>
	/// essa funcao habilita o tiro
	/// </summary>
	protected virtual void EnableShoot()
	{
		foreach (var shootBase in shootBases)
			shootBase.DisableShoot = false;
	}

	/// <summary>
	/// essa funcao desabilita o tiro
	/// </summary>
	protected virtual void DisableShoot()
	{
		foreach (var shootBase in shootBases)
			shootBase.DisableShoot = true;
	}

	private void ChangeToColor(Color currentColor, Color nextColor)
	{
		StartCoroutine(ChangeColorCoroutine(currentColor, nextColor));
	}

	// coroutinas na unity é mt bom pra fazer funcóes em sequencia
	private IEnumerator ChangeColorCoroutine(Color currentColor, Color nextColor)
	{
		float timer = 0;

		while (timer < colorChangeDuration)
		{
			Color color = Color.Lerp(currentColor, nextColor, colorChangeSpeed * timer);

			MaterialPropertyBlock.SetColor("_Tint", color);
			foreach (var meshRenderer in meshRenderers)
				meshRenderer.SetPropertyBlock(MaterialPropertyBlock);

			timer += Time.deltaTime;
			yield return null;
		}
	}
}
