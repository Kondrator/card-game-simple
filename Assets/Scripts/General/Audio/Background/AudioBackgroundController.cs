using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LevelManager {


    public class AudioBackgroundController : AudioBackgroundController<AudioBackgroundView> {

    }


    public class AudioBackgroundController<TView> : AudioBaseController<TView> where TView : AudioBackgroundView {



        protected override void PreInitiate() {}

        protected override void Initiate() {


            Add(
                SceneController.NOTIFY_LOADED,
                data => {
                    PlayRepeat( View.Background );
                }
            );


        }



    }




}