﻿using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TelerikUI;
using System.Drawing;
using System.Collections.Generic;

namespace Examples
{
	public class LiveData : ExampleViewController
	{
		TKChart chart;
		List<TKChartDataPoint> dataPoints;
		TKChartLineSeries lineSeries;

		public LiveData ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.chart = new TKChart (this.ExampleBounds);
			this.chart.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			this.View.AddSubview (chart);
			this.dataPoints = new List<TKChartDataPoint> ();
			NSTimer.CreateRepeatingScheduledTimer (0.127, UpdateChart);
		}

		public void UpdateChart () {
			this.chart.RemoveAllData ();
			Random r = new Random ();
			TKChartDataPoint dataPoint = new TKChartDataPoint (NSDate.Now, new NSNumber (r.Next () % 70));
			this.dataPoints.Add (dataPoint);
			if (this.dataPoints.Count > 25) {
				this.dataPoints.RemoveAt (0);
			}

			this.chart.YAxis = new TKChartNumericAxis (new NSNumber (0), new NSNumber (100));
			TKChartDataPoint firstPoint = this.dataPoints [0];
			TKChartDataPoint lastPoint = this.dataPoints [this.dataPoints.Count - 1];
			TKChartDateTimeAxis xAxis = new TKChartDateTimeAxis (firstPoint.DataXValue, lastPoint.DataXValue);
			xAxis.Style.LabelStyle.FitMode = TKChartAxisLabelFitMode.None;
			xAxis.Style.MajorTickStyle.MaxTickClippingMode = TKChartAxisClippingMode.Visible;
			xAxis.MajorTickIntervalUnit = TKChartDateTimeAxisIntervalUnit.Seconds;
			this.chart.XAxis = xAxis;

			this.lineSeries = new TKChartLineSeries (this.dataPoints.ToArray ());
			this.chart.AddSeries (this.lineSeries);
			this.chart.ReloadData ();
		}
	}
}

