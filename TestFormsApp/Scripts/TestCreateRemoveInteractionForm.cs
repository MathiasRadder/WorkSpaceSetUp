using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestFormsApp.Scripts;
using WorkSpaceSetUp.Scripts.Model;
using WorkSpaceSetUp.Scripts.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace WorkSpaceSetUp.Scripts
{
    [TestClass]
    public sealed class TestCreateRemoveInteractionForm
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
        [DataRow(new string[2] { "string", "#$%" }, new string[2] { "string", "#$%" }, "Create_DataTest1")]
        [DataRow(new string[2] { "string", "#$%" }, new string[2] { "Diff", "Diff2" }, "Create_DataTest2")]
        [DataRow(new string[2] { "string", "#$%" }, new string[1] { "string" }, "Create_DataTest3")]
        [DataRow(new string[1] { "string" }, new string[2] { "Diff", "Diff2" }, "Create_DataTest4")]

        public async Task CreateFileGroup_Strength_Test(string[] arrayFileGroup, string[] arrayFileGroupSave, string filename)
        {
            InteractionFormHandler interactionFormHandler = new();
            interactionFormHandler.InitialiseTestingData
                (_testHelper.CreateDefaultFileGroup(arrayFileGroup),
                _testHelper.CreateDefaultFileGroup(arrayFileGroupSave),
                filename, _testHelper.FolderPath);

            ListBox listBoxTestFileGroup = _testHelper.CreateListBox(arrayFileGroup, -1);
            System.Windows.Forms.ListView listView = new();

            Task.Run(async () =>
            {
                TResult<FileGroup[]?> tResult = await interactionFormHandler.CreateAndAddFileGroup(listBoxTestFileGroup, listView, null);
                Assert.IsTrue(tResult.IsSuccess);
            }).GetAwaiter().GetResult();
        }



        [TestMethod]
        [DataRow(new string[2] { "string", "#$%" }, new string[2] { "string", "#$%" }, "Remove_DataTest1")]
        [DataRow(new string[2] { "string", "#$%" }, new string[2] { "Diff", "Diff2" }, "Remove_DataTest2")]
        [DataRow(new string[2] { "string", "#$%" }, new string[1] { "string" }, "Remove_DataTest3")]
        [DataRow(new string[1] { "string" }, new string[2] { "Diff", "Diff2" }, "Remove_DataTest4")]
        public async Task RemoveFileGroup_Strength_Test(string[] arrayFileGroup, string[] arrayFileGroupSave, string filename)
        {
            InteractionFormHandler interactionFormHandler = new();
            interactionFormHandler.InitialiseTestingData
                (_testHelper.CreateDefaultFileGroup(arrayFileGroup),
                _testHelper.CreateDefaultFileGroup(arrayFileGroupSave),
                filename, _testHelper.FolderPath);

            ListBox listBoxTestFileGroup = _testHelper.CreateListBox(arrayFileGroup, -1);
            System.Windows.Forms.ListView listView = new();

            Task.Run(async () =>
            {
                TResult<FileGroup[]?> tResult = await interactionFormHandler.RemoveSelectedFileGroup(listBoxTestFileGroup, listView);
                Assert.IsTrue(tResult.IsSuccess);
            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        [DataRow(new string[2] { "string", "#$%" }, "RemovePath_DataTest1", true, new string[] { "string", "#$%" }, 0, 10, new int[] { 5, 1, 6 })]
        [DataRow(new string[2] { "string", "#$%" }, "RemovePath_DataTest5", false, new string[] { "string", "dqwdqqdw" }, 5, 10, new int[] { 3, 1, 2 })]
        [DataRow(new string[2] { "string", "#$%" }, "RemovePath_DataTest3", false, new string[] { "string", "#$%", "header", "header" }, 3, 5, new int[] { 0, 1, 2 })]
        [DataRow(new string[1] { "string" }, "RemovePath_DataTest4", false, new string[] { "string", "#$%", "header" }, 1, 8, new int[] { 5, 0, 2 })]

        public async Task RemoveFilePath_Strength_Test(string[] arrayFileGroup, string filename, bool useEmptyView,
            string[] headers, int startIndexRandomWord, int lengthOfRandomWords, int[] selectedIndexArray)
        {
            InteractionFormHandler interactionFormHandler = new();
            FileGroup[] filegroups = _testHelper.CreateDefaultFileGroup(arrayFileGroup);
            interactionFormHandler.InitialiseTestingData
                (filegroups, filegroups, filename, _testHelper.FolderPath);

            Task.Run(async () =>
            {
                System.Windows.Forms.ListView listView = new();
                using (var f = new DummyForm(listView))
                {

                    if (!useEmptyView)
                    {
                        _testHelper.InitListView(headers, startIndexRandomWord, lengthOfRandomWords, selectedIndexArray, listView, f);
                        string[] pathData = _testHelper.CreateStringArrayFromListViewItems(listView);
                        interactionFormHandler.SetUpdateTestFileGroupAndPathData(filegroups, 0, pathData, selectedIndexArray[0]);
                    }

                    TResult<FileGroup[]?> tResult = await interactionFormHandler.RemoveSelectedPathOfFileGroup(listView);
                    Assert.IsTrue(tResult.IsSuccess);

                    f.Close();
                }
            }).GetAwaiter().GetResult();
        }



        [TestMethod]
        [DataRow(new string[2] { "string", "#$%" }, 0, new string[2] { "string", "554" }, 0)]
        [DataRow(new string[2] { "Null", "#$%" }, 1, new string[2] { "string", "554" }, 1)]
        [DataRow(new string[2] { "string", "#$%" }, 0, new string[2] { "string", "554" }, -1)]
        [DataRow(new string[2] { "string", "#$%" }, -1, new string[2] { "string", "554" }, 0)]
        [DataRow(new string[2] { "string", "#$%" }, -1, new string[2] { "string", "554" }, -1)]
        public void OpenFileGroupData_Strength_Test(string[] listToTOpen, int selectedIndexList = -1,
           string[] fileGroup = null, int selectedFileGroup = -1)
        {
            InteractionFormHandler interactionFormHandler = new();
            ListBox? fileDataListBox = _testHelper.CreateListBox(listToTOpen, selectedIndexList);
            interactionFormHandler.SetUpdateTestFileGroupAndPathData(_testHelper.CreateDefaultFileGroup(fileGroup), selectedFileGroup, listToTOpen, selectedIndexList);

            TResult<int?> tResult = interactionFormHandler.Open_FileGroupDataInList(fileDataListBox);

            Assert.IsTrue(tResult.IsSuccess);
        }
        #endregion
    }

}
