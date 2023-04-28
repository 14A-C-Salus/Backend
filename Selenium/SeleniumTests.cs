using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumCsharp
{
    public class BeforeLoginTests
    {
        IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {

            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
        }

        [Test]
        public void LoginButton()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            IWebElement login = driver.FindElement(By.XPath("//a[contains(text(), 'Login')]"));
            login.Click();
            Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/Login");
        }




        [Test]
        public void RegisterInvalid()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            IWebElement register = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
            register.Click();
            IWebElement? alert;
            try
            {
                alert = driver.FindElement(By.CssSelector("alert-danger"));
            }
            catch 
            {
                alert = null;
            }
            bool isNull = alert == null;
            IWebElement username = driver.FindElement(By.Id("username"));
            username.SendKeys("1234567");
            alert = driver.FindElement(By.CssSelector("div.alert.alert-danger"));
            Assert.IsTrue(alert.Text.Contains("Username must be at least 8 characters long"));
            Assert.IsTrue(isNull);
        }

        [Test]
        public void RegisterValid()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            IWebElement register = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
            register.Click();

            IWebElement username = driver.FindElement(By.Id("username"));
            username.SendKeys("12345678");
            IWebElement? alert;
            try
            {
                alert = driver.FindElement(By.CssSelector("alert-danger"));
            }
            catch
            {
                alert = null;
            }
            Assert.IsNull(alert);
        }

        [Test]
        public void RegisterButton()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            IWebElement register = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
            register.Click();
            Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/Register");
        }





        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
