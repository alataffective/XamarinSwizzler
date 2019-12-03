using AppKit;
using Foundation;

namespace XamarinSwizzler
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        static AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            new Swizzler().AttemptSwizzle();
        }
    }
}
