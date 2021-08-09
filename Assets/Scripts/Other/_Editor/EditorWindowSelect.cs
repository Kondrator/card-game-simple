#if UNITY_EDITOR

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using UnityEditor;



    public class EditorWindowSelect : EditorWindow {

        public class Item {

            private int id = -1;
            public int ID { get { return id; } }

            private string name = "";
            public string Name { get { return name; } }

            private Sprite icon = null;
            public Sprite Icon { get { return icon; } }

            private string[] detailParams = null;
            public string[] DetailParams { get { return detailParams; } }


            public Item( int id, string name, Sprite icon, params string[] detailParams ) {
                this.id = id;
                this.name = name;
                this.icon = icon;
                this.detailParams = detailParams;
            }

        }



#region Styles

        private GUIStyle styleItemArea = null;
        private GUIStyle StyleItemArea {
            get {
                if( styleItemArea == null ) {
                    styleItemArea = new GUIStyle( "area" );
                }
                return styleItemArea;
            }
        }

        private GUIStyle styleItemAreaHover = null;
        private GUIStyle StyleItemAreaHover {
            get {
                if( styleItemAreaHover == null ) {
                    styleItemAreaHover = new GUIStyle( "area" );
                    styleItemAreaHover.normal.background = MyOperationEditor.GetColorTexture( 171, 171, 171 );
                }
                return styleItemAreaHover;
            }
        }

        private GUIStyle styleItemAreaActive = null;
        private GUIStyle StyleItemAreaActive {
            get {
                if( styleItemAreaActive == null ) {
                    styleItemAreaActive = new GUIStyle( "area" );
                    styleItemAreaActive.normal.background = MyOperationEditor.GetColorTexture( 203, 241, 243 );
                }
                return styleItemAreaActive;
            }
        }

        private GUIStyle styleItemID = null;
        private GUIStyle StyleItemID {
            get {
                if( styleItemID == null ) {
                    styleItemID = new GUIStyle( "label" );
                    styleItemID.fontStyle = FontStyle.Bold;
                    styleItemID.fontSize = 12;
                    styleItemID.normal.textColor = Color.gray;
                }
                return styleItemID;
            }
        }

        private GUIStyle styleItemName = null;
        private GUIStyle StyleItemName {
            get {
                if( styleItemName == null ) {
                    styleItemName = new GUIStyle( "label" );
                    styleItemName.alignment = TextAnchor.MiddleCenter;
                }
                return styleItemName;
            }
        }

#endregion



        private Item[] items = null;
        private System.Action<Item> callbackSelect = null;
        private int idSelected = -1;

        private string filter = "";
        private int size = 100, sizeMin = 70, sizeMax = 200;
        private bool IsSizeMinimal { get { return size == sizeMin; } }
        private Vector2 scrollItemsPos;

        private System.DateTime? timeLastDown = null;
        private Vector2? mousePositionLast = null;

        private bool isChangeDetailHeight = false;
        private float heightDetail = 100, heightDetailMin = 73, heightDetailMax = 400;

        private bool isClosed = false;






        public static void Show( Item[] items, System.Action<Item> callbackSelect, int idSelected = -1 ) {
            Show( "Select Item", items, callbackSelect, idSelected );
        }

        public static void Show( string title, Item[] items, System.Action<Item> callbackSelect, int idSelected = -1 ) {
            EditorWindowSelect window = (EditorWindowSelect)GetWindow( typeof( EditorWindowSelect ) );
            window.titleContent = new GUIContent( title );
            window.items = items;
            window.callbackSelect = callbackSelect;
            window.idSelected = idSelected;
            window.wantsMouseEnterLeaveWindow = true;
            window.Init();
            window.Show();
        }



        private void Init() {
            isClosed = false;

            size = MyOperationEditor.LoadParam( "WindowSelectConfig", "size", size );
            heightDetail = MyOperationEditor.LoadParam( "WindowSelectConfig", "height_detail", heightDetail );

            wantsMouseMove = true;
            minSize = new Vector2( 300, 200 );
        }






        private new void Close() {
            if( isClosed == true ) {
                return;
            }
            isClosed = true;

            MyOperationEditor.SaveParam( "WindowSelectConfig", "size", size );
            MyOperationEditor.SaveParam( "WindowSelectConfig", "height_detail", heightDetail );

            base.Close();
        }

        private void OnLostFocus() {
            Close();
        }



        private void OnGUI() {

            if( items == null ) {
                Close();
                return;
            }


            DrawFilter();

            float heightItemDetail = heightDetail;
            Item itemDetail = items.FirstOrDefault( value => value.ID == idSelected );
            if( itemDetail == null ) {
                heightItemDetail = 0;
            }

            Rect rectItems = new Rect( 0, 0, position.width, position.height - heightItemDetail );
            DrawItems( rectItems );

            if( itemDetail != null ) {
                Rect rectItemDetail = new Rect( 0, rectItems.y + rectItems.height, position.width, heightItemDetail );
                DrawDBItemDetail( rectItemDetail, itemDetail );
            }


            if( Event.current.type == EventType.MouseMove
                || Event.current.type == EventType.MouseDown
            ) {
                Repaint();
            }

        }





        /// <summary>
        /// Draw text field (filter)
        /// </summary>
        private void DrawFilter() {
            GUILayout.BeginHorizontal( GUI.skin.FindStyle( "Toolbar" ) );
            
            filter = MyOperationEditor.DrawSeachable( filter );

            GUILayout.Space( 5 );
            size = (int)GUILayout.HorizontalSlider( size, sizeMin, sizeMax, GUILayout.Width( 100 ) );

            GUILayout.EndHorizontal();
        }



        /// <summary>
        /// Draw items
        /// </summary>
        /// <param name="rect">Area</param>
        private void DrawItems( Rect rect ) {

            // filter items
            System.Func<Item, bool> filter = ( Item item ) => {
                if( string.IsNullOrEmpty( this.filter ) == true ) {
                    return true;
                }

                if( item.Name.ToLower().Contains( this.filter.ToLower() ) == true ) {
                    return true;
                }

                int number = 0;
                if( int.TryParse( this.filter, out number ) ) {
                    return item.ID == number;
                }

                return false;
            };

            Item[] items = this.items.Where( filter ).ToArray();
            items = items.InsertItem( 0, new Item( -1, "None", null ) );



            Rect rectItem = new Rect( 0, 0, size, size );
            float padding = 10;
            Vector2 celling = new Vector2( padding, padding );

            // calculate count items in one line
            int width = Mathf.Min( items.Length, Mathf.Max( 1, (int)((rect.width - padding * 2 - 50) / (rectItem.width + padding)) ) );
            celling.x = (rect.width - rectItem.width * width - padding) / width;
            celling.y = padding;


            scrollItemsPos = GUILayout.BeginScrollView( scrollItemsPos, GUILayout.Width( rect.width ), GUILayout.Height( rect.height ) );

            // lines
            if( IsSizeMinimal == true ) {
                rectItem.width = rect.width;
                rectItem.height = 25;
                for( int i = 0; i < items.Length; i++ ) {
                    rectItem.x = rect.x;
                    rectItem.y = rect.y + i * rectItem.height;

                    DrawDBItem( rectItem, items[i] );
                }

            // grid
            } else {
                int i = 0;
                int y = 0;
                while( i < items.Length ) {
                    for( int x = 0; x < width; x++ ) {

                        if( i >= items.Length ) {
                            break;
                        }

                        rectItem.x = rect.x + x * rectItem.width + x * celling.x;
                        rectItem.y = rect.y + padding + y * rectItem.height + y * celling.y;

                        DrawDBItem( rectItem, items[i++] );

                    }
                    y++;
                }
            }

            GUILayout.Space( rectItem.y + rectItem.height + padding );

            GUILayout.EndScrollView();

        }




        /// <summary>
        /// Draw item
        /// </summary>
        /// <param name="rect">Area</param>
        /// <param name="item">Item</param>
        private void DrawDBItem( Rect rect, Item item ) {

            // style - normal
            GUIStyle styleArea = StyleItemArea;

            // style - normal select ?
            if( idSelected == item.ID ) {
                styleArea = StyleItemAreaActive;
            }

            // style - hover ?
            if( rect.Contains( Event.current.mousePosition ) ) {
                // this item not select ?
                if( idSelected != item.ID ) {
                    styleArea = StyleItemAreaHover;
                }

                // click on item ?
                if( Event.current.type == EventType.MouseDown ) {
                    // this item is new select
                    if( idSelected != item.ID ) {
                        callbackSelect( item );
                        idSelected = item.ID;
                        timeLastDown = null;
                    }

                    // detect double click
                    System.DateTime timeDown = System.DateTime.Now;
                    if( timeLastDown.HasValue == true
                        && (timeDown - timeLastDown.Value).TotalMilliseconds < 300
                    ) {
                        Close();
                    }
                    timeLastDown = timeDown;
                }
            }




            GUILayout.BeginArea( rect, styleArea );
            
                if( IsSizeMinimal == true ) {
                    float padding = 5;

                    // image
                    Rect rectImage = new Rect( padding * 3, padding, rect.height - padding * 2, rect.height - padding * 2 );
                    MyOperationEditor.DrawTexturePreview( rectImage, item.Icon );

                    // name
                    Rect rectName = new Rect( rectImage.x + rectImage.width + padding * 2, (rect.height - 15) / 2, rect.width - (rectImage.x + rectImage.width + padding), 15 );
                    GUI.Label( rectName, item.Name );

                    
                }else{
                    float padding = rect.width * 0.1f;

                    // image
                    Rect rectImage = new Rect( padding, padding * 1.5f, rect.width - padding * 2, rect.height - padding * 5 );
                    MyOperationEditor.DrawTexturePreview( rectImage, item.Icon );

                    // id
                    Rect rectID = new Rect( 5, 5, rect.width - 10, 20 );
                    GUI.Label( rectID, item.ID.ToString(), StyleItemID );

                    // name
                    Rect rectName = new Rect( 0, rect.height - padding * 2.5f, rect.width, 20 );
                    GUI.Label( rectName, item.Name.Cut( (int)(padding * 1.1f) ), StyleItemName );
                }

            GUILayout.EndArea();
            

        }



        /// <summary>
        /// Draw item detail
        /// </summary>
        /// <param name="rect">Area</param>
        /// <param name="item">Item</param>
        private void DrawDBItemDetail( Rect rect, Item item ) {

            

            GUIStyle separator = new GUIStyle( "box" );
            separator.border.top = 1;
            separator.border.bottom = 0;
            separator.margin.top = separator.margin.bottom = 0;
            separator.padding.top = separator.padding.bottom = 0;


            Rect rectBorderTop = new Rect( rect.x, rect.y + 17, rect.width, 1 );
            GUI.Box( rectBorderTop, "", separator );

            // detect mouse in border
            rectBorderTop.height = 10;
            if( rectBorderTop.Contains( Event.current.mousePosition ) ) {
                EditorGUIUtility.AddCursorRect( new Rect( Event.current.mousePosition, new Vector2( 100, 100 ) ), MouseCursor.ResizeVertical );

                // enable reszie mode height ?
                if( Event.current.type == EventType.MouseDown ) {
                    isChangeDetailHeight = true;
                    mousePositionLast = Event.current.mousePosition;
                }
            }

            // eanbled mode change height ?
            if( isChangeDetailHeight == true ) {
                // detect disable mode
                if( Event.current.type == EventType.MouseUp
                    || Event.current.type == EventType.MouseLeaveWindow
                ) {
                    isChangeDetailHeight = false;
                    mousePositionLast = null;

                // change height
                } else {
                    heightDetail = Mathf.Clamp( heightDetail + mousePositionLast.Value.y - Event.current.mousePosition.y, heightDetailMin, heightDetailMax );
                    mousePositionLast = Event.current.mousePosition;
                    Repaint();
                }
            }


            GUILayout.BeginArea( rect, StyleItemArea );

                // image
                Rect rectImage = new Rect( 10, 30, rect.height - 40, rect.height - 40 );
                MyOperationEditor.DrawTexturePreview( rectImage, item.Icon );

                // name
                Rect rectName = new Rect( rectImage.x + rectImage.width + 20, rectImage.y, rect.width - rectImage.x - rectImage.width - 40, 20 );
                GUI.Label( rectName, string.Format( "[{0}] {1}", item.ID, item.Name ), EditorStyles.boldLabel );

                // detail params
                Rect rectDetailParam = new Rect( rectName.x, rectName.y, rectName.width, rectName.height );
                for( int i = 0; i < item.DetailParams.Length; i++ ) {
                    rectDetailParam.y += rectDetailParam.height;
                    GUI.Label( rectDetailParam, item.DetailParams[i] );
                }

            GUILayout.EndArea();


        }

    }

#endif