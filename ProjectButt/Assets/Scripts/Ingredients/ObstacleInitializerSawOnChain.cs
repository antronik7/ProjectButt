using UnityEngine;

public class ObstacleInitializerSawOnChain : ObstacleInitializer {
    public MovingSawController movingSawController;
    public float offsetY = 0f;

    public override void Initialize(float initialY)
    {
        transform.position = new Vector3(transform.position.x, initialY + offsetY, transform.position.z);
        movingSawController.ResetMovingSaw();
    }
}
