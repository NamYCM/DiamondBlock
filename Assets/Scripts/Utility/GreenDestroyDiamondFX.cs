using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDestroyDiamondFX : MonoBehaviour
{
    [SerializeField] Animator greenDesotryFX;

    public bool isActive => gameObject.activeSelf;

    private void Start() {
        if (!greenDesotryFX)
        {
            if (!TryGetComponent<Animator>(out greenDesotryFX))
            {
                Debug.LogWarning("Missing animator in green destroy effect");
            }
        }
    }

    public void Play() 
    {
        gameObject.SetActive (true);

        if (greenDesotryFX) 
        {
            greenDesotryFX.SetBool("isStart", true);
        }
    }

    public void ReturnPool ()
    {
        if (greenDesotryFX) greenDesotryFX.SetBool("isStart", false);
        gameObject.SetActive(false);
    }
}
