using UnityEngine;
using System.Collections;

public class GridMotionController : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToPosition(Vector3 pos, float time)
    {
        StartCoroutine(Moveto(pos, time));
    }

    IEnumerator Moveto(Vector3 pos, float time)
    {
        Vector3 oripos = this.transform.position;
        float currenttime = 0.0f;
        float tparam = 1.0f / time;
        yield return 0;

        while(currenttime < time)
        {
            currenttime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(
            oripos,
            pos,
            currenttime * tparam
                );
            yield return 0;
        }

    }
}
