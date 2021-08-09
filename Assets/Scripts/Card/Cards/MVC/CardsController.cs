using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class CardsController : CardsController<CardsModel, CardsView> {




}



public class CardsController<TModel, TView> : Controller<TModel, TView> where TModel : CardsModel where TView : CardsView {



    protected override void PreInitiate() { }

    protected override void Initiate() {



    }


}