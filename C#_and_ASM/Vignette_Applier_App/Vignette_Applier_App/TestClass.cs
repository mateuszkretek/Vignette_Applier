using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows;

namespace Vignette_Applier_App
{
    class TestClass
    {
        [DllImport("C:/Users/mateu/Desktop/Vignette_Applier_App/x64/Debug/ASMdll.dll")]
        public static extern int MyProc1(int a, int b);

    }
}
