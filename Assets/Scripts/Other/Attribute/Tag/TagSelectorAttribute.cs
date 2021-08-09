using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TagSelectorAttribute : PropertyAttribute{

	public bool UseDefaultTagFieldDrawer = false;

}


/*
	--- HOW TO USE ---

public class TagSelectorExample : MonoBehaviour
{

	[TagSelector]
	public string TagFilter = "";
  
	[TagSelector]
	public string[] TagFilterArray = new string[] { };

}

*/