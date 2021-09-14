using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GreenDestroyFXPool : SingletonMono<GreenDestroyFXPool>
{
    private GreenDestroyFXPool () {}
    [SerializeField] private bool expanable = true;

    public List<GreenDestroyDiamondFX> poolObjects;
    // Start is called before the first frame update
    void Awake()
    {
        poolObjects = new List<GreenDestroyDiamondFX>();
    }
    
    public GreenDestroyDiamondFX GetObject (GreenDestroyDiamondFX prefab) 
    {
        foreach (var g in poolObjects)
        {
            if (!g.isActive)
            {
                g.Play();
                return g;
            }
        }

        if (expanable) 
        {
            GreenDestroyDiamondFX g = GenerateNewObject(prefab);
            
            g.Play();

            return g;
        }
        else return null;
    }

    private GreenDestroyDiamondFX GenerateNewObject (GreenDestroyDiamondFX prefab)
    {
        GreenDestroyDiamondFX gameObject = Instantiate(prefab);
        gameObject.transform.parent = transform;
        // gameObject.Play();
        poolObjects.Add(gameObject);
        
        return gameObject;
    }
}
