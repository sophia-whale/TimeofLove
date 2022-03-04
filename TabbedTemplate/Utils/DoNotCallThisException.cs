using System;
using System.Collections.Generic;
using System.Text;

namespace TabbedTemplate.Utils
{
    public class DoNotCallThisException : Exception
    {
        public DoNotCallThisException() : base("不应该调用此项目")
        {

        }
    }
}
