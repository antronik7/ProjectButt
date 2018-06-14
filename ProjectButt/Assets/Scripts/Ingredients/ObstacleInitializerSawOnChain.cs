using UnityEngine;

public class ObstacleInitializerSawOnChain : ObstacleInitializer {
    public float offsetY = 0f;

    public override void Initialize()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
    }
}
