using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using PhoneDirectory;
using System.Text.RegularExpressions;

namespace UnitTest
{
    [TestClass]
    public class TestMainWindow
    {
        [TestMethod]
        public void ShowData()
        {
            MainWindow mw = new MainWindow();
            DataTable ds = mw.Show();
            Assert.AreEqual(ds.Rows.Count, 6);
        }

        [TestMethod]
        public void SortData()
        {
            MainWindow mw = new MainWindow();
            DataTable ds = mw.Sort("фамилия");
            Assert.AreEqual(ds.Rows.Count, 6);
        }

        [TestMethod]
        public void SearchData()
        {
            MainWindow mw = new MainWindow();
            DataTable ds = mw.Search("Никита");
            Assert.AreEqual(ds.Rows.Count, 1);
        }
    }

    [TestClass]
    public class TestAddContact
    {
        [TestMethod]
        public void ShowData()
        {
            AddContact ac = new AddContact();
            DataTable ds = ac.Show();
            Assert.AreEqual(ds.Rows.Count,6);
        }
    }

    [TestClass]
    public class TestDeleteContact
    {
        [TestMethod]
        public void ShowData()
        {
            DeleteContact dc = new DeleteContact();
            DataTable ds = dc.Show();
            Assert.AreEqual(ds.Rows.Count, 6);
        }

        [TestMethod]
        public void DeleteDataPass()
        {
            DeleteContact dc = new DeleteContact();
            int checkDelete = dc.Delete("12");
            Assert.IsTrue(checkDelete > 0);
            dc.RemoveDeleteData();
        }

        [TestMethod]
        public void DeleteDataFailed()
        {
            DeleteContact dc = new DeleteContact();
            int checkDelete = dc.Delete("50000");
            Assert.IsTrue(checkDelete <= 0);
        }
    }

    [TestClass]
    public class TestEditContact
    {
        [TestMethod]
        public void EditDataPass()
        {
            EditContact dc = new EditContact();
            int checkEdit = dc.Edit("12", "Иванов", "Иван", "Иванович", "+7(958)456-12-12", "ivan@mail.ru", "1", "1", "Друзья", "12.02.2000");
            Assert.IsTrue(checkEdit > 0);
        }

        [TestMethod]
        public void EditDataFailed()
        {
            EditContact dc = new EditContact();
            int checkEdit = dc.Edit("50000", "Иванов", "Иван", "Иванович", "+7(958)456-12-12", "ivan@mail.ru", "1", "1", "Друзья", "12.02.2000");
            Assert.IsTrue(checkEdit <= 0);
        }
    }
}
