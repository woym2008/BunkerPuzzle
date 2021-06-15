using Bunker.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bunker.Game
{
    /// <summary>
    /// 激光块 发射激光
    /// </summary>
    public class LaserTile : BaseTile
    {
        protected override int TileSize { get { return 2; } }

        LaserEmitter _laserEmitter;

        BattleTurnsModule _turnmodule;

        Vector2Int _dir = new Vector2Int(1, 0);

        public override void Init(string additionalData)
        {
            base.Init(additionalData);

            
            _turnmodule = ModuleManager.getInstance.GetModule<BattleTurnsModule>();
            _turnmodule.Notifications += OnNextTurn;

            var emitterObj = Node.transform.Find("laser_cube/emitter");
            _laserEmitter = emitterObj.GetComponent<LaserEmitter>();

            if (_laserEmitter == null)
            {
                _laserEmitter = emitterObj.gameObject.AddComponent<LaserEmitter>();
            }

            _laserEmitter.AddBlockTileType(typeof(BlockTile));
            _laserEmitter.AddBlockTileType(typeof(LockTile));
            _laserEmitter.AddBlockTileType(typeof(GemTile));
            _laserEmitter.AddBlockTileType(typeof(BombTile));

            _laserEmitter.AddBlockTileType(typeof(LaserTile));
            _laserEmitter.AddBlockTileType(typeof(DiskTile));

            _laserEmitter.AddBlockTileType(typeof(UpDirectTile));
            _laserEmitter.AddBlockTileType(typeof(DownDirectTile));
            _laserEmitter.AddBlockTileType(typeof(LeftDirectTile));
            _laserEmitter.AddBlockTileType(typeof(RightDirectTile));

            switch (additionalData)
            {
                case "up":
                    {
                        _dir = new Vector2Int(0, -1);
                        _laserEmitter.transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    break;
                case "down":
                    {
                        _dir = new Vector2Int(0, 1);
                        _laserEmitter.transform.localEulerAngles = new Vector3(0, 0, 180);
                    }
                    break;
                case "left":
                    {
                        _dir = new Vector2Int(-1, 0);
                        _laserEmitter.transform.localEulerAngles = new Vector3(0, 0, 90);
                    }
                    break;
                case "right":
                    {
                        _dir = new Vector2Int(1, 0);
                        _laserEmitter.transform.localEulerAngles = new Vector3(0, 0, -90);
                    }
                    break;
            }
        }

        public override void OnStart()
        {
            base.OnStart();

            _laserEmitter.Init();
        }

        public override void OnDestroy()
        {
            _turnmodule = ModuleManager.getInstance.GetModule<BattleTurnsModule>();
            _turnmodule.Notifications -= OnNextTurn;

            base.OnDestroy();

        }

        //开始移动的时间点
        public override void MoveTo(int x, int y, float movetime, int direct, bool usecopy = false)
        {
            base.MoveTo(x, y, movetime, direct, usecopy);

            _laserEmitter.Stop();
        }

        void OnNextTurn(CTurn turn)
        {
            if(turn is PlayerTurn)
            {
                _laserEmitter.Emit(this.ParentGrid, _dir);
            }
        }
    }
}

    