using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolGroup : MonoBehaviour
{
	#region Fields
	[Tooltip("List of object poolers that will have a pool setted up")]
	[SerializeField] private List<ObjectPooler> objectPoolers = new List<ObjectPooler>();
	#endregion

	#region Properties
	public List<ObjectPooler> ObjectPoolers => objectPoolers;
	#endregion

	#region Unity Messages
	private void Start()
	{
		foreach (ObjectPooler objectPooler in objectPoolers)
			objectPooler.Initialize(this);
	}
	#endregion

	#region Public Methods
	public ObjectPooler FindObjectPooler(PoolableObject poolableObject)
	{
		return objectPoolers.Find(x => x.PoolableObject == poolableObject);
	}

	public ObjectPooler FindObjectPooler(int index)
	{
		return objectPoolers[index];
	}
	#endregion
}
