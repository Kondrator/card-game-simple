using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UIWindowManager;
using UnityEngine;



public class AdsController : Controller<AdsModel, AdsView> {

    public class NOTIFY {

        /// <summary>
        /// Invoked SUCCESS or FAIL.
        /// After invoked COMPLETED.
        /// </summary>
        public class SHOW_VIDEO {

            public const string NAME = "ads.controller.show.video";

        }


        /// <summary>
        /// Type - System.Action
        /// </summary>
        public const string PARAM_SUCCESS = "success";

        /// <summary>
        /// Type - System.Action
        /// </summary>
        public const string PARAM_FAIL = "fail";

        /// <summary>
        /// Type - System.Action
        /// </summary>
        public const string PARAM_COMPLETED = "completed";

    }




    protected override void PreInitiate() {}

    protected override void Initiate() {

        Add(
            NOTIFY.SHOW_VIDEO.NAME,
            data => {
                System.Action success = data.GetParam<System.Action>( NOTIFY.PARAM_SUCCESS );
                System.Action fail = data.GetParam<System.Action>( NOTIFY.PARAM_FAIL );
                System.Action completed = data.GetParam<System.Action>( NOTIFY.PARAM_COMPLETED );

                UIMessageBox box = UIMessageBox.CreateShow( "Здесь будет рекламу!", UIMessageBoxButtons.YesNo, "Рекламу посмотрел", "Рекламу закрыл" );
                box.OnClickYes.AddListener( () => { success?.Invoke(); completed?.Invoke(); } );
                box.OnClickNo.AddListener( () => { fail?.Invoke(); completed?.Invoke(); } );
            }
        );

    }

}
