using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    [SerializeField]
    int blockHP = 5;
    [SerializeField]
    int blockScore = 1;
    [SerializeField]
    SpriteRenderer[] blockSprites;
    [SerializeField]
    Vector3 positionPool;
    [SerializeField]
    List<SpikeController> spikes = new List<SpikeController>();
    int currentBlockHP;

    // Use this for initialization
    void Start () {
        currentBlockHP = blockHP;
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
        for (int i = 0; i < blockSprites.Length; ++i)
        {
            blockSprites[i].color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public int GetCurrentHp()
    {
        return currentBlockHP;
    }

    void DestroyBlock()
    {
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
        for (int i = 0; i < blockSprites.Length; ++i)
        {
            float alpha = (float)currentBlockHP / (float)blockHP;
            blockSprites[i].color = new Color(1f, 1f, 1f, alpha);

            if (i < spikes.Count)
                spikes[i].ChangeBlockVisual(alpha);
        }
    }
}
