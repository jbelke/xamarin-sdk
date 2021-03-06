﻿using System;
using System.Collections.Generic;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;

using TelerikUI;

namespace Examples
{
	public class UIKitDynamicsAnimation: ExampleViewController
	{
		TKChart chart;
		UIDynamicAnimator animator;
		TKChartVisualPoint selectedPoint;
		PointF originalLocation;
		PointF originalPosition;
		List<TKChartDataPoint> points;
		List<PointF> originalValues;

		public UIKitDynamicsAnimation ()
		{
			this.AddOption ("Apply Gravity", applyGravity);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			chart = new TKChart (this.ExampleBounds);
			chart.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			chart.AllowAnimations = true;
			this.View.AddSubview (chart);

			this.reloadChart (this, EventArgs.Empty);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			TKChartVisualPoint[] points = chart.VisualPointsForSeries (chart.Series [0]);
			originalValues = new List<PointF> ();
			foreach (TKChartVisualPoint p in points) {
				originalValues.Add (p.CGPoint);
			}
			TKChartVisualPoint point = points[4];

			UISnapBehavior snap = new UISnapBehavior (point, point.Center);
			snap.Damping = 0.2f;

			UIPushBehavior push = new UIPushBehavior (new IUIDynamicItem[] { point }, UIPushBehaviorMode.Instantaneous);
			push.PushDirection = new CGVector (0.0f, -1.0f);
			push.Magnitude = 0.003f;

			UIDynamicAnimator animator = new UIDynamicAnimator();
			animator.AddBehavior(snap);
			animator.AddBehavior(push);

			point.Animator = animator;
		}

		public void applyGravity(object sender, EventArgs e)
		{
			animator = new UIDynamicAnimator (chart.PlotView);

			TKChartVisualPoint[] points = chart.VisualPointsForSeries (chart.Series [0]);

			for (int i=0; i<points.Length; i++) {
				TKChartVisualPoint point = points [i];
				PointF center = originalValues [i];
				if (point.Animator != null) {
					point.Animator.RemoveAllBehaviors();
					point.Animator = null;
				}
				point.Center = center;
			}

			UICollisionBehavior collision = new UICollisionBehavior (points);
			collision.TranslatesReferenceBoundsIntoBoundary = true;

			UIGravityBehavior gravity = new UIGravityBehavior (points);
			gravity.GravityDirection = new CGVector (0.0f, 2.0f);

			UIDynamicItemBehavior dynamic = new UIDynamicItemBehavior (points);
			dynamic.Elasticity = 0.5f;

			animator.AddBehavior(dynamic);
			animator.AddBehavior(gravity);
			animator.AddBehavior(collision);
		}

		public void reloadChart(object sender, EventArgs e)
		{
			Random r = new Random ();
			points = new List<TKChartDataPoint> ();
			for (int i = 0; i < 10; i++) {
				float x = i * 10;
				float y = r.Next () % 100;
				TKChartDataPoint point = new TKChartDataPoint (new NSNumber(x), new NSNumber(y));
				points.Add (point);
			}

			TKChartLineSeries lineSeries = new TKChartLineSeries (points.ToArray());
			float shapeSize = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? 14 : 17;
			lineSeries.SelectionMode = TKChartSeriesSelectionMode.DataPoint;
			lineSeries.Style.PointShape = new TKPredefinedShape (TKShapeType.Rhombus, new SizeF (shapeSize, shapeSize));
			chart.AddSeries (lineSeries);
			chart.YAxis.Style.LabelStyle.TextHidden = true;

			chart.ReloadData ();
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			UITouch touch = (UITouch)touches.AnyObject;
			PointF touchPoint = touch.LocationInView(chart.PlotView);
			TKChartSelectionInfo hitTestInfo = chart.HitTestForPoint(touchPoint);
			if (hitTestInfo != null) {
				selectedPoint = chart.VisualPointForSeries (hitTestInfo.Series, hitTestInfo.DataPointIndex);
				originalLocation = touchPoint;
				if (selectedPoint != null) {
					selectedPoint.Animator = null;
					originalPosition = selectedPoint.Center;
				}
			}
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);

			if (selectedPoint == null) {
				return;
			}

			UITouch touch = (UITouch)touches.AnyObject;
			PointF touchPoint = touch.LocationInView(chart.PlotView);
			PointF delta = new PointF(originalLocation.X - touchPoint.X, originalLocation.Y - touchPoint.Y);

			selectedPoint.Center = new PointF(originalPosition.X, originalPosition.Y - delta.Y);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			if (selectedPoint == null) {
				return;
			}

			UITouch touch = (UITouch)touches.AnyObject;
			PointF touchPoint = touch.LocationInView(chart.PlotView);
			PointF delta = new PointF(originalLocation.X, originalLocation.Y - touchPoint.Y);

			UISnapBehavior snap = new UISnapBehavior(selectedPoint, originalPosition);
			snap.Damping = 0.2f;

			UIPushBehavior push = new UIPushBehavior(new IUIDynamicItem[] { selectedPoint }, UIPushBehaviorMode.Instantaneous);
			push.PushDirection = new CGVector(0.0f, delta.Y > 0 ? -1.0f : -1.0f);
			push.Magnitude = 0.001f;

			UIDynamicAnimator animator = new UIDynamicAnimator();
			animator.AddBehavior(snap);
			animator.AddBehavior(push);

			selectedPoint.Animator = animator;
		}
	}
}

