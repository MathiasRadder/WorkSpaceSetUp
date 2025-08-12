using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestFormsApp.Scripts;
using WorkSpaceSetUp.Scripts.ErrorHandling;
using WorkSpaceSetUp.Scripts.Model;
using WorkSpaceSetUp.Scripts.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace WorkSpaceSetUp.Scripts
{
    [TestClass]
    public sealed class TestInitInteractionForm
    {

        #region Variables
        private static TestHelper _testHelper = null;
        #endregion


        #region Initialize 
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _testHelper = TestHelper.InstanceTestHelper();
        }

        #endregion

        #region TestMethods

        [TestMethod]
        [DataRow(default, "Init_ThrowTest1")]
        [DataRow(null, "Init_ThrowTest12")]
        public void AsyncInitialize_Throw_Test(object[] array, string filename)
        {
            InteractionFormHandler interactionFormHandler = new();
            interactionFormHandler.InitialiseTestingData(default, default, filename, _testHelper.FolderPath);
            ListBox.ObjectCollection? objCollection = _testHelper.CreateListBoxObjCollection(array);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => interactionFormHandler.AsyncInitialize(objCollection));
        }

        [TestMethod]
        [DataRow(new string[2] { "string", "#$%" }, new string[2] { "string", "#$%" }, "Init_DataTest1")]
        [DataRow(new string[2] { "5", "#$%" }, new string[2] { "string", "#$%" }, "Init_DataTest2")]
        [DataRow(new string[1] { "" }, new string[1] { (string)null }, "Init_DataTest3")]
        public async Task AsyncInitialize_Success_Test(string[] arrayFileGroup, string[] arrayFileGroupSave, string filename)
        {
            InteractionFormHandler interactionFormHandler = new();
            interactionFormHandler.InitialiseTestingData
                (_testHelper.CreateDefaultFileGroup(arrayFileGroup),
                _testHelper.CreateDefaultFileGroup(arrayFileGroupSave),
                filename, _testHelper.FolderPath);

            ListBox.ObjectCollection? objCollection = _testHelper.CreateListBoxObjCollection(arrayFileGroup);

            Task.Run(async () =>
            {
                TResult<FileGroup[]?> tResult = await interactionFormHandler.AsyncInitialize(objCollection);
                Assert.IsTrue(tResult.IsSuccess);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [DataRow(new string[2] { "string", "#$%" }, new string[2] { "string", "#$%" }, "Init_DataFailure1")]
        [DataRow(new string[2] { "null", "#$%" }, new string[3] { "string", "#$%", null }, "Init_DataFailure2")]
        public async Task AsyncInitialize_OpenFile_Test(string[] arrayFileGroup, string[] arrayFileGroupSave, string filename)
        {
            InteractionFormHandler interactionFormHandler = new();
            interactionFormHandler.InitialiseTestingData
                (_testHelper.CreateDefaultFileGroup(arrayFileGroup),
                _testHelper.CreateDefaultFileGroup(arrayFileGroupSave),
                filename, _testHelper.FolderPath);

            StreamReader? sr = null;
            string pathWithName = Path.Combine(_testHelper.FolderPath,
             filename + _testHelper.FileType);
            if (File.Exists(pathWithName))
                sr = new StreamReader(pathWithName);
            else
                sr = new StreamReader(File.Create(pathWithName));

            ListBox.ObjectCollection? objCollection = _testHelper.CreateListBoxObjCollection(arrayFileGroup);

            Task.Run(async () =>
            {
                TResult<FileGroup[]?> tResult = await interactionFormHandler.AsyncInitialize(objCollection);
                Assert.IsTrue(tResult.IsFailure);
                sr.Dispose();
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [DataRow(new string[2] { "string", "#$%" }, new string[2] { "string", "#$%" }, "Init_DataCorrupted1")]
        [DataRow(new string[2] { "null", "#$%" }, new string[3] { "string", "#$%", null }, "Init_DataCorrupted2")]
        public async Task AsyncInitialize_Corrupt_Test(string[] arrayFileGroup, string[] arrayFileGroupSave, string filename)
        {
            InteractionFormHandler interactionFormHandler = new();
            interactionFormHandler.InitialiseTestingData
                (_testHelper.CreateDefaultFileGroup(arrayFileGroup),
                _testHelper.CreateDefaultFileGroup(arrayFileGroupSave),
                filename, _testHelper.FolderPath, true);

            ListBox.ObjectCollection? objCollection = _testHelper.CreateListBoxObjCollection(arrayFileGroup);

            Task.Run(async () =>
            {
                TResult<FileGroup[]?> tResult = await interactionFormHandler.AsyncInitialize(objCollection);
                Assert.IsTrue(tResult.IsFailure);
            }).GetAwaiter().GetResult();
        }




        #endregion
    }

}
