using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Windows.Forms;
using WinFormsAppLatestNet.Scripts;
using NUnit.Framework;

namespace nUnitTest.Scripts
{
    [TestFixture]
    public class Tests
    {
        InteractionFormHandler _interactionFormHandler = new();

        [SetUp]
        public void Setup()
        {
             _interactionFormHandler = new();
        }

        [Test]
        [TestCase(null)]
        public void Test1(object[]? array)
        {

   
            ListBox _listBoxTest = new ListBox();
            ListBox.ObjectCollection? value = null;
            //if (array != null || array?.Length != 0 || array[0] != null)
            //{
            //    value = new ListBox.ObjectCollection(_listBoxTest, array);
            //}
            //_interactionFormHandler.AsyncInitialize(value);

            var a = NUnit.Framework.Assert.ThrowsAsync<ArgumentNullException>(() => _interactionFormHandler.AsyncInitialize(value));
        }
    }
}
