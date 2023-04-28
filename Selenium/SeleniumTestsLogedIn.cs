using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumCsharp
{
    public class AfterLoginTests
    {
        IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {

            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
            Login();
        }

        public void Login()
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
        }

        [Test]
        public void MealButton()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            IWebElement meal = driver.FindElement(By.XPath("//a[contains(text(), 'Meal')]"));
            meal.Click();
            Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/Meals");
        }
        [Test]
        public void RecipeButton()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            IWebElement recipe = driver.FindElement(By.XPath("//a[contains(text(), 'Recipe')]"));
            recipe.Click();
            Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/Recipe");
        }
        [Test]
        public void AdminButton()
        {
            driver.Navigate().GoToUrl("https://salus-healthy-lifestyle.netlify.app");
            Thread.Sleep(1000);
            IWebElement admin = driver.FindElement(By.XPath("//a[contains(text(), 'Admin')]"));
            admin.Click();
            Assert.AreEqual(driver.Url, "https://salus-healthy-lifestyle.netlify.app/Admin");
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
