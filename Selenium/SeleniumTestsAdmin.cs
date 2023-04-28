using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace SeleniumCsharp
{
    public class AdminTests
    {
        IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {

            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
            LoginAndNavigateToAdmin();
        }

        public void LoginAndNavigateToAdmin()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            IWebElement login = driver.FindElement(By.XPath("//a[contains(text(), 'Login')]"));
            login.Click();
            IWebElement email = driver.FindElement(By.Id("email"));
            email.SendKeys("vbazsi17@gmail.com");
            IWebElement password = driver.FindElement(By.Id("password"));
            password.SendKeys("vbazsi17");
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[contains(text(), 'Log in')]"));
            loginBtn.Click();
            while (driver.Url == "https://salus-healthy-lifestyle.netlify.app/Login")
            {

            }
            Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/");
            //wait for admin button appear
            System.Threading.Thread.Sleep(1000);
            IWebElement admin = driver.FindElement(By.XPath("//a[contains(text(), 'Admin')]"));
            admin.Click();
            Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/Admin");
        }
        [Test]
        public void TestAddDiet()
        {
            // find Diets box and click the add button
            IWebElement dietsBox = driver.FindElement(By.XPath("//h2[text()='Diets']/ancestor::div[@class='box']"));
            IWebElement addDietButton = dietsBox.FindElement(By.CssSelector("button.btn-primary"));
            addDietButton.Click();

            // wait for the modal to appear and enter input
            IWebElement modal = driver.FindElement(By.CssSelector(".modal"));
            // Enter the name of the new diet
            var name = Guid.NewGuid().ToString();
            driver.FindElement(By.CssSelector("div.modal-body > input[type='text']")).SendKeys(name);

            // Enter the min and max fat values
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[0].SendKeys("10");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[1].SendKeys("20");

            // Enter the min and max carbohydrate values
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[2].SendKeys("30");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[3].SendKeys("40");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[4].SendKeys("30");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[5].SendKeys("40");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[6].SendKeys("30");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[7].SendKeys("40");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[8].SendKeys("10");
            driver.FindElements(By.CssSelector("div.modal-body > ul > li > p > input[type='number']"))[9].SendKeys("20");
            driver.FindElement(By.CssSelector("div.modal-body > ul > li > p > input[type='text']")).SendKeys("Valid description.");

            driver.FindElement(By.XPath("//button[contains(text(), 'Save')]")).Click();
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            System.Threading.Thread.Sleep(1000);
            IWebElement admin = driver.FindElement(By.XPath("//a[contains(text(), 'Admin')]"));
            admin.Click();
            System.Threading.Thread.Sleep(1000);
            IWebElement? newDiet;
            try
            {
                var newDietPath = $"//label[contains(text(), '{name}')]";
                newDiet = driver.FindElement(By.XPath(newDietPath));

            }
            catch
            {
                newDiet = null;
            }
            Assert.NotNull(newDiet);

        }

        //[Test]
        //public void RegisterInvalid()
        //{
        //    driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
        //    IWebElement register = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
        //    register.Click();
        //    IWebElement? alert;
        //    try
        //    {
        //        alert = driver.FindElement(By.CssSelector("alert-danger"));
        //    }
        //    catch
        //    {
        //        alert = null;
        //    }
        //    bool isNull = alert == null;
        //    IWebElement username = driver.FindElement(By.Id("username"));
        //    username.SendKeys("1234567");
        //    alert = driver.FindElement(By.CssSelector("div.alert.alert-danger"));
        //    Assert.IsTrue(alert.Text.Contains("Username must be at least 8 characters long"));
        //    Assert.IsTrue(isNull);
        //}

        //[Test]
        //public void RegisterValid()
        //{
        //    driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
        //    IWebElement register = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
        //    register.Click();

        //    IWebElement username = driver.FindElement(By.Id("username"));
        //    username.SendKeys("12345678");
        //    IWebElement? alert;
        //    try
        //    {
        //        alert = driver.FindElement(By.CssSelector("alert-danger"));
        //    }
        //    catch
        //    {
        //        alert = null;
        //    }
        //    Assert.IsNull(alert);
        //}

        //[Test]
        //public void RegisterButton()
        //{
        //    driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
        //    IWebElement register = driver.FindElement(By.XPath("//a[contains(text(), 'Register')]"));
        //    register.Click();
        //    Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/Register");
        //}
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
