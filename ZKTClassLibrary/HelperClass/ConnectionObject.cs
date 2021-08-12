using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;

namespace ZKTClassLibrary.HelperClass
{

    public static class ConnectionObject
    {

        private static CZKEM _ZKObject;
        public static CZKEM ZKObject
        {
            get
            {
                if (_ZKObject == null)
                        _ZKObject = new CZKEM();
                return _ZKObject;
            }
            
        }
        
    }
}
