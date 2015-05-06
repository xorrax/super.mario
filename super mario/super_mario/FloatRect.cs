using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace super_mario
{
    public class FloatRect
    {
        float top, bottom, left, right;

        public float Top
        {
            get { return top; }
        }

        public float     Bottom
        {
            get { return bottom; }
        }

        public float Left
        {
            get { return left; }
        }

        public float Right
        {
            get { return right; }
        }

        public FloatRect(float x, float y, float width, float height)
        {
            left = x;
            right = x + width;
            top = y;
            bottom = y + height;
        }

        public bool Intersects(FloatRect f)
        {
            if (right <= f.Left || left >= f.Right || top >= f.bottom || bottom <= f.Top)
                return false;
            return true;
        }
    }
}
