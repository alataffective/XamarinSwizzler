using System;
using System.Runtime.InteropServices;
using AppKit;
using Foundation;
using ObjCRuntime;

namespace XamarinSwizzler
{
    [Register("Swizzler")]
    public class Swizzler : NSObject
    {
        [DllImport("/usr/lib/libobjc.dylib")] public static extern IntPtr class_getInstanceMethod(IntPtr classHandle, IntPtr Selector);
        [DllImport("/usr/lib/libobjc.dylib")] public static extern bool method_exchangeImplementations(IntPtr m1, IntPtr m2);

        public void AttemptSwizzle()
        {
            var swizzledClassPtr = Class.GetHandle("Swizzled");
            var swizzlerClassPtr = Class.GetHandle("Swizzler");
            SwizzleInstanceMethod(swizzledClassPtr, new Selector("originalMethod"), swizzlerClassPtr, new Selector("newMethod"));

            var swizzled = new Swizzled();
            swizzled.PerformSelector(new Selector("originalMethod"));
        }

        internal void SwizzleInstanceMethod(IntPtr originalClassPtr, Selector originalSelector, IntPtr newClassPtr, Selector newSelector)
        {
            var originalMethod = class_getInstanceMethod(originalClassPtr, originalSelector.Handle);
            var swizzledMethod = class_getInstanceMethod(newClassPtr, newSelector.Handle);

            method_exchangeImplementations(originalMethod, swizzledMethod);
        }

        [Export("newMethod")]
        public virtual void NewMethod()
        {
            Console.WriteLine("New method called");
        }
    }

    [Register("Swizzled")]
    public class Swizzled : NSObject
    {
        [Export("originalMethod")]
        public virtual void OriginalMethod()
        {
            Console.WriteLine("Original method called");
        }
    }
}
