using Microsoft.SolverFoundation.Solvers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SolverFoundation.Services;
using ZedGraph;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Security.Permissions;
using static System.Windows.Forms.LinkLabel;

namespace LinearProgrammingTasks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Graphics _graphics;
        private List<Point> _points = new List<Point>();
        private Point _currentPoint;
        private Point _destinationPoint;
        private int _selectedPointIndex;
        private bool _isDragging;

        private void button1_Click(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            GraphPane pane = zedGraphControl1.GraphPane;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();

            for (double i = -10; i <= 10; i += 0.1)
            {
                double x1 = i;
                double x2 = (10 - 2 * x1);
                list1.Add(x1, x2);

                x2 = (6 + 2 * x1) / 3;
                list2.Add(x1, x2);

                x2 = (8 - 2 * x1) / 4;
                list3.Add(x1, x2);
            }

            LineItem myCurve = pane.AddCurve("2x1 + x2 ≤ 10", list1, Color.Blue, SymbolType.None);
            LineItem myCurve2 = pane.AddCurve("-2x1 + 3x2 ≤ 6", list2, Color.Red, SymbolType.None);
            LineItem myCurve3 = pane.AddCurve("2x1 + 4x2 ≥ 8", list3, Color.Green, SymbolType.None);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            GraphPane myPane = zedGraphControl1.GraphPane;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();
            PointPairList list5 = new PointPairList();

            for (double x2 = -10; x2 <= 10; x2 += 0.1)
            {
                double x1_1 = 4 - 2 * x2;
                double x1_2 = 3 - 3 * x2;
                double x1_3 = (8 - x2) / 2;

                list1.Add(x1_1, x2);
                list2.Add(x1_2, x2);
                list3.Add(x1_3, x2);
            }

            list4.Add(0, 0);
            list4.Add(0, 1);

            LineItem myCurve1 = myPane.AddCurve("x1 + 2x2 = 4", list1, Color.Blue, SymbolType.None);
            LineItem myCurve2 = myPane.AddCurve("x1 + 3x2 = 3", list2, Color.Red, SymbolType.None);
            LineItem myCurve3 = myPane.AddCurve("2x1 + x2 = 8", list3, Color.Red, SymbolType.None);
            LineItem myCurve4 = myPane.AddCurve("", list4, Color.Black, SymbolType.None);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
    }
}
