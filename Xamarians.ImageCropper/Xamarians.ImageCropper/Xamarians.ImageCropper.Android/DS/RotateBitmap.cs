using Android.Graphics;


namespace Xamarians.ImageCropper.Droid.DS
{
    public class RotateBitmap
    {
        public RotateBitmap(Bitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public RotateBitmap(Bitmap bitmap, int rotation)
        {
            Bitmap = bitmap;
            Rotation = rotation % 360;
        }

        public int Rotation
        {
            get;
            set;
        }

        public Bitmap Bitmap
        {
            get;
            set;
        }

        public Matrix GetRotateMatrix()
        {
            // By default this is an identity matrix.
            Matrix matrix = new Matrix();

            if (Rotation != 0)
            {
                // We want to do the rotation at origin, but since the bounding
                // rectangle will be changed after rotation, so the delta values
                // are based on old & new width/height respectively.
                int cx = Bitmap.Width / 2;
                int cy = Bitmap.Height / 2;
                matrix.PreTranslate(-cx, -cy);
                matrix.PostRotate(Rotation);
                matrix.PostTranslate(Width / 2, Height / 2);
            }

            return matrix;
        }

        public bool IsOrientationChanged
        {
            get
            {
                return (Rotation / 90) % 2 != 0;
            }
        }

        public int Height
        {
            get
            {
                if (IsOrientationChanged)
                {
                    return Bitmap.Width;
                }
                else
                {
                    return Bitmap.Height;
                }
            }
        }

        public int Width
        {
            get
            {
                if (IsOrientationChanged)
                {
                    return Bitmap.Height;
                }
                else
                {
                    return Bitmap.Width;
                }
            }
        }
        public static Bitmap rotateImage(Bitmap b, int degrees)
        {
            if (degrees != 0 && b != null)
            {
                Matrix m = new Matrix();
                m.SetRotate(degrees,
                        (float)b.Width / 2, (float)b.Height / 2);
                try
                {
                    Bitmap b2 = Bitmap.CreateBitmap(
                            b, 0, 0, b.Width, b.Height, m, true);
                    if (b != b2)
                    {
                        b.Recycle();
                        b = b2;
                    }
                }
                catch (Java.Lang.OutOfMemoryError)
                {
                    // We have no memory to rotate. Return the original bitmap.
                }
            }

            return b;
        }
    }
}