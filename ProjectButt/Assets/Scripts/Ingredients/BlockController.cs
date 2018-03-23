using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

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
    float impactSleepDuration = 0.02f;
    [SerializeField]
    Vector3 positionPool;
    [SerializeField]
    List<SpikeController> spikes = new List<SpikeController>();
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

    //probably should do it by reference there...
    public void AddSpike(SpikeController spikeController)
    {
        spikes.Add(spikeController);
    }

    public void PlaceBlock(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void DamageBlock(int damageValue)
    {
        currentBlockHP -= damageValue;
        StartCoroutine("ShowImpact");
        GameManager.instance.Sleep(impactSleepDuration);

        if (currentBlockHP <= 0)
            DestroyBlock();
        else
            ChangeBlockVisual();
    }

    public void ResetBlock()
    {
        currentBlockHP = blockHP;
        ChangeBlockVisual();
        spikes.Clear();
    }

    public int GetCurrentHp()
    {
        return currentBlockHP;
    }

    void DestroyBlock()
    {
        LaunchChunks();

        if(blockScore > 0)
            GameManager.instance.AddScore(blockScore);

        for (int i = 0; i < spikes.Count; ++i)
        {
            spikes[i].DestroySpike();
        }

        transform.position = positionPool;
    }

    void ChangeBlockVisual()
    {
        if (currentBlockHP == 1)
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

    IEnumerator ShowImpact()
    {
        impactSprite.enabled = true;
        yield return new WaitForSeconds(impactSpriteDuration);
        impactSprite.enabled = false;
    }
}
