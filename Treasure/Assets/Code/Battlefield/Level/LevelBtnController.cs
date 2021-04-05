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

    public void SetName(string name)
    {
        if(_levelName == null)
        {
            _levelName = this.gameObject.GetComponentInChildren<Text>();
        }
        _levelName.text = name;
    }
}
