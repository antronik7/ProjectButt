using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    [SerializeField]
    int blockHP = 3;
    [SerializeField]
    SpriteRenderer[] blockSprites;

    int blockCurrentHP;

    // Use this for initialization
    void Start () {
        blockCurrentHP = blockHP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DamageBlock(int damageValue)
    {
        blockCurrentHP -= damageValue;

        if (blockCurrentHP <= 0)
            DestroyBlock();
        else
            ChangeBlockVisual();
    }

    void DestroyBlock()
    {
        Destroy(gameObject);
    }

    void ChangeBlockVisual()
    {
        for (int i = 0; i < blockSprites.Length; ++i)
        {
            blockSprites[i].color = new Color(1f, 1f, 1f, (float)blockCurrentHP / (float)blockHP);
        }
    }
}
