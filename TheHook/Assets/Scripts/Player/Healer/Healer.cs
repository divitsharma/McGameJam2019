﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Healer : BasePlayer
{

    // Start is called before the first frame update
    protected override void Start()
    {
        movementSpeed = 6;
        base.Start();
    }

}
