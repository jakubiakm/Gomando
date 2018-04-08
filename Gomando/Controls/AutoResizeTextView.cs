using System;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Java.Lang;
using Math = System.Math;
using String = System.String;

namespace Gomando.Controls
{
    /// 
    /// TextView that automatically resizes it's content to fit the layout dimensions
    /// 
    public class AutoResizeTextView : TextView
    {

        // Minimum text size for this text view
        public static float MIN_TEXT_SIZE = 2;

        // Interface for resize notifications
        public interface IOnTextResizeListener
        {
            void OnTextResize(TextView textView, float oldSize, float newSize);
        }

        #region Fields

        // Our ellipse string
        private const String mEllipsis = "...";

        // Registered resize listener
        private IOnTextResizeListener mTextResizeListener;

        // Flag for text and/or size changes to force a resize
        private bool mNeedsResize = false;

        // Text size that is set from code. This acts as a starting point for resizing
        private float mTextSize;

        // Temporary upper bounds on the starting text size
        private float mMaxTextSize = 0;

        // Lower bounds for text size
        private float mMinTextSize = MIN_TEXT_SIZE;

        // Text view line spacing multiplier
        private float mSpacingMult = 1.0f;

        // Text view additional line spacing
        private float mSpacingAdd = 0.0f;

        // Add ellipsis to text that overflows at the smallest text size
        private bool mAddEllipsis = true;
        #endregion

        #region Constructors
        public AutoResizeTextView(IntPtr a, JniHandleOwnership b) : base(a, b) { }

        // Default constructor override
        public AutoResizeTextView(Context context)
            : this(context, null)
        {

        }

        // Default constructor when inflating from XML file
        public AutoResizeTextView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        {

        }

        // Default constructor override
        public AutoResizeTextView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            mTextSize = TextSize;
        }
        #endregion

        #region Public Methods
        //When text changes, set the force resize flag to true and reset the text size.
        protected override void OnTextChanged(ICharSequence text, int start, int before, int after)
        {
            mNeedsResize = true;
            // Since this view may be reused, it is good to reset the text size
            ResetTextSize();
            RequestLayout();
            Invalidate();
        }

        // If the text view size changed, set the force resize flag to true
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            if (w != oldw || h != oldh)
            {
                mNeedsResize = true;
            }
        }

        //Register listener to receive resize notifications
        public void SetOnResizeListener(IOnTextResizeListener listener)
        {
            mTextResizeListener = listener;
        }


        public override void SetTextSize(ComplexUnitType unitType, float size)
        {
            base.SetTextSize(unitType, size);
            mTextSize = TextSize;
        }

        /**
         * Override the set text size to update our internal reference values
         */
        //@Override
        //public void setTextSize(int unit, float size) {
        //    super.setTextSize(unit, size);
        //    mTextSize = getTextSize();
        //}

        // Override the set line spacing to update our internal reference values
        public override void SetLineSpacing(float add, float mult)
        {
            base.SetLineSpacing(add, mult);
            mSpacingMult = mult;
            mSpacingAdd = add;
        }

        //Set the upper text size limit and invalidate the view
        public void SetMaxTextSize(float maxTextSize)
        {
            mMaxTextSize = maxTextSize;
            RequestLayout();
            Invalidate();
        }

        //Return upper text size limit
        public float GetMaxTextSize()
        {
            return mMaxTextSize;
        }

        //Set the lower text size limit and invalidate the view
        public void SetMinTextSize(float minTextSize)
        {
            mMinTextSize = minTextSize;
            RequestLayout();
            Invalidate();
        }

        //Return lower text size limit
        public float SetMinTextSize()
        {
            return mMinTextSize;
        }

        //Set flag to add ellipsis to text that overflows at the smallest text size
        public void SetAddEllipsis(bool addEllipsis)
        {
            mAddEllipsis = addEllipsis;
        }

        //Return flag to add ellipsis to text that overflows at the smallest text size
        public bool GetAddEllipsis()
        {
            return mAddEllipsis;
        }

        //Reset the text to the original size
        public void ResetTextSize()
        {
            if (mTextSize > 0)
            {
                base.SetTextSize(ComplexUnitType.Px, mTextSize);
                mMaxTextSize = mTextSize;
            }
        }

        //Resize text after measuring
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            if (changed || mNeedsResize)
            {
                int widthLimit = (right - left) - CompoundPaddingLeft - CompoundPaddingRight;
                int heightLimit = (bottom - top) - CompoundPaddingBottom - CompoundPaddingTop;
                ResizeText(widthLimit, heightLimit);
            }
            base.OnLayout(changed, left, top, right, bottom);
        }

        //Resize the text size with default width and height
        public void ResizeText()
        {
            int heightLimit = Height - PaddingBottom - PaddingTop;
            int widthLimit = Width - PaddingLeft - PaddingRight;
            ResizeText(widthLimit, heightLimit);
        }


        // Resize the text size with specified width and height
        public void ResizeText(int width, int height)
        {
            ICharSequence text = new Java.Lang.String(Text);
            // Do not resize if the view does not have dimensions or there is no text
            if (text == null || text.Length() == 0 || height <= 0 || width <= 0 || mTextSize == 0)
                return;
            if (TransformationMethod != null)
            {
                text = TransformationMethod.GetTransformationFormatted(text, this);
            }
            // Get the text view's paint object
            TextPaint textPaint = Paint;
            // Store the current text size
            float oldTextSize = textPaint.TextSize;
            // If there is a max text size set, use the lesser of that and the default text size
            float targetTextSize = mMaxTextSize > 0 ? Math.Min(mTextSize, mMaxTextSize) : mTextSize;

            // Get the required text height
            int textHeight = GetTextHeight(text, textPaint, width, targetTextSize);

            // Until we either fit within our text view or we had reached our min text size, incrementally try smaller sizes
            while (textHeight > height && targetTextSize > mMinTextSize)
            {
                targetTextSize = Math.Max(targetTextSize - 2, mMinTextSize);
                textHeight = GetTextHeight(text, textPaint, width, targetTextSize);
            }

            // If we had reached our minimum text size and still don't fit, append an ellipsis
            if (mAddEllipsis && targetTextSize == mMinTextSize && textHeight > height)
            {
                TextPaint paint = new TextPaint(textPaint);
                // Draw using a static layout
                StaticLayout layout = new StaticLayout(text, paint, width, Layout.Alignment.AlignNormal, mSpacingMult, mSpacingAdd, false);
                // Check that we have a least one line of rendered text
                if (layout.LineCount > 0)
                {
                    // Since the line at the specific vertical position would be cut off,
                    // we must trim up to the previous line
                    int lastLine = layout.GetLineForVertical(height) - 1;
                    // If the text would not even fit on a single line, clear it
                    if (lastLine < 0)
                    {
                        Text = "";
                    }
                    // Otherwise, trim to the previous line and add an ellipsis
                    else
                    {
                        int start = layout.GetLineStart(lastLine);
                        int end = layout.GetLineEnd(lastLine);
                        float lineWidth = layout.GetLineWidth(lastLine);
                        float ellipseWidth = textPaint.MeasureText(mEllipsis);

                        // Trim characters off until we have enough room to draw the ellipsis
                        while (width < lineWidth + ellipseWidth)
                        {
                            lineWidth = textPaint.MeasureText(text.SubSequence(start, --end + 1).ToString());
                        }
                        Text = (text.SubSequence(0, end) + mEllipsis);
                    }
                }
            }

            // Some devices try to auto adjust line spacing, so force default line spacing
            // and invalidate the layout as a side effect
            SetTextSize(ComplexUnitType.Px, targetTextSize);
            SetLineSpacing(mSpacingAdd, mSpacingMult);

            // Notify the listener if registered
            if (mTextResizeListener != null)
            {
                mTextResizeListener.OnTextResize(this, oldTextSize, targetTextSize);
            }

            // Reset force resize flag
            mNeedsResize = false;
        }

        #endregion

        #region Private methods


        // Set the text size of the text paint object and use a static layout to render text off screen before measuring
        private int GetTextHeight(ICharSequence source, TextPaint paint, int width, float textSize)
        {
            TextPaint paintCopy = new TextPaint(paint);
            // Update the text paint object
            paintCopy.TextSize = textSize;
            // Measure using a static layout
            StaticLayout layout = new StaticLayout(source, paintCopy, width, Layout.Alignment.AlignNormal, mSpacingMult, mSpacingAdd, true);
            return layout.Height;
        }
        #endregion
    }
}