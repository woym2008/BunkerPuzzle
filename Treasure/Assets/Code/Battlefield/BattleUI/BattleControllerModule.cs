using Bunker.Game;
using Bunker.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleControllerModule : LogicModule
{
    public GridField Field
    {
        get;set;
    }
    public BattleControllerModule() : base(typeof(BattleControllerModule).ToString())
    {

    }
    public BattleControllerModule(string name) : base(name)
    {

    }

    public override void Create()
    {
        base.Create();
    }

    public override void Release()
    {
        base.Release();
    }

    public void Update(float dt)
    {
        if(_controllerStack.Count == 0)
        {
            return;
        }

        if(_currentController == null || _currentController.IsFinish())
        {
            _currentController = _controllerStack.Pop();

            _currentController.Excute();
        }
    }

    public override void OnStart(params object[] data)
    {
        base.OnStart();
    }

    public override void OnStop()
    {
        base.OnStop();

        Field = null;
    }

    GridFieldControllerBase _currentController;
    Stack<GridFieldControllerBase> _controllerStack = new Stack<GridFieldControllerBase>();

    public bool UseController<T>(params object[] datas) where T : GridFieldControllerBase
    {
        if(Field == null)
        {
            Debug.Log("Null Field");
            return false;
        }

        var controller = Activator.CreateInstance(typeof(T)) as T;
        controller.Init(datas);

        controller.SetGridField(Field);

        if (_currentController != null)
        {
            if (!_currentController.IsFinish())
            {
                if(controller.CanWaitInReadyStack)
                {
                    _controllerStack.Push(controller);
                }

                return false;
            }
        }

        _currentController = controller;
        controller.Excute();


        return true;
    }
}
