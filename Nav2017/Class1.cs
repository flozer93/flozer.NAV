using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics.Nav.Client;
using Microsoft.Dynamics.Nav.DotNetBridge;
using Microsoft.Dynamics.Nav.Types;
using Microsoft.Dynamics.Nav.Types.Exceptions;
using Microsoft.Dynamics.Nav.Common;
using Microsoft.Dynamics.Nav.Model;
using Microsoft.Dynamics.Nav.MetaMetaModel;

namespace Nav2017
{
    public class Class1
    {
        public Class1()
        {
            //DotNetBridge dtb = new DotNetBridgeClient();
            ClientConfiguration config = new ClientConfiguration();
                config.ServerName = "localhost";
                config.Port = 7046;
                config.CompanyName = "CRONUS International Ltd.";
                config.ClientName = "1000";
                config.UserName = "admin";
                config.Password = "admin";
        }

        public void Test()
        {
            ModelType modelType = new ModelType();
            modelType.
            using (DotNetBridgeClient dtb = new DotNetBridgeClient())
            {
                ClientConfiguration config = new ClientConfiguration();
                config.ServerName = "localhost";
                config.Port = 7046;
                config.CompanyName = "CRONUS International Ltd.";
                config.ClientName = "1000";
                config.UserName = "admin";
                config.Password = "admin";
                dtb.Connect(config);
                var result = dtb.Invoke("HelloWorld", new object[] { "Nav2017" });
                Console.WriteLine(result);
            }
        }
}
