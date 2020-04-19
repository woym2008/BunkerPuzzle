using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IBattleUI
{
    void SetLevelText(int num);
    void AddItem(GameObject item);

    void AddMissionItem(GameObject item);

    void SetAPNum(float n);

    /* 0~1 */
    void SetProgressNum(float n);
    void SetBossHead(Sprite head);
    void SetBossText(string sentence);
    void ShowBossWarning(bool s);
    void ShowBossBar(bool s);
}
