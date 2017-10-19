using System;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Linq;

namespace BruteForceEmulator
{
    public partial class BruteForceEmulator : Form
    {
        public static string UserName = "BRL.Test";
        public static string UserNameXPath = "//input[@id='Username']";
        public static string PasswordXPath = "//input[@id='Password']";
        public static string AssertLoginProblem = "Login Failed";

        public static Random Random = new Random();

        public BruteForceEmulator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IWebDriver webDriver = new FirefoxDriver();
            webDriver.Manage().Window.Maximize();

            webDriver.Navigate().GoToUrl("https://testlogin.meddbase.com/");

            RunAttack(webDriver);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UserName = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            UserNameXPath = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            PasswordXPath = textBox3.Text;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static void RunAttack(IWebDriver driver)
        {
            try
            {
                do
                {
                    IWebElement usernameTextBox = driver.FindElement(By.XPath(UserNameXPath));
                    IWebElement passwordTextBox = driver.FindElement(By.XPath(PasswordXPath));

                    usernameTextBox.Clear();
                    usernameTextBox.SendKeys(UserName);

                    passwordTextBox.Clear();
                    passwordTextBox.SendKeys(RandomString(16));

                    driver.FindElement(By.XPath("//input[@id='Login']")).Click();

                    AssertLoginProblem = driver.FindElement(By.XPath("//div[@id='LoginProblemTitle']")).Text.ToString();

                    driver.FindElement(By.XPath("//input[@id='Login']")).Click();
                }
                while (AssertLoginProblem == "Login Failed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Application.Exit();
            }
        }
    }
}