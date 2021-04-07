using UnityEngine;
using UnityEditor;

public class MaskTile : MonoBehaviour
{
    Transform _renderObject;
    //Transform _transferObject;
    private void Awake()
    {
        _renderObject = this.gameObject.transform.GetChild(0);
        //_transferObject = this.gameObject.transform.GetChild(1);
    }
    public void InitMask(int scale, int sort, int direct, bool iscopy)
    {
        if(direct < 0)
        {
            if (scale > 1)
            {
                _renderObject.transform.localScale = new Vector3(
                    _renderObject.transform.localScale.x,
                    scale,
                    _renderObject.transform.localScale.z
                    );

                _renderObject.transform.localPosition = new Vector3(
                    _renderObject.transform.localPosition.x,
                    (scale - 1) * 0.5f,
                    _renderObject.transform.localPosition.z
                    );

                //if(IntoMask)
                //{
                //    _transferObject.transform.localPosition = new Vector3(
                //    _transferObject.transform.localPosition.x,
                //    (scale - 1) * 0.5f + 0.35f,
                //    _transferObject.transform.localPosition.z
                //    );
                //}
                
            }
        }
        else
        {
            //if(direct != 0)
            //{
            //    _renderObject.transform.localPosition = new Vector3(
            //        _renderObject.transform.localPosition.x,
            //        scale - 1,
            //        _renderObject.transform.localPosition.z
            //        );
            //}

            _renderObject.transform.localScale = new Vector3(
                    _renderObject.transform.localScale.x,
                    1,
                    _renderObject.transform.localScale.z
                    );

            _renderObject.transform.localPosition = new Vector3(
                    _renderObject.transform.localPosition.x,
                    0,
                    _renderObject.transform.localPosition.z
                    );

            if (iscopy)
            {
                //_transferObject.transform.localPosition = new Vector3(
                //_transferObject.transform.localPosition.x,
                //0.4f,
                //_transferObject.transform.localPosition.z
                //);
                if(direct == 1)
                {
                    _renderObject.transform.localScale = new Vector3(
                    _renderObject.transform.localScale.x,
                    scale,
                    _renderObject.transform.localScale.z
                    );

                    _renderObject.transform.localPosition = new Vector3(
                            _renderObject.transform.localPosition.x,
                            (scale - 1) * 0.5f,
                            _renderObject.transform.localPosition.z
                            );
                }
                
            }
            else
            {
                //_transferObject.transform.localPosition = new Vector3(
                //_transferObject.transform.localPosition.x,
                //- 0.4f,
                //_transferObject.transform.localPosition.z
                //);

                if (direct == 2)
                {
                    _renderObject.transform.localScale = new Vector3(
                    _renderObject.transform.localScale.x,
                    scale,
                    _renderObject.transform.localScale.z
                    );

                    _renderObject.transform.localPosition = new Vector3(
                            _renderObject.transform.localPosition.x,
                            (scale - 1) * 0.5f,
                            _renderObject.transform.localPosition.z
                            );
                }
            }

            //// up
            //if (direct == 1)
            //{
            //    _renderObject.transform.localPosition = new Vector3(
            //        _renderObject.transform.localPosition.x,
            //        scale,
            //        _renderObject.transform.localPosition.z
            //        );
            //}
            ////down
            //else if (direct == 2)
            //{
            //    _renderObject.transform.localPosition = new Vector3(
            //        _renderObject.transform.localPosition.x,
            //        0,
            //        _renderObject.transform.localPosition.z
            //        );
            //}

        }

        var sp = _renderObject.GetComponent<SpriteMask>();
        sp.backSortingOrder = sort;
        sp.frontSortingOrder = sort + 1;
    }
}