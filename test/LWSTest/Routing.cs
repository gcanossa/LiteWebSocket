using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiteWebSocket.Routing;
using LiteWebSocket.Routing.Impl;
using LiteWebSocket.Models;

namespace LiteWsTest
{
    [TestClass]
    public class Routing
    {
        #region nested types

        [MessageType("prova-xy","Root","sub_Level")]
        public class TestMessage : Message
        {

        }

        #endregion

        [TestMethod]
        public void MessageAttributeResolver()
        {
            IMessageNameResolutionConvention conv = new MessageAttributeNameResolutionConvention();

            RouteData data = conv.GetRouteData(new TestMessage());

            Assert.AreEqual("root:sub-level:prova-xy", data.MessageType);
            Assert.AreEqual("prova-xy", data.MessageName);
            CollectionAssert.AreEquivalent(new string[] { "root", "sub-level" }, data.MessageScopes);
        }
    }
}
