using System;
using UnityEngine;
using Zenject;
using Screen = UnityEngine.Device.Screen;

namespace Features.App.Controllers
{
    public class ScreenSleepController
    {
        public void SetAlwaysOn()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void SetDefault()
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
    }
}