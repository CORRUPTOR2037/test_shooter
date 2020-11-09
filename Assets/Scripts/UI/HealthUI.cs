using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    [SerializeField] private Image iconPrefab;
    [SerializeField] private float spaceBetween = 10;

    private void Awake() {
        if (iconPrefab == null)
            throw new NullReferenceException("Icon prefab not found");

        pointIcons.Add(iconPrefab);

        MessageBroker.Default.Receive<SpaceshipMessage>()
            .Where(ms => ms.Type == SpaceshipMessage.MessageType.Created)
            .Subscribe(ms => RecreateIcons(LevelController.Current.Spaceship.Health));

        MessageBroker.Default.Receive<SpaceshipMessage>()
            .Where(ms => ms.Type == SpaceshipMessage.MessageType.HealthChanged)
            .Subscribe(ms => SetHealthPoints(LevelController.Current.Spaceship.Health));
    }

    private List<Image> pointIcons = new List<Image>();

    private void RecreateIcons(int newSize) {
        while (pointIcons.Count > newSize) {
            Image icon = pointIcons[pointIcons.Count - 1];
            Destroy(icon.gameObject);
            pointIcons.Remove(icon);
        }
            

        while (pointIcons.Count < newSize) {
            Image icon = UnityEngine.Object.Instantiate(iconPrefab, transform);
            icon.rectTransform.anchoredPosition = iconPrefab.rectTransform.anchoredPosition +
                new Vector2((iconPrefab.rectTransform.sizeDelta.x + spaceBetween) * pointIcons.Count, 0);
            pointIcons.Add(icon);
        }

        SetHealthPoints(newSize);
    }

    private void SetHealthPoints(int HealthPoints) {
        for (int i = 0; i < pointIcons.Count; i++) {
            pointIcons[i].gameObject.SetActive(HealthPoints > i);
        }
    }
}
