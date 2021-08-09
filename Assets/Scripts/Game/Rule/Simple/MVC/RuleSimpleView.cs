using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RuleSimpleView : View {


    private DeckController deck = null;
    public DeckController Deck { get { return deck = Find( deck ); } }


    private TableController table = null;
    public TableController Table { get { return table = Find( table ); } }


    private PlayerController player = null;
    public PlayerController Player { get { return player = Find( player ); } }


}
