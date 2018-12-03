using FP.SeScreenWindowSetter;
using SeScreenWindowSetter.FScreen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeScreenWindowSetter.FState
{
    public class SetWindowsInPositionBlock
    {
        public static Func<List<RectangleWithProcesses[,]>, PositionBlockState, List<RectangleWithProcesses[,]>>
            Init = (l, s) =>
        {
            var t = s.PipeForward(ScreenGridChek).
                       PipeForward(ScreenGridSet).
                       PipeForward(SetPartsWidthAndHight).
                       PipeForward(FillSplitModel).
                       ScreenParts;
            l.Add(t);
            return l;
        };

        public static Func<PositionBlockState, PositionBlockState>
            FillSplitModel = (s) =>
             {
                 var arr = s.ScreenParts;
                 var pw = s.PH(s.MonitorInfo.Bounds.Width);
                 var ph = s.PW(s.MonitorInfo.Bounds.Height);

                 for (int i = 0; i < arr.GetLength(0); i++)
                 {
                     for (int j = 0; j < arr.GetLength(1); j++)
                     {
                         arr[i, j].X = pw * i;
                         arr[i, j].Y = ph * j;
                         arr[i, j].Width = ph;
                         arr[i, j].Height = pw;
                         //arr[i, j].Processes = s.Config.Positions.Where(z => z.PositionTitle == $"{i}.{j}").FirstOrDefault()?.Processes;
                     }
                 }

                 return s;
             };

        public static Func<PositionBlockState, PositionBlockState>
            ScreenGridChek = (s) =>
            {
                var type = s.Config.TypeTitle;
                var dict = s.ScreenGridConverter;
                s.ScreenGridDimension = dict.ContainsKey(type) ? dict[type] : dict.First().Value;
                return s;
            };

        public static Func<PositionBlockState, PositionBlockState>
            ScreenGridSet = (s) =>
            {
                s.ScreenParts = new RectangleWithProcesses[s.ScreenGridDimension.Item1, s.ScreenGridDimension.Item2];
                return s;
            };

        public static Func<PositionBlockState, PositionBlockState>
            SetPartsWidthAndHight = (s) =>
            {
                s.PH = s.LenghtSplitFunctionResolver[s.ScreenGridDimension.Item1];
                s.PW = s.LenghtSplitFunctionResolver[s.ScreenGridDimension.Item2];

                return s;
            };
    }
}
