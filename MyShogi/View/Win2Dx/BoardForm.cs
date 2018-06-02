using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyShogi.Model.Shogi.Core;
using MyShogi.View.Win2Dx.Matrix;
using DColor = System.Drawing.Color;
using SColor = MyShogi.Model.Shogi.Core.Color;

/*
# TODO

* 駒台の駒の配置計算・表示の実装
* 各種カスタマイズ可能な項目を整理
  * 視点
  * 盤駒のサイズ・形状・色など
  * 駒の配置戦略（揃え位置・駒台の整列順・駒台駒の分割戦略）
  * 駒文字を、スクリーンフォント以外に任意の2Dベクタ図形にカスタマイズ出来るように
    * ベクタ図形はSVG Path文字列で入力する？
* 着手アニメーションのための状態管理制御
  * 時間による再描画制御（アニメーションが全て終了したら過剰な再描画は避けるべき）
  * 着手アニメーションの補間処理は取り敢えず2パターン用意予定
    * 近距離(隣接マス程度まで): 盤上を滑らすような直線的な移動
    * 遠距離: 一旦駒を盤から持ち上げるような山なりの移動
* 入力周りの制御・状態管理
  * まだ全然やり方調べてない
  * 成/不成の選択画面はどうする？
  * 着手中の駒の表示はどうする？
  * 着手のキャンセル（誤操作への優しい対処）はどうする？
  * カーソル位置の方向を示すRayの計算、Rayと交差する駒・マス目の算出
* 諸々デザイン
  * 見直すべき所色々ありそう
  * 例えば検討時の候補手、どんな表示をする？
* 盤駒へのテクスチャ画像貼り付け(優先度低)
  * 射影変換による自由変形とラスタライズ処理を自前で実装する必要がある
  * 工数の割にパフォーマンス悪化で没になる恐れ
  * それほど重くならずに実装できるならしたい所
* 影の描画(優先度低)
  * 駒同士で影は干渉せず、まずは盤・駒台にのみ影を落とす方向で
  * 見栄え上、ハードシャドウではなくソフトシャドウが望まれるが、パフォーマンス?
*/

namespace MyShogi.View.Win2Dx
{
    // 疑似3D盤面Viewフォームの実験実装
    public partial class BoardForm : Form
    {
        Position pos;
        TextRenderingHint textRenderingHint = TextRenderingHint.ClearTypeGridFit;
        SmoothingMode smoothingMode = SmoothingMode.HighQuality;
        string[] pieceFontFamilyName = { "Noto Serif CJK JP Bold", "游明朝 Demibold", "ＭＳ Ｐ明朝" };
        string[] coordFontFamilyName = { "Noto Sans CJK JP Bold", "游ゴシック Medium", "ＭＳ Ｐゴシック" };
        double ymul = 1, xmul = 0.913, handWidth = 3.17, handInnerWidth = 2, handDepth = 0.16, handGap = 0.02, lineWidth = 0.02, starRadius = 0.04;
        HandOrder horder = HandOrder.BigBL;
        PieceCharset pieceCharset = PieceCharset.oneChar;
        CoordCharset coordCharset = CoordCharset.NORMAL;
        Vector3 cameraPos, cameraLookAt, cameraUp;

        private void viewNormal1(object sender, EventArgs e)
        {
            cameraPos = new Vector3(0, 0, -13);
            cameraLookAt = new Vector3(0, 0, 0);
            cameraUp = new Vector3(0, +1, 0);
            Refresh();
        }
        private void viewReverse1(object sender, EventArgs e)
        {
            cameraPos = new Vector3(0, 0, -13);
            cameraLookAt = new Vector3(0, 0, 0);
            cameraUp = new Vector3(0, -1, 0);
            Refresh();
        }
        private void viewNormal2(object sender, EventArgs e)
        {
            cameraPos = new Vector3(0, -5.5, -11);
            cameraLookAt = new Vector3(0, -0.7, 0);
            cameraUp = new Vector3(0, +1, 0);
            Refresh();
        }
        private void viewReverse2(object sender, EventArgs e)
        {
            cameraPos = new Vector3(0, +5.5, -11);
            cameraLookAt = new Vector3(0, +0.7, 0);
            cameraUp = new Vector3(0, -1, 0);
            Refresh();
        }
        private void viewNormal3(object sender, EventArgs e)
        {
            cameraPos = new Vector3(+0.5, -9, -9);
            cameraLookAt = new Vector3(+0.5, -1.25, 0);
            cameraUp = new Vector3(0, +1, 0);
            Refresh();
        }
        private void viewReverse3(object sender, EventArgs e)
        {
            cameraPos = new Vector3(-0.5, +9, -9);
            cameraLookAt = new Vector3(-0.5, +1.25, 0);
            cameraUp = new Vector3(0, -1, 0);
            Refresh();
        }
        private void viewNormal4(object sender, EventArgs e)
        {
            cameraPos = new Vector3(+1.25, -10, -7);
            cameraLookAt = new Vector3(+1.25, -1.5, 0);
            cameraUp = new Vector3(0, +1, 0);
            Refresh();
        }
        private void viewReverse4(object sender, EventArgs e)
        {
            cameraPos = new Vector3(-1.25, +10, -7);
            cameraLookAt = new Vector3(-1.25, +1.5, 0);
            cameraUp = new Vector3(0, -1, 0);
            Refresh();
        }

        private void BigBL(object sender, EventArgs e)
        {
            horder = HandOrder.BigBL;
            Refresh();
        }

        private void BigBR(object sender, EventArgs e)
        {
            horder = HandOrder.BigBR;
            Refresh();
        }

        private void BigUL(object sender, EventArgs e)
        {
            horder = HandOrder.BigUL;
            Refresh();
        }

        private void BigUR(object sender, EventArgs e)
        {
            horder = HandOrder.BigUR;
            Refresh();
        }

        private void pieceOneChar(object sender, EventArgs e)
        {
            pieceCharset = PieceCharset.oneChar;
            Refresh();
        }

        private void pieceTwoChar(object sender, EventArgs e)
        {
            pieceCharset = PieceCharset.twoChar;
            Refresh();
        }

        private void coordNone(object sender, EventArgs e)
        {
            coordCharset = CoordCharset.NONE;
            Refresh();
        }

        private void coordNormal(object sender, EventArgs e)
        {
            coordCharset = CoordCharset.NORMAL;
            Refresh();
        }

        private void coordArabic(object sender, EventArgs e)
        {
            coordCharset = CoordCharset.ARABIC;
            Refresh();
        }

        private void coordChesslike(object sender, EventArgs e)
        {
            coordCharset = CoordCharset.CHESSLIKE;
            Refresh();
        }

        private void viewNormal5(object sender, EventArgs e)
        {
            cameraPos = new Vector3(+8, -8, -7);
            cameraLookAt = new Vector3(+1, -1, 0);
            cameraUp = new Vector3(-0.4, +1, 0);
            Refresh();
        }
        private void viewReverse5(object sender, EventArgs e)
        {
            cameraPos = new Vector3(-8, +8, -7);
            cameraLookAt = new Vector3(-1, +1, 0);
            cameraUp = new Vector3(+0.4, -1, 0);
            Refresh();
        }

        private void setPos1(object sender, EventArgs e)
        {
            pos.SetSfen("l6nl/5+P1gk/2np1S3/p1p4Pp/3P2Sp1/1PPb2P1P/P5GS1/R8/LN4bKL w RGgsn5p 1");
            Refresh();
        }

        private void setPos2(object sender, EventArgs e)
        {
            pos.UsiPositionCmd("startpos moves 7g7f 8c8d 2g2f 8d8e 2f2e 4a3b 8h7g 3c3d 7i6h 2b7g+ 6h7g 3a2b 3i3h 2b3c 3h2g 7c7d 2g2f 7a7b 2f1e B*4e 6i7h 1c1d 2e2d 2c2d 1e2d P*2g 2h4h 3c2d B*5e 6c6d 5e1a+ 2a3c 1a2a 3b4b P*2c S*2h 4g4f 4e6c 2c2b+ 3c2e 4f4e 2d3e 3g3f 2e3g+ 2i3g 2h3g+ 4h6h 3e4f 2b3b");
            Refresh();
        }

        private void setPos3(object sender, EventArgs e)
        {
            pos.SetSfen("4k4/9/9/9/9/9/9/9/9 b 2r2b4g4s4n4l18p 1");
            Refresh();
        }

        public BoardForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            pos = new Position();
            pos.InitBoard();
            pos.SetSfen("l6nl/5+P1gk/2np1S3/p1p4Pp/3P2Sp1/1PPb2P1P/P5GS1/R8/LN4bKL w RGgsn5p 1");
            cameraPos = new Vector3(0, -5.5, -11);
            cameraLookAt = new Vector3(0, -0.7, 0);
            cameraUp = new Vector3(0, 1, 0);
            pictureBox1.Paint += new PaintEventHandler(OnPaintHandler);
            pictureBox1.Resize += (s, e) => { Refresh(); };
        }
        static void OnPaintHandler(object s, PaintEventArgs e)
        {
            PictureBox pb = (PictureBox)s;
            BoardForm board = (BoardForm)pb.Parent;
            e.Graphics.SmoothingMode = board.smoothingMode;
            e.Graphics.TextRenderingHint = board.textRenderingHint;

            Matrix4 view = Matrix4.lookAt(
                board.cameraPos,
                board.cameraLookAt,
                board.cameraUp
            );
            Matrix4 projection =
                Matrix4.perspective(Math.PI * 0.42, (double)pb.Width / (double)pb.Height, 1, 100);
            //    Matrix4.orthographic(+11, -11, -11 * (double)pb.Width / (double)pb.Height, +11 * (double)pb.Width / (double)pb.Height, 1, 100);
            Matrix4 screen = Matrix4.translation(0.5 * pb.Width, 0.5 * pb.Height, 0)
                .mul(Matrix4.scaling(-pb.Width, -pb.Height, 1));
            Matrix4 vps = screen.mul(projection).mul(view);
            Ray rayLight = new Ray(
                new Vector3(-12, 0, -16),
                new Vector3(0.6, 0, 0.8).normalize()
            );

            // 駒台描画
            for (int i = 1; i >= -1; i -= 2)
            {
                new Face(
                    new Vector3[] {
                        new Vector3(
                            i * (4.5 * board.xmul + 0.3 * board.ymul),
                            +i * (4.5 * board.ymul + 0.25 * board.ymul),
                            board.handDepth
                        ),
                        new Vector3(
                            i * (4.5 * board.xmul + 0.3 * board.ymul),
                            -i * (4.5 * board.ymul + 0.25 * board.ymul),
                            board.handDepth
                        ),
                        new Vector3(
                            i * (4.5 * board.xmul + 0.3 * board.ymul + board.handWidth),
                            -i * (4.5 * board.ymul + 0.25 * board.ymul),
                            board.handDepth
                        ),
                        new Vector3(
                            i * (4.5 * board.xmul + 0.3 * board.ymul + board.handWidth),
                            +i * (4.5 * board.ymul + 0.25 * board.ymul),
                            board.handDepth
                        )
                    },
                    new Vector3(0, 0, -1)
                ).render(e.Graphics, vps, -1, DColor.DarkGray, rayLight, Quaternion.identity());
            }

            // 盤面描画
            new Face(
                new Vector3[] {
                    new Vector3(
                        -4.5 * board.xmul - 0.25 * board.ymul,
                        +4.5 * board.ymul + 0.25 * board.ymul,
                        0
                    ),
                    new Vector3(
                        -4.5 * board.xmul - 0.25 * board.ymul,
                        -4.5 * board.ymul - 0.25 * board.ymul,
                        0
                    ),
                    new Vector3(
                        +4.5 * board.xmul + 0.25 * board.ymul,
                        -4.5 * board.ymul - 0.25 * board.ymul,
                        0
                    ),
                    new Vector3(
                        +4.5 * board.xmul + 0.25 * board.ymul,
                        +4.5 * board.ymul + 0.25 * board.ymul,
                        0
                    ),
                },
                new Vector3(0, 0, -1)
            ).render(e.Graphics, vps, -1, DColor.Silver, rayLight, Quaternion.identity());

            // 盤面の線
            for (int i = 0; i <= 9; i++)
            {
                new Face(
                    new Vector3[] {
                        new Vector3((i - 4.5) * board.xmul - 0.5 * board.lineWidth, +4.5 * board.ymul + 0.5 * board.lineWidth, 0),
                        new Vector3((i - 4.5) * board.xmul - 0.5 * board.lineWidth, -4.5 * board.ymul - 0.5 * board.lineWidth, 0),
                        new Vector3((i - 4.5) * board.xmul + 0.5 * board.lineWidth, -4.5 * board.ymul - 0.5 * board.lineWidth, 0),
                        new Vector3((i - 4.5) * board.xmul + 0.5 * board.lineWidth, +4.5 * board.ymul + 0.5 * board.lineWidth, 0)
                    },
                    new Vector3(0, 0, -1)
                ).render(e.Graphics, vps, 0, DColor.Black, rayLight, Quaternion.identity());
                new Face(
                    new Vector3[] {
                        new Vector3(-4.5 * board.xmul - 0.5 * board.lineWidth, (i - 4.5) * board.ymul + 0.5 * board.lineWidth, 0),
                        new Vector3(-4.5 * board.xmul - 0.5 * board.lineWidth, (i - 4.5) * board.ymul - 0.5 * board.lineWidth, 0),
                        new Vector3(+4.5 * board.xmul + 0.5 * board.lineWidth, (i - 4.5) * board.ymul - 0.5 * board.lineWidth, 0),
                        new Vector3(+4.5 * board.xmul + 0.5 * board.lineWidth, (i - 4.5) * board.ymul + 0.5 * board.lineWidth, 0)
                    },
                    new Vector3(0, 0, -1)
                ).render(e.Graphics, vps, 0, DColor.Black, rayLight, Quaternion.identity());
            }

            // 盤面の星目
            for (double x = -1.5; x <= +1.5; x += 3)
            {
                for (double y = -1.5; y <= +1.5; y += 3)
                {
                    int cdiv = 36;
                    Vector3[] points = new Vector3[cdiv];
                    for (int i = 0; i < cdiv; i++)
                    {
                        double a = Math.PI * 2 * i / cdiv;
                        points[i] = new Vector3(x * board.xmul + Math.Cos(a) * board.starRadius, y * board.ymul + Math.Sin(a) * board.starRadius, 0);
                    }
                    new Face(
                        points,
                        new Vector3(0, 0, -1)
                    ).render(e.Graphics, vps, 0, DColor.Black, rayLight, Quaternion.identity());
                }
            }
            {
                string[] file_char = { "１","２","３","４","５","６","７","８","９" };
                string[] rank_char = { "一","二","三","四","五","六","七","八","九" };
                switch(board.coordCharset)
                {
                    case CoordCharset.NONE:
                        file_char = new string[] { "", "", "", "", "", "", "", "", "" };
                        rank_char = new string[] { "", "", "", "", "", "", "", "", "" };
                        break;
                    case CoordCharset.ARABIC:
                        file_char = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                        rank_char = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                        break;
                    case CoordCharset.CHESSLIKE:
                        file_char = new string[] { "1", "2", "３", "4", "5", "6", "7", "8", "9" };
                        rank_char = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
                        break;
                    case CoordCharset.NORMAL:
                    default:
                        file_char = new string[] { "１", "２", "３", "４", "５", "６", "７", "８", "９" };
                        rank_char = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九" };
                        break;
                }
                StringFormat fmt = new StringFormat();
                fmt.Alignment = StringAlignment.Center;
                fmt.LineAlignment = StringAlignment.Center;
                FontFamily ffam = FontFamily.GenericSansSerif;
                foreach (var famn in board.coordFontFamilyName)
                foreach (var fam in FontFamily.Families)
                if (fam.Name == famn)
                {
                    ffam = new FontFamily(famn);
                    goto fontsuccess;
                }
                fontsuccess:;
                FontStyle fsty = FontStyle.Regular;
                for (int i = 0; i < 9; ++i)
                {
                    var p = new GraphicsPath();
                    p.AddString(file_char[i], ffam, (int)fsty, 1, new PointF(0, 0), fmt);
                    RectangleF bounds = p.GetBounds();
                    if (bounds.Width == 0 || bounds.Height == 0) continue;
                    float bCx = (bounds.Left + bounds.Right) * 0.5f;
                    float bCy = (bounds.Top + bounds.Bottom) * 0.5f;
                    float bW = Math.Max(bounds.Width, 1f);
                    float bH = Math.Max(bounds.Height, 1f);
                    for (int j = -1; j <= +1; j += 2)
                    new CharFace(
                        new Vector2[] {
                            new Vector2(-0.6 * bW + bCx, +0.6 * bH + bCy),
                            new Vector2(-0.6 * bW + bCx, -0.6 * bH + bCy),
                            new Vector2(+0.6 * bW + bCx, -0.6 * bH + bCy),
                            new Vector2(+0.6 * bW + bCx, +0.6 * bH + bCy),
                        },
                        new Vector3[] {
                            new Vector3(-0.125 * board.ymul + (4 - i) * board.xmul, (-0.125 + j * 4.625) * board.ymul, 0),
                            new Vector3(-0.125 * board.ymul + (4 - i) * board.xmul, (+0.125 + j * 4.625) * board.ymul, 0),
                            new Vector3(+0.125 * board.ymul + (4 - i) * board.xmul, (+0.125 + j * 4.625) * board.ymul, 0),
                            new Vector3(+0.125 * board.ymul + (4 - i) * board.xmul, (-0.125 + j * 4.625) * board.ymul, 0),
                            new Vector3(-0.125 * board.ymul + (4 - i) * board.xmul, (-0.125 + j * 4.625) * board.ymul, 0),
                        },
                        p,
                        0
                    ).render(e.Graphics, vps, 0, new Pen(DColor.Transparent), new SolidBrush(DColor.Black));
                }
                for (int i = 0; i < 9; ++i)
                {
                    var p = new GraphicsPath();
                    p.AddString(rank_char[i], ffam, (int)fsty, 1, new PointF(0, 0), fmt);
                    RectangleF bounds = p.GetBounds();
                    if (bounds.Width == 0 || bounds.Height == 0) continue;
                    float bCx = (bounds.Left + bounds.Right) * 0.5f;
                    float bCy = (bounds.Top + bounds.Bottom) * 0.5f;
                    float bW = Math.Max(bounds.Width, 1f);
                    float bH = Math.Max(bounds.Height, 1f);
                    for (int j = -1; j <= +1; j += 2)
                    new CharFace(
                        new Vector2[] {
                            new Vector2(-0.6 * bW + bCx, +0.6 * bH + bCy),
                            new Vector2(-0.6 * bW + bCx, -0.6 * bH + bCy),
                            new Vector2(+0.6 * bW + bCx, -0.6 * bH + bCy),
                            new Vector2(+0.6 * bW + bCx, +0.6 * bH + bCy),
                        },
                        new Vector3[] {
                            new Vector3((j-1) * 0.125 * board.ymul + j * 4.5 * board.xmul, (-0.125 + 4-i) * board.ymul, 0),
                            new Vector3((j-1) * 0.125 * board.ymul + j * 4.5 * board.xmul, (+0.125 + 4-i) * board.ymul, 0),
                            new Vector3((j+1) * 0.125 * board.ymul + j * 4.5 * board.xmul, (+0.125 + 4-i) * board.ymul, 0),
                            new Vector3((j+1) * 0.125 * board.ymul + j * 4.5 * board.xmul, (-0.125 + 4-i) * board.ymul, 0),
                            new Vector3((j-1) * 0.125 * board.ymul + j * 4.5 * board.xmul, (-0.125 + 4-i) * board.ymul, 0),
                        },
                        p,
                        1
                    ).render(e.Graphics, vps, 0, new Pen(DColor.Transparent), new SolidBrush(DColor.Black));
                }
            }
            // 文字テスト
            string[][] chars;
            switch (board.pieceCharset)
            {
                case PieceCharset.twoChar:
                    chars = new string[][] {
                        new string[] { "玉将" },
                        new string[] { "歩兵", "と" },
                        new string[] { "香車", "成香" },
                        new string[] { "桂馬", "成桂" },
                        new string[] { "銀将", "成銀" },
                        new string[] { "角行", "竜馬" },
                        new string[] { "飛車", "龍王" },
                        new string[] { "金将" },
                        new string[] { "玉将" },
                    };
                    break;
                case PieceCharset.oneChar:
                default:
                    chars = new string[][] {
                        new string[] { "玉" },
                        new string[] { "歩", "と" },
                        new string[] { "香", "成香" },
                        new string[] { "桂", "成桂" },
                        new string[] { "銀", "成銀" },
                        new string[] { "角", "馬" },
                        new string[] { "飛", "龍" },
                        new string[] { "金" },
                        new string[] { "玉" },
                    };
                    break;
            }
            CharFace[][] cfaces = new CharFace[chars.Length][];
            {
                StringFormat fmt = new StringFormat();
                fmt.Alignment = StringAlignment.Center;
                fmt.LineAlignment = StringAlignment.Center;
                fmt.FormatFlags |= StringFormatFlags.DirectionVertical;
                FontFamily ffam = FontFamily.GenericSerif;
                foreach (var famn in board.pieceFontFamilyName)
                {
                    foreach (var fam in FontFamily.Families)
                    {
                        if (fam.Name == famn)
                        {
                            ffam = new FontFamily(famn);
                            goto fontsuccess;
                        }
                    }
                }
                fontsuccess:;
                FontStyle fsty = FontStyle.Regular;
                for (int i = 0; i < chars.Length; ++i)
                {
                    cfaces[i] = new CharFace[chars[i].Length];
                    for (int j = 0; j < chars[i].Length; ++j)
                    {
                        var p = new GraphicsPath();
                        p.AddString(chars[i][j], ffam, (int)fsty, 1, new PointF(0, 0), fmt);
                        RectangleF bounds = p.GetBounds();
                        float bCx = (bounds.Left + bounds.Right) * 0.5f;
                        float bCy = (bounds.Top + bounds.Bottom) * 0.5f;
                        float bW = Math.Max(bounds.Width, 0.9f);
                        float bH = Math.Max(bounds.Height, 1.5f);
                        float padV = .55f;
                        float padT = .55f;
                        float padB = .70f;
                        cfaces[i][j] = new CharFace(
                            new Vector2[] {
                                new Vector2(
                                    -padT * bW + bCx,
                                    -padV * bH + bCy
                                ),
                                new Vector2(
                                    -padB * bW + bCx,
                                    +padV * bH + bCy
                                ),
                                new Vector2(
                                    +padB * bW + bCx,
                                    +padV * bH + bCy
                                ),
                                new Vector2(
                                    +padT * bW + bCx,
                                    -padV * bH + bCy
                                ),
                            },
                            j == 0 ?
                            new Vector3[] {
                                PieceGeo.preset[i].p12,
                                PieceGeo.preset[i].p13,
                                PieceGeo.preset[i].p14,
                                PieceGeo.preset[i].p15,
                            }:
                            j == 1 ?
                            new Vector3[] {
                                PieceGeo.preset[i].p22,
                                PieceGeo.preset[i].p23,
                                PieceGeo.preset[i].p24,
                                PieceGeo.preset[i].p25,
                            }:
                            new Vector3[] {},
                            p,
                            j
                        );
                    }
                }
            }

            // 駒の配置計算
            var ptrans = board.calcPieces();
            // 遠い方が先になるようにソート
            var ptransSorted = new PieceTransform[ptrans.Length];
            ptrans.CopyTo(ptransSorted, 0);
            Array.Sort(ptransSorted, (x, y) => {
                // null は後回し
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return +1;
                // NaN は後回し
                if (Double.IsNaN(x.length) && Double.IsNaN(y.length)) return 0;
                if (Double.IsNaN(x.length)) return -1;
                if (Double.IsNaN(y.length)) return +1;
                // 遠い方を先に選ぶ
                double comp = y.length - x.length;
                if (Double.IsNaN(comp)) return 0;
                return Math.Sign(comp);
            });
            // 駒の描画
            foreach (var item in ptransSorted)
            {
                if (item == null) continue;
                Matrix4 vpsmat4 = vps.mul(item.trans.toMatrix4());
                // 駒面の描画
                foreach (var face in item.pgeo.faces)
                {
                    DColor c = item.piece.PieceColor() != SColor.WHITE ? DColor.FromArgb(240, 40, 32, 48) : DColor.FromArgb(240, 255, 255, 255);
                    face.render(e.Graphics, vpsmat4, -1, c, rayLight, item.trans.rot);
                }
                // 駒文字の描画
                foreach (CharFace cface in cfaces[(int)item.piece.RawPieceType()])
                {
                    Pen pen = new Pen(DColor.Transparent);
                    pen.LineJoin = LineJoin.Bevel;
                    Brush brush = new SolidBrush(cface.faceid == 0 ?
                        (item.piece.PieceColor() != SColor.WHITE ? DColor.White : DColor.Black) :
                        (item.piece.PieceColor() != SColor.WHITE ? DColor.FromArgb(255, 64, 64) : DColor.FromArgb(192, 0, 0)));
                    cface.render(e.Graphics, vpsmat4, -1, pen, brush);
                }
            }

        }

        // 盤面からの駒配置計算
        private PieceTransform[] calcPieces()
        {
            var ptrans = new PieceTransform[(int)pos.lastPieceNo];

            Console.WriteLine(pos.Pretty());

            // 盤面上の駒の配置計算
            for (Square sq = 0; sq < Square.NB; sq++)
            {
                File f = sq.ToFile();
                Rank r = sq.ToRank();

                Piece p = pos.PieceOn(sq);
                if (p == Piece.NO_PIECE) { continue; }
                Piece raw = p.RawPieceType();
                SColor c = p.PieceColor();
                bool pro = raw == Piece.NO_PIECE ? false : p.IsPromote();

                PieceGeo pgeo = PieceGeo.preset[(int)raw];

                // Modelの配置計算
                ModelTransform mtrans = new ModelTransform(new Vector3(0, 0, 0), Quaternion.rotationY(pro ? Math.PI : 0));
                mtrans.translation(new Vector3(0, pgeo.hheight, -pgeo.hthick));
                mtrans.rotation(Quaternion.rotationX(pgeo.gamma));
                mtrans.translation(new Vector3(0, -PieceGeo.preset[(int)Piece.KING].hfacevert, 0));
                if (c == SColor.WHITE) { mtrans.rotation(Quaternion.rotationZ(Math.PI)); }
                mtrans.translation(new Vector3((4 - (int)f) * xmul, (4 - (int)r) * ymul, 0));

                ptrans[(int)pos.PieceNoOn(sq)] = new PieceTransform(p, pgeo, mtrans, cameraPos);
            }

            // 駒台上の駒の配置計算
            foreach (SColor c in new SColor[] { SColor.BLACK, SColor.WHITE })
            {
                double hRank = 0;
                var hpieces = new List<PieceWithNo>[Piece.KING.ToInt()];
                var chpart1 = new CalcHandPart[Piece.KING.ToInt()];
                var l2r = (horder == HandOrder.BigBL || horder == HandOrder.BigUR);
                for (var p = Piece.PAWN; p < Piece.KING; ++p)
                {
                    var plist = hpieces[p.ToInt()] = new List<PieceWithNo>();
                    var hand = pos.Hand(c);
                    var count = hand.Count(p);
                    for (int i = 0; i < count; ++i)
                    {
                        plist.Add(new PieceWithNo(p, pos.HandPieceNo(c, p, i)));
                    }
                    chpart1[p.ToInt()] = CalcHandPart.calcPart(PieceGeo.preset, plist.ToArray(), l2r, handGap);
                }
                var tmp = new CalcHandPart();
                foreach (
                    var p in (horder == HandOrder.BigBL || horder == HandOrder.BigBR) ?
                    new Piece[] { Piece.ROOK, Piece.BISHOP, Piece.GOLD, Piece.SILVER, Piece.KNIGHT, Piece.LANCE, Piece.PAWN }:
                    new Piece[] { Piece.PAWN, Piece.LANCE, Piece.KNIGHT, Piece.SILVER, Piece.GOLD, Piece.BISHOP, Piece.ROOK }
                )
                {
                    if (chpart1[p.ToInt()].width > handInnerWidth)
                    {
                        Console.WriteLine("{0} overwidth", p);
                        var partCount = chpart1[p.ToInt()].separate.Count;
                        if (tmp.separate.Count > 0) hRank += renderHandPiece(ptrans, c, hRank, tmp, 1);
                        for (int i = 2; i < partCount; ++i)
                        {
                            int partlen = (partCount + i - 1) / i;
                            tmp = CalcHandPart.calcPart(PieceGeo.preset, chpart1[p.ToInt()].partPieces(0, partlen), l2r, handGap);
                            if (tmp.width > handInnerWidth) continue;
                            for (var j = 0; i > 0; --i)
                            {
                                partlen = (partCount + i - j - 1) / i;
                                tmp = CalcHandPart.calcPart(PieceGeo.preset, chpart1[p.ToInt()].partPieces(j, j + partlen), l2r, handGap);
                                hRank += renderHandPiece(ptrans, c, hRank, tmp, i);
                                j += partlen;
                            }
                            tmp = new CalcHandPart();
                            break;
                        }
                        continue;
                    }
                    var tmp2 = CalcHandPart.calcPart(PieceGeo.preset, tmp.combinePieces(chpart1[p.ToInt()]), l2r, handGap);
                    if (tmp2.width > handInnerWidth)
                    {
                        hRank += renderHandPiece(ptrans, c, hRank, tmp, 1);
                        tmp = chpart1[p.ToInt()];
                    }
                    else
                    {
                        tmp = tmp2;
                    }
                }
                hRank += renderHandPiece(ptrans, c, hRank, tmp, 1);
            }
            return ptrans;
        }

        double renderHandPiece(PieceTransform[] ptrans, SColor c, double hRank, CalcHandPart part, int j)
        {
            double distMul = 1.18;
            // 同種の持ち駒を多段に並べる必要がある時は j > 1
            // 後述のZオーダの問題により実質封印
            //double distMul = j > 1 ? 0.5 : 1.15;
            double distMax = 0;
            double thickMax = 0;
            foreach (var elem in part.separate)
            {
                distMax = Math.Max(distMax, elem.pGeo.fheight);
                thickMax = Math.Max(thickMax, elem.pGeo.thick);
            }
            double dist = distMax * distMul;
            foreach (var elem in part.separate)
            {
                var mtrans = new ModelTransform();
                mtrans.translation(new Vector3(0, elem.pGeo.hheight, -elem.pGeo.hthick));
                mtrans.rotation(Quaternion.rotationX(
                    // 駒を半分重ねる処理を試しに実装した跡。重ねた場合のZオーダリングが未解決のため実質封印。
                    // （静的な盤面表示を前提に描画のオーダリングを決め打ちする手もあるが、着手アニメーションを実装する場合は競合するのが見えているので）
                    dist >= elem.pGeo.height ? elem.pGeo.gamma : elem.pGeo.gamma - Math.Atan2(thickMax, dist)
                ));
                mtrans.rotation(Quaternion.rotationZ(elem.rotZ));
                mtrans.translation(elem.cPos);
                mtrans.translation(new Vector3(4.5 * xmul + 0.3 * ymul + handWidth * 0.5, (hRank - 4.5) * ymul, handDepth));
                if (c == SColor.WHITE) mtrans.rotation(Quaternion.rotationZ(Math.PI));
                // Console.WriteLine(String.Format("c:{0} piece:{1} pieceno:{2} rotZ:{3} trans:{4} rotQ:{5}", c, elem.pn.piece, elem.pn.no, elem.rotZ, mtrans.trans, mtrans.rot));
                ptrans[(int)elem.pn.no] = new PieceTransform(Util.MakePiece(c, elem.pn.piece), elem.pGeo, mtrans, cameraPos);
            }
            return dist;
        }

    }
    class Ray
    {
        public Vector3 position;
        public Vector3 direction;
        public Ray(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }
    class ModelTransform
    {
        public Vector3 trans;
        public Quaternion rot;
        public ModelTransform()
        {
            trans = new Vector3(0, 0, 0);
            rot = new Quaternion(0, 0, 0, 1);
        }
        public ModelTransform(Vector3 t, Quaternion r)
        {
            trans = t;
            rot = r;
        }
        public void translation(Vector3 t)
        {
            trans = trans.add(t);
        }
        public void rotation(Quaternion r)
        {
            trans = r.rotate(trans);
            rot = r.mul(rot);
        }
        public Matrix4 toMatrix4() => Matrix4.translation(trans.x, trans.y, trans.z).mul(rot.toRotationMatrix4());
    }
    class PieceTransform
    {
        public Piece piece;
        public PieceGeo pgeo;
        public ModelTransform trans;
        public double length;
        public PieceTransform(Piece piece, PieceGeo pgeo, ModelTransform trans, Vector3 cameraPos)
        {
            this.piece = piece;
            this.pgeo = pgeo;
            this.trans = trans;
            double length = double.PositiveInfinity;
            foreach (var point in new Vector3[] {
                pgeo.p11, pgeo.p12, pgeo.p13, pgeo.p14, pgeo.p15,
                pgeo.p21, pgeo.p22, pgeo.p23, pgeo.p24, pgeo.p25,
            })
            {
                length = Math.Min(trans.rot.rotate(point).add(trans.trans).sub(cameraPos).magnitude(), length);
            }
            this.length = length;
        }
    }
    class Face
    {
        public Vector3[] points;
        public Vector3 normal;
        public Face(Vector3[] points, Vector3 normal)
        {
            this.points = points;
            this.normal = normal;
        }
        public void render(Graphics g, Matrix4 mat4, int side, DColor color, Ray ray, Quaternion rot)
        {
            if (points == null || points.Length < 3)
            {
                return;
            }
            Vector3[] vert = new Vector3[points.Length + 1];
            PointF[] pts = new PointF[points.Length + 1];
            for (int i = 0; i < points.Length; i++)
            {
                vert[i] = mat4.mul(points[i]).cart();
                pts[i] = new PointF((float)vert[i].x, (float)vert[i].y);
            }
            vert[points.Length] = vert[0];
            pts[points.Length] = pts[0];

            // 左回り・右回り判定
            double s = 0;
            for (int i = 0; i < points.Length; i++)
            {
                s += vert[i].x * vert[i + 1].y - vert[i + 1].x * vert[i].y;
            }
            if (Double.IsNaN(s) || side != 0 && side != Math.Sign(s)) { return; }

            double dot = Math.Min(Math.Max(-rot.rotate(normal).dot(ray.direction) * 1.25 + 0.4, 0.4), 1);

            Pen pen = new Pen(color, 1);
            // LineJoin のプロパティ値はデフォルトで LineJoin.Miter （マイター結合）であり、
            // このままでは非常に鋭角な折れ線を描画する際に角が飛び出してしまう。
            // 折れ線の角が飛び出すのは見苦しいので、他の接合スタイルを明示的に指定。
            pen.LineJoin = LineJoin.Bevel;
            SolidBrush brush = new SolidBrush(DColor.FromArgb(color.A, (int)(color.R * dot), (int)(color.G * dot), (int)(color.B * dot)));
            g.FillPolygon(brush, pts);
            g.DrawLines(pen, pts);

            brush.Dispose();
            pen.Dispose();
        }
    }
    enum HandOrder
    {
        BigUL,
        BigUR,
        BigBL,
        BigBR,
    }
    enum PieceCharset
    {
        oneChar,
        twoChar,
    }
    enum CoordCharset
    {
        NONE,
        NORMAL,
        ARABIC,
        CHESSLIKE,
    }
    struct PieceWithNo
    {
        public Piece piece;
        public PieceNo no;
        public PieceWithNo(Piece _p, PieceNo _no)
        {
            piece = _p;
            no = _no;
        }
    }
    class CalcHandPart
    {
        public TransHand overall { get; private set; }
        public List<TransHand> separate { get; private set; }
        public double width { get; private set; }
        public CalcHandPart()
        {
            overall = new TransHand(PieceGeo.preset[Piece.NO_PIECE.ToInt()], new PieceWithNo(Piece.ZERO, PieceNo.NONE), Vector3.zero(), Vector3.zero(), Vector3.zero(), 0);
            separate = new List<TransHand>();
            width = 0;
        }
        CalcHandPart rotationZ(double ang)
        {
            overall.rotationZ(ang);
            foreach (var th in separate)
            {
                th.rotationZ(ang);
            }
            return this;
        }
        CalcHandPart translate(Vector3 v)
        {
            overall.translate(v);
            foreach (var th in separate)
            {
                th.translate(v);
            }
            return this;
        }
        public PieceWithNo[] partPieces(int start, int end)
        {
            var res = new PieceWithNo[end - start];
            for (var i = 0; start + i < end; ++i)
            {
                res[i] = separate[start + i].pn;
            }
            return res;
        }
        public PieceWithNo[] combinePieces(CalcHandPart b)
        {
            var res = new PieceWithNo[separate.Count + b.separate.Count];
            for (var i = 0; i < separate.Count; ++i)
            {
                res[i] = separate[i].pn;
            }
            for (var i = 0; i < b.separate.Count; ++i)
            {
                res[i + separate.Count] = b.separate[i].pn;
            }
            return res;
        }
        public static CalcHandPart calcPart(PieceGeo[] pgeos, PieceWithNo[] pieces, bool l2r, double gapwidth)
        {
            var res = new CalcHandPart();
            foreach (var p in pieces)
            {
                var geo = pgeos[p.piece.RawPieceType().ToInt()];
                // これまで並べた駒をbetaの角度で回転+駒幅分ずらす
                res.rotationZ(l2r ? -geo.beta : +geo.beta);
                res.translate(new Vector3(l2r ? -geo.width - gapwidth : geo.width + gapwidth, 0, 0));
                // 次の持駒を配置
                res.separate.Add(l2r ? new TransHand(
                    geo,
                    p,
                    new Vector3(-(geo.width) - gapwidth, 0, 0),
                    new Vector3(-(geo.hwidth) - 0.5 * gapwidth, 0, 0),
                    Vector3.zero(),
                    0.0
                ) : new TransHand(
                    geo,
                    p,
                    Vector3.zero(),
                    new Vector3(geo.hwidth + 0.5 * gapwidth, 0, 0),
                    new Vector3(geo.width + gapwidth, 0, 0),
                    0.0
                ));
                // さらにbetaの角度で回転
                res.rotationZ(l2r ? -(geo.beta) : +(geo.beta));
                // 端点を再設定
                if (l2r)
                    res.overall.rPosClear();
                else
                    res.overall.lPosClear();
            }
            res.translate(new Vector3(
                -0.5 * (res.overall.lPos.x + res.overall.rPos.x),
                -0.5 * (res.overall.lPos.y + res.overall.rPos.y),
                -0.5 * (res.overall.lPos.z + res.overall.rPos.z)
            ));
            res.overall.cPosClear();
            if (pieces.Length > 0)
            {
                res.rotationZ(-Math.Atan2(res.overall.rPos.y, res.overall.rPos.x));
            }
            {
                var minX = Double.PositiveInfinity;
                var maxX = Double.NegativeInfinity;
                var minY = Double.PositiveInfinity;
                foreach (var th in res.separate)
                {
                    minX = Math.Min(minX, th.rPos.x);
                    maxX = Math.Max(maxX, th.lPos.x);
                    minY = Math.Min(minY, th.lPos.y);
                }
                res.translate(new Vector3(
                    -0.5 * (maxX + minX),
                    -minY,
                    0
                ));
                res.width = maxX - minX;
            }
            return res;
        }
    }
    class TransHand
    {
        public PieceGeo pGeo;
        public PieceWithNo pn;
        public Vector3 lPos, cPos, rPos;
        public double rotZ;
        public TransHand(PieceGeo _pGeo, PieceWithNo _pn, Vector3 _lPos, Vector3 _cPos, Vector3 _rPos, double _rotZ)
        {
            pGeo = _pGeo;
            pn = _pn;
            lPos = _lPos;
            cPos = _cPos;
            rPos = _rPos;
            rotZ = _rotZ;
        }
        public TransHand rotationZ(double ang)
        {
            Quaternion q = Quaternion.rotationZ(ang);
            lPos = q.rotate(lPos);
            cPos = q.rotate(cPos);
            rPos = q.rotate(rPos);
            rotZ += ang;
            return this;
        }
        public TransHand translate(Vector3 b)
        {
            lPos = lPos.add(b);
            cPos = cPos.add(b);
            rPos = rPos.add(b);
            return this;
        }

        internal void lPosClear()
        {
            lPos = Vector3.zero();
        }

        internal void cPosClear()
        {
            cPos = Vector3.zero();
        }

        internal void rPosClear()
        {
            rPos = Vector3.zero();
        }
    }
    class CharFace
    {
        public Vector2[] fromPoints;
        public Vector3[] toPoints;
        public GraphicsPath path;
        public int faceid;
        public CharFace(Vector2[] fromPoints, Vector3[] toPoints, GraphicsPath path, int faceid)
        {
            this.fromPoints = fromPoints;
            this.toPoints = toPoints;
            this.path = path;
            this.faceid = faceid;
        }
        public void render(Graphics g, Matrix4 mat4, int side, Pen pen, Brush brush)
        {
            if (fromPoints == null || fromPoints.Length < 4 || toPoints == null || toPoints.Length < 4)
            {
                return;
            }
            Vector2[] toPoints_ = new Vector2[5];
            PointF[] toPoints_border = new PointF[5];
            for (int i = 0; i < 4; ++i)
            {
                toPoints_[i] = mat4.mul(toPoints[i]).cart().xy();
                toPoints_border[i] = new PointF((float)toPoints_[i].x, (float)toPoints_[i].y);
                // Console.WriteLine(String.Format("i:{0} x:{1} y:{2}", i, toPoints_[i].x, toPoints_[i].y));
            }
            toPoints_[4] = toPoints_[0];
            toPoints_border[4] = toPoints_border[0];

            // 左回り・右回り判定
            double s = 0;
            for (int i = 0; i < 4; ++i)
            {
                s += toPoints_[i].x * toPoints_[i + 1].y - toPoints_[i + 1].x * toPoints_[i].y;
            }
            if (Double.IsNaN(s) || side != 0 && side != Math.Sign(s)) { return; }

            // デバッグ用(文字描画目標範囲境界線)
            //g.DrawLines(new Pen(DColor.Red), toPoints_border);

            Matrix3 mat3 = Matrix3.projectiveTransform(
                toPoints_[0], toPoints_[1], toPoints_[2], toPoints_[3]
            ).mul(Matrix3.projectiveInvTransform(
                fromPoints[0], fromPoints[1], fromPoints[2], fromPoints[3]
            ));

            PointF[] fromPts = path.PathData.Points;
            byte[] fromType = path.PathData.Types;
            PointF[] toPts = new PointF[fromPts.Length];

            for (int i = 0; i < fromPts.Length; ++i)
            {
                Vector2 vec2 = mat3.mul(new Vector2(fromPts[i].X, fromPts[i].Y)).cart();
                toPts[i] = new PointF((float)vec2.x, (float)vec2.y);
            }

            GraphicsPath toPath = new GraphicsPath(toPts, fromType);

            g.DrawPath(pen, toPath);
            g.FillPath(brush, toPath);

        }
    }
    class PieceGeo
    {
        public Piece piece;
        public Face[] faces;
        public double
            height, width, thick,
            alpha, beta, gamma,
            sinAlpha, cosAlpha, tanAlpha,
            sinBeta, cosBeta, tanBeta,
            sinGamma, cosGamma, tanGamma,
            hheight, hwidth, hthick, hfacevert,
            shol, sholw, sholh,
            side, sidew, sideh,
            hsholt, sholt,
            hheadt, headt,
            sholy, perpe, radius, fheight;
        public Vector3
            p11, p12, p13, p14, p15,
            p21, p22, p23, p24, p25;
        public PieceGeo(
            Piece piece,
            double width,
            double height,
            double thick,
            double alpha = 18 * Math.PI / 180,
            double beta = 9 * Math.PI / 180,
            double gamma = 5 * Math.PI / 180
        )
        {
            // 駒種
            this.piece = piece;
            // 高さ
            this.height = height;
            // 幅
            this.width = width;
            // 厚さ
            this.thick = thick;
            // 駒底面と駒肩面のなす角
            this.alpha = alpha;
            this.sinAlpha = Math.Sin(alpha);
            this.cosAlpha = Math.Cos(alpha);
            this.tanAlpha = Math.Tan(alpha);
            // 駒底面の垂線と駒脇面のなす角
            this.beta = beta;
            this.sinBeta = Math.Sin(beta);
            this.cosBeta = Math.Cos(beta);
            this.tanBeta = Math.Tan(beta);
            // 駒底面の垂線と駒表面のなす角
            this.gamma = gamma;
            this.sinGamma = Math.Sin(gamma);
            this.cosGamma = Math.Cos(gamma);
            this.tanGamma = Math.Tan(gamma);
            // 0.5 * 高さ
            this.hheight = 0.5 * height;
            // 0.5 * 幅
            this.hwidth = 0.5 * width;
            // 0.5 * 厚さ
            this.hthick = 0.5 * thick;
            // 0.5 * 駒表面縦長さ
            this.hfacevert = hheight / cosGamma;
            // 肩部長さ
            this.shol = (hwidth - height * tanBeta) / (cosAlpha - sinAlpha * tanBeta);
            // 肩部幅
            this.sholw = shol * cosAlpha;
            // 肩部高さ
            this.sholh = shol * sinAlpha;
            // 側部長さ
            this.side = (height - hwidth * tanAlpha) / (cosBeta - sinBeta * tanAlpha);
            // 側部幅
            this.sidew = side * sinBeta;
            // 側部高さ
            this.sideh = side * cosBeta;
            // 0.5 * 肩厚さ
            this.hsholt = hthick - sideh * tanGamma;
            // 肩厚さ
            this.sholt = 2 * hsholt;
            // 0.5 * 頭厚さ
            this.hheadt = hthick - height * tanGamma;
            // 頭厚さ
            this.headt = 2 * hheadt;
            // 中心～肩の高さ
            this.sholy = hheight - sholh;
            // 中心～駒表面までの長さ
            this.perpe = hthick * cosGamma - hheight * sinGamma;
            // 中心から腰までの長さ
            this.radius = Math.Sqrt(hwidth * hwidth + hheight * hheight + hthick * hthick);
            // 頂点リスト
            p11 = new Vector3(0, +hheight, -hheadt);
            p12 = new Vector3(-sholw, +sholy, -hsholt);
            p13 = new Vector3(-hwidth, -hheight, -hthick);
            p14 = new Vector3(+hwidth, -hheight, -hthick);
            p15 = new Vector3(+sholw, +sholy, -hsholt);
            p21 = new Vector3(0, +hheight, +hheadt);
            p22 = new Vector3(+sholw, +sholy, +hsholt);
            p23 = new Vector3(+hwidth, -hheight, +hthick);
            p24 = new Vector3(-hwidth, -hheight, +hthick);
            p25 = new Vector3(-sholw, +sholy, +hsholt);
            {
                // 底面端～各頂点までの最大長
                fheight = 0;
                foreach (var p in  new Vector3[] { p11, p12, p13, p14, p15 })
                {
                    fheight = Math.Max(p11.sub(p23).magnitude(), fheight);
                }
            }
            /// 面定義
            this.faces = new Face[] {
            new Face(
                new Vector3[] { p11, p12, p13, p14, p15 },
                new Vector3(0, +sinGamma, -cosGamma)),
            new Face(
                new Vector3[] { p21, p22, p23, p24, p25 },
                new Vector3(0, +sinGamma, +cosGamma)),
            new Face(
                new Vector3[] { p11, p21, p25, p12 },
                new Vector3(-sinAlpha, +cosAlpha, 0)),
            new Face(
                new Vector3[] { p12, p25, p24, p13 },
                new Vector3(-cosBeta, +sinBeta, 0)),
            new Face(
                new Vector3[] { p13, p24, p23, p14 },
                new Vector3(0, -1, 0)),
            new Face(
                new Vector3[] { p14, p23, p22, p15 },
                new Vector3(+cosBeta, +sinBeta, 0)),
            new Face(
                new Vector3[] { p15, p22, p21, p11 },
                new Vector3(+sinAlpha, +cosAlpha, 0)),
        };
        }
        public static PieceGeo[] preset = new PieceGeo[] {
        new PieceGeo(Piece.KING,   0.741, 0.825, 0.251),
        new PieceGeo(Piece.PAWN,   0.581, 0.700, 0.200),
        new PieceGeo(Piece.LANCE,  0.607, 0.725, 0.207),
        new PieceGeo(Piece.KNIGHT, 0.659, 0.750, 0.214),
        new PieceGeo(Piece.SILVER, 0.690, 0.775, 0.227),
        new PieceGeo(Piece.BISHOP, 0.715, 0.800, 0.240),
        new PieceGeo(Piece.ROOK,   0.715, 0.800, 0.240),
        new PieceGeo(Piece.GOLD,   0.690, 0.775, 0.227),
        new PieceGeo(Piece.KING,   0.741, 0.825, 0.251)
    };
    }
}
