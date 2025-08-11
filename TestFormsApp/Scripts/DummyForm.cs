using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//I got this idea from https://stackoverflow.com/questions/304844/why-do-selectedindices-and-selecteditems-not-work-when-listview-is-instantiated
//This is a fix to properly unit test list view

namespace TestFormsApp.Scripts
{
    internal class DummyForm : Form
    {
        public DummyForm(ListView listView)
        {
            // Minimize and make it not appear in taskbar to
            // avoid flicker etc when running the tests
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Controls.Add(listView);
        }
    }
}
