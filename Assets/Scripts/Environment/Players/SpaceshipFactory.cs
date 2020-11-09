using UnityEngine;

public class SpaceshipFactory
{

    public static SpaceshipController CreateSpaceship(SpaceshipModel model) {

        SpaceshipController controller = EntitiesData.Storage.SpaceshipBase;

        if (controller == null)
            throw new UnityException("Cannot find Player base prefab");

        controller = Object.Instantiate(controller);
        controller.SetupModel(model);

        return controller;
    }

    public static SpaceshipGun CreateGun(SpaceshipModel model) {
        return Object.Instantiate(EntitiesData.Storage.SpaceshipGun);
    }

    public static SpaceshipEngine CreateEngine(SpaceshipModel model) {
        return Object.Instantiate(EntitiesData.Storage.SpaceshipEngine);
    }
}
