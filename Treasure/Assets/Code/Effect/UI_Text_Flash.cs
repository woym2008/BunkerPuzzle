using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Text_Flash : MonoBehaviour
{
    public Text UIText;
    private float timer;
    Color a_Color = new Color32(76, 218, 255, 255);
    Color b_Color = new Color32(76, 218, 255, 185);
    //
    [Range(0, 20)]
    public int scale = 4;
    [Range(0, 20f)]
    public float threshold = 1.0f;
    public bool rand_flash = false;

    private void Awake()
    {
        UIText = GetComponent<Text>();
    }
    // Use this for initialization
    void Start()
    {
        timer = 0.0f;
        if (rand_flash) threshold = Random.Range(0.1f, scale);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * scale;
        if (timer % scale > threshold)
        {
            UIText.color = a_Color;
        }
        else
        {
            UIText.color = b_Color;
        }
        
        if(timer > 4)
        {
            timer = 0;
            threshold = Random.Range(0.1f, scale);
        }
        
    }
}
