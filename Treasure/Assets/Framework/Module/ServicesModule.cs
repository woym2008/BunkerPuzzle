using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bunker.Module
{
    public class ServicesModule<T> : BasicModule where T : ServicesModule<T>, new()
    {
        static T _instance = default(T);
        public static T getInstance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }


    }

}

