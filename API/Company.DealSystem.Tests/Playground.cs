using System;
using Xunit;

namespace Company.DealSystem.Tests
{
    public class MyBaseClass
    {
        public string CalledMethod()
        {
            return "baseclass return";
        }
        public virtual string M1()
        {
            return CalledMethod();
        }
    }

    public class MyChildClass : MyBaseClass
    {
        public new string CalledMethod()
        {
            return "childclass return";
        }
        public override string M1()
        {
            return CalledMethod();
        }
    }

    public class Playground
    {
        [Fact]
        public void Test1()
        {
            var child = new MyChildClass();

            Assert.Equal("childclass return", child.M1());

        }
    }
}
