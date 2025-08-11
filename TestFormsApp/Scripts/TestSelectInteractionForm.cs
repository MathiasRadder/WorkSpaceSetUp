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
    public sealed class TestSelectInteractionForm
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
        [DataRow(0, new string[1] { "#$%" },  "Select_DataTestDef", new string[] { "string", "#$%", "null" }, 0, 20, new int[] { 0, 1, 10 })]
        [DataRow(1, new string[2] { "string", "#$%" }, "Select_DataTestDef1", new string[] { "string", "#$%", "null" }, 0, 20, new int[] { 0, 1, 10 })]

        public void SelectFileGroupInList_Success_Test(int selectedIndex, string[] fileGroupArray, string fileName, 
            string[] headers, int startIndexRandomWord, int lengthOfRandomWords, int[] selectedIndexArray)
        {
            InteractionFormHandler interactionFormHandler = new();
            FileGroup[] fileArray = _testHelper.CreateDefaultFileGroup(fileGroupArray); ;
            interactionFormHandler.InitialiseTestingData(fileArray, fileArray, fileName, _testHelper.FolderPath);

            ListBox fileGroupListBox = _testHelper.CreateListBox(fileGroupArray, selectedIndex);
            System.Windows.Forms.ListView listView = new();
            using (var f = new DummyForm(listView))
            {
                listView = _testHelper.InitListView(headers, startIndexRandomWord, lengthOfRandomWords, selectedIndexArray, listView, f);              

                TResult<int> tResult = interactionFormHandler.Select_FileGroupInList(fileGroupListBox, listView.Items, null);
                Assert.IsTrue(tResult.IsSuccess);
                f.Close();
            }
 
        }

        [TestMethod]
        [DataRow(0, new object[1] { (object)"#$%" }, null, new string[] { "string", "#$%" }, 0, 10, new int[] { 5, 1, 2 })]
        [DataRow(1, new object[2] { (object)"string", (object)"#$%" }, null, new string[] { "string", "#$%" }, 0, 10, new int[] { 5, 1, 2 })]
        public void SelectFileGroupInList_Failure_Test(int selectedIndex, object[] fileGroupArray, System.Windows.Forms.TextBox textBox, string[] headers,
            int startIndexRandomWord, int lengthOfRandomWords, int[] selectedIndexArray)
        {
            InteractionFormHandler interactionFormHandler = new();
            ListBox fileGroupListBox = _testHelper.CreateListBox(fileGroupArray, selectedIndex); 
            System.Windows.Forms.ListView listView = new();
            using (var f = new DummyForm(listView))
            {
                listView = _testHelper.InitListView(headers, startIndexRandomWord, lengthOfRandomWords, selectedIndexArray, listView, f);
                TResult<int> tResult = interactionFormHandler.Select_FileGroupInList(fileGroupListBox, listView.Items, textBox);
                Assert.IsTrue(tResult.IsFailure);
                f.Close();
            }
        }

        [TestMethod]
        [DataRow(new string[] { "string", "#$%" }, 0, 10, new int[] { 5, 1, 2 })]
        [DataRow(new string[] { "string", "#$%", "null" }, 0, 20, new int[] { 0, 1, 10 })]
        [DataRow(new string[] { "Null", "#$%", "null" }, 5, 8, new int[] { 0, 1, 3 })]
        [DataRow(new string[] { "a" }, 5, 8, new int[] { 0, 1, 3 })]
        public void SelectPathInList_Success_Test(string[] headers, int startIndexRandomWord, int lengthOfRandomWords, int[] selectedIndexArray)
        {
            InteractionFormHandler interactionFormHandler = new();
            System.Windows.Forms.ListView listView = new();
            using (var f = new DummyForm(listView))
            {
                _testHelper.InitListView(headers, startIndexRandomWord, lengthOfRandomWords, selectedIndexArray, listView, f);
                string[] pathData = _testHelper.CreateStringArrayFromListViewItems(listView);
                interactionFormHandler.SetUpdateTestFileGroupAndPathData(_testHelper.CreateDefaultFileGroup(pathData), 0, pathData, selectedIndexArray[0]);

                TResult<int> tResult = interactionFormHandler.Select_FileGroupPathInList(listView);
                Assert.IsTrue(tResult.IsSuccess);
                f.Close();
            }
        }

        [TestMethod]
        [DataRow(new string[] { "string", "#$%" }, 0, 10, new int[] { 5, 1, 2 })]
        [DataRow(new string[] { "string", "#$%", "null" }, 0, 20, new int[] { 0, 1, 10 })]
        [DataRow(new string[] { "Null", "#$%", "null" }, 5, 8, new int[] { 0, 1, 3 })]
        [DataRow(new string[] { "a" }, 5, 8, new int[] { 0, 1, 3 })]
        public void SelectPathInList_Failure_Test(string[] headers, int startIndexRandomWord, int lengthOfRandomWords, int[] selectedIndexArray)
        {
            InteractionFormHandler interactionFormHandler = new();
            System.Windows.Forms.ListView listView = new();
            using (var f = new DummyForm(listView))
            {
                _testHelper.InitListView(headers, startIndexRandomWord, lengthOfRandomWords, selectedIndexArray, listView, f);
                TResult<int> tResult = interactionFormHandler.Select_FileGroupPathInList(listView);
                Assert.IsTrue(tResult.IsFailure);
                f.Close();
            }
        }




        [TestMethod]
        [DataRow(new string[2] { "qqqq", "aaa" }, 1, "oqwkdpoq", "EndUpdateFile_DataTest1")]
        [DataRow(new string[2] { "qqqq", "aaa" }, 0, "", "EndUpdateFile_DataTest2")]
        public void EndUpdateFileGroupName_Strength_Test(string[] arrayFileGroup, int selectedIndex, string name, string filename)
        {
            InteractionFormHandler interactionFormHandler = new();
            interactionFormHandler.InitialiseTestingData
               (_testHelper.CreateDefaultFileGroup(arrayFileGroup),
               _testHelper.CreateDefaultFileGroup(arrayFileGroup),
               filename, _testHelper.FolderPath);
            interactionFormHandler.SetUpdateTestFileGroupAndPathData(_testHelper.CreateDefaultFileGroup(arrayFileGroup), selectedIndex, arrayFileGroup, 0);

            ListBox listBoxTestFileGroup = _testHelper.CreateListBox(arrayFileGroup, selectedIndex);
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            textBox.Text = name;

            Task.Run(async () =>
            {
                TResult<FileGroup[]?> tResult = await interactionFormHandler.EndUpdateFileGroupName(listBoxTestFileGroup, textBox);
                Assert.IsTrue(tResult.IsSuccess);
            }).GetAwaiter().GetResult();
        }

        #endregion
    }

}
