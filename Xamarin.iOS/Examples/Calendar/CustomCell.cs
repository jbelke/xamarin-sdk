﻿using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TelerikUI;
using System.Drawing;

namespace Examples
{
	public class CustomCell : TKCalendarDayCell
	{
		static UIImage gDayImage;
		static UIImage gSelectedDayImage;
		static UIImage gCurrentDayImage;

		public UIImage CurrentImage {
			get;
			set;
		}

		public CustomCell (RectangleF frame) : base(frame)
		{
		}

		public CustomCell ()
		{
			this.Initialize ();
		}

		public void Initialize ()
		{
			gDayImage = new UIImage ("calendar_cell.png");
			gDayImage = gDayImage.CreateResizableImage (new UIEdgeInsets (4, 4, 4, 4), UIImageResizingMode.Stretch);

			gCurrentDayImage = new UIImage ("calendar_current_cell.png");
			gCurrentDayImage = gCurrentDayImage.CreateResizableImage (new UIEdgeInsets (4, 4, 4, 4), UIImageResizingMode.Stretch);

			gSelectedDayImage = new UIImage ("calendar_selected_cell.png");
			gSelectedDayImage = gSelectedDayImage.CreateResizableImage (new UIEdgeInsets (4, 4, 4, 4), UIImageResizingMode.Stretch);
		}

		public override void UpdateVisuals ()
		{
			base.UpdateVisuals ();
			this.Style.BackgroundColor = UIColor.Clear;
			this.Style.Shape = null;
			this.Style.TopBorderColor = null;
			this.Style.BottomBorderColor = null;

			if ((this.State & TKCalendarDayState.Selected) != 0) {
				this.CurrentImage = gSelectedDayImage;
			} else if ((this.State & TKCalendarDayState.Today) != 0) {
				this.CurrentImage = gCurrentDayImage;
				this.Label.TextColor = UIColor.White;
			} else {
				this.CurrentImage = gDayImage;
			}
		}

		public override void Draw (RectangleF rect)
		{
			this.CurrentImage.Draw (rect);
			base.Draw (rect);
		}
	}
}

