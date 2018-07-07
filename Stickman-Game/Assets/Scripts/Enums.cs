using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums {

    enum CharacterState:int
    {
        GROUNDED=100000000,
        FALLING,
        JUMPING,
        JUMP_OVER_BOX,
        CLIMB
    }

}
