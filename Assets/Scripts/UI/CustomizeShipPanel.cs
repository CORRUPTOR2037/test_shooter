using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeShipPanel : MonoBehaviour
{
    [SerializeField] private ButtonScriptPairSpaceship[] ShipButtons = new ButtonScriptPairSpaceship[0];
    [SerializeField] private ButtonScriptPairProjectiles[] ProjectileButtons = new ButtonScriptPairProjectiles[0];

    void Start()
    {
        foreach (var pair in ShipButtons) {
            pair.button.onClick.AddListener(() => SetSpacehsip(pair.data));
            pair.button.interactable = GameConfig.Current.Spaceship != pair.data;
        }
        foreach (var pair in ProjectileButtons) {
            pair.button.onClick.AddListener(() => SetProjectile(pair.data));
            pair.button.interactable = GameConfig.Current.Projectiles != pair.data;
        }
    }

    public void SetSpacehsip(SpaceshipModel model) {
        GameConfig.Current.Spaceship = model;
        GameConfig.Current.Save();
        foreach (var pair in ShipButtons) {
            pair.button.interactable = GameConfig.Current.Spaceship != pair.data;
        }
    }

    public void SetProjectile(ProjectileModel model) {
        GameConfig.Current.Projectiles = model;
        GameConfig.Current.Save();
        foreach (var pair in ProjectileButtons) {
            pair.button.interactable = GameConfig.Current.Projectiles != pair.data;
        }
    }


    [System.Serializable]
    private class ButtonScriptPair<T> {
        public Button button;
        public T data;
    }

    [System.Serializable]
    private class ButtonScriptPairSpaceship : ButtonScriptPair<SpaceshipModel> { }

    [System.Serializable]
    private class ButtonScriptPairProjectiles : ButtonScriptPair<ProjectileModel> { }


}
