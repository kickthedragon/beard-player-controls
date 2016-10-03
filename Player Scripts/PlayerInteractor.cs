using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerInteractor : MonoBehaviour
{

	public List<TriggerObject> _interactableObjects = new List<TriggerObject>();

	public bool canInteractWithMob { get { return (from o in _interactableObjects where o.GetType().IsSubclassOf(typeof(NPC)) select o).Any(); } }

	public bool hasItem { get { return (from o in _interactableObjects where o.GetType().IsSubclassOf(typeof(Item)) select o).Any(); } }

    public bool hasQuest { get { return (from o in _interactableObjects where o.GetType() == (typeof(QuestStart)) select o).Any(); } }

	private static List<PlayerInteractor> _allInteractors = new List<PlayerInteractor>();

	void Start()
	{
		_allInteractors.Add(this);
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		//	Debug.Log("Added " + col.name);

		TriggerObject[] obj = col.GetComponents<TriggerObject>();
        if (obj != null)
        {
            for (int i = 0; i < obj.Length; i++)
                Add(obj[i]);
        }
	}

	void OnTriggerExit2D(Collider2D col)
	{
        //	Debug.Log("Removed " + col.name);

        TriggerObject[] obj = col.GetComponents<TriggerObject>();
        if (obj != null)
        {
            for (int i = 0; i < obj.Length; i++)
                Remove(obj[i]);
        }   

	}

	public TriggerObject SelectClosest()
	{
		return _interactableObjects.OrderBy(o => Vector2.Distance(o.transform.position, transform.position)).FirstOrDefault();
	}


	public T Select<T>() where T : TriggerObject
	{
		return (T)(from o in _interactableObjects where o is T || o.GetType().IsSubclassOf(typeof(T)) select o).OrderBy(o => Vector2.Distance(o.transform.position, transform.position)).FirstOrDefault();
	}


	public bool Contains(TriggerObject obj)
	{
		return _interactableObjects.Contains(obj);
	}

	public bool Add(TriggerObject obj)
	{
		if (_interactableObjects.Contains(obj))
			return false;
		_interactableObjects.Add(obj);
		return true;
	}


	public bool Remove(TriggerObject obj)
	{
		return _interactableObjects.Remove(obj);
	}

	public static void RemoveFromAll(TriggerObject obj)
	{
		foreach (PlayerInteractor i in _allInteractors)
			i.Remove(obj);
	}


}
