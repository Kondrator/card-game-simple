using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioRuleSimpleView : View {



    [SerializeField]
    private AudioSet addPoints = null;
    public AudioSet AddPoints { get { return addPoints ??= new AudioSet(); } }

    [SerializeField]
    private AudioSet win = null;
    public AudioSet Win { get { return win ??= new AudioSet(); } }



}
