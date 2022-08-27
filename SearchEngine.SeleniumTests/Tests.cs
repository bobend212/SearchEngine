using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace SearchEngine.SeleniumTests
{
    public class Tests
    {
        private IWebDriver driver;
        private string localHostAddress;
        private string searchText;
        private string searchTextNoResults;

        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver(); //tested on Edge - ver 104.0.1293.70
            localHostAddress = "https://localhost:7061/";
            searchText = "Cyclosporin dermatology Covid-19";
            searchTextNoResults = "Cyclosporin dermatology Covid-19 123456789";
        }

        [Test]
        public void Test_IfRecords_CheckTheAmount()
        {
            driver.Navigate().GoToUrl(localHostAddress);
            IWebElement searchField = driver.FindElement(By.CssSelector("[name='search']"));
            searchField.SendKeys(searchText);
            searchField.Submit();

            string expectedTotalCount = "96"; // as of 27.08.2022
            IWebElement totalCountField = driver.FindElement(By.CssSelector("[class='totalCountText']"));

            Assert.That(totalCountField.Text, Is.EqualTo(expectedTotalCount), "They are not equal.");
        }

        [Test]
        public void Test_IfNoRecords_CheckDisplayedInfo()
        {
            driver.Navigate().GoToUrl(localHostAddress);
            IWebElement searchField = driver.FindElement(By.CssSelector("[name='search']"));
            searchField.SendKeys(searchTextNoResults);
            searchField.Submit();

            string expectedInfo = $"There are no publications matching \"{searchTextNoResults}\" query."; // as of 27.08.2022
            IWebElement displayedInfo = driver.FindElement(By.CssSelector("[class='no_records']"));

            Assert.That(displayedInfo.Text, Is.EqualTo(expectedInfo), "They are not equal.");
        }

        [TearDown]
        public void QuitDriver()
        {
            driver.Quit();
        }
    }
}