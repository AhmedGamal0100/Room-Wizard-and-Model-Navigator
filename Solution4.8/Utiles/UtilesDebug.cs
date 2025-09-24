using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 This Debug Utiles Will contain all the methods we'll handle debug any code in the project
 */

namespace Solution4.Utiles
{
    public static class UtilesDebug
    {
        public static void ShowInDebug(this List<string> strings)
        {
            strings.ForEach(e => Debug.WriteLine(e));
        }
    }
}
