﻿using System;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using TelerikUI;

namespace Examples
{
	public class BindingWithDataPoint: ExampleViewController
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TKChart chart = new TKChart (this.ExampleBounds);
			chart.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			this.View.AddSubview (chart);

			Random r = new Random ();
			List<DataPointImpl> data = new List<DataPointImpl> ();
			for (int i = 0; i <= 5; i++) {
				DataPointImpl impl = new DataPointImpl () {
					ObjectID = i,
					Value = r.Next(100)
				};
				data.Add (impl);
			}

			TKChartColumnSeries columnSeries = new TKChartColumnSeries (data.ToArray ());
			columnSeries.SelectionMode = TKChartSeriesSelectionMode.Series;
			chart.AddSeries (columnSeries);
		}

		class DataPointImpl: TKChartDataPoint
		{
			public int ObjectID { get; set; }

			public double Value { get; set; }

			override public NSObject DataXValue {
				[Export ("dataXValue")]
				get {
					return new NSNumber(this.ObjectID);
				}
			}

			override public NSObject DataYValue {
				[Export ("dataYValue")]
				get {
					return new NSNumber(this.Value);
				}
			}
		}
	}
}

