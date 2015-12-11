using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using FubuMVC.Core;
using StructureMap;
using FubuMVC.Core.Diagnostics;

namespace JasperProject
{
    public class Global : HttpApplication
    {
        private FubuRuntime _runtime;

        protected void Application_Start(object sender, EventArgs e)
        {
            
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _runtime.Dispose();
        }
    }
}