using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioCardView : View {


    [SerializeField]
    private AudioSet card = null;
    public AudioSet Card { get { return card ??= new AudioSet(); } }

}
