using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace My2048
{
    public interface IInputHandlerBase
    {
        bool isInputUp { get; }
        bool isInputDown { get; }
        Vector2 inputPos { get; } // Screen Pixel 좌표
    }
}

