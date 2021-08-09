using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LevelManager {

    public class AudioBackgroundView : View {


        [SerializeField]
        private AudioSet background = null;
        public AudioSet Background { get { return background = background ?? new AudioSet(); } }


    }

}