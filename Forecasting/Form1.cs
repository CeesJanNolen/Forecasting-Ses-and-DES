using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Forecasting
{
  public partial class Form1 : Form
  {
	public Form1()
	{
	  InitializeComponent();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
	  CreateGraph(zedGraphControl1, "hoi", "as", "asd"); 
	}

	public void CreateGraph(ZedGraphControl zgc, string Title, string Xas, string Yas)
	{
	  //add reference
	  GraphPane MyGraph = zgc.GraphPane;

	  //add the titles to the graph
	  MyGraph.Title.Text = Title;
	  MyGraph.XAxis.Title.Text = Xas;
	  MyGraph.YAxis.Title.Text = Yas;

	  double x;
	  double y1;
	  double y2;

	  PointPairList List1 = new PointPairList();
	  PointPairList List2 = new PointPairList();

	  for (int i = 0; i < 10; i++)
	  {
		x = (double)0 + i;
		y1 = x * x;
		y2 = x * 2;

		List1.Add(x, y1);
		List2.Add(x, y2);
	  }

	  MyGraph.AddCurve("x^2", List1, Color.Blue, SymbolType.Square);
	  MyGraph.AddCurve("2x", List2, Color.Red, SymbolType.Star);

	  zgc.AxisChange();

	}
  }
}
