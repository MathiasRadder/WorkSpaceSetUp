using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkSpaceSetUp.Scripts;
using WorkSpaceSetUp.Scripts.Model;

namespace TestFormsApp.Scripts
{
    public class TestHelper
    {
        #region Variables
        private static readonly TestHelper instance = new();
        private string _testPath = string.Empty;
        private const string _folderName = "TestSaveFiles";
        private const string _fileType = ".json";
        private const string listViewTextFileName = "ListViewRandomWords.txt";
        private string[] _listViewTextArray = null;


        public string FolderPath { get { return _testPath; } }
        public string FileType { get { return _fileType; } }
        #endregion

        #region Singleton
        public static TestHelper InstanceTestHelper()
        {
            return instance;
        }
        #endregion

        #region Initialize 
        private TestHelper()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string? projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
            _testPath = Path.Combine(projectDirectory, _folderName);

            StreamReader? sr = null;
            string errorOrigin = string.Empty;

            string pathWithName = Path.Combine(_testPath, listViewTextFileName);
            if (File.Exists(pathWithName))
                sr = new StreamReader(pathWithName);
            else
                return;

            List<string> stringList = new List<string>();
            string? line = sr.ReadLine();
            while (line != null)
            {
                stringList.Add(line);
                line = sr.ReadLine();
            }
            _listViewTextArray = stringList.ToArray();
        }
        #endregion

        #region PublicMethods

        public FileGroup[] CreateDefaultFileGroup(string[] nameArray)
        {
            FileGroup[] fileGroupArray = new FileGroup[nameArray.Length];
            for (int i = 0; i < nameArray.Length; i++)
            {
                fileGroupArray[i] = new FileGroup(nameArray[i]);
            }
            return fileGroupArray;
        }

        public ListBox CreateListBox(object[] stringArray, int selIndex)
        {
            ListBox fileDataListBox = null;
            if (stringArray == null)
            {
                return fileDataListBox;
            }
            fileDataListBox = new ListBox();
            fileDataListBox.Items.AddRange(stringArray);
            if (selIndex != -1)
            {
                fileDataListBox.SelectedIndex = selIndex;
                fileDataListBox.SelectedItem = stringArray[selIndex];
            }
            return fileDataListBox;
        }

        public ListBox.ObjectCollection CreateListBoxObjCollection(object[] objArray)
        {
            ListBox listBoxTest = new ListBox();
            ListBox.ObjectCollection? objCollection = null;
            if (objArray != null)
                objCollection = new ListBox.ObjectCollection(listBoxTest, objArray);

            return objCollection;
        }

        public System.Windows.Forms.ListView InitListView(string[] headers, int startIndexRandomWord, int lengthOfRandomWords, int[] selectedIndexArray,
            System.Windows.Forms.ListView listView, Form f)
        {
            //I got this idea from https://stackoverflow.com/questions/304844/why-do-selectedindices-and-selecteditems-not-work-when-listview-is-instantiated
            //This is a fix to properly unit test list view

            if (headers == null || headers.Length == 0)
                return listView;

            foreach (string header in headers) listView.Columns.Add(header);

            for (int i = startIndexRandomWord; i < startIndexRandomWord + lengthOfRandomWords; i++)
            {
                string[] splittedString = _listViewTextArray[i].Split(' ', headers.Length);
                ListViewItem listItem = new(splittedString);
                listView.Items.Add(listItem);
            }


            f.Show();
            foreach (int index in selectedIndexArray) listView.SelectedIndices.Add(index);

            return listView;
        }

        public string[] CreateStringArrayFromListViewItems(System.Windows.Forms.ListView listView)
        {
            List<string> pathList = new List<string>();
            foreach (ListViewItem item in listView.Items) pathList.Add(item.Text);
            return pathList.ToArray();
        }

        #endregion
    }
}
