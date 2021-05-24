using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour
{
    public EffectModule module;
    public string type;
    public SpriteRenderer renderer;

    public bool AutoRelease = false;

    private void Awake()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Release()
    {
        module.ReleaseEffect(type, this.gameObject);
    }

    public void SetAutoRelease(float time)
    {
        AutoRelease = true;
        StartCoroutine(TimeCount(time));
    }

    IEnumerator TimeCount(float t)
    {
        yield return new WaitForSeconds(t);
        AutoRelease = false;
        Release();
    }
}
