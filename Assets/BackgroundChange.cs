using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();
    private SpriteRenderer renderer;

    private void Awake()
    {
        var index = Random.Range(0, backgrounds.Count);

        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = backgrounds[index];
    }
}
