using MyShogi.Model.Common.String;
using System.Collections.Generic;
using System.Linq;

namespace MyShogi.Model.Shogi.EngineDefine
{
    /// <summary>
    /// 『将棋神　やねうら王』の各エンジンの"engine_define.xml"を書き出すサンプル
    /// </summary>
    public static class EngineDefineSample
    {
        /// <summary>
        /// 『将棋神　やねうら王』の5つのエンジンの"engine_define.xml"を書き出す。
        /// engine/フォルダ配下の各フォルダに書き出す。
        /// </summary>
        public static void WriteEngineDefineFiles2018()
        {
            EnginePreset stdPreset(string name, string desc, long nodes, int skill, int threads) => new EnginePreset(
                name,
                desc,
                threads > 0 ?
                    new EngineOption[]
                    {
                        // スレッド数直接指定
                        new EngineOption("AutoThread_", "false"),
                        new EngineOption("Threads", $"{threads}"),
                        new EngineOption("NodesLimit", $"{nodes}"),
                        new EngineOption("DepthLimit", "0"),
                        new EngineOption("MultiPV", "1"),
                        new EngineOption("SkillLevel", $"{skill}"),
                    } :
                nodes >= 550000 ?
                    new EngineOption[]
                    {
                        //new EngineOption("AutoThread_","true"), // 自動スレッドでのスレッド割り当て
                        // →　エンジン詳細設定の値、上書きするのあまりいい挙動ではないのでやめとく。
                        // スレッドはエンジンの詳細設定に従う
                        // (MyShogiを２つ起動したいかも知れないので..)
                        new EngineOption("NodesLimit", $"{nodes}"),
                        new EngineOption("DepthLimit", "0"),
                        new EngineOption("MultiPV", "1"),
                        new EngineOption("SkillLevel", $"{skill}"),
                    } :
                nodes >= 5000 ?
                    new EngineOption[]
                    {
                        // 標準的な棋力設定オプション
                        new EngineOption("AutoThread_", "false"),
                        new EngineOption("Threads", "2"),
                        new EngineOption("NodesLimit", $"{nodes}"),
                        new EngineOption("DepthLimit", "0"),
                        new EngineOption("MultiPV", "1"),
                        new EngineOption("SkillLevel", $"{skill}"),
                    } :
                    new EngineOption[]
                    {
                        // ノード数が少なすぎると複数スレッドの探索では探索局面数が超過しがちなので、1スレッドに制限する。
                        new EngineOption("AutoThread_", "false"),
                        new EngineOption("Threads", "1"),
                        new EngineOption("NodesLimit", $"{nodes}"),
                        new EngineOption("DepthLimit", "0"),
                        new EngineOption("MultiPV", "1"),
                        new EngineOption("SkillLevel", $"{skill}"),
                    }
            );

            // 棋力調査用セット追加
            void numPreset(List<EnginePreset> list)
            {
                // log10(100000000) = 8.00
                // log10(100) = 2.00
                for (int nodediv = 120, lgnode = 8 * nodediv; lgnode >= 2 * nodediv; --lgnode)
                {
                    var nodes = (long)System.Math.Round(System.Math.Pow(10, ((double)lgnode) / nodediv));
                    for (var skill = 20; skill >= 0; --skill)
                    {
                        list.Add(stdPreset(
                            nodes >= 99500000000 ? $"{nodes / 1000000000.0:0}G_{skill}" :
                            nodes >= 9950000000 ? $"{nodes / 1000000000.0:0.0}G_{skill}" :
                            nodes >= 995000000 ? $"{nodes / 1000000000.0:0.00}G_{skill}" :
                            nodes >= 99500000 ? $"{nodes / 1000000.0:0}M_{skill}" :
                            nodes >= 9950000 ? $"{nodes / 1000000.0:0.0}M_{skill}" :
                            nodes >= 995000 ? $"{nodes / 1000000.0:0.00}M_{skill}" :
                            nodes >= 100000 ? $"{nodes / 1000.0:0}k_{skill}" :
                            $"{nodes}_{skill}",
                            "棋力調査用のプリセットです。\r\n" +
                            $"NodesLimit = {nodes}, SkillLevel = {skill}",
                            nodes,
                            skill,
                            0
                        ));
                    }
                }
            }

            // 各棋力ごとのエンジンオプション
            // (これでエンジンのdefault optionsがこれで上書きされる)
            var default_preset = new List<EnginePreset>
            {
                // -- 棋力制限なし
                new EnginePreset(
                    "将棋神",
                    "棋力制限一切なしで強さは設定された持ち時間、PCスペックに依存します。\r\n" +
                    "CPU負荷率が気になる方は、詳細設定の「スレッド数」のところで調整してください。",
                        new EngineOption[] {
                            // スレッドはエンジンの詳細設定に従う
                            new EngineOption("NodesLimit", "0"),
                            new EngineOption("DepthLimit", "0"),
                            new EngineOption("MultiPV", "1"),
                            new EngineOption("SkillLevel", "20"),

                        // 他、棋力に関わる部分は設定すべき…。
                })
            };

            // -- 段位が指定されている場合は、NodesLimitで調整する。

            // スレッド数で棋力が多少変わる。4スレッドで計測したのでこれでいく。
            // 実行環境の論理スレッド数がこれより少ない場合は、自動的にその数に制限される。

            // ここの段位は、持ち時間15分切れ負けぐらいの時の棋力。

            /*
            uuunuuunさんの実験によるとthreads = 4で、
            rating =  386.16 ln( nodes/1000) + 1198.8
            の関係があるらしいのでここからnodes数を計算。
            ratingは将棋倶楽部24のものとする。またlnは自然対数を意味する。

            二次式で近似したほうが正確らしく、uuunuuunさんいわく「この式を使ってください」とのこと。
            NodesLimit = 1000*Exp[(537-Sqrt[537^2 + 4*26.13(975-rate)]/(2*26.13))]

            Excelの式で言うと　=1000*EXP((537-SQRT(537^2+4*26.13*(975-A1)))/(2*26.13))

                4600  32565907 // 16段
                4400  16537349 // 15段
                4200   8397860 // 14段
                4000   4264532 // 13段
                3800   2165579 // 12段 // ここまで使う
                3600   1099707 // 11段
                3400    558444 // 10段
                3200	315754→283584 // 九段
                3000	144832→144007 // 八段
                2800	73475 // 七段
                2600	39959 // 六段
                2400	22885 // 五段
                2200	13648 // 四段
                2000	8410 // 三段
                1800	5325 // 二段
                1600	3450 // 初段
                1500	2799 // 1級
                1400	2281 // 2級 →　新1級
                1300	1867 // 3級
                1200	1534 // 4級 →　新2級
                1100	1266 // 5級
                1000	1048 // 6級 →　新3級
                900	870 //  7級
                800	726 //  8級 → 新4級
                700	607 //  9級
                600	509 // 10級 → 新5級

                500 428 // 11級
                400 361 // 12級 → 新6級
                300 305 // 13級
                200 258 // 14級 → 新7級
                100 219 // 15級

                以下、線形補間で外挿するなら
                0    187 // 16級 → 新8級
                -100 159 // 17級
                -200 136 // 18級 → 新9級
                -300 117 // 19級
                -400 100 // 20級 → 新10級

                // このへん、SkillLevel使わないと調整不可能。

                -500 86
                -600 74 // → 新11級(相当)
                -700 64
                -800 55 // → 新12級(相当)
                -900 48

                */

            // 8段 = R3000
            // 9段は将棋倶楽部24では存在しない(?) 初段からはR200ごとの増加なので、おそらくR3200。

            // 10段、11段、12段は、自動スレッドでのスレッド割り当て。
            // 理由1. node数が多いので、スレッド数が増えてもそこまで強さがバラけないと思われるため
            // 理由2. node数が多いので2スレッドで探索すると時間がかかるため

            foreach (var entry in new []{
                new { name = "十六段", rate =  4600, nodes = 32565907, skill = 20, threads = 0 },
                new { name = "十五段", rate =  4400, nodes = 16537349, skill = 20, threads = 0 },
                new { name = "十四段", rate =  4200, nodes =  8397860, skill = 20, threads = 0 },
                new { name = "十三段", rate =  4000, nodes =  4264532, skill = 20, threads = 0 },
                new { name = "十二段", rate =  3800, nodes =  2165579, skill = 20, threads = 0 },
                new { name = "十一段", rate =  3600, nodes =  1099707, skill = 20, threads = 0 },
                new { name =   "十段", rate =  3400, nodes =   558444, skill = 20, threads = 0 },
                new { name =   "九段", rate =  3200, nodes =   283584, skill = 20, threads = 0 },
                new { name =   "八段", rate =  3000, nodes =   144007, skill = 20, threads = 0 },
                new { name =   "七段", rate =  2800, nodes =    73475, skill = 20, threads = 0 },
                new { name =   "六段", rate =  2600, nodes =    39959, skill = 20, threads = 0 },
                new { name =   "五段", rate =  2400, nodes =    22885, skill = 20, threads = 0 },
                new { name =   "四段", rate =  2200, nodes =    13648, skill = 20, threads = 0 },
                new { name =   "三段", rate =  2000, nodes =     8410, skill = 20, threads = 0 },
                new { name =   "二段", rate =  1800, nodes =     5325, skill = 20, threads = 0 },
                new { name =   "初段", rate =  1600, nodes =     3450, skill = 20, threads = 0 },
                new { name =   "１級", rate =  1400, nodes =     2281, skill = 20, threads = 0 },
                new { name =   "２級", rate =  1200, nodes =     1534, skill = 20, threads = 0 },
                new { name =   "３級", rate =  1000, nodes =     1048, skill = 20, threads = 0 },
                new { name =   "４級", rate =   800, nodes =      726, skill = 20, threads = 0 },
                new { name =   "５級", rate =   600, nodes =      509, skill = 20, threads = 0 },
                new { name =   "６級", rate =   400, nodes =      361, skill = 20, threads = 0 },
                new { name =   "７級", rate =   200, nodes =      258, skill = 20, threads = 0 },
                new { name =   "８級", rate =     0, nodes =      187, skill = 20, threads = 0 },
                new { name =   "９級", rate =  -200, nodes =      136, skill = 20, threads = 0 },
                new { name =   "10級", rate =  -400, nodes =      100, skill = 20, threads = 0 },
                new { name = "Ｓ九段", rate =  3200, nodes =  1450000, skill = 13, threads = 2 }, // [2018/10/06 02:00] , 216-11-229 R+10
                new { name = "Ｓ八段", rate =  3000, nodes =  1200000, skill = 11, threads = 2 }, // [2018/10/03 15:30] , 257-5-251 R-4
                new { name = "Ｓ七段", rate =  2800, nodes =   500000, skill = 10, threads = 2 }, // [2018/10/05 04:45] , 301-14-300 R0
                new { name = "Ｓ六段", rate =  2600, nodes =   230000, skill =  9, threads = 2 }, // [2018/10/04 21:00] , 521-13-466 R-19
                new { name = "Ｓ五段", rate =  2400, nodes =   130000, skill =  8, threads = 2 }, // [2018/10/04 17:10] , 512-10-478 R+11
                new { name = "Ｓ四段", rate =  2200, nodes =   150000, skill =  7, threads = 2 }, // [2018/10/04 11:50] , 503-10-487 R-5
                new { name = "Ｓ三段", rate =  2000, nodes =    66000, skill =  6, threads = 2 }, // [2018/10/03 06:20] , 431-8-366 R-28
                new { name = "Ｓ二段", rate =  1800, nodes =    30000, skill =  6, threads = 2 }, // [2018/10/03 03:30] , 316-6-319 R+2
                new { name = "Ｓ初段", rate =  1600, nodes =    22000, skill =  5, threads = 2 }, // [2018/10/03 02:15] , 281-4-268 R+8
                new { name = "Ｓ１級", rate =  1400, nodes =    17800, skill =  4, threads = 2 }, // [2018/10/03 19:20] , 454-5-541 R+30
                new { name = "Ｓ２級", rate =  1200, nodes =     8000, skill =  4, threads = 2 }, // [2018/10/03 22:10] , 503-6-491 R-4
                new { name = "Ｓ３級", rate =  1000, nodes =     8000, skill =  3, threads = 2 }, // [2018/10/04 01:00] , 505-4-491 R-5
                new { name = "Ｓ４級", rate =   800, nodes =     5500, skill =  3, threads = 2 }, // [2018/10/04 02:10] , 489-7-504 R+5
                new { name = "Ｓ５級", rate =   600, nodes =     3500, skill =  3, threads = 1 }, // [2018/10/04 03:00] , 485-4-511 R+9
                new { name = "Ｓ６級", rate =   400, nodes =     2200, skill =  3, threads = 1 }, // [2018/10/04 12:20] , 486-2-512 R+9
                new { name = "Ｓ７級", rate =   200, nodes =     1500, skill =  3, threads = 1 }, // [2018/10/04 13:10] , 494-15-491 R-1
                new { name = "Ｓ８級", rate =     0, nodes =     1100, skill =  3, threads = 1 }, // [2018/10/04 14:20] , 499-12-489 R-3
                new { name = "Ｓ９級", rate =  -200, nodes =      920, skill =  3, threads = 1 }, // [2018/10/04 17:30] , 497-13-490 R-2
                new { name = "Ｓ10級", rate =  -400, nodes =      800, skill =  3, threads = 1 }, // [2018/10/04 18:30] , 486-19-495 R+3
                new { name = "Ｓ11級", rate =  -600, nodes =     1000, skill =  0, threads = 1 }, // [2018/10/04 20:30] , 733-4-363 R+22 (S10級との対局結果)
                // NodesLimit = 100、SkillLevel = 0にしてもS11級と強さがほぼ変わらない。(むしろR+50ぐらい強くなる)
                // 原因はともかく、SkillLevelで調整する以上、これ以上の弱さにすることは出来ない。

                // 初段 R1600.00
                // １級 R1465.35
                // ２級 R1390.56
                // ３級 R1311.39
                // ５級 R1203.35
                // 10級 R1140.23
                // S10級 R1143.42
                // S11級 R904.98

                // 初段 R1600
                // １級 R1465
                // ２級 R1390
                // ３級 R1310
                // ５級 R1200
                // 10級 R1140
                // 11級 R905

                // [2018/10/09 02:30] やねうら王(初段) vs やねうら王(Ｓ初段) 548-0-451 (49.9% R-0.6 ~ 59.7% R+68.4)
                // [2018/03/25 03:25] やねうら王(１級) vs やねうら王(Ｓ１級) 537-0-463 (48.8% R-8.6 ~ 58.6% R+60.2)
                // [----/--/-- --:--] やねうら王(２級) vs やねうら王(Ｓ２級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(３級) vs やねうら王(Ｓ３級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(４級) vs やねうら王(Ｓ４級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(５級) vs やねうら王(Ｓ５級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(６級) vs やねうら王(Ｓ６級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(７級) vs やねうら王(Ｓ７級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(８級) vs やねうら王(Ｓ８級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(９級) vs やねうら王(Ｓ９級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(10級) vs やねうら王(Ｓ10級) -0- (.% R-. ~ .% R+.)
                // [2018/10/07 12:24] やねうら王(初段) vs やねうら王(１級) 686-0-314 (63.9% R+99.2 ~ 73.0% R+173.2)
                // [2018/10/07 13:21] やねうら王(１級) vs やねうら王(２級) 606-0-394 (55.7% R+39.9 ~ 65.3% R+110.1)
                // [2018/10/07 14:14] やねうら王(２級) vs やねうら王(３級) 612-0-388 (56.3% R+44.2 ~ 65.9% R+114.6)
                // [2018/10/08 21:00] やねうら王(３級) vs やねうら王(４級) 593-3-404 (54.6% R+31.9 ~ 64.3% R+101.9)
                // [2018/10/07 15:02] やねうら王(３級) vs やねうら王(５級) 650-1-349 (60.3% R+72.3 ~ 69.7% R+144.4)
                // [2018/10/08 21:54] やねうら王(４級) vs やねうら王(５級) 547-4-449 (50.0% R-0.2 ~ 59.8% R+69.0)
                // [2018/10/07 10:33] やねうら王(５級) vs やねうら王(６級) 557-0-443 (50.8% R-5.4 ~ 60.6% R+74.4)
                // [2018/10/08 19:46] やねうら王(５級) vs やねうら王(７級) 589-2-409 (54.1% R+28.6 ~ 63.8% R+98.5)
                // [2018/10/07 11:19] やねうら王(５級) vs やねうら王(10級) 581-15-404 (54.0% R+28.1 ~ 63.8% R+98.5)
                // [2018/10/07 09:37] やねうら王(６級) vs やねうら王(７級) 522-5-473 (47.5% R-17.3 ~ 57.4% R+51.6)
                // [2018/10/07 08:45] やねうら王(７級) vs やねうら王(８級) 430-15-555 (38.8% R-79.3 ~ 48.6% R-9.6)
                // [2018/10/07 15:03] やねうら王(７級) vs やねうら王(10級) 549-18-433 (50.9% R+6.5 ~ 60.8% R+76.2)
                // [2018/10/07 02:55] やねうら王(８級) vs やねうら王(９級) 451-5-544 (40.4% R-67.2 ~ 50.3% R+1.9)
                // [2018/10/07 02:07] やねうら王(９級) vs やねうら王(10級) 463-50-487 (43.7% R-44.0 ~ 53.8% R+26.4)
                // [----/--/-- --:--] やねうら王(初段) vs やねうら王(Ｓ１級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(１級) vs やねうら王(Ｓ２級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(２級) vs やねうら王(Ｓ３級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(３級) vs やねうら王(Ｓ４級) -0- (.% R-. ~ .% R+.)
                // [2018/10/09 00:34] やねうら王(３級) vs やねうら王(Ｓ５級) 759-0-241 (71.5% R+159.8 ~ 79.9% R+240.2)
                // [----/--/-- --:--] やねうら王(４級) vs やねうら王(Ｓ５級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(５級) vs やねうら王(Ｓ６級) -0- (.% R-. ~ .% R+.)
                // [2018/10/08 22:35] やねうら王(５級) vs やねうら王(Ｓ10級) 699-0-301 (65.2% R+109.4 ~ 74.3% R+184.3)
                // [----/--/-- --:--] やねうら王(６級) vs やねうら王(Ｓ７級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(７級) vs やねうら王(Ｓ８級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(８級) vs やねうら王(Ｓ９級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(９級) vs やねうら王(Ｓ10級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(10級) vs やねうら王(Ｓ11級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ初段) vs やねうら王(１級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ１級) vs やねうら王(２級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ２級) vs やねうら王(３級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ３級) vs やねうら王(４級) -0- (.% R-. ~ .% R+.)
                // [2018/10/08 23:55] やねうら王(Ｓ３級) vs やねうら王(５級) 538-0-462 (48.9% R-7.9 ~ 58.7% R+61.0)
                // [----/--/-- --:--] やねうら王(Ｓ４級) vs やねうら王(５級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ５級) vs やねうら王(６級) -0- (.% R-. ~ .% R+.)
                // [2018/10/08 23:15] やねうら王(Ｓ５級) vs やねうら王(10級) 456-0-544 (40.7% R-65.2 ~ 50.5% R+3.7)
                // [----/--/-- --:--] やねうら王(Ｓ６級) vs やねうら王(７級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ７級) vs やねうら王(８級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ８級) vs やねうら王(９級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ９級) vs やねうら王(10級) -0- (.% R-. ~ .% R+.)
                // [2018/10/08 10:48] やねうら王(Ｓ初段) vs やねうら王(Ｓ１級) 722-0-278 (67.6% R+128.0 ~ 76.5% R+204.7)
                // [2018/10/08 05:45] やねうら王(Ｓ１級) vs やねうら王(Ｓ２級) 643-0-357 (59.5% R+66.7 ~ 68.9% R+138.4)
                // [2018/10/07 18:43] やねうら王(Ｓ２級) vs やねうら王(Ｓ３級) 677-0-323 (63.0% R+92.2 ~ 72.2% R+165.7)
                // [2018/10/08 11:54] やねうら王(Ｓ３級) vs やねうら王(Ｓ４級) 558-0-442 (50.9% R+6.1 ~ 60.7% R+75.2)
                // [2018/10/07 17:21] やねうら王(Ｓ３級) vs やねうら王(Ｓ５級) 603-0-397 (55.4% R+37.8 ~ 65.0% R+107.9)
                // [2018/10/08 12:44] やねうら王(Ｓ４級) vs やねうら王(Ｓ５級) 617-0-383 (56.8% R+47.8 ~ 66.4% R+118.4)
                // [----/--/-- --:--] やねうら王(Ｓ５級) vs やねうら王(Ｓ６級) -0- (.% R-. ~ .% R+.)
                // [2018/10/07 13:23] やねうら王(Ｓ５級) vs やねうら王(Ｓ７級) 611-0-389 (56.2% R+43.5 ~ 65.8% R+113.9)
                // [2018/10/07 16:07] やねうら王(Ｓ５級) vs やねうら王(Ｓ10級) 632-0-368 (58.4% R+58.6 ~ 67.9% R+129.8)
                // [----/--/-- --:--] やねうら王(Ｓ６級) vs やねうら王(Ｓ７級) -0- (.% R-. ~ .% R+.)
                // [----/--/-- --:--] やねうら王(Ｓ７級) vs やねうら王(Ｓ８級) -0- (.% R-. ~ .% R+.)
                // [2018/10/08 14:14] やねうら王(Ｓ７級) vs やねうら王(Ｓ10級) 539-0-461 (49.0% R-7.2 ~ 58.8% R+61.7)
                // [----/--/-- --:--] やねうら王(Ｓ８級) vs やねうら王(Ｓ９級) -0- (.% R-. ~ .% R+.)
                // [2018/10/07 00:27] やねうら王(Ｓ９級) vs やねうら王(Ｓ10級) 481-1-518 (43.2% R-47.3 ~ 53.1% R+21.4)
                // [2018/10/07 01:15] やねうら王(Ｓ10級) vs やねうら王(Ｓ11級) 797-1-202 (75.6% R+196.5 ~ 83.5% R+282.3)
            })
            {
                default_preset.Add(stdPreset(
                    entry.name,
                    string.IsNullOrEmpty(entry.name) ?
                        null
                    :
                    !entry.name.StartsWith("Ｓ") ?
                        $"{entry.name}ぐらいの強さ(R{entry.rate})になるように棋力を調整したものです。持ち時間、PCのスペックにほとんど依存しません。" +
                        "短い持ち時間だと切れ負けになるので持ち時間無制限での対局をお願いします。\r\n" +
                        $"NodesLimit = {entry.nodes}, SkillLevel = {entry.skill}"
                    :
                        $"{entry.name.Substring(1)}ぐらいの強さ(R{entry.rate})になるように棋力を調整したものです。" +
                        "棋力名が「Ｓ」で始まるものは、序盤が弱いのに終盤だけ強いということはなく、まんべんなく同じ強さになるように調整されています。" +
                        "思考時間は「Ｓ」のつかない同じ段位のものの10倍ぐらい必要となります。\r\n" +
                        $"NodesLimit = {entry.nodes}, SkillLevel = {entry.skill}"

                        // + "また、段・級の設定は、将棋倶楽部24基準なので町道場のそれより少し辛口の調整になっています。";
                        // あえて書くほどでもないか…。
                    ,
                    entry.nodes,
                    entry.skill,
                    entry.threads
                ));
            }

            // 棋力調査用セット追加
            numPreset(default_preset);

            var default_cpus = new List<CpuType>(new[] { CpuType.NO_SSE, CpuType.SSE2, CpuType.SSE41, CpuType.SSE42, CpuType.AVX2 });

            var default_extend = new List<ExtendedProtocol>( new[] { ExtendedProtocol.UseHashCommandExtension , ExtendedProtocol.HasEvalShareOption } );
            var default_nnue_extend = new List<ExtendedProtocol>(new[] { ExtendedProtocol.UseHashCommandExtension });
            var gps_extend = new List<ExtendedProtocol>( new[] { ExtendedProtocol.UseHashCommandExtension } );

            // EngineOptionDescriptionsは、エンジンオプション共通設定に使っているDescriptionsと共用。
            var common_setting = EngineCommonOptionsSample.CreateEngineCommonOptions(new EngineCommonOptionsSampleOptions() {
                UseEvalDir = true, // ただし、"EvalDir"オプションはエンジンごとに固有に異なる値を保持しているのが普通であるから共通オプションにこの項目を足してやる。
            });

            var default_descriptions = common_setting.Descriptions;
            var default_descriptions_nnue = new List<EngineOptionDescription>(default_descriptions);
            default_descriptions_nnue.RemoveAll(x => x.Name == "EvalShare"); // NNUEはEvalShare持ってない。

            // -- 各エンジン用の設定ファイルを生成して書き出す。

            {
                // やねうら王
                var engine_define = new EngineDefine()
                {
                    DisplayName = "やねうら王",
                    EngineExeName = "Yaneuraou2018_kpp_kkpt",
                    SupportedCpus = default_cpus ,
                    EvalMemory = 480, // KPP_KKPTは、これくらい？
                    WorkingMemory = 200 ,
                    StackPerThread = 40, // clangでコンパイルの時にstack size = 25[MB]に設定している。ここに加えてheapがスレッド当たり15MBと見積もっている。
                    Presets = default_preset,
                    DescriptionSimple = "やねうら王 2018年度版",
                    Description = "プロの棋譜を一切利用せずに自己学習で身につけた異次元の大局観。"+
                        "従来の将棋の常識を覆す指し手が飛び出すかも？",
                    DisplayOrder = 10005,
                    SupportedExtendedProtocol = default_extend,
                    EngineOptionDescriptions = default_descriptions,
                };
                EngineDefineUtility.WriteFile("engine/yaneuraou2018/engine_define.xml", engine_define);

                // 試しに実行ファイル名を出力してみる。
                //Console.WriteLine(EngineDefineUtility.EngineExeFileName(engine_define));
            }

            {
                // tanuki_sdt5
                var engine_define = new EngineDefine()
                {
                    DisplayName = "tanuki- SDT5",
                    EngineExeName = "YaneuraOu2018KPPT",
                    SupportedCpus = default_cpus,
                    EvalMemory = 850, // KPPTは、これくらい？
                    WorkingMemory = 150,
                    StackPerThread = 40, // clangでコンパイルの時にstack size = 25[MB]に設定している。ここに加えてheapがスレッド当たり15MBと見積もっている。
                    Presets = default_preset,
                    DescriptionSimple = "tanuki- SDT5版",
                    Description = "SDT5(第5回 将棋電王トーナメント)で絶対王者Ponanzaを下し堂々の優勝を果たした実力派。" +
                        "SDT5 出場名『平成将棋合戦ぽんぽこ』",
                    DisplayOrder = 10004,
                    SupportedExtendedProtocol = default_extend,
                    EngineOptionDescriptions = default_descriptions,
                };
                EngineDefineUtility.WriteFile("engine/tanuki_sdt5/engine_define.xml", engine_define);
            }

            {
                // tanuki2018
                var engine_define = new EngineDefine()
                {
                    DisplayName = "tanuki- 2018",
                    EngineExeName = "YaneuraOu2018NNUE",
                    SupportedCpus = default_cpus,
                    EvalMemory = 200, // NNUEは、これくらい？
                    WorkingMemory = 200,
                    StackPerThread = 40, // clangでコンパイルの時にstack size = 25[MB]に設定している。ここに加えてheapがスレッド当たり15MBと見積もっている。
                    Presets = default_preset,
                    DescriptionSimple = "tanuki- 2018年版",
                    Description = "WCSC28(第28回 世界コンピュータ将棋選手権)に出場した時からさらに強化されたtanuki-シリーズ最新作。" +
                        "ニューラルネットワークを用いた評価関数で、他のソフトとは毛並みの違う新時代のコンピュータ将棋。"+
                        "PC性能を極限まで使うため、CPUの温度が他のソフトの場合より上がりやすいので注意してください。",
                    DisplayOrder = 10003,
                    SupportedExtendedProtocol = default_nnue_extend,
                    EngineOptionDescriptions = default_descriptions_nnue,
                };
                EngineDefineUtility.WriteFile("engine/tanuki2018/engine_define.xml", engine_define);
            }

            {
                // qhapaq2018
                var engine_define = new EngineDefine()
                {
                    DisplayName = "Qhapaq 2018",
                    EngineExeName = "YaneuraOu2018KPPT",
                    SupportedCpus = default_cpus,
                    EvalMemory = 850, // KPPTは、これくらい？
                    WorkingMemory = 200,
                    StackPerThread = 40, // clangでコンパイルの時にstack size = 25[MB]に設定している。ここに加えてheapがスレッド当たり15MBと見積もっている。
                    Presets = default_preset,
                    DescriptionSimple = "Qhapaq 2018年版",
                    Description = "河童の愛称で知られるQhapaqの最新版。"+
                        "非公式なレーティング計測ながら2018年6月時点で堂々の一位の超強豪。",
                    DisplayOrder = 10002,
                    SupportedExtendedProtocol = default_extend,
                    EngineOptionDescriptions = default_descriptions,
                };
                EngineDefineUtility.WriteFile("engine/qhapaq2018/engine_define.xml", engine_define);
            }

            {
                // yomita2018
                var engine_define = new EngineDefine()
                {
                    DisplayName = "読み太 2018",
                    EngineExeName = "YaneuraOu2018KPPT",
                    SupportedCpus = default_cpus,
                    EvalMemory = 850, // KPPTは、これくらい？
                    WorkingMemory = 200,
                    StackPerThread = 40, // clangでコンパイルの時にstack size = 25[MB]に設定している。ここに加えてheapがスレッド当たり15MBと見積もっている。
                    Presets = default_preset,
                    DescriptionSimple = "読み太 2018年版",
                    Description = "直感精読の個性派、読みの確かさに定評あり。" +
                        "毎回、大会で上位成績を残している常連組。",
                    DisplayOrder = 10001,
                    SupportedExtendedProtocol = default_extend,
                    EngineOptionDescriptions = default_descriptions,
                };
                EngineDefineUtility.WriteFile("engine/yomita2018/engine_define.xml", engine_define);
            }

#if true
            {
                var engine_define = new EngineDefine()
                {
                    DisplayName = "駒得",
                    EngineExeName = "YaneuraOu2018_material",
                    SupportedCpus = default_cpus,
                    EvalMemory = 10,
                    WorkingMemory = 200,
                    StackPerThread = 40, // clangでコンパイルの時にstack size = 25[MB]に設定している。ここに加えてheapがスレッド当たり15MBと見積もっている。
                    Presets = new List<EnginePreset>(new[] {
                        // -- 棋力制限なし
                        new EnginePreset("駒得王",
                            "棋力制限一切なしで強さは設定された持ち時間、PCスペックに依存します。\r\n" +
                            "CPU負荷率が気になる方は、詳細設定の「スレッド数」のところで調整してください。",
                            new EngineOption[]
                            {
                                // スレッドはエンジンの詳細設定に従う
                                new EngineOption("NodesLimit","0"),
                                new EngineOption("DepthLimit","0"),
                                new EngineOption("MultiPV","1"),
                                new EngineOption("SkillLevel", "20"),
                                // 他、棋力に関わる部分は設定すべき…。
                            }
                        ),
                    }),
                    DescriptionSimple = "駒得評価関数（非公式）",
                    Description = "駒得のみの評価関数（非公式）。",
                    DisplayOrder = 721,
                    SupportedExtendedProtocol = new List<ExtendedProtocol>(new[] { ExtendedProtocol.UseHashCommandExtension }),
                    EngineOptionDescriptions = default_descriptions,
                };

                foreach (var entry in new []{
                    // 駒得評価関数では1手1500万局面を読む設定にしても棋力が殆ど伸びず、やねうら王(五段)の棋力には及ばないので、五段以上の設定は用意しない
                    // [----/--/-- --:--] 駒得(15M_20) vs やねうら王(五段) -0- (.% R-. ~ .% R+.)
                    // [----/--/-- --:--] 駒得(15M_20) vs やねうら王(Ｓ五段) -0- (.% R-. ~ .% R+.)
                    new { name =   "四段", rate =  2200, nodes = 4000000, skill = 20, threads = 0 },
                    // [2018/10/03 17:15] 駒得(4.0M_20) vs やねうら王(四段) 41-0-59 (26.3% R-178.7 ~ 56.9% R+48.2)
                    // [2018/10/03 11:40] 駒得(4.0M_20) vs やねうら王(Ｓ四段) 50-0-50 (34.5% R-111.5 ~ 65.5% R+111.5)
                    new { name =   "三段", rate =  2000, nodes = 2000000, skill = 20, threads = 0 },
                    // [2018/10/04 10:02] 駒得(2.0M_20) vs やねうら王(三段) 51-0-49 (35.4% R-104.4 ~ 66.4% R+118.7)
                    // [2018/10/-- --:--] 駒得(2.0M_20) vs やねうら王(Ｓ三段) 63-0-37 (47.1% R-20.2 ~ 77.1% R+211.2)
                    new { name =   "二段", rate =  1800, nodes = 1000000, skill = 20, threads = 0 },
                    // [2018/10/03 23:33] 駒得(1.0M_20) vs やねうら王(二段) 58-0-42 (42.1% R-55.2 ~ 72.8% R+170.9)
                    // [2018/10/03 22:25] 駒得(1.0M_20) vs やねうら王(Ｓ二段) 51-0-49 (35.4% R-104.4 ~ 66.4% R+118.7)
                    new { name =   "初段", rate =  1600, nodes =  250000, skill = 20, threads = 0 },
                    // [2018/10/03 20:25] 駒得(250k_20) vs やねうら王(初段) 44-0-56 (29.0% R-155.6 ~ 59.8% R+69.1)
                    // [2018/10/03 21:05] 駒得(250k_20) vs やねうら王(Ｓ初段) 62-0-38 (46.1% R-27.2 ~ 76.3% R+202.9)
                    new { name =   "１級", rate =  1400, nodes =  250000, skill = 19, threads = 0 },
                    // [2018/10/04 05:12] 駒得(250k_19) vs やねうら王(１級) 48-0-52 (32.6% R-126.0 ~ 63.6% R+97.3)
                    // [2018/10/04 05:42] 駒得(250k_19) vs やねうら王(Ｓ１級) 64-0-36 (48.1% R-13.1 ~ 78.0% R+219.7)
                    new { name =   "２級", rate =  1200, nodes =  120000, skill = 18, threads = 0 },
                    // [2018/10/04 06:23] 駒得(120k_18) vs やねうら王(２級) 52-0-48 (36.4% R-97.3 ~ 67.4% R+126.0)
                    // [2018/10/04 06:03] 駒得(120k_18) vs やねうら王(Ｓ２級) 67-0-33 (51.2% R+8.3 ~ 80.5% R+246.2)
                    new { name =   "３級", rate =  1000, nodes =   60000, skill = 17, threads = 0 },
                    // [2018/10/04 06:41] 駒得(100k_17) vs やねうら王(３級) 58-0-42 (42.1% R-55.2 ~ 72.8% R+170.9)
                    // [2018/10/04 07:04] 駒得(76k_17) vs やねうら王(３級) 45-0-55 (29.9% R-148.1 ~ 60.8% R+76.1)
                    // [2018/10/04 20:32] 駒得(69k_17) vs やねうら王(３級) 48-0-52 (32.6% R-126.0 ~ 63.6% R+97.3)
                    // [2018/10/04 21:07] 駒得(63k_17) vs やねうら王(３級) 52-0-48 (36.4% R-97.3 ~ 67.4% R+126.0)
                    // [2018/10/-- --:--] 駒得(58k_17) vs やねうら王(３級) 34-0-66 (20.3% R-237.2 ~ 49.8% R-1.1)
                    // [2018/10/-- --:--] 駒得(100k_17) vs やねうら王(Ｓ３級) -0- (.% R-. ~ .% R+.)
                    new { name =   "４級", rate =   800, nodes =   41368, skill = 16, threads = 0 },
                    // [2018/10/03 06:44] 駒得(73k_16) vs やねうら王(４級) 52-0-48 (36.4% R-97.3 ~ 67.4% R+126.0)
                    // [2018/10/04 07:23] 駒得(58k_16) vs やねうら王(４級) 60-0-40 (44.1% R-41.2 ~ 74.5% R+186.7)
                    // [2018/10/04 07:35] 駒得(52k_16) vs やねうら王(４級) 59-0-41 (43.1% R-48.2 ~ 73.7% R+178.7)
                    // [2018/10/04 18:25] 駒得(48k_16) vs やねうら王(４級) 55-0-45 (39.2% R-76.1 ~ 70.1% R+148.1)
                    // [2018/10/06 10:26] 駒得(41368_16) vs やねうら王(４級) 45-1-54 (30.2% R-145.3 ~ 61.3% R+79.9)
                    // [2018/10/04 19:25] 駒得(40k_16) vs やねうら王(４級) 44-0-56 (29.0% R-155.6 ~ 59.8% R+69.1)
                    // [2018/10/05 04:28] 駒得(50k_16) vs やねうら王(Ｓ４級) 61-0-39 (45.1% R-34.2 ~ 75.4% R+194.7)
                    // [2018/10/05 04:41] 駒得(46k_16) vs やねうら王(Ｓ４級) 56-0-44 (40.2% R-69.1 ~ 71.0% R+155.6)
                    // [2018/10/05 05:00] 駒得(44668_16) vs やねうら王(Ｓ４級) 62-0-38 (46.1% R-27.2 ~ 76.3% R+202.9)
                    // [2018/10/05 05:13] 駒得(42987_16) vs やねうら王(Ｓ４級) 59-0-41 (43.1% R-48.2 ~ 73.7% R+178.7)
                    // [2018/10/06 10:14] 駒得(41368_16) vs やねうら王(Ｓ４級) 56-0-44 (40.2% R-69.1 ~ 71.0% R+155.6)
                    new { name =   "５級", rate =   600, nodes =   39811, skill = 15, threads = 0 },
                    // [2018/10/03 05:54] 駒得(40k_15) vs やねうら王(５級) 54-0-46 (38.3% R-83.2 ~ 69.2% R+140.6)
                    // [2018/10/06 10:39] 駒得(39811_15) vs やねうら王(５級) 55-0-45 (39.2% R-76.1 ~ 70.1% R+148.1)
                    // [2018/10/06 10:02] 駒得(40k_15) vs やねうら王(Ｓ５級) 65-0-35 (49.1% R-6.0 ~ 78.8% R+228.4)
                    // [2018/10/06 11:45] 駒得(39811_15) vs やねうら王(Ｓ５級) 51-0-49 (35.4% R-104.4 ~ 66.4% R+118.7)
                    new { name =   "６級", rate =   400, nodes =   39811, skill = 14, threads = 0 },
                    // [2018/10/03 06:54] 駒得(40k_14) vs やねうら王(６級) 49-0-51 (33.6% R-118.7 ~ 64.6% R+104.4)
                    // [2018/10/06 16:08] 駒得(39811_14) vs やねうら王(６級) 48-0-52 (32.6% R-126.0 ~ 63.6% R+97.3)
                    // [2018/10/06 11:59] 駒得(39811_14) vs やねうら王(Ｓ６級) 57-0-43 (41.2% R-62.2 ~ 71.9% R+163.2)
                    new { name =   "７級", rate =   200, nodes =   39811, skill = 13, threads = 0 },
                    // [2018/10/03 07:05] 駒得(40k_13) vs やねうら王(７級) 43-1-56 (28.4% R-160.5 ~ 59.4% R65.8)
                    // [2018/10/06 17:46] 駒得(39811_13) vs やねうら王(７級) 56-0-44 (40.2% R-69.1 ~ 71.0% R+155.6)
                    // [2018/10/06 18:00] 駒得(39811_13) vs やねうら王(Ｓ７級) 63-0-37 (47.1% R-20.2 ~ 77.1% R+211.2)
                    new { name =   "８級", rate =     0, nodes =   39811, skill = 12, threads = 0 }, // 未調整
                    // [2018/10/06 18:11] 駒得(39811_12) vs やねうら王(８級) 53-0-47 (37.3% R-90.2 ~ 68.3% R+133.3)
                    // [2018/10/06 18:22] 駒得(39811_12) vs やねうら王(Ｓ８級) 67-0-33 (51.2% R+8.3 ~ 80.5% R+246.2)
                    // [2018/10/06 19:46] 駒得(19953_12) vs やねうら王(８級) 35-0-65 (21.2% R-228.4 ~ 50.9% R+6.0)
                    // [2018/10/06 19:46] 駒得(19953_12) vs やねうら王(Ｓ８級) 43-0-57 (28.1% R-163.2 ~ 58.8% R+62.2)
                   new { name =   "９級", rate =  -200, nodes =   39811, skill = 11, threads = 0 }, // 未調整
                    // [2018/10/06 18:34] 駒得(39811_11) vs やねうら王(９級) 50-0-50 (34.5% R-111.5 ~ 65.5% R+111.5S)
                    // [2018/10/06 18:44] 駒得(39811_11) vs やねうら王(Ｓ９級) 70-0-30 (54.3% R+30.3 ~ 82.9% R+274.7)
                    new { name =   "10級", rate =  -400, nodes =   39811, skill = 10, threads = 0 },
                    // [2018/10/03 05:21] 駒得(40k_10) vs やねうら王(10級) 44-0-56 (29.0% R-155.6 ~ 59.8% R+69.1)
                    // [2018/10/06 18:55] 駒得(39811_10) vs やねうら王(10級) 51-0-49 (35.4% R-104.4 ~ 66.4% R+118.7)
                    // [2018/10/06 19:04] 駒得(39811_10) vs やねうら王(Ｓ10級) 58-0-42 (42.1% R-55.2 ~ 72.8% R+170.9)
                    new { name =   "11級", rate =  -600, nodes =   19953, skill =  9, threads = 0 },
                    // [2018/10/06 08:04] 駒得(19953_9) vs やねうら王(初段) 3-0-97 (0.2% R-1086.1 ~ 12.4% R-339.3)
                    // [2018/10/06 07:57] 駒得(19953_9) vs やねうら王(１級) 10-0-90 (3.1% R-600.7 ~ 22.5% R-215.2)
                    // [2018/10/06 07:50] 駒得(19953_9) vs やねうら王(２級) 10-0-90 (3.1% R-600.7 ~ 22.5% R-215.2)
                    // [2018/10/06 07:42] 駒得(19953_9) vs やねうら王(３級) 19-0-81 (8.7% R-407.4 ~ 33.5% R-119.2)
                    // [2018/10/06 07:34] 駒得(19953_9) vs やねうら王(４級) 21-0-79 (10.2% R-378.5 ~ 35.8% R-101.6)
                    // [2018/10/06 07:28] 駒得(19953_9) vs やねうら王(５級) 32-0-68 (18.7% R-255.5 ~ 47.8% R-15.6)
                    // [2018/10/06 07:20] 駒得(19953_9) vs やねうら王(６級) 25-0-75 (13.1% R-328.1 ~ 40.3% R-68.6)
                    // [2018/10/06 07:12] 駒得(19953_9) vs やねうら王(７級) 36-0-64 (22.0% R-219.7 ~ 51.9% R+13.1)
                    // [2018/10/06 07:06] 駒得(19953_9) vs やねうら王(８級) 39-0-61 (24.6% R-194.7 ~ 54.9% R+34.2)
                    // [2018/10/06 06:56] 駒得(19953_9) vs やねうら王(９級) 42-0-58 (27.2% R-170.9 ~ 57.9% R+55.2)
                    // [2018/10/06 06:49] 駒得(19953_9) vs やねうら王(10級) 37-0-63 (22.9% R-211.2 ~ 52.9% R+20.2)
                    // [2018/10/06 08:22] 駒得(19953_9) vs やねうら王(Ｓ初段) 6-0-94 (1.1% R-776.9 ~ 17.9% R-275.4)
                    // [2018/10/06 08:30] 駒得(19953_9) vs やねうら王(Ｓ１級) 11-0-89 (3.6% R-570.6 ~ 23.8% R-202.5)
                    // [2018/10/06 08:39] 駒得(19953_9) vs やねうら王(Ｓ２級) 22-0-78 (10.9% R-365.0 ~ 36.9% R-93.0)
                    // [2018/10/06 08:47] 駒得(19953_9) vs やねうら王(Ｓ３級) 38-0-62 (23.7% R-202.9 ~ 53.9% R+27.2)
                    // [2018/10/06 09:16] 駒得(19953_9) vs やねうら王(Ｓ４級) 27-0-73 (14.7% R-305.7 ~ 42.4% R-52.9)
                    // [2018/10/06 09:24] 駒得(19953_9) vs やねうら王(Ｓ５級) 34-0-66 (20.3% R-237.2 ~ 49.8% R-1.1)
                    // [2018/10/06 09:32] 駒得(19953_9) vs やねうら王(Ｓ６級) 39-0-61 (24.6% R-194.7 ~ 54.9% R+34.2)
                    // [2018/10/06 06:42] 駒得(19953_9) vs やねうら王(Ｓ７級) 44-0-56 (29.0% R-155.6 ~ 59.8% R+69.1)
                    // [2018/10/06 06:34] 駒得(19953_9) vs やねうら王(Ｓ８級) 50-0-50 (34.5% R-111.5 ~ 65.5% R+111.5)
                    // [2018/10/06 06:26] 駒得(19953_9) vs やねうら王(Ｓ９級) 40-0-60 (25.5% R-186.7 ~ 55.9% R+41.2)
                    // [2018/10/06 06:19] 駒得(19953_9) vs やねうら王(Ｓ10級) 49-0-51 (33.6% R-118.7 ~ 64.6% R+104.4)
                    // [2018/10/-- --:--] 駒得(19953_9) vs やねうら王(Ｓ11級) -0- (.% R-. ~ .% R+.)
                    // [2018/10/-- --:--] 駒得(18478_9) vs やねうら王(Ｓ11級) -0- (.% R-. ~ .% R+.)
                    new { name =   "12級", rate =  -800, nodes =   11085, skill =  8, threads = 0 }, // 未調整
                    new { name =   "13級", rate = -1000, nodes =    6158, skill =  7, threads = 0 }, // 未調整
                    new { name =   "14級", rate = -1200, nodes =    3421, skill =  6, threads = 0 }, // 未調整
                    new { name =   "15級", rate = -1400, nodes =    1900, skill =  5, threads = 0 }, // 未調整
                    new { name =   "16級", rate = -1600, nodes =    1055, skill =  4, threads = 0 }, // 未調整
                    new { name =   "17級", rate = -2000, nodes =     586, skill =  3, threads = 0 }, // 未調整
                    new { name =   "18級", rate = -2200, nodes =     325, skill =  2, threads = 0 }, // 未調整
                    new { name =   "19級", rate = -2400, nodes =     180, skill =  1, threads = 0 }, // 未調整
                    new { name =   "20級", rate = -2600, nodes =     100, skill =  0, threads = 0 }, // 未調整
                    // [----/--/-- --:--] 駒得(_) vs やねうら王(Ｓ--級) -0- (.% R-. ~ .% R+.)
                })
                {
                    engine_define.Presets.Add(stdPreset(
                        entry.name,
                        $"{entry.name}(R{entry.rate})ぐらいの強さになるように棋力を調整したものです。持ち時間、PCのスペックにほとんど依存しません。\r\n" +
                            "短い持ち時間だと切れ負けになるので持ち時間無制限での対局をお願いします。\r\n" +
                            $"NodesLimit = {entry.nodes}, SkillLevel = {entry.skill}",
                        entry.nodes,
                        entry.skill,
                        entry.threads
                    ));
                }

                // 棋力調査用セット追加
                numPreset(engine_define.Presets);

                EngineDefineUtility.WriteFile("engine/material/engine_define.xml", engine_define);
            }
#endif

#if false
            {
                // gpsfish(動作テスト用) 『将棋神　やねうら王』には含めない。
                var engine_define = new EngineDefine()
                {
                    DisplayName = "gpsfish",
                    EngineExeName = "gpsfish",
                    SupportedCpus = new List<CpuType>(new[] { CpuType.SSE2 }),
                    EvalMemory = 10, // gpsfishこれくらいで動くような？
                    WorkingMemory = 100,
                    StackPerThread = 25,
                    Presets = default_preset,
                    DescriptionSimple = "GPS将棋(テスト用)",
                    Description = "いまとなっては他のソフトと比べると棋力的には見劣りがするものの、" +
                        "ファイルサイズが小さいので動作検証用に最適。",
                    DisplayOrder = 10000,
                    SupportedExtendedProtocol = gps_extend,
                    EngineOptionDescriptions = null,
                };
                EngineDefineUtility.WriteFile("engine/gpsfish/engine_define.xml", engine_define);

                //Console.WriteLine(EngineDefineUtility.EngineExeFileName(engine_define));
            }

            {
                // gpsfish2(動作テスト用) 『将棋神　やねうら王』には含めない。
                // presetの動作テストなどに用いる。
                var engine_define = new EngineDefine()
                {
                    DisplayName = "Gpsfish2",
                    EngineExeName = "gpsfish",
                    SupportedCpus = new List<CpuType>(new[] { CpuType.SSE2 }),
                    EvalMemory = 10, // gpsfishこれくらいで動くような？
                    WorkingMemory = 100,
                    StackPerThread = 25,
                    Presets = default_preset,
                    DescriptionSimple = "GPS将棋2(テスト用)",
                    Description = "presetなどのテスト用。",
                    DisplayOrder = 9999,
                    SupportedExtendedProtocol = gps_extend,
                    EngineOptionDescriptions = default_descriptions,
                };
                EngineDefineUtility.WriteFile("engine/gpsfish2/engine_define.xml", engine_define);
            }

#endif

            {
                // -- 詰将棋エンジン

                // このnamesにあるもの以外、descriptionから削除してしまう。
                var names = new[] { "AutoHash_","Hash_" ,"AutoThread_","Threads","MultiPV","WriteDebugLog","NetworkDelay","NetworkDelay2", "MinimumThinkingTime",
                    "SlowMover","DepthLimit","NodesLimit","Contempt","ContemptFromBlack","EvalDir","MorePreciseMatePV","MaxMovesToDraw"};

                // このnamesHideにあるものは隠す。
                var namesHide = new[] { "SlowMover", "Comtempt", "ContemptFromBlack" , "EvalDir"};

                var descriptions = new List<EngineOptionDescription>();
                foreach (var d in default_descriptions)
                {
                    // この見出し不要
                    if (d.DisplayName == "定跡設定" || d.DisplayName == "評価関数の設定")
                        continue;

                    if (names.Contains(d.Name) || d.Name == null /* 見出し項目なので入れておく */)
                    {
                        if (namesHide.Contains(d.Name))
                            d.Hide = true;

                        descriptions.Add(d);
                    }
                }

                {
                    var d = new EngineOptionDescription("MorePreciseMatePv", null , "なるべく正確な詰み手順を返します。",
                        "この項目をオンにすると、なるべく正確な詰み手順を返すようになります。\r\n" +
                        "オフにしていると受け方(詰みを逃れる側)が最長になるように逃げる手順になりません。\r\n" +
                        "ただし、この項目をオンにしても攻め方(詰ます側)が最短手順の詰みになる手順を選択するとは限りません。",
                        "option name MorePreciseMatePv type check default true");

                    // 「読み筋の表示」の直後に挿入
                    var index = descriptions.FindIndex((x) => x.DisplayName == "読み筋の表示") + 1;
                    descriptions.Insert(index , d);
                }

                var engine_define = new EngineDefine()
                {
                    DisplayName = "tanuki-詰将棋エンジン",
                    EngineExeName = "tanuki_mate",
                    SupportedCpus = default_cpus,
                    EvalMemory = 0, // KPPTは、これくらい？
                    WorkingMemory = 200,
                    StackPerThread = 40, // clangでコンパイルの時にstack size = 25[MB]に設定している。ここに加えてheapがスレッド当たり15MBと見積もっている。
                    Presets = new List<EnginePreset>() ,
                    DescriptionSimple = "tanuki-詰将棋エンジン",
                    Description = "長手数の詰将棋が解ける詰将棋エンジンです。\r\n" +
                        "詰手順が最短手数であることは保証されません。\r\n" +
                        "複数スレッドでの探索には対応していません。",
                    DisplayOrder = 10006,
                    SupportedExtendedProtocol = default_extend,
                    EngineOptionDescriptions = descriptions,
                    EngineType = 1, // go mateコマンドに対応している。通常探索には使えない。
                };
                EngineDefineUtility.WriteFile("engine/tanuki_mate/engine_define.xml", engine_define);
            }

        }
    }
}
