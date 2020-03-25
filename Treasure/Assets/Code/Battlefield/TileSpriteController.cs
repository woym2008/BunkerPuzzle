using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : MonoBehaviour
{
    SpriteRenderer _renderer;
    private void Awake()
    {
        _renderer = this.gameObject.GetComponent<SpriteRenderer>();

        _renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
}
