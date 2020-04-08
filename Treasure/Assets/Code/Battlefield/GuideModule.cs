using Bunker.Game;
using Bunker.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GuideModule : LogicModule
{
    private GridField _field;
    public GridField Field
    {
        get
        {
            return _field;
        }
    }

    //GridFieldControllerBase _currentController;

    public GuideModule() : base(typeof(GuideModule).ToString())
    {

    }
    public GuideModule(string name) : base(name)
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

    public override void OnStart()
    {
        base.OnStart();

        _field = new GridField();
        var areastr = string.Format("Area_{0}", 0);
        var levelstr = string.Format("Level_{0}", 1);
        _field.Load(areastr, levelstr);
        _field.OnElimination = OnElimination;

        var battlecontroller = ModuleManager.getInstance.GetModule<BattleControllerModule>();
        battlecontroller.Field = _field;
    }

    public override void OnStop()
    {
        base.OnStop();
    }

    public void Update(float dt)
    {
    }

    //public bool UseController<T>(params object[] datas) where T : GridFieldControllerBase
    //{
    //    if (_currentController != null)
    //    {
    //        if (!_currentController.IsFinish())
    //        {
    //            return false;
    //        }
    //    }
    //    var controller = Activator.CreateInstance(typeof(T)) as T;

    //    controller.SetGridField(_field);

    //    controller.Excute(datas);

    //    _currentController = controller;

    //    return true;
    //}

    void OnElimination(int num)
    {
        Debug.LogFormat("一共消除了{0}个块", num);
    }
}
