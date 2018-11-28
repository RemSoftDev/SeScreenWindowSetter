using SeScreenWindowSetter.FConfig;
using SeScreenWindowSetter.FScreen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeScreenWindowSetter.FState
{
    public static class BridgeConfigAndScreenInfo
    {
        public static Func<
            Func<MonitorInfo, FConfig.GridType, PositionBlockState>,
            List<MonitorInfo>,
            ManagerConfigModel,
                List<PositionBlockState>>
            Init =
            (fs, mi, c) =>
        {
            var res = mi.
                    SelectMany(z => GetPositionsForScreen(fs, z, c)).
                    ToList();

            return res;
        };

        private static Func<Func<MonitorInfo, FConfig.GridType, PositionBlockState>, MonitorInfo, ManagerConfigModel, List<PositionBlockState>>
            GetPositionsForScreen =
            (fs, mi, c) =>
        {
            var res = new List<PositionBlockState>();

            var t = c.
                    Screens.
                    Where(z => z.ScreenNumber == mi.ScreenNumber)
                    .FirstOrDefault()?.
                    Types.
                    Select(x => fs(mi, x));

            if (t != null)
            {
                res.AddRange(t);
            }

            return res;
        };
    }
}
