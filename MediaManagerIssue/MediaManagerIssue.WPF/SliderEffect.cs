using MediaManagerIssue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

[assembly: ResolutionGroupName("MediaManagerIssue")]
[assembly: ExportEffect(typeof(MediaManagerIssue.WPF.SliderEffect), "SliderEffect")]
namespace MediaManagerIssue.WPF
{
    public class SliderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            (Control as System.Windows.Controls.Slider).IsMoveToPointEnabled = true;
        }

        protected override void OnDetached()
        {
        }

        /*protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            // You can do effects only when certain properties change here.
        }*/
    }
}