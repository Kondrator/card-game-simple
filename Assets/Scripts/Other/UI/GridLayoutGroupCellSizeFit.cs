using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine.UI {

    [RequireComponent( typeof(GridLayoutGroup) )]
    public class GridLayoutGroupCellSizeFit : MonoBehaviour {

        [SerializeField]
        [Range( 1, 10 )]
        private int count = 3;

        [SerializeField]
        private bool isSetWidth = true;

        [SerializeField]
        private bool isSetHeight = true;



        private RectTransform rect = null;
        private GridLayoutGroup grid = null;


        private void Awake() {

            rect = this.transform as RectTransform;
            grid = GetComponent<GridLayoutGroup>();

        }

        private void OnEnable() {
            UpdateSize();
        }


        /// <summary>
        /// Calculate size cell fit
        /// </summary>
        private void UpdateSize() {
            float size = rect.GetWidth();
            size -= grid.padding.left + grid.padding.right;
            size -= grid.spacing.x * (count - 1);
            size /= count;

            grid.cellSize = new Vector2(    isSetWidth ? size : grid.cellSize.x,
                                            isSetHeight ? size : grid.cellSize.y
                                       );
        }

    }

}