using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace forecasting
{
    public partial class Form1 : Form
    {
        private const string Title = "Visualisation of Demands of the sword per month";
        private const string XAxis = "Month";
        private const string YAxis = "Demand";

        private readonly List<DesResult> _desResults = new List<DesResult>();
        private readonly List<SesResult> _sesResults = new List<SesResult>();

        private readonly PointPairList _normal = new PointPairList();
        private SesResult _finalSes;
        private DesResult _finalDes;

        private double _lowestY;
        private double _highestY;

        private const string PathToFile =
            "C:\\Users\\ceesj\\Documents\\hogeschool\\data science\\forecasting\\forecasting\\SwordForecasting.csv";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //parse csv file
            var lines = File.ReadAllLines(PathToFile)
                .Skip(1)
                .Take(36)
                .Select(line => line.Split(';').Select(int.Parse).ToList())
                .ToList();
            foreach (var line in lines)
            {
                _normal.Add(line[0], line[1]);
            }

            //calculate ses and forecast it
            Ses(lines);
            ForecastSes(12);

            //calculate des and forecast it
            Des(lines);
            ForecastDes(12);

            var lowestNormalY = lines.Min(line => line[1]);
            var highestNormalY = lines.Max(line => line[1]);

            var lowestSesY = _finalSes.Ses.Min(line => line.Y);
            var highestSesY = _finalSes.Ses.Max(line => line.Y);

            var lowestDesY = _finalDes.Des.Min(line => line.Y);
            var highestDesY = _finalDes.Des.Max(line => line.Y);
            _lowestY = lowestNormalY < lowestDesY ? lowestNormalY : lowestDesY;
            _highestY = highestNormalY > highestDesY ? highestNormalY : highestDesY;
            _lowestY = _lowestY < lowestSesY ? _lowestY : lowestSesY;
            _highestY = _highestY > highestSesY ? _highestY : highestSesY;
            _lowestY -= 10;
            _highestY += 10;

            ShowGraph();

            MessageBox.Show(this,
                @"Best Alpha for Ses: " + _finalSes.GetAlpha() + @"
" + @"Best Alpha for Des: " + _finalDes.GetAlpha() +
                @"
" + @"Best Bet for Des: " + _finalDes.GetBeta());
        }

        /// <summary>
        /// On resize action, resize the ZedGraphControl to fill most of the Form, with a small
        /// margin around the outside
        /// </summary>
        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zg1.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(ClientRectangle.Width - 20,
                ClientRectangle.Height - 20);
        }

        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private static string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
            CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            var pt = curve[iPt];

            return "Demand is " + pt.Y.ToString("f2") + " units at month " + pt.X.ToString("f1");
        }


        private void ShowGraph()
        {
            // Get a reference to the GraphPane instance in the ZedGraphControl
            var myPane = zg1.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = Title;
            myPane.XAxis.Title.Text = XAxis;
            myPane.YAxis.Title.Text = YAxis;


            // Generate a red curve with diamond symbols, and "Alpha" in the legend
            myPane.AddCurve("normal",
                _normal, Color.Blue, SymbolType.Diamond);

            myPane.AddCurve("ses",
                _finalSes.Ses, Color.Red, SymbolType.Circle);

            myPane.AddCurve("des",
                _finalDes.Des, Color.Green, SymbolType.Triangle);

            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = _lowestY;
            myPane.YAxis.Scale.Max = _highestY;
            myPane.XAxis.Scale.Max = _finalDes.Des.Count + 2;

            // Enable scrollbars if needed
            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsAutoScrollRange = true;
            zg1.IsScrollY2 = true;

            //Show tooltips when the mouse hovers over a point
            zg1.IsShowPointValues = true;
            zg1.PointValueEvent += MyPointValueHandler;

            // Size the control to fit the window
            SetSize();

            // Tell ZedGraph to calculate the axis ranges
            // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
            // up the proper scrolling parameters
            zg1.AxisChange();
            // Make sure the Graph gets redrawn
            zg1.Invalidate();
        }

        private void Ses(IList<List<int>> lines)
        {
            var sesAlpha = 0.0;
            for (var j = 0; j < 10; j++)
            {
                sesAlpha += 0.1;
                var ses = new PointPairList {{lines[0][0], lines[0][1]}};
                //CALCULATE SES
                var startcount = lines[1][0];
                for (var i = 1; i < lines.Count + 1; i++)
                {
                    var smooth = sesAlpha * lines[i - 1][1] + (1 - sesAlpha) * ses[i - 1].Y;
                    ses.Add(startcount++, smooth);
                }
                var sesresult = new SesResult(ses, sesAlpha, lines);
                _sesResults.Add(sesresult);
            }
            _finalSes = _sesResults.Aggregate((curMin, x) => x.Error < curMin.Error ? x : curMin);
        }

        private void ForecastSes(int forecastamount)
        {
            for (var i = 0; i < forecastamount; i++)
            {
                _finalSes.Ses.Add(_finalSes.Ses.Count, _finalSes.Ses[_finalSes.Ses.Count - 1].Y);
            }
        }

        private void Des(IList<List<int>> lines)
        {
            var alpha = 0.0;
            var beta = 0.0;

            for (var j = 0; j < 10; j++)
            {
                alpha += 0.5;
                for (var k = 0; k < 10; k++)
                {
                    beta += 0.5;

                    var des = new PointPairList
                    {
                        {lines[0][0], lines[0][1]},
                        {lines[1][0], lines[1][1]}
                    };
                    var trends = new List<double>
                    {
                        lines[0][1],
                        lines[1][1] - lines[0][1]
                    };

                    //CALCULATE DES
                    for (var i = 2; i < lines.Count; i++)
                    {
                        var smoothfactor = alpha * lines[i][1] + (1 - alpha) * (des[i - 1].Y + trends[i - 1]);
                        des.Add(lines[i][0], smoothfactor);
                        var trend = beta * (des[i].Y - des[i - 1].Y) + (1 - beta) * trends[i - 1];
                        trends.Add(trend);
                    }
                    var desresult = new DesResult(des, trends, alpha, beta, lines);
                    _desResults.Add(desresult);
                }
            }
            _finalDes = _desResults.Aggregate((curMin, x) => x.Error < curMin.Error ? x : curMin);
        }

        private void ForecastDes(int forecastAmount)
        {
            var lastTrend = _finalDes.Trends[_finalDes.Trends.Count - 1];
            var lastSmooth = _finalDes.Des[_finalDes.Des.Count - 1];
            for (var i = 0; i < forecastAmount; i++)
            {
                _finalDes.Des.Add(_finalDes.Des.Count, lastSmooth.Y + i * lastTrend);
            }
        }
    }
}