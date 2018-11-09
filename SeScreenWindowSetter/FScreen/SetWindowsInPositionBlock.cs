using System;
using System.Collections.Generic;
using System.Linq;

namespace SeScreenWindowSetter.FScreen
{
    public class SetWindowsInPositionBlock
    {
        public SetWindowsInPositionBlock(PositionBlockState s)
        {
            var typeCheck = ScreenGridChek(s.ScreenGridConverter, s.Config.TypeTitle);
            typeCheck.
                PipeForward(ScreenGridSet.Curry()(s)).
                PipeForward(SetPartsWidthAndHight.Curry()(typeCheck)).
                PipeForward(FillSplitModel);
        }

        public static Func<PositionBlockState, PositionBlockState>
            FillSplitModel = (s) =>
             {
                 var arr = s.ScreenParts;
                 var pw = s.PW(s.MonitorInfo.Bounds.Width);
                 var ph = s.PW(s.MonitorInfo.Bounds.Height);

                 for (int i = 0; i < arr.GetLength(0); i++)
                 {
                     for (int j = 0; j < arr.GetLength(1); j++)
                     {
                         arr[i,j].X = pw * i;
                         arr[i,j].Y = ph * j;
                         arr[i,j].Width = pw;
                         arr[i,j].Height = ph;
                         arr[i,j].Processes = s.Config.Positions.Where(z => z.PositionTitle == $"{i}.{j}").FirstOrDefault().Processes;
                     }
                 }

                 return s;
             };

        public static Func<Dictionary<string, (int, int)>, string, (int, int)>
            ScreenGridChek = (dict, type) => dict.ContainsKey(type) ? dict[type] : dict.First().Value;

        public static Func<PositionBlockState, (int, int), PositionBlockState>
            ScreenGridSet = (s, t) =>
            {
                s.ScreenParts = new RectangleWithProcesses[t.Item1, t.Item2];
                return s;
            };

        public static Func<(int, int), PositionBlockState, PositionBlockState>
            SetPartsWidthAndHight = (t, s) =>
            {
                s.PH = s.LenghtSplitFunctionResolver[t.Item1];
                s.PW = s.LenghtSplitFunctionResolver[t.Item2];

                return s;
            };
    }
}
