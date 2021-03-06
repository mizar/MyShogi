﻿using System;
using MyShogi.Model.Common.Math;

namespace MyShogi.Model.Test
{
    public static class SpecialFuncTest
    {
        public static void TestGammaLn()
        {
            // 対数ガンマ（Γ）関数
            foreach (var x in new[] { -3.5, -1.5, 0.1, 0.2, 0.3, 0.4, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 1000, 10000, 100000, 1000000, long.MaxValue })
            {
                Console.WriteLine($"GammaLn({x}) = {SpecialFunc.GammaLn(x)}");
            }
            /*
            GammaLn(-3.5) = -1.30900668499304
            GammaLn(-1.5) = 0.860047015376481
            GammaLn(0.1) = 2.25271265173421
            GammaLn(0.2) = 1.52406382243078
            GammaLn(0.3) = 1.09579799481808
            GammaLn(0.4) = 0.796677817701784
            GammaLn(0.5) = 0.5723649429247
            GammaLn(1) = 0
            GammaLn(1.5) = -0.120782237635245
            GammaLn(2) = 0
            GammaLn(2.5) = 0.284682870472919
            GammaLn(3) = 0.693147180559945
            GammaLn(3.5) = 1.20097360234707
            GammaLn(4) = 1.79175946922806
            GammaLn(4.5) = 2.45373657084244
            GammaLn(5) = 3.17805383034794
            GammaLn(1000) = 5905.22042320918
            GammaLn(10000) = 82099.7174964424
            GammaLn(100000) = 1051287.70897366
            GammaLn(1000000) = 12815504.5691476
            GammaLn(9.22337203685478E+18) = 3.93545350287029E+20
            */
            // 最後の検算例: http://ja.wolframalpha.com/input/?i=lGamma(2%5E63-1)
        }
        public static void TestGamma()
        {
            // ガンマ（Γ）関数
            foreach (var x in new[] { -3.5, -2.5, -1.5, -0.5, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 6 })
            {
                Console.WriteLine($"Gamma({x}) = {SpecialFunc.Gamma(x)}");
            }
            /*
            Gamma(-3.5) = 0.270088205852269
            Gamma(-2.5) = -0.945308720482942
            Gamma(-1.5) = 2.36327180120735
            Gamma(-0.5) = -3.54490770181103
            Gamma(0.5) = 1.77245385090552
            Gamma(1) = 1
            Gamma(1.5) = 0.886226925452758
            Gamma(2) = 1
            Gamma(2.5) = 1.32934038817914
            Gamma(3) = 2
            Gamma(3.5) = 3.32335097044784
            Gamma(4) = 6
            Gamma(4.5) = 11.6317283965674
            Gamma(5) = 24
            Gamma(6) = 120
            */
        }
        public static void TestGammaPQ()
        {
            foreach (var a in new[] { 0.5, 1.0, 1.5 })
                foreach (var x in new[] {
                0.01, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9,
                1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9,
                2, 3, 4, 5, 6, 7, 8, 9, 10,
            })
                {
                    Console.WriteLine($"Gamma?({a}, {x}) = P: {SpecialFunc.GammaP(a, x)} Q: {SpecialFunc.GammaQ(a, x)} P+Q: {SpecialFunc.GammaP(a, x) + SpecialFunc.GammaQ(a, x)}");
                }
            /*
            Gamma?(0.5, 0.01) = P: 0.112462916018285 Q: 0.887530713753795 P+Q: 0.99999362977208
            Gamma?(0.5, 0.1) = P: 0.345279153981423 Q: 0.654720846018553 P+Q: 0.999999999999976
            Gamma?(0.5, 0.2) = P: 0.472910743134462 Q: 0.527089256865528 P+Q: 0.99999999999999
            Gamma?(0.5, 0.3) = P: 0.561421973919 Q: 0.438578026081006 P+Q: 1.00000000000001
            Gamma?(0.5, 0.4) = P: 0.628906630477303 Q: 0.371093369522701 P+Q: 1
            Gamma?(0.5, 0.5) = P: 0.682689492137086 Q: 0.317310507862915 P+Q: 1
            Gamma?(0.5, 0.6) = P: 0.726678321707702 Q: 0.273321678292299 P+Q: 1
            Gamma?(0.5, 0.7) = P: 0.763276429362143 Q: 0.236723570637857 P+Q: 0.999999999999999
            Gamma?(0.5, 0.8) = P: 0.794096789267932 Q: 0.205903210732067 P+Q: 0.999999999999999
            Gamma?(0.5, 0.9) = P: 0.820287505121 Q: 0.179712494879 P+Q: 1
            Gamma?(0.5, 1) = P: 0.842700792949715 Q: 0.157299207050285 P+Q: 1
            Gamma?(0.5, 1.1) = P: 0.861989262431341 Q: 0.13801073756866 P+Q: 1
            Gamma?(0.5, 1.2) = P: 0.878664749641518 Q: 0.121335250358482 P+Q: 1
            Gamma?(0.5, 1.3) = P: 0.893136285006621 Q: 0.106863714993379 P+Q: 1
            Gamma?(0.5, 1.4) = P: 0.90573569315879 Q: 0.0942643068412104 P+Q: 1
            Gamma?(0.5, 1.5) = P: 0.91673548333645 Q: 0.0832645166635505 P+Q: 1
            Gamma?(0.5, 1.6) = P: 0.926361729879698 Q: 0.0736382701203026 P+Q: 1
            Gamma?(0.5, 1.7) = P: 0.93480358092187 Q: 0.06519641907813 P+Q: 1
            Gamma?(0.5, 1.8) = P: 0.942220428876403 Q: 0.0577795711235973 P+Q: 1
            Gamma?(0.5, 1.9) = P: 0.94874741714263 Q: 0.0512525828573696 P+Q: 1
            Gamma?(0.5, 2) = P: 0.954499736103642 Q: 0.0455002638963584 P+Q: 1
            Gamma?(0.5, 3) = P: 0.985694121564571 Q: 0.0143058784354297 P+Q: 1
            Gamma?(0.5, 4) = P: 0.995322265018953 Q: 0.00467773498104727 P+Q: 1
            Gamma?(0.5, 5) = P: 0.998434597741996 Q: 0.00156540225800255 P+Q: 0.999999999999999
            Gamma?(0.5, 6) = P: 0.999467994494862 Q: 0.000532005505139249 P+Q: 1
            Gamma?(0.5, 7) = P: 0.999817189367016 Q: 0.000182810632981835 P+Q: 0.999999999999998
            Gamma?(0.5, 8) = P: 0.999936657516344 Q: 6.33424836662399E-05 P+Q: 1.00000000000001
            Gamma?(0.5, 9) = P: 0.999977909502996 Q: 2.20904969985855E-05 P+Q: 0.999999999999995
            Gamma?(0.5, 10) = P: 0.999992255783628 Q: 7.74421643104408E-06 P+Q: 1.00000000000006
            Gamma?(1, 0.01) = P: 0.00995016625083195 Q: 0.990049833749168 P+Q: 1
            Gamma?(1, 0.1) = P: 0.0951625819640404 Q: 0.90483741803596 P+Q: 1
            Gamma?(1, 0.2) = P: 0.181269246922018 Q: 0.818730753077982 P+Q: 1
            Gamma?(1, 0.3) = P: 0.259181779318282 Q: 0.740818220681718 P+Q: 1
            Gamma?(1, 0.4) = P: 0.329679953964361 Q: 0.670320046035639 P+Q: 1
            Gamma?(1, 0.5) = P: 0.393469340287366 Q: 0.606530659712633 P+Q: 1
            Gamma?(1, 0.6) = P: 0.451188363905973 Q: 0.548811636094027 P+Q: 1
            Gamma?(1, 0.7) = P: 0.503414696208591 Q: 0.49658530379141 P+Q: 1
            Gamma?(1, 0.8) = P: 0.550671035882779 Q: 0.449328964117222 P+Q: 1
            Gamma?(1, 0.9) = P: 0.593430340259401 Q: 0.406569659740599 P+Q: 1
            Gamma?(1, 1) = P: 0.632120558828558 Q: 0.367879441171442 P+Q: 1
            Gamma?(1, 1.1) = P: 0.667128916301921 Q: 0.33287108369808 P+Q: 1
            Gamma?(1, 1.2) = P: 0.698805788087798 Q: 0.301194211912202 P+Q: 1
            Gamma?(1, 1.3) = P: 0.727468206965988 Q: 0.272531793034013 P+Q: 1
            Gamma?(1, 1.4) = P: 0.753403036058394 Q: 0.246596963941607 P+Q: 1
            Gamma?(1, 1.5) = P: 0.77686983985157 Q: 0.22313016014843 P+Q: 1
            Gamma?(1, 1.6) = P: 0.798103482005345 Q: 0.201896517994655 P+Q: 1
            Gamma?(1, 1.7) = P: 0.817316475947265 Q: 0.182683524052735 P+Q: 1
            Gamma?(1, 1.8) = P: 0.834701111778413 Q: 0.165298888221587 P+Q: 1
            Gamma?(1, 1.9) = P: 0.850431380777365 Q: 0.149568619222635 P+Q: 1
            Gamma?(1, 2) = P: 0.864664716763388 Q: 0.135335283236613 P+Q: 1
            Gamma?(1, 3) = P: 0.950212931632136 Q: 0.049787068367864 P+Q: 1
            Gamma?(1, 4) = P: 0.981684361111265 Q: 0.0183156388887342 P+Q: 0.999999999999999
            Gamma?(1, 5) = P: 0.993262053000916 Q: 0.00673794699908547 P+Q: 1
            Gamma?(1, 6) = P: 0.997521247823335 Q: 0.00247875217666636 P+Q: 1
            Gamma?(1, 7) = P: 0.999088118034463 Q: 0.000911881965554516 P+Q: 1.00000000000002
            Gamma?(1, 8) = P: 0.999664537372128 Q: 0.000335462627902512 P+Q: 1.00000000000003
            Gamma?(1, 9) = P: 0.999876590195872 Q: 0.00012340980408668 P+Q: 0.999999999999959
            Gamma?(1, 10) = P: 0.999954600070244 Q: 4.53999297624849E-05 P+Q: 1.00000000000001
            Gamma?(1.5, 0.01) = P: 0.000747755339391198 Q: 0.99926103440422 P+Q: 1.00000878974361
            Gamma?(1.5, 0.1) = P: 0.0224107022383506 Q: 0.977589297761546 P+Q: 0.999999999999897
            Gamma?(1.5, 0.2) = P: 0.0597575051606393 Q: 0.940242494839362 P+Q: 1
            Gamma?(1.5, 0.3) = P: 0.103567626658089 Q: 0.896432373341899 P+Q: 0.999999999999987
            Gamma?(1.5, 0.4) = P: 0.150532966608175 Q: 0.849467033391843 P+Q: 1.00000000000002
            Gamma?(1.5, 0.5) = P: 0.198748043098799 Q: 0.801251956901198 P+Q: 0.999999999999997
            Gamma?(1.5, 0.6) = P: 0.246995688343542 Q: 0.753004311656457 P+Q: 0.999999999999999
            Gamma?(1.5, 0.7) = P: 0.294465268795909 Q: 0.705534731204085 P+Q: 0.999999999999993
            Gamma?(1.5, 0.8) = P: 0.340610180288015 Q: 0.659389819711986 P+Q: 1
            Gamma?(1.5, 0.9) = P: 0.385065064217463 Q: 0.614934935782537 P+Q: 1
            Gamma?(1.5, 1) = P: 0.42759329552912 Q: 0.572406704470882 P+Q: 1
            Gamma?(1.5, 1.1) = P: 0.468051628789512 Q: 0.531948371210486 P+Q: 0.999999999999998
            Gamma?(1.5, 1.2) = P: 0.506365377288272 Q: 0.493634622711727 P+Q: 0.999999999999999
            Gamma?(1.5, 1.3) = P: 0.542510453121817 Q: 0.457489546878182 P+Q: 0.999999999999998
            Gamma?(1.5, 1.4) = P: 0.576500082944541 Q: 0.423499917055459 P+Q: 1
            Gamma?(1.5, 1.5) = P: 0.608374823728911 Q: 0.391625176271089 P+Q: 1
            Gamma?(1.5, 1.6) = P: 0.638194972502468 Q: 0.361805027497533 P+Q: 1
            Gamma?(1.5, 1.7) = P: 0.66603475090984 Q: 0.333965249090161 P+Q: 1
            Gamma?(1.5, 1.8) = P: 0.691977828441007 Q: 0.308022171558993 P+Q: 1
            Gamma?(1.5, 1.9) = P: 0.716113869240173 Q: 0.283886130759827 P+Q: 1
            Gamma?(1.5, 2) = P: 0.73853587005089 Q: 0.261464129949111 P+Q: 1
            Gamma?(1.5, 3) = P: 0.888389774905287 Q: 0.111610225094713 P+Q: 1
            Gamma?(1.5, 4) = P: 0.953988294310771 Q: 0.0460117056892313 P+Q: 1
            Gamma?(1.5, 5) = P: 0.981433864536967 Q: 0.0185661354630432 P+Q: 1.00000000000001
            Gamma?(1.5, 6) = P: 0.992616839494603 Q: 0.00738316050535977 P+Q: 0.999999999999962
            Gamma?(1.5, 7) = P: 0.997094847225775 Q: 0.00290515277426744 P+Q: 1.00000000000004
            Gamma?(1.5, 8) = P: 0.998866015710072 Q: 0.00113398428978532 P+Q: 0.999999999999858
            Gamma?(1.5, 9) = P: 0.999560150347263 Q: 0.000439849652838829 P+Q: 1.0000000000001
            Gamma?(1.5, 10) = P: 0.9998302575648 Q: 0.000169742435552826 P+Q: 1.00000000000035
            */
        }
        public static void TestErfc()
        {
            foreach (var x in new[] {
                -10, -9, -8, -7, -6, -5, -4, -3, -2, -1,
                0, 0.01, 0.1, 0.2, 0.3, 0.4, 0.5,
                0.6, 0.7, 0.8, 0.9,
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            })
            {
                Console.WriteLine($"Erfc({x}) = {SpecialFunc.Erfc(x)}");
            }
            /*
            Erfc(-10) = 2
            Erfc(-9) = 2
            Erfc(-8) = 2
            Erfc(-7) = 2
            Erfc(-6) = 2
            Erfc(-5) = 1.99999999999846
            Erfc(-4) = 1.99999998458274
            Erfc(-3) = 1.999977909503
            Erfc(-2) = 1.99532226501895
            Erfc(-1) = 1.84270079294971
            Erfc(0) = 1
            Erfc(0.01) = 0.98871658444415
            Erfc(0.1) = 0.887537083981715
            Erfc(0.2) = 0.777297410789522
            Erfc(0.3) = 0.671373240540873
            Erfc(0.4) = 0.571607644953332
            Erfc(0.5) = 0.479500122186961
            Erfc(0.6) = 0.396143909152069
            Erfc(0.7) = 0.322198806162582
            Erfc(0.8) = 0.25789903529234
            Erfc(0.9) = 0.203091787577166
            Erfc(1) = 0.157299207050285
            Erfc(2) = 0.00467773498104726
            Erfc(3) = 2.20904969985854E-05
            Erfc(4) = 1.541725790028E-08
            Erfc(5) = 1.53745979442803E-12
            Erfc(6) = 2.15197367124989E-17
            Erfc(7) = 4.18382560777942E-23
            Erfc(8) = 1.12242971729829E-29
            Erfc(9) = 4.13703174651381E-37
            Erfc(10) = 2.08848758376255E-45
            Erfc(11) = 1.44086613794369E-54
            Erfc(12) = 1.3562611692059E-64
            Erfc(13) = 1.73955731546672E-75
            Erfc(14) = 3.03722984775031E-87
            Erfc(15) = 7.21299417245121E-100
            Erfc(16) = 2.32848575157153E-113
            Erfc(17) = 1.02122801509426E-127
            Erfc(18) = 6.0823692318164E-143
            Erfc(19) = 4.91772283925648E-159
            Erfc(20) = 5.3958656116079E-176
            Erfc(21) = 8.03245387102246E-194
            Erfc(22) = 1.62190586093347E-212
            Erfc(23) = 4.44126594808806E-232
            Erfc(24) = 1.64898258315193E-252
            Erfc(25) = 8.30017257119652E-274
            Erfc(26) = 5.66319240885614E-296
            Erfc(27) = 5.23704643935263E-319
            Erfc(28) = 0
            Erfc(29) = 0
            Erfc(30) = 0
            */
        }
        public static void TestErfcx()
        {
            foreach (var x in new[] {
                0, 0.01, 0.1, 0.2, 0.3, 0.4, 0.5,
                0.6, 0.7, 0.8, 0.9,
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            })
            {
                Console.WriteLine($"Erfcx({x}) = {SpecialFunc.Erfcx(x)}");
            }
            /*
            Erfcx(0) = 1
            Erfcx(0.01) = 0.988815461046343
            Erfcx(0.1) = 0.896456979969127
            Erfcx(0.2) = 0.809019519901581
            Erfcx(0.3) = 0.734599334567655
            Erfcx(0.4) = 0.670787785294761
            Erfcx(0.5) = 0.615690344192936
            Erfcx(0.6) = 0.56780471738658
            Erfcx(0.7) = 0.525930337349441
            Erfcx(0.8) = 0.489100589223116
            Erfcx(0.9) = 0.456531651323113
            Erfcx(1) = 0.427583576155808
            Erfcx(2) = 0.255395676310506
            Erfcx(3) = 0.17900115118139
            Erfcx(4) = 0.136999457625061
            Erfcx(5) = 0.110704637733069
            Erfcx(6) = 0.0927765678005384
            Erfcx(7) = 0.079800054329153
            Erfcx(8) = 0.0699851662008809
            Erfcx(9) = 0.0623077240377747
            Erfcx(10) = 0.0561409927438226
            Erfcx(11) = 0.0510805947580884
            Erfcx(12) = 0.0468542210148938
            Erfcx(13) = 0.0432719218646097
            Erfcx(14) = 0.0401972286502185
            Erfcx(15) = 0.0375296063885058
            Erfcx(16) = 0.0351933778249308
            Erfcx(17) = 0.0331304999997255
            Erfcx(18) = 0.0312957178159052
            Erfcx(19) = 0.0296532306412622
            Erfcx(20) = 0.0281743487410513
            Erfcx(21) = 0.026835813158648
            Erfcx(22) = 0.0256185700058795
            Erfcx(23) = 0.0245068620892826
            Erfcx(24) = 0.0234875460636826
            Erfcx(25) = 0.0225495724326414
            Erfcx(26) = 0.0216835848505629
            Erfcx(27) = 0.0208816079904209
            Erfcx(28) = 0.0201368019642143
            Erfcx(29) = 0.0194432673182228
            Erfcx(30) = 0.0187958888614168
            */
        }
        public static void TestBeta()
        {
            // ベータ関数
            foreach (var x in new[] {
                new[] { 0.0, 0.0 },
                new[] { -0.5, 1.5 },
                new[] { 1.0, 1.0 },
                new[] { 2.0, 4.0 },
            })
            {
                Console.WriteLine($"Beta({x[0]}, {x[1]}) = {SpecialFunc.Beta(x[0], x[1])}");
            }
            /*
            Beta(0, 0) = NaN
            Beta(-0.5, 1.5) = -3.14159265358979
            Beta(1, 1) = 1
            Beta(2, 4) = 0.05
            */
        }
        public static void TestIBeta()
        {
            // 正則ベータ関数
            foreach (var x in new[] {
                new[] { 0.5, 1, 0.5 },
                new[] { 1, 1, 0.5 },
                new[] { 2, 3, 0.5 },
                new[] { 1, 1000, 0.5 },
                new[] { 1.5, 11, 0.001 },
                new[] { 1.5, 11, 0.5 },
                new[] { 0.1, 0.8, 0.4 },
                new[] { 0.1, 2.3, 0.4 },
                new[] { 5, 40, 0.99 },
                new[] { 5, 10, 0.99 },
                new[] { 10, 38, 0.02 },
                new[] { 70, 10, 0.85 },
                new[] { 70, 50, 0.99 },
                new[] { 70, 50, 0.10 },
                new[] { 500, 501, 0.60 },
                new[] { 1000, 1000, 0.5 },
                new[] { 1000, 1000, 0.55 },
                new[] { 1000, 1001, 0.49 },
                new[] { 300000, 300000, 0.5 },
                new[] { 300000, 300000, 0.499 },
            })
            {
                if (true)
                    Console.WriteLine($"IBeta({x[0]}, {x[1]}, {x[2]}) = {SpecialFunc.IBeta(x[0], x[1], x[2])}");
                if (x[0] != x[1] && x[2] != 0.5)
                    Console.WriteLine($"IBeta({x[1]}, {x[0]}, {1 - x[2]}) = {SpecialFunc.IBeta(x[1], x[0], 1 - x[2])}");
                if (x[0] != x[1])
                    Console.WriteLine($"IBeta({x[1]}, {x[0]}, {x[2]}) = {SpecialFunc.IBeta(x[1], x[0], x[2])}");
                if (x[2] != 0.5)
                    Console.WriteLine($"IBeta({x[0]}, {x[1]}, {1 - x[2]}) = {SpecialFunc.IBeta(x[0], x[1], 1 - x[2])}");
            }
            /*
            IBeta(0.5, 1, 0.5) = 0.707106781186548
            IBeta(1, 0.5, 0.5) = 0.292893218813452
            IBeta(1, 1, 0.5) = 0.5
            IBeta(2, 3, 0.5) = 0.6875
            IBeta(3, 2, 0.5) = 0.3125
            IBeta(1, 1000, 0.5) = 1
            IBeta(1000, 1, 0.5) = 9.33263618503219E-302
            IBeta(1.5, 11, 0.001) = 0.000891701109425656
            IBeta(11, 1.5, 0.999) = 0.999108298890574
            IBeta(11, 1.5, 0.001) = 3.86655279495811E-33
            IBeta(1.5, 11, 0.999) = 1
            IBeta(1.5, 11, 0.5) = 0.998610687098641
            IBeta(11, 1.5, 0.5) = 0.00138931290135875
            IBeta(0.1, 0.8, 0.4) = 0.887767052353014
            IBeta(0.8, 0.1, 0.6) = 0.112232947646987
            IBeta(0.8, 0.1, 0.4) = 0.0704216567316711
            IBeta(0.1, 0.8, 0.6) = 0.92957834326833
            IBeta(0.1, 2.3, 0.4) = 0.974489768373613
            IBeta(2.3, 0.1, 0.6) = 0.0255102316263873
            IBeta(2.3, 0.1, 0.4) = 0.00803415137115995
            IBeta(0.1, 2.3, 0.6) = 0.99196584862884
            IBeta(5, 40, 0.99) = 1
            IBeta(40, 5, 0.01) = 1.305304681141E-75
            IBeta(40, 5, 0.99) = 0.999921520035897
            IBeta(5, 40, 0.01) = 7.84799641027022E-05
            IBeta(5, 10, 0.99) = 1
            IBeta(10, 5, 0.01) = 9.6509742715001E-18
            IBeta(10, 5, 0.99) = 0.999999814310572
            IBeta(5, 10, 0.01) = 1.85689428386355E-07
            IBeta(10, 38, 0.02) = 2.69444356133061E-08
            IBeta(38, 10, 0.98) = 0.999999973055564
            IBeta(38, 10, 0.02) = 3.13767129310589E-56
            IBeta(10, 38, 0.98) = 1
            IBeta(70, 10, 0.85) = 0.23472449416824
            IBeta(10, 70, 0.15) = 0.76527550583176
            IBeta(10, 70, 0.85) = 1
            IBeta(70, 10, 0.15) = 1.03377375070382E-47
            IBeta(70, 50, 0.99) = 1
            IBeta(50, 70, 0.01) = 5.42790707316285E-67
            IBeta(50, 70, 0.99) = 1
            IBeta(70, 50, 0.01) = 4.70830540683411E-107
            IBeta(70, 50, 0.1) = 4.74387748621261E-39
            IBeta(50, 70, 0.9) = 1
            IBeta(50, 70, 0.1) = 8.76731215367684E-20
            IBeta(70, 50, 0.9) = 1
            IBeta(500, 501, 0.6) = 0.99999999993299
            IBeta(501, 500, 0.4) = 6.70097701348269E-11
            IBeta(501, 500, 0.6) = 0.99999999989852
            IBeta(500, 501, 0.4) = 1.01480303844161E-10
            IBeta(1000, 1000, 0.5) = 0.499999999999047
            IBeta(1000, 1000, 0.55) = 0.999996316801131
            IBeta(1000, 1000, 0.45) = 3.68319886900009E-06
            IBeta(1000, 1001, 0.49) = 0.191531104395437
            IBeta(1001, 1000, 0.51) = 0.808468895604563
            IBeta(1001, 1000, 0.49) = 0.179574214467526
            IBeta(1000, 1001, 0.51) = 0.820425785532474
            IBeta(300000, 300000, 0.499) = 0.0606675165803856
            IBeta(300000, 300000, 0.501) = 0.939332483419614
            IBeta(300000, 300001, 0.499) = 0.0608226408876596
            IBeta(300001, 300000, 0.501) = 0.93917735911234
            IBeta(300001, 300000, 0.499) = 0.0605123922823503
            IBeta(300000, 300001, 0.501) = 0.93948760771765
            */
            // IBeta(300000, 300000, 0.499) = 0.060667516591632372561341851490575695093977031968414704948...
            // http://ja.wolframalpha.com/input/?i=beta(0.499,300000,300000)%2Fbeta(300000,300000)
        }
        public static void TestInvBeta()
        {
            // ベータ逆累積分布関数
            foreach (var x in new[] {
                new[] { 0.5, 1, 0.5 },
                new[] { 1, 1, 0.5 },
                new[] { 2, 3, 0.5 },
                new[] { 3, 2, 0.5 },
                new[] { 1, 1000, 0.000001 },
                new[] { 1000, 1, 0.000001 },
                new[] { 1, 10000, 0.000000001 },
                new[] { 10000, 1, 0.000000001 },
                new[] { 1000, 1000, 0.1 },
                new[] { 1000, 1000, 0.001 },
                new[] { 1000, 1000, 0.000001 },
                new[] { 1000, 1000, 0.000000001 },
            })
            {
                Console.WriteLine($"InvBeta({x[0]}, {x[1]}, {x[2]}) = {SpecialFunc.InvBeta(x[0], x[1], x[2])} :: {SpecialFunc.IBeta(x[0], x[1], SpecialFunc.InvBeta(x[0], x[1], x[2]))}");
            }
            /*
            InvBeta(0.5, 1, 0.5) = 0.25 :: 0.5
            InvBeta(1, 1, 0.5) = 0.5 :: 0.5
            InvBeta(2, 3, 0.5) = 0.38572756813239 :: 0.5
            InvBeta(3, 2, 0.5) = 0.61427243186761 :: 0.5
            InvBeta(1, 1000, 1E-06) = 1.00000047131843E-09 :: 1.00000002734024E-06
            InvBeta(1000, 1, 1E-06) = 0.98627948563121 :: 1.00000000000001E-06
            InvBeta(1, 10000, 1E-09) = 9.99755833674953E-14 :: 9.99200722162641E-10
            InvBeta(10000, 1, 1E-09) = 0.997929819202528 :: 1.00000000000106E-09
            InvBeta(1000, 1000, 0.1) = 0.485672969031551 :: 0.100000000000002
            InvBeta(1000, 1000, 0.001) = 0.465487051520944 :: 0.000999999999999957
            InvBeta(1000, 1000, 1E-06) = 0.446998268074395 :: 9.99999999999962E-07
            InvBeta(1000, 1000, 1E-09) = 0.433234632477568 :: 9.9999999999986E-10
            */
        }

        public static void TestAll()
        {
            TestGammaLn();
            TestGamma();
            TestGammaPQ();
            TestErfc();
            TestErfcx();
            TestBeta();
            TestIBeta();
            TestInvBeta();
            ;
        }
    }
}
