using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CardsModel : Model {

	public class NOTIFY {

		public class CHANGED {

			public class CARDS {

				public const string NAME = "cards.notify.changed.cards";

			}

			public class CARD {

				public class ADD {

					public const string NAME = "cards.notify.changed.card.add";

				}

				public class REMOVE {

					public const string NAME = "cards.notify.changed.card.remove";

				}


				/// <summary>
				/// Type = CardParams
				/// </summary>
				public const string PARAM_CARD = "card";

			}

		}

	}




	private CardsList list = null;
	public CardsList List { get { return list ??= GetComponentInChildren<CardsList>(); } }




	public void Set( CardParams[] cards ) {

		List.Set( cards );

		Notify( NOTIFY.CHANGED.CARDS.NAME );
	}

	public void Clear() {

		List.Clear();

		Notify( NOTIFY.CHANGED.CARDS.NAME );
	}




	public void Add( CardParams card ) {

		List.Add( card );

		Notify(
			NOTIFY.CHANGED.CARD.ADD.NAME,
			new NotifyData.Param( NOTIFY.CHANGED.CARD.PARAM_CARD, card )
		);
	}

	public void Remove( CardParams card ) {

		List.Remove( card );

		Notify(
			NOTIFY.CHANGED.CARD.REMOVE.NAME,
			new NotifyData.Param( NOTIFY.CHANGED.CARD.PARAM_CARD, card )
		);
	}





	public void Add( CardController card ) {

		List.Add( card );

		Notify(
			NOTIFY.CHANGED.CARD.ADD.NAME,
			new NotifyData.Param( NOTIFY.CHANGED.CARD.PARAM_CARD, card.Model.Params )
		);

	}

	public CardController Pop( CardParams card ) {
		CardController result = List.Pop( card );
		Remove( card );
		return result;
	}


}
