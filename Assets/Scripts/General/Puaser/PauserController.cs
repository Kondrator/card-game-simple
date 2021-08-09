using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PauserController : Controller<PauserModel, PauserView> {

    public class NOTIFY {

        /// <summary>
        /// Add object is want add pause
        /// </summary>
        public class ADD {

            public const string NAME = "pauser.controller.add";

        }

        /// <summary>
        /// Add object is want remove pause
        /// </summary>
        public class REMOVE {

            public const string NAME = "pauser.controller.remove";

        }

        /// <summary>
        /// Clear all object from pause
        /// </summary>
        public class CLEAR {

            public const string NAME = "pauser.controller.clear";

        }


        /// <summary>
        /// Type = MonoBehaviour
        /// </summary>
        public const string PARAM_INITOR = "initor";

    }




    protected override void PreInitiate() {}

    protected override void Initiate() {

        Add(
            NOTIFY.ADD.NAME,
            data => {
                MonoBehaviour initor = data.GetParam<MonoBehaviour>( NOTIFY.PARAM_INITOR ) ?? data.Initiator;

                Model.Add( initor );
            }
        );


        Add(
            NOTIFY.REMOVE.NAME,
            data => {
                MonoBehaviour initor = data.GetParam<MonoBehaviour>( NOTIFY.PARAM_INITOR ) ?? data.Initiator;

                Model.Remove( initor );
            }
        );


        Add(
            NOTIFY.CLEAR.NAME,
            data => {
                Model.Clear();
            }
        );

    }


}
