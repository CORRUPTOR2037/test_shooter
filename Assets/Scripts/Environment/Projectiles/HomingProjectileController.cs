
using UnityEngine;

class HomingProjectileController : SimpleProjectileController {

    [SerializeField]
    private float rotationSpeed;

    private void Update()
    {
        MoveToNearestEnemy();
    }

    private void MoveToNearestEnemy()
    {
        float angle = float.MaxValue, dt;
        BaseMeteor nearest = null;
        foreach (BaseMeteor meteor in LevelController.Current.Meteors) {
            if (angle > (dt = Vector3.Angle(meteor.transform.position - transform.position, transform.up))) {
                angle = dt;
                nearest = meteor;
            }
        }
        if (nearest == null) return;

        Vector3 forward = nearest.transform.position - transform.position;
        Rigidbody.AddForce(Time.deltaTime * forward);
        transform.up = Vector3.MoveTowards(transform.up, forward, Time.deltaTime * rotationSpeed);
    }
}