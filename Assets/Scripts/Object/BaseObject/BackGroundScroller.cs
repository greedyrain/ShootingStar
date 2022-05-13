using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private Vector2 speed;
    private Material background;
    private void Awake()
    {
        background = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        background.mainTextureOffset += speed * Time.deltaTime;
    }
}
