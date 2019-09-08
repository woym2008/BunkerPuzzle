using UnityEngine;
using System.Collections;

namespace Bunker.Game
{
    abstract public class BaseGrid : IGridObject
    {
        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        protected int _x;
        protected int _y;
        protected Vector3 _zeropos;

        protected GameObject _object;
        public GameObject Node
        {
            get
            {
                return _object;
            }
        }

        string _name;


        public void CreateGrid(string name, Vector3 zeropos, int x = 0, int y = 0)
        {
            _name = name;

            var prefab = Resources.Load("Prefabs/Tiles/" + name) as GameObject;
            _object = GameObject.Instantiate(prefab);
            _x = x;
            _y = y;
            _zeropos = zeropos;
            var selfpos = _zeropos + new Vector3(_x*Constant.TileSize.x, -_y * Constant.TileSize.y, 0);

            _object.transform.position = selfpos;

            Init();
        }

        public virtual void Init()
        {

        }

        private void SetPos(int x, int y)
        {
            _x = x;
            _y = y;
        }

        virtual public bool CanMove()
        {
            return true;
        }

        virtual public void UpdateGrid(int x, int y)
        {
            if(_object != null)
            {
                SetPos(x, y);
                var selfpos = _zeropos + new Vector3(_x * Constant.TileSize.x, -_y * Constant.TileSize.y, 0);
                _object.transform.position = selfpos;
            }
        }
    }
}

