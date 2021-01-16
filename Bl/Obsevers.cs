﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace Bl
{
    public class ObseverWatch
    {
        Action<TimeSpan> upTime;
        public ObseverWatch(Action<TimeSpan> action)
        {
            upTime = action;
            Watch.Instance.TimeChanged += TimeChanged;   

        }
        private void TimeChanged(object sender, EventArgs e)
        {
            if (!(e is TimeChangedEventArgs))
                return;
            var temp = (TimeChangedEventArgs)e;
            upTime(temp.NewTime);
        }
    }
}