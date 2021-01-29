using System;
using System.Linq;
using UnityEngine;

namespace Line
{
    public class LineDrawer : MonoBehaviour
    {
        public GridObject GridObject { get; set; }

        public bool IsSelected { get; set; }

        private void Awake()
        {
            Constants.C.lineDrawers.Add(this);
        }

        private void Update()
        {
            
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                IsSelected = !IsSelected;

                if (IsSelected)
                {
                    if (Constants.C.lineDrawers.Any(x => x != this && x.IsSelected))
                    {
                        
                    }
                }
            }
        }
    }
}
