using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private float xPosCorrection;
	[SerializeField] private float zPosCorrection;
	[Header("References")]
	public Enemy enemy;
	public ElementManager elementManager;
	public HealthController healthController;
    [SerializeField] public Element currentElement_;
    [Header("feedback damage")]
    public Material materialDamage; 
    public Material materialOriginal;
    public Renderer renderer;
    public float durationMax = 0.10f;
    
    [Header("Debug")]
	[SerializeField, ReadOnly] private float distancePlayer;
	[SerializeField, ReadOnly] private float distanceXZero;
	[SerializeField, ReadOnly] private float differenceXZero;
	[SerializeField, ReadOnly] private float direction;
	#endregion

	#region Properties
	private void Update()
	{
		// mantem a colisao no Y zero
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);

		// calcula o offset do objeto child (localPosition)
		// o offset tem que olhar a distancia do player pra se ajustar
		distancePlayer = Vector3.Distance(PlayerManager.Player.transform.position, transform.position);
		distanceXZero = transform.parent.position.x;
		direction = xPosCorrection == 0 ? 0 : (distanceXZero > 0 ? 1 : -1);
		differenceXZero = Mathf.Abs(distanceXZero) - Mathf.Abs(xPosCorrection);
		if (differenceXZero < 0)
			differenceXZero = 0;

		transform.localPosition = new Vector3(direction * differenceXZero, transform.localPosition.y, zPosCorrection * distancePlayer);

        
        if(durationMax >= 0)
        {
            durationMax -= Time.deltaTime; 
        }
        else
        {
            renderer.material = materialOriginal;
        }
           
     

	}
	#endregion

	#region Unity Messages
	private void OnTriggerEnter(Collider other)
	{
		// os inimigos primeiro aparecem na tela, a� rola um feedback mostrando que � possivel atirar nele
		// at� esse feedback visual aparecer o inimigo n�o pode ser hitable

		if (enemy == null) return;
		if (enemy.IsHitable == false) return;
		
		BulletBase bullet_ = other.gameObject.GetComponent<BulletBase>();


        #region colision with water
        if (other.CompareTag("Water"))
        {
            // if current element = lighning
            if (currentElement_ == Element.Lightning)
            {
               // healthController.TakeDamage(bullet_.Damage, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();

            }

            if (currentElement_ == Element.Fire)
            {
                healthController.TakeDamage(bullet_.Damage * 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            if (currentElement_ == Element.Water)
            {
                healthController.TakeDamage(bullet_.Damage / 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            
            renderer.material = materialDamage;
            durationMax = 0.10f;
        }
        #endregion

        #region colision with lighning
        if (other.CompareTag("Lightning"))
        {
            // if current element = lighning
            if (currentElement_ == Element.Lightning)
            {
                healthController.TakeDamage(bullet_.Damage / 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            if (currentElement_ == Element.Fire)
            {
               // healthController.TakeDamage(bullet_.Damage * 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            if (currentElement_ == Element.Water)
            {
                healthController.TakeDamage(bullet_.Damage * 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            renderer.material = materialDamage;
            durationMax = 0.10f;
        }
        #endregion

        #region colision with fire
        if (other.CompareTag("Fire"))
        {
            // if current element = lighning
            if (currentElement_ == Element.Lightning)
            {
                healthController.TakeDamage(bullet_.Damage * 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            if (currentElement_ == Element.Fire)
            {
                healthController.TakeDamage(bullet_.Damage / 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            if (currentElement_ == Element.Water)
            {
                //healthController.TakeDamage(bullet_.Damage / 2, enemy.PlayerDestroyEnemy);
                bullet_.DestroyBullet();
            }

            
                renderer.material = materialDamage;
                durationMax = 0.10f;


        }
        #endregion

  
    }
    #endregion
}
