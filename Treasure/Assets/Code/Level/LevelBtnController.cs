using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtnController : MonoBehaviour
{
    private Text _levelName;

    private void Awake()
    {
        _levelName = this.gameObject.GetComponentInChildren<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(string name)
    {
        if(_levelName == null)
        {
            _levelName = this.gameObject.GetComponentInChildren<Text>();
        }
        _levelName.text = name;
    }
}
