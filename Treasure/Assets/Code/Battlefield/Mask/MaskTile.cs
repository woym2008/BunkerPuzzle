using UnityEngine;
using UnityEditor;

public class MaskTile : MonoBehaviour
{
    Transform _renderObject;
    private void Awake()
    {
        _renderObject = this.gameObject.transform.GetChild(0);
    }
    public void InitMask(int scale)
    {
        if(scale > 1)
        {
            _renderObject.transform.localScale = new Vector3(
                _renderObject.transform.localScale.x,
                scale,
                _renderObject.transform.localScale.z
                );

            _renderObject.transform.localPosition = new Vector3(
                _renderObject.transform.localPosition.x,
                (scale-1) * 0.5f,
                _renderObject.transform.localPosition.z
                );
        }
    }
}