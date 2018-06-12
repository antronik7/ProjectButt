using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    [SerializeField]
    bool canTakeDamage = true;
    [SerializeField]
    int blockHP = 5;
    [SerializeField]
    int blockScore = 1;
    [SerializeField]
    SpriteRenderer crackSprite;
    [SerializeField]
    SpriteRenderer impactSprite;
    [SerializeField]
    float impactSpriteDuration = 0.2f;
    [SerializeField]
    Vector3 positionPool;
    [SerializeField]
    float numberChunks;
    [SerializeField]
    GameObject[] chunks;

    int currentBlockHP;

    // Use this for initialization
    void Start () {
        currentBlockHP = blockHP;
        ChangeBlockVisual();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void PlaceBlock(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void DamageBlock(int damageValue, float impactSleepDuration, float playerPreviousVelocity)
    {
        if (!canTakeDamage)
            return;

        currentBlockHP -= damageValue;
        ShowImpact();
        ChangeBlockVisual();

        IEnumerator coroutine = applyDamage(impactSleepDuration, playerPreviousVelocity);
        StartCoroutine(coroutine);
    }

    public void ResetBlock()
    {
        currentBlockHP = blockHP;
        ChangeBlockVisual();
    }

    public int GetCurrentHp()
    {
        return currentBlockHP;
    }

    IEnumerator applyDamage(float impactSleepDuration, float previousVelocity)
    {
        Time.timeScale = 0.0f;

        float sleepEndTime = Time.realtimeSinceStartup + impactSleepDuration;
        while (Time.realtimeSinceStartup < sleepEndTime)
        {
            yield return 0;
        }

        HideImpact();

        if(currentBlockHP <= 0)
            DestroyBlock();

        GameManager.instance.SetVelocityPlayer(previousVelocity);

        Time.timeScale = 1;
    }

    void DestroyBlock()
    {
        LaunchChunks();

        if(blockScore > 0)
            GameManager.instance.AddScore(blockScore);

        transform.position = positionPool;
    }

    void ChangeBlockVisual()
    {
        if (!canTakeDamage)
            return;

        if (currentBlockHP <= 1)
            crackSprite.enabled = true;
        else
            crackSprite.enabled = false;
    }

    void LaunchChunks()
    {
        int currentChunkIndex = 0;

        for (int i = 0; i < numberChunks; i++)
        {
            Instantiate(chunks[currentChunkIndex], transform.position, Quaternion.identity);

            ++currentChunkIndex;
            if (currentChunkIndex >= chunks.Length)
                currentChunkIndex = 0;
        }
    }

    void ShowImpact()
    {
        impactSprite.enabled = true;
    }

    void HideImpact()
    {
        impactSprite.enabled = false;
    }
}
