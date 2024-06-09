using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace My2048
{
    public enum Swipe
    {
        NONE = -1,
        RIGHT = 0,
        UP = 1, 
        LEFT = 2,
        DOWN = 3
    }

    public static class TouchEvaluator
    {
        // Down -> Up 될 때 두 position 차이를 이용해 swipe 방향을 구한다
        // UP : 45~ 135, LEFT : 135 ~ 225, DOWN : 225 ~ 315, RIGHT : 0 ~ 45, 0 ~ 315
        public static Swipe EvalSwipeDir(Vector2 vtStart, Vector2 vtEnd)
        {
            float angle = EvalDragAngle(vtStart, vtEnd);

            if (angle < 0)
                return Swipe.NONE;
            int swipe = (((int) angle + 45) % 360) / 90;
            switch (swipe)
            {
                case 0 : return Swipe.RIGHT;
                case 1 : return Swipe.UP;
                case 2 : return Swipe.LEFT;
                case 3 : return Swipe.DOWN;
            }

            return Swipe.NONE;
        }
        
        static float EvalDragAngle(Vector2 vtStart, Vector2 vtEnd)
        {
            Vector2 dragDir = vtEnd - vtStart;
            float aimAngle = Mathf.Atan2(dragDir.y, dragDir.x);
            // Debug.Log(aimAngle);
            if (aimAngle < 0f)
            {
                aimAngle = Mathf.PI * 2 + aimAngle;
            }

            return aimAngle * Mathf.Rad2Deg;
        }
    }
    
    
}


