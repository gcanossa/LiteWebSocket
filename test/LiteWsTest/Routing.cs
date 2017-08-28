using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiteWebSocket.Routing;
using LiteWebSocket.Routing.Impl;
using LiteWebSocket.Models;

namespace LWSTest
{
    [TestClass]
    public class Routing
    {
        #region nested types

        public class Root_Level
        {
            //[MessageType("prova-xy", "Root", "sub_Level")]
            [MessageType("root:sub-level:prova-xy")]
            public class TestMessage : Message
            {

            }
            [MessageTypePrefix("root:sub-level")]
            public class Root_Level2
            {
                //[MessageType("prova-xy", "Root", "sub_Level")]
                [MessageType("root:sub-level:prova-xy")]
                public class TestMessage : Message
                {

                }
            }
            [MessageTypePrefix("root:sub-level")]
            public class Root_Level3
            {
                public class TestMessage : Message
                {

                }
            }
        }

        #endregion

        [TestMethod]
        public void MessageAttributeResolver()
        {
            IMessageNameResolutionConvention conv = new DefaultMessageNameResolutionConvention();

            RouteData data = conv.GetRouteData(new Root_Level.TestMessage());

            Assert.AreEqual("root:sub-level:prova-xy", data.MessageType);
            Assert.AreEqual("prova-xy", data.Name);
            CollectionAssert.AreEquivalent(new string[] { "root", "sub-level" }, data.Scopes);

            data = conv.GetRouteData(new Root_Level.Root_Level2.TestMessage());

            Assert.AreEqual("root:sub-level:prova-xy", data.MessageType);
            Assert.AreEqual("prova-xy", data.Name);
            CollectionAssert.AreEquivalent(new string[] { "root", "sub-level" }, data.Scopes);

            data = conv.GetRouteData(new Root_Level.Root_Level3.TestMessage());

            Assert.AreEqual("root:sub-level:test", data.MessageType);
            Assert.AreEqual("test", data.Name);
            CollectionAssert.AreEquivalent(new string[] { "root", "sub-level" }, data.Scopes);
        }
    }
}
