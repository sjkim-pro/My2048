using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace My2048
{
    public class GameTile
    {
        public int pos = -1; // 0 ~ 15
        public int number = -1;
        public GameTile(int pos)
        {
            this.pos = pos;
            this.number = -1;
        }
    }    
}

