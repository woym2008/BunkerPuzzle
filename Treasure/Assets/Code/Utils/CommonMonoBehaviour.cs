using UnityEngine;
using System.Collections;

public delegate void OnUpdate(float dt);
public class CommonMonoBehaviour : MonoBehaviour
{
    public event OnUpdate onupdate;
    private void Awake()
    {

    }

    private void Update()
    {
        onupdate?.Invoke(Time.deltaTime);
    }
}
