using SeScreenWindowSetter.FConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeScreenWindowSetter.FScreen
{
    public class SplitScreenState
    {
        public IntPtr HDesktop;
        public Func<List<MonitorInfo>> MonitorInfo;
        public ManagerConfigModel Config;

        public int W;
        public int H;

        public Func<int, int> PW;
        public Func<int, int> PH;

        public Rectangle[,] ScreenParts;

        public Dictionary<string, (int, int)> ScreenGridConverter = new Dictionary<string, (int, int)>();
        public Dictionary<int, Func<int, int>> LenghtSplitFunctionResolver = new Dictionary<int, Func<int, int>>();
    }
    public class SplitScreen
    {
        public SplitScreen(Func<List<MonitorInfo>> f, ManagerConfigModel c)
        {
            Init(f, c).
            PipeForward(InitScreenGridConverter).
            PipeForward(InitLenghtSplitFunctionResolver);

            string a1 = "2_Vertical";
            //var FillSplitModelCurry3 = FillSplitModel.Curry()(3840)(2110)(ScreenGridChek(ScreenGridConverter, a1).PipeForward(ScreenGridSet));

        }

        public SplitScreenState Init(Func<List<MonitorInfo>> f, ManagerConfigModel c)
        {
            return new SplitScreenState() { MonitorInfo = f, Config = c };
        }

        public static Func<SplitScreenState, SplitScreenState>
            SetWidthAndHeight = (s) =>
            {
                var t = s.MonitorInfo();
                // s.w = t.
                return s;
            };

        public static Func<int, int, Rectangle[,], int, int, Rectangle[,]>
            FillSplitModel = (w, h, arr, pw, ph) =>
             {
                 for (int i = 0; i < arr.GetLength(0); i++)
                 {
                     for (int j = 0; j < arr.GetLength(1); j++)
                     {
                         arr[j, i].X = pw * i;
                         arr[j, i].Y = ph * j;
                         arr[j, i].Width = pw;
                         arr[j, i].Height = ph;
                     }
                 }

                 return arr;
             };

        public static Func<SplitScreenState, SplitScreenState>
            InitScreenGridConverter = s =>
            {
                s.ScreenGridConverter.Add("2_Horisontal", (1, 2));
                s.ScreenGridConverter.Add("2_Vertical", (2, 1));
                s.ScreenGridConverter.Add("4", (2, 2));

                return s;
            };

        public static Func<SplitScreenState, SplitScreenState>
            InitLenghtSplitFunctionResolver = s =>
            {
                s.LenghtSplitFunctionResolver.Add(1, GetLenghtSplit1);
                s.LenghtSplitFunctionResolver.Add(2, GetLenghtSplit2);
                s.LenghtSplitFunctionResolver.Add(3, GetLenghtSplit3);

                return s;
            };

        public static Func<Func<Func<int, int>, Func<int, int>, Rectangle[,]>, SplitModel>
                SetSplit = (fill) =>
                {
                    var res = new SplitModel();
                    res.ScreenParts = fill(GetLenghtSplit2, GetLenghtSplit2);

                    return res;
                };

        public static Func<Dictionary<string, (int, int)>, string, (int, int)>
            ScreenGridChek = (dict, type) => dict.ContainsKey(type) ? dict[type] : dict.First().Value;

        public static Func<(int, int), Rectangle[,]>
            ScreenGridSet = (t) => new Rectangle[t.Item1, t.Item2];

        public static Func<int, int>
            GetLenghtSplit1 = (x) => 1;

        public static Func<int, int>
            GetLenghtSplit2 = (x) => x / 2;

        public static Func<int, int>
            GetLenghtSplit3 = (x) =>
            {
                var res = (int)Math.Floor((double)x / 3);
                return res;
            };
    }
}
