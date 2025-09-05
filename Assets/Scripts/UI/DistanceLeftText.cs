using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DistanceLeftText : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();

        Observable.EveryUpdate()
            .Subscribe(_ => text.text = string.Format(
                "Distance to end left: \n{0:0.0}",
                LevelController.Current.Distance - LevelController.Current.Spaceship.transform.position.y
            ))
            .AddTo(this);
    }
}
