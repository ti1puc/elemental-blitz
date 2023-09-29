using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Plain C# class to facilitate the creating and handling of object pools
/// </summary>
[System.Serializable]
public class ObjectPooler
{
	#region Fields
	[SerializeField] private PoolableObject poolableObjectPrefab;
	[SerializeField] private int defaultCapacity = 6;
	[SerializeField] private int maxCapacity = 50;
	[SerializeField] private bool followParentPosition = false;
	private MonoBehaviour parentObject;
	private ObjectPool<PoolableObject> pool;
	#endregion

	#region Properties
	public PoolableObject PoolableObject => poolableObjectPrefab;
	public ObjectPool<PoolableObject> Pool => pool;
	#endregion

	#region Public Methods
	/// <summary>
	/// Should be called before anything else, used to setup the pool
	/// </summary>
	/// <param name="parent"></param>
	public void Initialize(MonoBehaviour parent)
	{
		parentObject = parent;
		pool = new ObjectPool<PoolableObject>(CreatePoolableObject, OnGetPoolableObjectFromPool, OnReleasePoolableObjectToPool, poolableObj => GameObject.Destroy(poolableObj.gameObject), true, defaultCapacity, maxCapacity);
	}

	/// <summary>
	/// On spawn the object is simply activated (get) and their killAction is setted
	/// </summary>
	public PoolableObject SpawnPoolableObject()
	{
		PoolableObject poolableObject = pool.Get();
		poolableObject.SetKillAction(ObjectKillAction);

		return poolableObject;
	}
	#endregion

	#region Private Methods
	// defines the method that will be called when the pool needs to get an object but there isn't one available, therefore it creates one
	private PoolableObject CreatePoolableObject()
	{
		if (followParentPosition)
			return GameObject.Instantiate(poolableObjectPrefab, parentObject.transform.position, parentObject.transform.rotation, parentObject.transform);
		else
			return GameObject.Instantiate(poolableObjectPrefab, parentObject.transform.position, parentObject.transform.rotation);
	}

	// method called when the object is 'getted', activating gameObject is essential
	private void OnGetPoolableObjectFromPool(PoolableObject poolableObject)
	{
		poolableObject.transform.position = parentObject.transform.position;
		poolableObject.gameObject.SetActive(true);
		poolableObject.OnSpawn();
	}

	// method called when the object is released, deactivating gameObject is essential
	private void OnReleasePoolableObjectToPool(PoolableObject poolableObject)
	{
		poolableObject.gameObject.SetActive(false);
		poolableObject.OnDestroyed();
	}
	#endregion

	// the killAction is simply a release from the pool, this way we can kill the object from the poolable class
	private void ObjectKillAction(PoolableObject poolableObject) => pool.Release(poolableObject);
}
