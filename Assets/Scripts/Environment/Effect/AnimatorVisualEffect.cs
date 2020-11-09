using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorVisualEffect : BaseVisualEffect {

    private new Animation animation;

    protected virtual void Awake() {
        animation = GetComponent<Animation>();
        animation.Stop();
        animation.Rewind();
    }

    public override void PlayAt(Vector2 position) {
        transform.parent = null;
        transform.position = position;
        animation.Stop();
        animation.Rewind();
        animation.Play();
    }
}
