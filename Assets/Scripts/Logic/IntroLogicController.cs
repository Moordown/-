﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLogicController : LogicController
{
    public string NewSceneName;

    public override void StartLogic()
    {
        StartLoading(NewSceneName);
    }
}
