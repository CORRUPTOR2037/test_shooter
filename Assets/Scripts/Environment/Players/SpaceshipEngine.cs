using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpaceshipEngine : MonoBehaviour
{

    [SerializeField]
    private float animationMult = 1;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetForce(float value) {
        animator.SetFloat("Value", value * animationMult);
    }
}
