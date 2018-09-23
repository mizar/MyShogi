---
title: 特殊関数・初等関数の計算メモ
---

# 所

２者間の勝敗数からレーティング差の区間推定を行う計算を、二項累積分布と正則ベータ関数の関係を用いようとした際のまとめ書きです。

# 指数関数・対数関数

- [ja.wikipedia: 指数関数](https://ja.wikipedia.org/wiki/%E6%8C%87%E6%95%B0%E9%96%A2%E6%95%B0)
- [ja.wikipedia: 底に関する指数函数](https://ja.wikipedia.org/wiki/%E5%BA%95%E3%81%AB%E9%96%A2%E3%81%99%E3%82%8B%E6%8C%87%E6%95%B0%E5%87%BD%E6%95%B0)
- [en.wikipedia: Exponential function](https://en.wikipedia.org/wiki/Exponential_function)
- [ja.wikipedia: 冪乗](https://ja.wikipedia.org/wiki/%E5%86%AA%E4%B9%97)
- [en.wikipedia: Exponentiation](https://en.wikipedia.org/wiki/Exponentiation)
- [ja.wikipedia: 対数](https://ja.wikipedia.org/wiki/%E5%AF%BE%E6%95%B0)
- [en.wikipedia: Logarithm](https://en.wikipedia.org/wiki/Logarithm)
- [ja.wikipedia: 自然対数](https://ja.wikipedia.org/wiki/%E8%87%AA%E7%84%B6%E5%AF%BE%E6%95%B0)
- [en.wikipedia: Natural logarithm](https://en.wikipedia.org/wiki/Natural_logarithm)

```math
\begin{aligned}
e^x&=1+x+\frac{x^2}{2!}+\frac{x^3}{3!}+\frac{x^4}{4!}+\cdots\\
&=1+x\left(1+\frac{x}{2}\left(1+\frac{x}{3}\left(1+\frac{x}{4}\left(1+\cdots\right)\right)\right)\right)\\
e^x-1&=x+\frac{x^2}{2!}+\frac{x^3}{3!}+\frac{x^4}{4!}+\cdots\\
&=x\left(1+\frac{x}{2}\left(1+\frac{x}{3}\left(1+\frac{x}{4}\left(1+\cdots\right)\right)\right)\right)\\
e^{2x}-1&=(e^x-1)^2+2(e^x-1)\\
\ln(1+x)&=x-\frac{1}{2}x^2+\frac{1}{3}x^3-\frac{1}{4}x^4+\cdots\qquad(-1 < x\le 1)\\
\ln(1+x)&=\ln\left(\frac{1+y}{1-y}\right)=2\left(y+\frac{1}{3}y^3+\frac{1}{5}y^5+\frac{1}{7}y^7+\cdots\right)\qquad\left(-1 < y =\frac{x}{x+2} < 1\right)\\
\ln(2)&=3\,\ln\left(1+\frac{1}{80}\right)+5\,\ln\left(1+\frac{1}{24}\right)+7\,\ln\left(1+\frac{1}{15}\right)\\
&=3\,\ln\left(\frac{1+\frac{1}{161}}{1-\frac{1}{161}}\right)+5\ln\left(\frac{1+\frac{1}{49}}{1-\frac{1}{49}}\right)+7\,\ln\left(\frac{1+\frac{1}{31}}{1-\frac{1}{31}}\right)\\
\ln\left(a\cdot 2^n\right)&=\ln(a)+\ln\left(2^n\right) =\ln(a)+n\,\ln(2)\\
\ln\left(e^a+e^b\right)&=a+\ln\left(1+e^{b-a}\right)=b+\ln\left(1+e^{a-b}\right)\\
\ln\left(e^a-e^b\right)&=a+\ln\left(1-e^{b-a}\right)\\
\end{aligned}
```

## ロジット

- [ja.wikipedia: ロジット](https://ja.wikipedia.org/wiki/%E3%83%AD%E3%82%B8%E3%83%83%E3%83%88)
- [en.wikipedia: Logit](https://en.wikipedia.org/wiki/Logit)
- [ja.wikipedia: ロジスティック方程式](https://ja.wikipedia.org/wiki/%E3%83%AD%E3%82%B8%E3%82%B9%E3%83%86%E3%82%A3%E3%83%83%E3%82%AF%E9%96%A2%E6%95%B0)
- [en.wikipedia: Logistic function](https://en.wikipedia.org/wiki/Logistic_function)

対数の底は$1$より大きければ何でも良いが、ここでは特に明示のない限り自然対数の底 $e$ を用いることとする。

```math
\begin{aligned}
\operatorname{logit}(p)&=\log\left(\frac{p}{1-p}\right)=\log(p)-\log(1-p)=-\log\left(\frac{1}{p}-1\right)\\
\operatorname{logit}^{-1}(\alpha)&=\operatorname{logistic}(\alpha)=\frac{1}{1+\exp(-\alpha)}=\frac{\exp(\alpha)}{\exp(\alpha)+1}\\
\log(R)&=\log\left(\frac{p_1/(1-p_1)}{p_2/(1-p_2)}\right)=\log\left(\frac{p_1}{1-p_1}\right)-\log\left(\frac{p_2}{1-p_2}\right)=\operatorname{logit}(p_1)-\operatorname{logit}(p_2)\\
\end{aligned}
```

## イロレーティング

- [ja.wikipedia: イロレーティング](https://ja.wikipedia.org/wiki/%E3%82%A4%E3%83%AD%E3%83%AC%E3%83%BC%E3%83%86%E3%82%A3%E3%83%B3%E3%82%B0)
- [en.wikipedia: Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system)

ロジスティック関数の一種を用いて定義されており、ロジットとイロレーティングの値は一次関数を用いて変換できる。

```math
\begin{aligned}
E_A&=\frac{1}{1+10^{(R_B-R_A)/400}}=\frac{Q_A}{Q_A+Q_B}\\
E_B&=\frac{1}{1+10^{(R_A-R_B)/400}}=\frac{Q_B}{Q_A+Q_B}\\
E_A+E_B&=1\\
Q_A&=10^{R_A/400}\\
Q_B&=10^{R_B/400}\\
\end{aligned}
```

# 三角関数

- [ja.wikipedia: 三角関数](https://ja.wikipedia.org/wiki/%E4%B8%89%E8%A7%92%E9%96%A2%E6%95%B0)
- [en.wikipedia: Trigonometric functions](https://en.wikipedia.org/wiki/Trigonometric_functions)

```math
\begin{aligned}
\sin z&=\sum_{n=0}^\infty\frac{(-1)^n}{(2n+1)!}z^{2n+1}\quad\text{for all $z$}\\
\cos z&=\sum_{n=0}^\infty\frac{(-1)^n}{(2n)!}z^{2n}\quad\text{for all $z$}\\
\sin(x\pm y)&=\sin x\cos y\pm\cos x\sin y\\
\cos(x\pm y)&=\cos x\cos y\mp\sin x\sin y\\
\sin(2x)&=2\sin x\cos x\\
\cos(2x)&=\cos^2x-\sin^2x=2\cos^2x-1=1-\sin^2x\\
\sin(3x)&=3\sin x-4\sin^3 x\\
\cos(3x)&=4\cos^3x-3\cos x\\
\sin^2\frac{x}{2}&=\frac{1-\cos x}{2}\\
\cos^2\frac{x}{2}&=\frac{1+\cos x}{2}\\
\tan^2\frac{x}{2}&=\frac{1-\cos x}{1+\cos x}\\
\end{aligned}
```

# 階乗

- [ja.wikipedia: 階乗](https://ja.wikipedia.org/wiki/%E9%9A%8E%E4%B9%97)
- [en.wikipedia: Factorial](https://en.wikipedia.org/wiki/Factorial)

```math
\begin{aligned}
n!&=\prod_{k=1}^nk=n(n-1)(n-2)\cdots 2\cdot 1\\
0!&=1\\
1!&=1\\
2!&=2\\
3!&=6\\
4!&=24\\
5!&=120\\
6!&=720\\
7!&=5040\\
\end{aligned}
```

# 二重階乗

- 二重階乗 (double fractorial) / 半階乗 (semifactorial)
- [ja.wikipedia: 二重階乗](https://ja.wikipedia.org/wiki/%E4%BA%8C%E9%87%8D%E9%9A%8E%E4%B9%97)
- [en.wikipedia: Double factorial](https://en.wikipedia.org/wiki/Double_factorial)

```math
\begin{aligned}
n!!&=\prod_{k=0}^{\left\lceil\frac{n}{2}\right\rceil-1}(n-2k)=n(n-2)(n-4)\cdots\\
n!!&=\prod_{k=1}^\frac{n}{2}(2k)=n(n-2)(n-4)\cdots 4\cdot 2\qquad(n\text{ is even})\\
0!!&=1\\
2!!&=2\\
4!!&=8\\
6!!&=48\\
8!!&=384\\
10!!&=3840\\
12!!&=46080\\
14!!&=645120\\
n!!&=\prod_{k=1}^\frac{n+1}{2}(2k-1)=n(n-2)(n-4)\cdots 3\cdot 1\qquad(n\text{ is odd})\\
1!!&=1\\
3!!&=3\\
5!!&=15\\
7!!&=105\\
9!!&=945\\
11!!&=10395\\
13!!&=135135\\
15!!&=2027025\\
\end{aligned}
```

# ガンマ関数

- [ja.wikipedia: ガンマ関数](https://ja.wikipedia.org/wiki/%E3%82%AC%E3%83%B3%E3%83%9E%E9%96%A2%E6%95%B0)
- [en.wikipedia: Gamma function](https://en.wikipedia.org/wiki/Gamma_function)

```math
\begin{aligned}
\Gamma(z)&=\int_0^\infty t^{z-1}\,e^{-t}\,\mathrm{d}t\qquad(\Re{z} > 0)\\
\Gamma(z+1)&=z\,\Gamma(z)\\
\Gamma\left(-\frac{3}{2}\right)&=\frac{4\sqrt{\pi}}{3}\approx 2.363\\
\Gamma\left(-\frac{1}{2}\right)&=-2\sqrt{\pi}\approx-3.545\\
\Gamma\left(\frac{1}{2}\right)&=\sqrt{\pi}\approx 1.772\\
\Gamma(1)&=0!=1\\
\Gamma\left(\frac{3}{2}\right)&=\frac{\sqrt{\pi}}{2}\approx 0.886\\
\Gamma(2)&=1!=1\\
\Gamma\left(\frac{5}{2}\right)&=\frac{3\sqrt{\pi}}{4}\approx 1.329\\
\Gamma(3)&=2!=2\\
\Gamma\left(\frac{7}{2}\right)&=\frac{15\sqrt{\pi}}{8}\approx 3.323\\
\Gamma(4)&=3!=6\\
\Gamma(n+1)&=n!\\
\Gamma\left(\frac{1}{2}+n\right)&=\frac{(2n-1)!!}{2^n}\sqrt{\pi}\qquad(n\ge 1)\\
\Gamma\left(\frac{1}{2}-n\right)&=\frac{(-2)^n}{(2n-1)!!}\sqrt{\pi}\qquad(n\ge 1)\\
\Gamma(z)\,\Gamma(1-z)&=-z\,\Gamma(z)\,\Gamma(-z)=\frac{\pi}{\sin\pi{z}}\\
\end{aligned}
```

## Lanczos近似

- Lanczos近似 : [en.wikipedia: Lanczos approximation](https://en.wikipedia.org/wiki/Lanczos_approximation)
- パラメータ算出 : [https://mrob.com/pub/ries/lanczos-gamma.html](https://mrob.com/pub/ries/lanczos-gamma.html)

```math
\begin{aligned}
z! =\Gamma(z+1)&\approx\sqrt{2\pi}\left(z+g+\frac{1}{2}\right)^{z+\frac{1}{2}}e^{-\left(z+g+\frac{1}{2}\right)} A_g(z)\\
A_g(z)&\approx c_0+\sum_{k=1}^{N-1}\frac{c_k}{z+k}\\
\end{aligned}
```

- $g=5,N=7$

```math
\begin{aligned}
c_0&=+1.000000000190015\\
c_1&=+76.18009172947146\\
c_2&=-86.50532032941677\\
c_3&=+24.01409824083091\\
c_4&=-1.231739572450155\\
c_5&=+0.1208650973866179E-2\\
c_6&=-0.5395239384953E-5\\
\end{aligned}
```

- $g=7,N=9$

```math
\begin{aligned}
c_0&=+0.99999999999980993227684700473478\\
c_1&=+676.520368121885098567009190444019\\
c_2&=-1259.13921672240287047156078755283\\
c_3&=+771.3234287776530788486528258894\\
c_4&=-176.61502916214059906584551354\\
c_5&=+12.507343278686904814458936853\\
c_6&=-0.13857109526572011689554707\\
c_7&=+9.984369578019570859563E-6\\
c_8&=+1.50563273514931155834E-7\\
\end{aligned}
```

- $g=9,N=11$

```math
\begin{aligned}
c_0&=+1.000000000000000174663\\
c_1&=+5716.400188274341379136\\
c_2&=-14815.30426768413909044\\
c_3&=+14291.49277657478554025\\
c_4&=-6348.160217641458813289\\
c_5&=+1301.608286058321874105\\
c_6&=-108.1767053514369634679\\
c_7&=+2.605696505611755827729\\
c_8&=-0.7423452510201416151527E-2\\
c_9&=+0.5384136432509564062961E-7\\
c_{10}&=-0.4023533141268236372067E-8\\
\end{aligned}
```

- $g=607/128=4.7421875,N=15$

```math
\begin{aligned}
c_0&=+0.99999999999999709182\\
c_1&=+57.156235665862923517\\
c_2&=-59.597960355475491248\\
c_3&=+14.136097974741747174\\
c_4&=-0.49191381609762019978\\
c_5&=+0.33994649984811888699E-4\\
c_6&=+0.46523628927048575665E-4\\
c_7&=-0.98374475304879564677E-4\\
c_8&=+0.15808870322491248884E-3\\
c_9&=-0.21026444172410488319E-3\\
c_{10}&=+0.21743961811521264320E-3\\
c_{11}&=-0.16431810653676389022E-3\\
c_{12}&=+0.84418223983852743293E-4\\
c_{13}&=-0.26190838401581408670E-4\\
c_{14}&=+0.36899182659531622704E-5\\
\end{aligned}
```

## 大浦による実ガンマ関数近似

- [大浦拓哉, ガンマ関数および誤差関数の初等関数近似とその最適化](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html) [(ja)](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html) [(en)](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf.html)

```math
\begin{aligned}
\Gamma(x)&\approx\exp((x-0.5)\ln(x+V)-x)\\
&\quad\cdot((\cdots(A_n/(x+B_n)+\cdots A_1)/(x+B_1)+A_0)/x+A_r)\qquad(0 < x <\infty)\\
\end{aligned}
```

- $N=2$ のとき、相対誤差の上限 $=3.11289669E-8$

```math
\begin{aligned}
V  &=2.102394798991390E+0\\
A_r&=3.062185443705942E-1\\
A_0&=1.024166094985555E+0\\
A_1&=4.258010456317367E-1\\
B_1&=1.000008131602802E+0\\
\end{aligned}
```

- $N=6$ のとき、相対誤差の上限 $=2.09144255E-18$

```math
\begin{aligned}
V  &=6.0975075753906857609437558E+0\\
A_r&=5.6360656189756064967977564E-3\\
A_0&=1.2242597732687991784645973E-1\\
A_1&=8.5137081316503418312411656E-1\\
A_2&=2.2502304753561816836695856E+0\\
A_3&=2.0962955353894997733869983E+0\\
A_4&=5.0219722703392090725884168E-1\\
A_5&=1.1240582657165407383437999E-2\\
B_1&=1.0000000000006553243170562E+0\\
B_2&=1.9999999996201023058065171E+0\\
B_3&=3.0000000467265241458431618E+0\\
B_4&=3.9999966300007508932097016E+0\\
B_5&=5.0003589884831925541613237E+0\\
\end{aligned}
```

- $N=13$ のとき、相対誤差の上限 $=8.43420741E-37$

```math
\begin{aligned}
V  &=1.35781220007039464739769136052735188826566614E+1\\
A_r&=3.17823842997348984212895391439981193809771347E-6\\
A_0&=3.14820702833493003545826236239083394571995671E-4\\
A_1&=1.27937416087229845006934584904736618598070572E-2\\
A_2&=2.78748303060299808744345690552596166059251493E-1\\
A_3&=3.57487639582285701807582585579290271336089099E+0\\
A_4&=2.79272804215633250156669351783752812175650972E+1\\
A_5&=1.33213846503797389894468858322687847549726114E+2\\
A_6&=3.79504051924654223127926344491479357839857722E+2\\
A_7&=6.15621499930282594633468081962352923412741184E+2\\
A_8&=5.24004008691006507011182613589749851171431576E+2\\
A_9&=2.04187662020237118761681790759964964799068736E+2\\
A_{10}&=2.86456197727291086831913426471935542005422307E+1\\
A_{11}&=8.95072101413389847373058347512910403979947773E-1\\
A_{12}&=1.84108633157612656306027334817135207544862157E-3\\
B_1&=9.99999999999999999999999999829177067439186649E-1\\
B_2&=2.00000000000000000000000143725325109773686179E+0\\
B_3&=2.99999999999999999999878069870191969828857942E+0\\
B_4&=4.00000000000000000031822896056305388990947492E+0\\
B_5&=4.99999999999999996032765694796886928790459606E+0\\
B_6&=6.00000000000000300343091566980971296037249121E+0\\
B_7&=6.99999999999983752474626982882253159057375788E+0\\
B_8&=8.00000000000719155188030217651616848093810689E+0\\
B_9&=8.99999999970006818618226539512826008489484540E+0\\
B_{10}&=1.00000000142050052373091324295304916612836237E+1\\
B_{11}&=1.09999989539201196803612424783730853335056534E+1\\
B_{12}&=1.20002381089341943372805397259444226612900582E+1\\
\end{aligned}
```

# 不完全ガンマ関数

- 不完全ガンマ関数 (Incomplete gamma functions)
- [ja.wikipedia: 不完全ガンマ関数](https://ja.wikipedia.org/wiki/%E4%B8%8D%E5%AE%8C%E5%85%A8%E3%82%AC%E3%83%B3%E3%83%9E%E9%96%A2%E6%95%B0)
- [en.wikipedia: Incomplete gamma function](https://en.wikipedia.org/wiki/Incomplete_gamma_function)
- [boost.org: math toolkit/sf gamma/igamma.html #math toolkit.sf gamma.igamma.implementation](https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_gamma/igamma.html#math_toolkit.sf_gamma.igamma.implementation)

```math
\begin{aligned}
\gamma(a,x)&=\int_0^x t^{a-1}\,e^{-t}\mathrm{d}t\qquad\text{(lower incomplete gamma function)}\\
\Gamma(a,x)&=\int_x^\infty t^{a-1}\,e^{-t}\mathrm{d}t\qquad\text{(upper incomplete gamma function)}\\
\Gamma(a)&=\gamma(a,x)+\Gamma(a,x)\\
\gamma(a+1,x)&=a\,\gamma(a,x)-x^a\,e^{-x}\\
\Gamma(a+1,x)&=a\,\Gamma(a,x)+x^a\,e^{-x}\\
\gamma(a,0)&=0\\
\Gamma(a,0)&=\Gamma(a)\quad(\mathfrak{R}(a) > 0)\\
\gamma(a,x)&\to\Gamma(a)\quad(x\to\infty)\\
\Gamma(0,x)&=-\operatorname{Ei}(-x)\quad\text{ for } x > 0\\
\Gamma(1/2,x)&=\sqrt\pi\operatorname{erfc}\left(\sqrt x\right)\\
\gamma(1/2,x)&=\sqrt\pi\operatorname{erf} \left(\sqrt x\right)\\
\Gamma(1,x)&=e^{-x}\\
\gamma(1,x)&=1-e^{-x}\\
\end{aligned}
```

## 正則化ガンマ関数

- 正則化ガンマ関数 (Regularized Gamma functions)
- [en.wikipedia: Incomplete gamma function #Regularized Gamma functions and Poisson random variables](https://en.wikipedia.org/wiki/Incomplete_gamma_function#Regularized_Gamma_functions_and_Poisson_random_variables)

```math
\begin{aligned}
P(a,x)&=\int_0^x\frac{t^{a-1}\,e^{-t}}{\Gamma(a)}\mathrm{d}t =\frac{\gamma(a,x)}{\Gamma(a)}=1-Q(a,x)\\
Q(a,x)&=\int_x^\infty\frac{t^{a-1}\,e^{-t}}{\Gamma(a)}\mathrm{d}t =\frac{\Gamma(a,x)}{\Gamma(a)}=1-P(a,x)\\
P(a,x)+Q(a,x)&=1\\
P(a,0)&=0\\
Q(a,0)&=1\\
P(a,x)&\to 1\quad(x\to\infty)\\
Q(a,x)&\to 0\quad(x\to\infty)\\
\end{aligned}
```

# 誤差関数

- [ja.wikipedia: 誤差関数](https://ja.wikipedia.org/wiki/%E8%AA%A4%E5%B7%AE%E9%96%A2%E6%95%B0)
- [en.wikipedia: Error function](https://en.wikipedia.org/wiki/Error_function)
- 誤差関数 (error function)

```math
\operatorname{erf}(x)=\frac{2}{\sqrt{\pi}}\int_0^x e^{-t^2}\mathrm{d}t
```

- 相補誤差関数 (complementary error function)

```math
\operatorname{erfc}(x)=1-\operatorname{erf}(x) =\frac{2}{\sqrt{\pi}}\int_x^\infty e^{-t^2}\mathrm{d}t=e^{-x^2}\operatorname{erfcx}(x)
```

- スケーリング相補誤差関数 (scaled complementary error function)

```math
\operatorname{erfcx}(x)=e^{x^2}\operatorname{erfc}(x) =\frac{2}{\sqrt{\pi}} e^{x^2}\int_x^\infty e^{-t^2}\mathrm{d}t
```

## 大浦による実誤差関数近似

- [大浦拓哉, ガンマ関数および誤差関数の初等関数近似とその最適化](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html) [(ja)](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html) [(en)](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf.html)

```math
\operatorname{erf}(x)\approx\sum_{k=0}^{N-1}A_k\cdot x^{2k+1}\qquad(|x|\le 0.125)
```

- $N=3$ のとき、相対誤差の上限 $= 3.1833E-9$

```math
\begin{aligned}
A_{0}&=+1.12837916390979E+0\\
A_{1}&=-3.76122716908058E-1\\
A_{2}&=+1.12210466658717E-1\\
\end{aligned}
```

- $N=6$ のとき、相対誤差の上限 $= 8.5080E-19$

```math
\begin{aligned}
A_{0}&=+1.128379167095512573044943E+0\\
A_{1}&=-3.761263890318336015429662E-1\\
A_{2}&=+1.128379167066213010234749E-1\\
A_{3}&=-2.686616984476423776951305E-2\\
A_{4}&=+5.223878776856181012778436E-3\\
A_{5}&=-8.492024351869184700200701E-4\\
\end{aligned}
```

- $N=11$ のとき、相対誤差の上限 $= 7.8857E-36$

```math
\begin{aligned}
A_{0}&=+1.12837916709551257389615890312154516380003E+0\\
A_{1}&=-3.76126389031837524632052967707059546441135E-1\\
A_{2}&=+1.12837916709551257389615889999358860765393E-1\\
A_{3}&=-2.68661706451312517594320425149614323134575E-2\\
A_{4}&=+5.22397762544218784195192159042937479733406E-3\\
A_{5}&=-8.54832702345085235483932361340167472569759E-4\\
A_{6}&=+1.20553329817887746876164250246490906879364E-4\\
A_{7}&=-1.49256503573424184861275467598647653507484E-5\\
A_{8}&=+1.64621135485586937463784315237197152289516E-6\\
A_{9}&=-1.63654546924614288220125244596685202501682E-7\\
A_{10}&=+1.47019666508092845945340329089881956917798E-8\\
\end{aligned}
```

## 大浦による実相補誤差関数近似

- [大浦拓哉, ガンマ関数および誤差関数の初等関数近似とその最適化](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html) [(ja)](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html) [(en)](http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf.html)

```math
\begin{aligned}
\operatorname{erfc}(x)&\approx E(x)+x\exp(-x^2)\sum_{k=0}^{N-1}\frac{A_k}{x^2+B_k}\qquad(-\infty < x <\infty)\\
E(x)&=\begin{cases}
2/(1+\exp(\alpha x))&(x <\beta)\\
0&(x\ge\beta)\\
\end{cases}\\
\end{aligned}
```

- $N=4$ のとき、相対誤差の上限 $= 7.07284210E-9$

```math
\begin{aligned}
\alpha&=9.2088871045460211E+0\\
\beta&=5.0725473171624327E+0\\
A_{0}&=3.8664221739686797E-1\\
A_{1}&=1.5243017675919252E-1\\
A_{2}&=2.3814912488843075E-2\\
A_{3}&=1.3022729124288807E-3\\
B_{0}&=1.1638196508217325E-1\\
B_{1}&=1.0475380173789841E+0\\
B_{2}&=2.9213215631713289E+0\\
B_{3}&=6.0260843416158831E+0\\
\end{aligned}
```

- $N=8$ のとき、相対誤差の上限 $= 3.63856888E-17$

```math
\begin{aligned}
\alpha&=1.269748999651156838985811E+1\\
\beta&=6.103997330986881994334338E+0\\
A_{0}&=2.963168851992273778336357E-1\\
A_{1}&=1.815811251346370699640955E-1\\
A_{2}&=6.818664514249394930148282E-2\\
A_{3}&=1.569075431619667090378092E-2\\
A_{4}&=2.212901166815175728291522E-3\\
A_{5}&=1.913958130987428643791697E-4\\
A_{6}&=9.710132840105516234434841E-6\\
A_{7}&=1.666424471743077527468010E-7\\
B_{0}&=6.121586444955387580549294E-2\\
B_{1}&=5.509427800560020848936831E-1\\
B_{2}&=1.530396620587703969527527E+0\\
B_{3}&=2.999579523113006340465739E+0\\
B_{4}&=4.958677771282467011450533E+0\\
B_{5}&=7.414712510993354068147575E+0\\
B_{6}&=1.047651043565452375901435E+1\\
B_{7}&=1.484555573455979566646185E+1\\
\end{aligned}
```

- $N=17$ のとき、相対誤差の上限 $= 7.45563242E-36$

```math
\begin{aligned}
\alpha&=1.8296570980424689847157930974106706834567989E+1\\
\beta&=8.9588287394342176848213494031807385567358833E+0\\
A_{0}&=2.1226887418241545314975570224238841543405658E-1\\
A_{1}&=1.6766968820663231170102487414107148109808599E-1\\
A_{2}&=1.0461429607758480243524362040994242136794358E-1\\
A_{3}&=5.1557963860512142911764627378588661741526705E-2\\
A_{4}&=2.0070986488528139460346647533434778000221814E-2\\
A_{5}&=6.1717726506718148117513762897928828533989685E-3\\
A_{6}&=1.4990611906920858646769185063310410160420122E-3\\
A_{7}&=2.8760540416705806615617926157307107830366204E-4\\
A_{8}&=4.3585593590380741491013549969419946961138883E-5\\
A_{9}&=5.2174364856655433775383935118049845471172446E-6\\
A_{10}&=4.9333351722974670085736982894474122277208033E-7\\
A_{11}&=3.6846914376723888190666722894010079934846267E-8\\
A_{12}&=2.1729515092764086499231043367920037214553663E-9\\
A_{13}&=9.9870022842895735663712411206346261651079743E-11\\
A_{14}&=3.1775163189596489863458236395414830880404471E-12\\
A_{15}&=4.5657943993597540327708145643160878201453992E-14\\
A_{16}&=1.1940964427370412648558173558044106203476395E-16\\
B_{0}&=2.9482230394292049252878077330764031336911886E-2\\
B_{1}&=2.6534007354862844327590269604581049763591558E-1\\
B_{2}&=7.3705575985730123132195272141160572531634811E-1\\
B_{3}&=1.4446292893203104133929687855854497895783377E+0\\
B_{4}&=2.3880606619376559912235584857800710490361124E+0\\
B_{5}&=3.5673498777093386979273977202889759344704102E+0\\
B_{6}&=4.9824969366355296879760903991854492761727217E+0\\
B_{7}&=6.6335018387405633238409855625402006222862354E+0\\
B_{8}&=8.5203645862651289478197632097553870198987021E+0\\
B_{9}&=1.0643085317662274170216548777166393329207188E+1\\
B_{10}&=1.3001669850030489723387515813223808078289803E+1\\
B_{11}&=1.5596282517377690399267249728222735969695585E+1\\
B_{12}&=1.8429903207271748464995406180854691071748545E+1\\
B_{13}&=2.1533907893494593530979123915138686106579160E+1\\
B_{14}&=2.5076752889217226137869837117288885076749247E+1\\
B_{15}&=2.9515380437412601845256918753602002410389178E+1\\
B_{16}&=3.5792848810704122499184545805923520657604330E+1\\
\end{aligned}
```

# ベータ関数

- [ja.wikipedia: ベータ関数](https://ja.wikipedia.org/wiki/%E3%83%99%E3%83%BC%E3%82%BF%E9%96%A2%E6%95%B0)
- [en.wikipedia: Beta function](https://en.wikipedia.org/wiki/Beta_function)

```math
\begin{aligned}
\operatorname{B}(x,y)&=\int_0^1 t^{x-1}\,(1-t)^{y-1}\,\mathrm{d}t\\
\operatorname{B}(x,y)&=\frac{\Gamma(x)\Gamma(y)}{\Gamma(x+y)}\\
\operatorname{B}(x,y)&=\operatorname{B}(y,x)\\
x\,\operatorname{B}(x,y+1)&=y\,\operatorname{B}(x+1,y)\\
\operatorname{B}(x,y)&=\operatorname{B}(x+1,y)+\operatorname{B}(x,y+1)\\
\operatorname{B}(x+1,y)&=\operatorname{B}(x,y)\cdot\frac{x}{x+y}\\
\operatorname{B}(x,y+1)&=\operatorname{B}(x,y)\cdot\frac{y}{x+y}\\
\operatorname{B}(x,y)\cdot\operatorname{B}(x+y,1-y)&=\frac{\pi}{x\sin(\pi y)}\\
\operatorname{B}(x,1-x)&=\frac{\pi}{\sin(\pi x)}\\
\operatorname{B}(1,x)&=\frac{1}{x}\\
\end{aligned}
```

# 不完全ベータ関数

- 不完全ベータ関数 (incomplete beta function)
- [ja.wikipedia: 不完全ベータ関数](https://ja.wikipedia.org/wiki/%E4%B8%8D%E5%AE%8C%E5%85%A8%E3%83%99%E3%83%BC%E3%82%BF%E9%96%A2%E6%95%B0)
- [en.wikipedia: Beta function #Incomplete beta function](https://en.wikipedia.org/wiki/Beta_function#Incomplete_beta_function)

```math
\begin{aligned}
\operatorname{B}_z(a,b)&=\int_{0}^{z} t^{a-1}\,(1-t)^{b-1}\,\mathrm{d}t\qquad(0\le\Re{z}\le 1)\\
\end{aligned}
```

# 正則ベータ関数

- 正則ベータ関数 (regularized beta function) / 正則化不完全ベータ関数 (regularized incomplete beta function)
- [Incomplete Beta Functions-Implementation](https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_beta/ibeta_function.html#math_toolkit.sf_beta.ibeta_function.implementation)
- [Significant Digit Computation of the Incomplete Beta Function Ratios. AR DiDonato, AH Morris Jr-1988-dtic.mil](http://www.dtic.mil/dtic/tr/fulltext/u2/a210118.pdf)
- [Armido R. Didonato and Alfred H. Morris, Jr.. 1992. Algorithm 708: Significant digit computation of the incomplete beta function ratios. ACM Trans. Math. Softw. 18, 3(September 1992), 360-373.](https://doi.org/10.1145/131766.131776)

```math
\begin{aligned}
I_z(a,b)&=\frac{\operatorname{B}_z(a,b)}{\operatorname{B}(a,b)}\qquad(0\le\Re{z}\le 1)\\
p&=1-q=\frac{a}{a+b}\\
q&=1-p=\frac{b}{a+b}\\
y&=1-x\\
I_0(a,b)&=0\\
I_1(a,b)&=1\\
I_x(a,1)&=x^a\\
I_x(1,b)&=1-y^b\\
I_x(a,b)&=1-I_y(b,a)\\
I_x(a,b)&=I_x(a+n,b)+\frac{x^ay^b}{a B(a,b)}\sum_{i=0}^{n-1} d_i x^i\\
I_x(a+n,b)&=I_x(a,b)-\frac{x^ay^b}{a B(a,b)}\sum_{i=0}^{n-1} d_i x^i\\
I_x(1+n,b)&=1-y^b\left(1+b x\sum_{i=0}^{n-1} d_i x^i\right)\\
I_x(a,b)&=I_x(a,b+n)-\frac{x^ay^b}{b B(a,b)}\sum_{j=0}^{n-1}d'_j y^j\\
I_x(a,b+n)&=I_x(a,b)+\frac{x^ay^b}{b B(a,b)}\sum_{j=0}^{n-1}d'_j y^j\\
I_x(a,1+n)&=x^a\left(1+a y\sum_{j=0}^{n-1}d'_j y^j\right)\\
d_{i+1}&=\frac{a+b+i}{a+1+i}d_i,\ d_0=1\\
d'_{j+1}&=\frac{a+b+j}{b+1+j}d'_j,\ d'_0=1\\
\end{aligned}
```

## BPSER

```math
\begin{aligned}
I_x(a,b)&=\frac{x^a}{a\operatorname{B}(a,b)}\left(1+a\sum_{j=1}^\infty\frac{(1-b)(2-b)\cdots(j-b)}{j!(a+j)} x^j\right)\qquad(b\le 1,\ x\le 0.7)\ |\ (bx\le 0.7)\\
\end{aligned}
```

## BGRAT

```math
\begin{aligned}
I_x(a,b)&\approx M\sum_{n=0}^\infty p_n J_n(b,u)\qquad(a > b)\\
T&=a+\frac{b-1}{2}\\
u&=-T\ln x\\
H(c,u)&=\frac{e^{-u}u^c}{\Gamma(c)}\\
M&=\frac{H(b,u)\Gamma(a+b)}{\Gamma(a) T^b}\\
J_n(b,u)&=\left(\frac{u}{2T}\right)^{2n}\frac{Q(b+2n,u)}{H(b+2n,u)}
=\left(\frac{\ln x}{2}\right)^{2n}\frac{Q(b+2n,u)}{H(b+2n,u)}\\
Q(b,u)&=\int_u^\infty\frac{e^{-t}t^{b-1}}{\Gamma(b)}\mathrm{d}t\qquad\text{(incomplete gamma function)}\\
J_{n+1}(b,u)&=\frac{(b+2n)(b+2n+1)}{4T^2}J_n(b,u)+\frac{u+b+2n+1}{4T^2}\left(\frac{\ln x}{2}\right)^{2n}\\
J_0(b,u)&=\frac{Q(b,u)}{H(b,u)}\\
\end{aligned}
```

## BFRAC(1)

```math
\begin{aligned}
I_x(a,b)&=\frac{x^ay^b}{\operatorname{B}(a,b)}\left(\frac{\alpha_1}{\beta_1\ +}\frac{\alpha_2}{\beta_2\ +}\cdots\right)\qquad\left(x\le p\equiv\frac{a}{a+b}\right)\\
y&=1-x\\
\alpha_1&=1\\
\beta_1&=\frac{a}{a+1}(\lambda+1)\\
\lambda&=a-(a+b)x=(a+b)(p-x)\\
\alpha_{n+1}&=\frac{(a+n-1)(a+b+n-1)}{(a+2n-1)^2}n(b-n)x^2\qquad(n\ge 1)\\
\beta_{n+1}&=n+\frac{n(b-n)x}{a+2n-1}+\frac{a+n}{a+2n+1}[\lambda+1+n(1+y)]\qquad(n\ge 0)\\
\end{aligned}
```

## BFRAC(2)

```math
\begin{aligned}
I_x(a,b)&=\frac{x^ay^b}{a\operatorname{B}(a,b)}\left(\frac{1}{1\ +}\frac{d_1}{1\ +}\frac{d_2}{1\ +}\cdots\right)\qquad\left(x\le p\equiv\frac{a}{a+b}\right)\\
y&=1-x\\
d_{2n}&=\frac{n(b-n)}{(a+2n-1)(a+2n)}x\qquad(n > 0)\\
d_{2n+1}&=\frac{(a+n)(a+b+n)}{(a+2n)(a+2n+1)}x\qquad(n\ge 0)\\
\end{aligned}
```

## BASYM

```math
\begin{aligned}
I_x(a,b)&\approx\frac{2}{\sqrt{\pi}}Ue^{-z^2}\sum_{n=0}^\infty e_nL_n(z)(\beta\gamma)^n\qquad\left(x\le p\equiv\frac{a}{a+b}\right)\\
U&=\frac{p^aq^b}{\operatorname{B}(a,b)}\sqrt{\frac{2\pi(a+b)}{ab}}\\
z&=\sqrt{\varphi(x)}\\
\varphi(t)&=-\left(a\ln\frac{t}{p}+b\ln\frac{1-t}{q}\right)\ge 0\qquad(0 < t < 1)\\
\beta\gamma&=\sqrt{q/a}\qquad(a\le b)\\
\beta\gamma&=\sqrt{p/b}\qquad(a\ge b)\\
a_n&=\frac{2}{n+2}\,q\left[1+(-1)^n(a/b)^{n+1}\right]\qquad(a\le b)\\
a_n&=\frac{2}{n+2}\,p\left[(-1)^n+(b/a)^{n+1}\right]\qquad(a > b,\ n\ge 0)\\
p&=1-q =\frac{a}{a+b}\\
q&=1-p =\frac{b}{a+b}\\
b_0^{(r)}&=1\qquad\\
b_1^{(r)}&=ra_1\qquad(r\ne 0)\\
b_n^{(r)}&=ra_n+\frac{1}{n}\sum_{i=1}^{n-1}[(n-i)r-i]b_i^{(r)}a_{n-i}\qquad(n=2,3,\dots)\\
c_n&=\frac{1}{n}b_{n-1}^{(-n/2)}\qquad(n\ge 1)\\
e_0&=1\\
e_n&=-\sum_{i=0}^{n-1}e_i\,c_{n-i+1}\\
L_n(z)&=2^{(n/2)-1}e^{z^2}\int_z^\infty e^{-u^2}u^n\mathrm{d}u\\
L_0(z)&=\frac{\sqrt{\pi}}{4}e^{z^2}\operatorname{erfc}(z) =\frac{\sqrt{\pi}}{4}\operatorname{erfcx}(z)\\
L_1(z)&=2^{-3/2}\\
L_n(z)&=2^{-3/2}\left(\sqrt{2}z\right)^{n-1}+(n-1)L_{n-2}(z)\qquad(n=2,3,\dots)\\
\end{aligned}
```

# 二項累積分布と正則ベータ関数との関係

- [en.wikipedia: Beta function# Incomplete beta function](https://en.wikipedia.org/wiki/Beta_function#Incomplete_beta_function)

```math
F(k;\,n,p)=\operatorname{Pr}(X\le k)=I_{1-p}(n-k,k+1)=1-I_p(k+1,n-k)
```

# Maxima script

高精度の計算、自前実装ルーチンの検算用

- [gist:イロレーティング計算用テストデータ](https://gist.github.com/mizar/bb49ac4905742bfdef84f96146873d49)

```{ .maxima .numberLines startFrom="1" }
fpprec:64$
fpprintprec:36$
defstruct(wdl(w,d,l))$
defstruct(elorange(w,d,l,sr,r,low,high))$
normcdf(x):=erfc(-sqrt(5b-1)*x)*5b-1$
normcdfinv(r):=bf_find_root(normcdf(x)-r,x,-1b1,+1b1)$
elo(a,b,r):=bf_find_root(beta_incomplete_regularized(
  bfloat(a),bfloat(b),(1+10^(-25b-4*x))^-1)-r,x,-1b5,+1b5)$
elocalc(wdl,r):=new(elorange(wdl@w,wdl@d,wdl@l,-normcdfinv(r),r,
  elo(wdl@w+wdl@d*5b-1,wdl@l+wdl@d*5b-1+1,r),
  elo(wdl@w+wdl@d*5b-1+1,wdl@l+wdl@d*5b-1,1-r)))$
rlist:[2.5b-1,2b-1,normcdf(-1.0),1.5b-1,1.25b-1,1b-1,normcdf(-1.5),
  5b-2,2.5b-2,normcdf(-2.0),1b-2,normcdf(-2.5),5b-3,normcdf(-3.0),
  1b-3,5b-4,normcdf(-3.5),1b-4,5b-5,normcdf(-4.0),1b-5];
wlist:[
  new(wdl(0,1,0)),new(wdl(1,0,1)),new(wdl(1,1,1)),
  new(wdl(9,0,1)),new(wdl(9,1,0)),
  new(wdl(1e1,1,0)),new(wdl(1e1,0,1)),new(wdl(1e1,0,3)),
  new(wdl(1e1,1,10)),new(wdl(1e1,0,40)),new(wdl(1e1,1,40)),
  new(wdl(11,1,0)),new(wdl(12,1,0)),new(wdl(66,0,34)),
  new(wdl(1e2,1,0)),new(wdl(1e2,0,1)),new(wdl(1e2,0,3)),
  new(wdl(1e2,1,10)),new(wdl(1e2,0,40)),new(wdl(1e2,1,40)),
  new(wdl(550,0,450)),
  new(wdl(1e3,1,0)),new(wdl(1e3,0,1)),new(wdl(1e3,0,3)),
  new(wdl(1e3,1,10)),new(wdl(1e3,0,40)),new(wdl(1e3,1,40)),
  new(wdl(5155,0,4845)),
  new(wdl(1e4,1,0)),new(wdl(1e4,0,1)),new(wdl(1e4,0,3)),
  new(wdl(1e4,1,10)),new(wdl(1e4,0,40)),new(wdl(1e4,1,40)),
  new(wdl(50490,0,49510)),
  new(wdl(1e5,1,0)),new(wdl(1e5,0,1)),new(wdl(1e5,0,3)),
  new(wdl(1e5,1,10)),new(wdl(1e5,0,40)),new(wdl(1e5,1,40)),
  new(wdl(501546,0,498454)),
  new(wdl(1e6,1,0)),new(wdl(1e6,0,1)),new(wdl(1e6,0,3)),
  new(wdl(1e6,1,10)),new(wdl(1e6,0,40)),new(wdl(1e6,1,40)),
  new(wdl(5004887,0,4995113)),
  new(wdl(1e7,1,0)),new(wdl(1e7,0,1)),new(wdl(1e7,0,3)),
  new(wdl(1e7,1,10)),new(wdl(1e7,0,40)),new(wdl(1e7,1,40)),
  new(wdl(50015452,0,49984548)),
  new(wdl(1e8,1,0)),new(wdl(1e8,0,1)),new(wdl(1e8,0,3)),
  new(wdl(1e8,1,10)),new(wdl(1e8,0,40)),new(wdl(1e8,1,40)),
  new(wdl(500048862,0,499951138)),
  new(wdl(1e9,1,0)),new(wdl(1e9,0,1)),new(wdl(1e9,0,3)),
  new(wdl(1e9,1,10)),new(wdl(1e9,0,40)),new(wdl(1e9,1,40)),
  new(wdl(1e10,1,0)),new(wdl(1e10,0,1)),new(wdl(1e10,0,3)),
  new(wdl(1e10,1,10)),new(wdl(1e10,0,40)),new(wdl(1e10,1,40)),
  new(wdl(1e11,1,0)),new(wdl(1e11,0,1)),new(wdl(1e11,0,3)),
  new(wdl(1e11,1,10)),new(wdl(1e11,0,40)),new(wdl(1e11,1,40)),
  new(wdl(1e12,1,0)),new(wdl(1e12,0,1)),new(wdl(1e12,0,3)),
  new(wdl(1e12,1,10)),new(wdl(1e12,0,40)),new(wdl(1e12,1,40)),
  new(wdl(1e13,1,0)),new(wdl(1e13,0,1)),new(wdl(1e13,0,3)),
  new(wdl(1e13,1,10)),new(wdl(1e13,0,40)),new(wdl(1e13,1,40)),
  new(wdl(1e14,1,0)),new(wdl(1e14,0,1)),new(wdl(1e14,0,3)),
  new(wdl(1e14,1,10)),new(wdl(1e14,0,40)),new(wdl(1e14,1,40)),
  new(wdl(1e15,1,0)),new(wdl(1e15,0,1)),new(wdl(1e15,0,3)),
  new(wdl(1e15,1,10)),new(wdl(1e15,0,40)),new(wdl(1e15,1,40)),
  new(wdl(1e16,1,0)),new(wdl(1e16,0,1)),new(wdl(1e16,0,3)),
  new(wdl(1e16,1,10)),new(wdl(1e16,0,40)),new(wdl(1e16,1,40))
  ]$
for wi thru length(wlist) do
for ri thru length(rlist) do
  print(elocalc(wlist[wi],rlist[ri]))$
fpprec:40$
for ri thru length(rlist) do
  print(elocalc(new(wdl(5000154513,0,4999845487)),rlist[ri]))$
```
