using System;
using System.Threading.Tasks;
using Xamarians.ImageCropper.Controls;

namespace Xamarians.ImageCropper
{
    public interface IImageCropper
    {
     Task<string> CropImage(InputImage inputImage);
    }   
}
