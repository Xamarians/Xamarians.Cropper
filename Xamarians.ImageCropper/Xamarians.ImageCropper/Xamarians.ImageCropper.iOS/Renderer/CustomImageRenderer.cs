using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using Xamarians.ImageCropper.Controls;
using Xamarians.ImageCropper.iOS.Renderer;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]

namespace Xamarians.ImageCropper.iOS.Renderer
{
    public class CustomImageRenderer : ImageRenderer
        {
            CustomImage element;
            protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
            {
                base.OnElementChanged(e);
                element = (CustomImage)Element;
                if (Control == null)
                    return;

                var image = ScaleImageInRatio(UIImage.FromBundle(element.ImageSource));
                Control.Image = image;
              
            }

            private static UIImage ScaleImageInRatio(UIImage image)
            {
                var width = image.Size.Width;
                var height = image.Size.Height;
                var reqWidth = UIScreen.MainScreen.Bounds.Width;
                var reqHeight = UIScreen.MainScreen.Bounds.Height;
                GetRatio(width, height, ref reqWidth, ref reqHeight);


                image = image.Scale(new CGSize(reqWidth, reqHeight));
                return image;
            }

            public static void GetRatio(nfloat actualWidth, nfloat actualHeight, ref nfloat reqWidth, ref nfloat reqHeight)
            {
                if (reqWidth > reqHeight)
                {
                    if (reqWidth > actualWidth)
                        reqWidth = actualWidth;
                    reqHeight = reqWidth * actualHeight / actualWidth;
                }
                else
                {
                    if (reqHeight > actualHeight)
                        reqHeight = actualHeight;
                    reqWidth = reqHeight * actualWidth / actualHeight;
                }
            }
        }
    }
