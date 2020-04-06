using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Bunker.Process;

namespace Bunker.Game
{
    public class FinalBtn : MonoBehaviour
    {
        private void Awake()
        {
            GetComponentInChildren<Text>().enabled = false;
            Invoke("MakeValiable", 5);
        }

        public void Back()
        {
            ProcessManager.getInstance.Switch<TitleProcess>();
        }

        public void MakeValiable()
        {
            GetComponentInChildren<Text>().enabled = true;
            GetComponent<Button>().onClick.AddListener(Back);
        }
    }
}
