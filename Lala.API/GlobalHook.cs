using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Lala.API
{
    public class GlobalHook
    {
        public class KBHookEventArgs : EventArgs
        { 
            public int HookCode;
            public IntPtr wParam;
            public KBDLLHookStruct lParam;
        }
        
        //Structure returned by a WH_KEYBOARD_LL hook
        public struct KBDLLHookStruct
        { 
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        
        //Hook Types
        //more could be added
        public enum HookType : int
        { 
            WH_KEYBOARD_LL = 13,
        }
        /*Parts of this class stolen from MSDN and modified*/
        public class GlobalKBHook 
        {
            //Callback for the hook
            public delegate int KBHookProc(int code, IntPtr wParam, ref KBDLLHookStruct lParam);
            protected KBHookProc kbhook = null; 
            protected IntPtr hhook = IntPtr.Zero; 
            public delegate void KBHookEventHandler(object sender, KBHookEventArgs e);
            public event KBHookEventHandler KBHookInvoked;
            protected void OnKBHookInvoked(KBHookEventArgs e) 
            {
                if(KBHookInvoked != null)   KBHookInvoked(this, e);
            } 
            public GlobalKBHook()
            { 
                kbhook = new KBHookProc(this.CoreKBHook); 
            } 
            public int CoreKBHook(int code, IntPtr wParam, ref KBDLLHookStruct lParam)
            { 
                if (code < 0) 
                    return CallNextHookEx(hhook, code, wParam, lParam);
                KBHookEventArgs e = new KBHookEventArgs();
                e.HookCode = code;
                e.wParam = wParam;
                e.lParam = lParam;
                OnKBHookInvoked(e);           
                // Yield to the next hook in the chain   
               return CallNextHookEx(hhook, code, wParam, lParam); 
            }
            public void Install() 
            {  
                int hInstance = LoadLibrary("User32");
                hhook = SetWindowsHookEx( 
                    HookType.WH_KEYBOARD_LL,
                    kbhook, 
                    (IntPtr)hInstance, //IntPtr.Zero for local hooks 
                    0); //zero = global hook, otherwise use local thread ID
            }
            public void Uninstall() 
            {
                UnhookWindowsHookEx(hhook);
            } //win32 api function for creating hooks 
            [DllImport("user32.dll")] 
            protected static extern IntPtr SetWindowsHookEx(HookType code, GlobalKBHook.KBHookProc func,  IntPtr hInstance,  int threadID);  
            
            //win32 api function for unhooking 
            [DllImport("user32.dll")]  
            protected static extern int UnhookWindowsHookEx(IntPtr hhook);  //win32 api function for continuing the hook chain 
            [DllImport("user32.dll")]
            protected static extern int CallNextHookEx(IntPtr hhook,   int code,    IntPtr wParam,   KBDLLHookStruct lParam); 
            //Used to find HWND to user32.dll 
            [DllImport("kernel32")]
            public extern static int LoadLibrary(string lpLibFileName);}
    }
}
