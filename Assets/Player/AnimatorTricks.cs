using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTricks : MonoBehaviour
{
    public float RightHandWeight = 0f;

    public void ReleaseAnimationLayer(int layerIndex)
    {
        GetComponent<Animator>().SetLayerWeight(layerIndex, 0);
    }

    private void Update()
    {
        GetComponent<Animator>().SetLayerWeight(1, RightHandWeight);
    }
}
