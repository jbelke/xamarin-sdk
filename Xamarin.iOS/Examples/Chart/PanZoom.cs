﻿using System;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using TelerikUI;

namespace Examples
{
	public class PanZoom: ExampleViewController
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TKChart chart = new TKChart ();
			chart.Frame = this.ExampleBounds;
			chart.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			this.View.AddSubview (chart);

			Random r = new Random ();
			List<TKChartDataPoint> list = new List<TKChartDataPoint> ();
			for (int i = 0; i < 200; i++) {
				list.Add (new TKChartDataPoint(new NSNumber(r.Next() % 200), new NSNumber(r.Next() % 1000)));
			}
			chart.AddSeries (new TKChartScatterSeries (list.ToArray()));

			list = new List<TKChartDataPoint> ();
			for (int i = 0; i < 200; i++) {
				list.Add (new TKChartDataPoint(new NSNumber(r.Next() % 200), new NSNumber(r.Next() % 1000)));
			}
			chart.AddSeries (new TKChartScatterSeries (list.ToArray()));

			list = new List<TKChartDataPoint> ();
			for (int i = 0; i < 200; i++) {
				list.Add (new TKChartDataPoint(new NSNumber(r.Next() % 200), new NSNumber(r.Next() % 1000)));
			}

			chart.AddSeries (new TKChartScatterSeries (list.ToArray()));

			chart.XAxis.AllowZoom = true;
			chart.XAxis.AllowPan = true;
			chart.YAxis.AllowZoom = true;
			chart.YAxis.AllowPan = true;
		}
	}
}

