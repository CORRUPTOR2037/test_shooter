using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MeteorFactory {

    public static BaseMeteor Create (MeteorModel model) {

        BaseMeteor instance = EntitiesData.Storage.MeteorBase;
        instance = UnityEngine.Object.Instantiate(instance);
        instance.SetupModel(model);
        return instance;
    }

}
