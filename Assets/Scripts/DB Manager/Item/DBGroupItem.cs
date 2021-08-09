using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif




public abstract class DBGroupItem<T, K> : DBItem<T> where T : DBManager where K : DBItem {

	[SerializeField]
	private DBGroup<K> group = null;
	public DBGroup<K> Group { get { return group; } }


}