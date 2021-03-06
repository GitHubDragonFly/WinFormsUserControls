﻿//* Design by Godra
//*
//* 23-AUG-16  v1.0
//* 
//* This is a simple rotating image control.
//*
//* Its purpose is to demonstrate the VB Net code for the following:
//*  - Hardcoding an image and using it as a built-in image
//*  - Providing a choice of selecting specific anchor point for rotation
//*  - Rotating an image around the anchor point (arbitrary angle of rotation, -360 to 360 degree range)
//*  - Horizontal/Vertical flipping
//*  - Free or fixed image aspect ratio
//*

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

public class RotatingImage2 : Control
{
	//* Declare internal timer to be used for endless rotation of the image
	private readonly Timer Tmr;

	//* Declare built-in/hardcoded image (96x96 "Load Image" picture with transparent background)
	private readonly System.IO.MemoryStream PictureStream = new System.IO.MemoryStream(new byte[] {
		0x89, 0x50, 0x4e, 0x47, 0xd, 0xa, 0x1a, 0xa, 0x0, 0x0, 0x0, 0xd, 0x49, 0x48, 0x44, 0x52,
		0x0, 0x0, 0x0, 0x60, 0x0, 0x0, 0x0, 0x60, 0x8, 0x6, 0x0, 0x0, 0x0, 0xe2, 0x98, 0x77,
		0x38, 0x0, 0x0, 0x0, 0x9, 0x70, 0x48, 0x59, 0x73, 0x0, 0x0, 0xe, 0xc3, 0x0, 0x0, 0xe,
		0xc3, 0x1, 0xc7, 0x6f, 0xa8, 0x64, 0x0, 0x0, 0x3, 0x2b, 0x49, 0x44, 0x41, 0x54, 0x78, 0x9c,
		0xed, 0x9b, 0xd1, 0x95, 0xab, 0x30, 0xc, 0x5, 0xa9, 0x8b, 0x82, 0xa8, 0x87, 0x6a, 0x68, 0x86,
		0x62, 0xfc, 0x12, 0x20, 0x9, 0x1, 0x3b, 0x2b, 0x81, 0x75, 0x26, 0x79, 0x97, 0x8f, 0xf9, 0xd9,
		0x13, 0x9b, 0xc1, 0xb2, 0x6c, 0x6f, 0xac, 0x34, 0x29, 0xa5, 0xe6, 0x82, 0x3, 0x17, 0x50, 0x7,
		0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17,
		0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50,
		0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7,
		0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17,
		0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0x50, 0x7, 0x17, 0xf8, 0x1d, 0x86, 0xd4, 0xdd, 0x86, 0xab,
		0x1b, 0x52, 0xfa, 0x81, 0x0, 0x8c, 0xa9, 0x6f, 0x9b, 0xd4, 0xcc, 0xb6, 0x5f, 0x30, 0x78, 0x15,
		0x18, 0xba, 0xdb, 0x68, 0xb5, 0xa9, 0x1f, 0x7f, 0x21, 0x0, 0x63, 0x9f, 0xda, 0x5b, 0xd7, 0xed,
		0x6c, 0xcb, 0xf, 0x5e, 0x5, 0xc6, 0xbe, 0xbd, 0x8d, 0x56, 0x97, 0xe6, 0x4, 0xf8, 0xf6, 0x0,
		0x4, 0xcd, 0x16, 0x8e, 0xb8, 0x8c, 0xe, 0x11, 0x1e, 0xba, 0xe6, 0xdc, 0x6c, 0x99, 0x2, 0xd8,
		0xac, 0x70, 0xf4, 0xb5, 0x6b, 0xdb, 0xf8, 0x27, 0xc3, 0xb6, 0x8f, 0xb6, 0xd, 0xcb, 0xe8, 0xaf,
		0x9b, 0x2d, 0xfb, 0xe0, 0x2d, 0xfd, 0xb5, 0x7d, 0x1a, 0xd, 0x41, 0xb8, 0x2f, 0x15, 0xdb, 0x81,
		0xf2, 0x4c, 0x88, 0xec, 0x67, 0x97, 0x80, 0xd4, 0xde, 0x80, 0x83, 0x2, 0x30, 0x9f, 0x16, 0xe,
		0xcd, 0x96, 0xd2, 0xd2, 0x75, 0x76, 0x49, 0x9b, 0xda, 0x1b, 0x2, 0x50, 0x78, 0x4e, 0xd4, 0xfa,
		0x1f, 0x13, 0x80, 0xc3, 0xb3, 0xe5, 0x43, 0xe6, 0x2c, 0x9b, 0xba, 0xb5, 0xcf, 0x79, 0x16, 0x6f,
		0xf8, 0x33, 0x83, 0xca, 0xcf, 0x9f, 0xfa, 0x33, 0x66, 0x20, 0x1e, 0x80, 0xc3, 0xb3, 0xe5, 0xd3,
		0x20, 0x1b, 0x33, 0xe0, 0x39, 0xf0, 0x6f, 0x83, 0x68, 0x5c, 0x12, 0x8b, 0xcf, 0x9f, 0xdb, 0x47,
		0x9d, 0xe8, 0x2a, 0x77, 0xe8, 0x5b, 0xaf, 0xad, 0x83, 0x3c, 0x5, 0xf5, 0xaf, 0x3e, 0x8b, 0xed,
		0x8d, 0x4b, 0x62, 0x29, 0x73, 0x9d, 0xd9, 0x7, 0x7, 0xe0, 0xc4, 0xfa, 0x5f, 0x7c, 0x51, 0x5b,
		0x9f, 0xc5, 0x8d, 0x76, 0xea, 0xd7, 0xb0, 0x7f, 0x94, 0x2, 0x10, 0x7c, 0xa4, 0xae, 0xdb, 0xe1,
		0xa9, 0xd9, 0x92, 0xcb, 0x9e, 0x79, 0xf0, 0x2d, 0x27, 0xaa, 0x79, 0xe9, 0x7b, 0x1f, 0xa8, 0xf9,
		0x6f, 0xd6, 0x13, 0xd0, 0xfe, 0x59, 0xcf, 0xf6, 0x41, 0xeb, 0x7f, 0xf5, 0x0, 0xbc, 0x5e, 0x38,
		0x87, 0x65, 0x16, 0x2d, 0x41, 0x58, 0xb5, 0xb3, 0x7, 0x73, 0xdf, 0xf6, 0x9e, 0x35, 0xae, 0xd,
		0x74, 0x7b, 0xfe, 0xef, 0xfa, 0xf0, 0xaf, 0x54, 0x42, 0x3a, 0xbd, 0xb8, 0x2, 0xf0, 0x33, 0xe0,
		0x2, 0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2,
		0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2, 0xea, 0xe0, 0x2, 0xea,
		0xe0, 0x2, 0xea, 0x38, 0x3e, 0xbc, 0xff, 0xba, 0x37, 0xf2, 0x7b, 0x72, 0x15, 0xdc, 0xd, 0x22,
		0x2b, 0x4, 0x14, 0x71, 0x36, 0x38, 0x71, 0xe7, 0x7b, 0x51, 0x23, 0x0, 0xfe, 0x3b, 0xdf, 0xc7,
		0x8d, 0xd4, 0xf0, 0x76, 0x5b, 0xb6, 0x64, 0xd0, 0x72, 0x85, 0xb9, 0xbe, 0xc1, 0x32, 0xf5, 0x1b,
		0x51, 0xfd, 0xe6, 0x75, 0x78, 0x5c, 0x61, 0x1e, 0xba, 0xbd, 0x3b, 0x1a, 0x0, 0x77, 0xcd, 0xcf,
		0x6b, 0xdf, 0x78, 0xbd, 0xd8, 0x4a, 0x7c, 0x95, 0x49, 0x9e, 0xea, 0xb5, 0x3a, 0xd5, 0x6f, 0x9b,
		0x80, 0x39, 0xde, 0xed, 0x71, 0xf5, 0xba, 0xfe, 0x6c, 0xee, 0x4e, 0xba, 0x7a, 0x0, 0xdc, 0xeb,
		0x7f, 0xf6, 0x92, 0x3e, 0x5f, 0xa7, 0x73, 0x7a, 0x6f, 0x39, 0x59, 0xfd, 0xe6, 0x6b, 0x7f, 0xbc,
		0x7a, 0xe3, 0x54, 0x0, 0xdc, 0x15, 0x62, 0xd9, 0x97, 0xcd, 0xff, 0xd0, 0xc1, 0xdb, 0x37, 0x53,
		0xfd, 0xf6, 0x69, 0xf, 0xc, 0xf, 0x80, 0xbd, 0x44, 0xe4, 0x41, 0xb6, 0xa0, 0x2a, 0x1b, 0x14,
		0x7b, 0x41, 0x6f, 0x64, 0xf5, 0x9b, 0xb5, 0x7d, 0xb9, 0xf2, 0xc3, 0xbf, 0xf, 0xd8, 0x3, 0xe0,
		0xae, 0xf9, 0xf1, 0x2c, 0x35, 0xc6, 0x9f, 0xff, 0x44, 0x55, 0xbf, 0x59, 0x9f, 0x1f, 0x50, 0x25,
		0x67, 0xfe, 0xa0, 0x7f, 0x93, 0xc9, 0xf, 0xca, 0x34, 0x83, 0xb7, 0x33, 0xcd, 0x55, 0xfb, 0x9,
		0x56, 0xbf, 0x5, 0xfc, 0xf2, 0xc7, 0xfc, 0xc1, 0x9a, 0xeb, 0xff, 0xf6, 0x5, 0xac, 0x1b, 0x70,
		0x44, 0xf5, 0xdb, 0xf3, 0x38, 0x6a, 0x7a, 0xb7, 0xc7, 0xa9, 0x2e, 0x5f, 0xc2, 0x7e, 0x24, 0x30,
		0xc6, 0x99, 0xef, 0x3f, 0x6f, 0x67, 0x7, 0xb5, 0x90, 0xc2, 0xd9, 0xac, 0xf8, 0x38, 0x0, 0x6c,
		0xf5, 0xdb, 0xb1, 0x3, 0xc0, 0xc9, 0xc, 0xf8, 0x6f, 0x9, 0xae, 0x7e, 0xbe, 0x2, 0xb0, 0xe2,
		0x9e, 0x95, 0x96, 0xff, 0x49, 0xae, 0x0, 0x84, 0x90, 0xf9, 0x36, 0x17, 0x9c, 0xf9, 0x82, 0x1,
		0xf8, 0x4e, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1,
		0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75,
		0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70,
		0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1,
		0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75, 0x70, 0x1, 0x75,
		0x70, 0x1, 0x75, 0xfe, 0x1, 0x31, 0xbb, 0x32, 0x3a, 0xec, 0x5c, 0x4c, 0x60, 0x0, 0x0, 0x0,
		0x0, 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60, 0x82 });

	#region "Constructor"

	public RotatingImage2()
	{
		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.ContainerControl | ControlStyles.SupportsTransparentBackColor, true);
		base.DoubleBuffered = true;
		DoubleBuffered = true;
		BackColor = Color.Gainsboro;
		Size = new Size(96, 96);
		Resize += RotatingImage_Resize;
		DoubleClick += RotatingImage_DoubleClick;

		Tmr = new Timer { Enabled = false };
		Tmr.Tick += Tmr_Tick;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
			{
				Tmr.Tick -= Tmr_Tick;

				if (PictureStream != null)
					PictureStream.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	#endregion

	#region "Properties"

	private float ratio = 1f;
	private Image img = null;
	[Browsable(true), Category("Properties"), Description("Image to show and rotate. If none is selected then the built-in image will be used."), DefaultValue("")]
	public Image RI_Image
	{
		get { return img; }
		set
		{
			if (!object.ReferenceEquals(img, value))
			{
				img = value;
				if (img != null)
				{
					BackColor = Color.Transparent;
					//Custom loaded image
					ratio = img.Width / img.Height;
					Size = new Size(img.Width, img.Height);
				}
				else
				{
					BackColor = Color.Gainsboro;
					//Built-in image
					ratio = 1f;
					Size = new Size(96, 96);
				}
				Invalidate();
			}
		}
	}

	private bool m_fixedAspect;
	[Browsable(true), Category("Properties"), Description("Preserve image aspect ratio."), DefaultValue(false)]
	public bool RI_FixedAspectRatio
	{
		get { return m_fixedAspect; }
		set
		{
			if (m_fixedAspect != value)
			{
				m_fixedAspect = value;
				RotatingImage_Resize(this, System.EventArgs.Empty);
				Invalidate();
			}
		}
	}

	public enum Direction
	{
		Clockwise = 0,
		Counterclockwise = 1
	}

	private Direction m_direction = Direction.Clockwise;
	[Browsable(true), Category("Properties"), Description("Direction of perpetual rotation (clockwise or counterclockwise)."), DefaultValue(Direction.Clockwise)]
	public Direction RI_PerpetualRotationDirection
	{
		get { return m_direction; }
		set
		{
			if (m_direction != value)
			{
				m_direction = value;
				Invalidate();
			}
		}
	}

	private float m_Angle;
	[Browsable(true), Category("Properties"), Description("Angle of rotation (valid values -360 to 360)."), DefaultValue(0f)]
	public float RI_RotationAngle
	{
		get { return m_Angle; }
		set
		{
			if (value < -360 || value > 360)
				return;
			if (m_Angle != value)
			{
				m_Angle = value;
				Invalidate();
			}
		}
	}

	public enum AnchorPoint
	{
		TopLeft = 0,
		TopCenter = 1,
		TopRight = 2,
		MiddleLeft = 3,
		MiddleCenter = 4,
		MiddleRight = 5,
		BottomLeft = 6,
		BottomCenter = 7,
		BottomRight = 8
	}

	private AnchorPoint m_Anchor = AnchorPoint.MiddleCenter;
	[Browsable(true), Category("Properties"), Description("Anchor point of image rotation."), DefaultValue(AnchorPoint.MiddleCenter)]
	public AnchorPoint RI_AnchorPointOfRotation
	{
		get { return m_Anchor; }
		set
		{
			if (m_Anchor != value)
			{
				m_Anchor = value;
				Invalidate();
			}
		}
	}

	private bool m_flipV;
	[Browsable(true), Category("Properties"), Description("Flip control vertically."), DefaultValue(false)]
	public bool RI_FlipV
	{
		get { return m_flipV; }
		set
		{
			if (m_flipV != value)
			{
				m_flipV = value;
				Invalidate();
			}
		}
	}

	private bool m_flipH;
	[Browsable(true), Category("Properties"), Description("Flip control horizontally."), DefaultValue(false)]
	public bool RI_FlipH
	{
		get { return m_flipH; }
		set
		{
			if (m_flipH != value)
			{
				m_flipH = value;
				Invalidate();
			}
		}
	}

	private bool m_Value;
	[Browsable(true), Category("Properties"), Description("Enable endless rotation."), DefaultValue(false)]
	public bool Value
	{
		get { return m_Value; }
		set
		{
			if (m_Value != value)
			{
				m_Value = value;

				if (m_Value)
				{
					if (!Tmr.Enabled)
						Tmr.Enabled = true;
				}
				else
					Tmr.Enabled = false;

				Invalidate();
			}
		}
	}

	[Browsable(true), Category("Properties"), Description("Timer interval for endless rotation (valid values 5-10000)."), DefaultValue(100)]
	public int RI_PerpetualTimerInterval
	{
		get { return Tmr.Interval; }
		set
		{
			if (value < 5 || value > 10000)
			{
				MessageBox.Show("Invalid value!");
				return;
			}
			if (Tmr.Interval != value)
			{
				Tmr.Interval = value;
				Invalidate();
			}
		}
	}

	#endregion

	#region "Protected Metods"

	protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
		base.OnPaint(e);

		//* Create a new bitmap to hold the image and allow for its manipulation before it is displayed
		Bitmap backImage;

		if (img != null)
			backImage = new Bitmap(img);
		else
			backImage = new Bitmap(PictureStream);

        //* Rotate the image by using the arbitrary angle set by user.
        //* Move the origin to the selected center of rotation:
        switch (m_Anchor)
        {
            case AnchorPoint.TopCenter:
				e.Graphics.TranslateTransform(ClientRectangle.Width / 2f, 0f);
				break;
            case AnchorPoint.TopRight:
				e.Graphics.TranslateTransform(ClientRectangle.Width, 0f);
				break;
            case AnchorPoint.MiddleLeft:
				e.Graphics.TranslateTransform(0f, ClientRectangle.Height / 2f);
				break;
            case AnchorPoint.MiddleCenter:
				e.Graphics.TranslateTransform(ClientRectangle.Width / 2f, ClientRectangle.Height / 2f);
				break;
            case AnchorPoint.MiddleRight:
				e.Graphics.TranslateTransform(ClientRectangle.Width, ClientRectangle.Height / 2f);
				break;
            case AnchorPoint.BottomLeft:
				e.Graphics.TranslateTransform(0f, ClientRectangle.Height);
				break;
            case AnchorPoint.BottomCenter:
				e.Graphics.TranslateTransform(ClientRectangle.Width / 2f, ClientRectangle.Height);
				break;
            case AnchorPoint.BottomRight:
				e.Graphics.TranslateTransform(ClientRectangle.Width, ClientRectangle.Height);
				break;
        }

		//* Rotate the image:
		e.Graphics.RotateTransform(-m_Angle);

		//* Move the origin back to to the upper-left corner of the control:
		switch (m_Anchor)
		{
			case AnchorPoint.TopCenter:
				e.Graphics.TranslateTransform(-ClientRectangle.Width / 2f, 0f);
				break;
			case AnchorPoint.TopRight:
				e.Graphics.TranslateTransform(-ClientRectangle.Width, 0f);
				break;
			case AnchorPoint.MiddleLeft:
				e.Graphics.TranslateTransform(0f, -ClientRectangle.Height / 2f);
				break;
			case AnchorPoint.MiddleCenter:
				e.Graphics.TranslateTransform(-ClientRectangle.Width / 2f, -ClientRectangle.Height / 2f);
				break;
			case AnchorPoint.MiddleRight:
				e.Graphics.TranslateTransform(-ClientRectangle.Width, -ClientRectangle.Height / 2f);
				break;
			case AnchorPoint.BottomLeft:
				e.Graphics.TranslateTransform(0f, -ClientRectangle.Height);
				break;
			case AnchorPoint.BottomCenter:
				e.Graphics.TranslateTransform(-ClientRectangle.Width / 2f, -ClientRectangle.Height);
				break;
			case AnchorPoint.BottomRight:
				e.Graphics.TranslateTransform(-ClientRectangle.Width, -ClientRectangle.Height);
				break;
		}


		//* Pass the current image to the FlipHV sub to flip it (if H or V flip is enabled) and display it
		FlipHV(e.Graphics, backImage, m_flipV, m_flipH);

		backImage.Dispose();

		//* Reset the transformation
		e.Graphics.ResetTransform();
	}

	#endregion

	#region "Private Methods"

	private void RotatingImage_DoubleClick(object sender, EventArgs e)
	{
		Value = !Value;
	}

	private void RotatingImage_Resize(object sender, EventArgs e)
	{
		if (m_fixedAspect)
			Width = (int)(ratio * Height);
	}

	private void Tmr_Tick(object sender, EventArgs e)
	{
		if (m_Angle == 359 || m_Angle == -359)
			m_Angle = 0;
		else
		{
			if (m_direction == Direction.Clockwise)
				m_Angle -= 1;
			else
				m_Angle += 1;
		}

		Invalidate();
	}

	//* Reference: https://msdn.microsoft.com/en-us/library/3b575a03(v=vs.110).aspx?cs-save-lang=1&cs-lang=vb#code-snippet-1
	private void FlipHV(Graphics g, Bitmap img, bool flipV, bool flipH)
	{
		//* Original image points:   Upper-Left (0, 0); Upper-Right (Width, 0); Lower-Left (0, Height)

		//* Use points() array to store destination points for the above mentioned points of the original image

		//* No flipping - Destination Points are the same as original
		Point[] points = { new Point(0, 0), new Point(Width, 0), new Point(0, Height) };

		//* Flip image horizontally - Destination Points: (Width, 0); (0, 0); (Width, Height)
		if (flipH)
			points = new Point[] { new Point(Width, 0),	new Point(0, 0), new Point(Width, Height) };

		//* Flip image vertically
		if (flipV)
		{
			//* Account for horizontal flip
			if (flipH) //* Destination Points: (Width, Height); (0, Height); (Width, 0)
				points = new Point[] { new Point(Width, Height), new Point(0, Height), new Point(Width, 0) };
			else //* Destination Points: (0, Height); (Width, Height); (0, 0)
				points = new Point[] { new Point(0, Height), new Point(Width, Height), new Point(0, 0) };
		}

		//* Draw image using the resulting points() array
		g.DrawImage(img, points);
	}

	#endregion

}
