﻿using CaesarLib;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CaesarTests
{
    [TestFixture]
    public class TC_1_01_01
    {
        IWebDriver driver;
        WebDriverWait wait;

        [OneTimeSetUp]
        public void FirstInitialize()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        static List<String> LinksList = new List<String> { @"http://localhost:3000", @"http://localhost:3000/Groups/Dnipro", @"http://localhost:3000/admin" };

        [Test, TestCaseSource("LinksList")]
        public void Test_NavigateToLinks_LoginPageOpened(String link)
        {

            driver.Url = link;
            Assert.IsTrue(wait.Until((d) => LoginPage.IsLoginPageOpened(driver)));
        }

        [TearDown]
        public void CleanUp()
        {
            Log4Caesar.Log();
        }

        [OneTimeTearDown]
        public void FinalCleanUp()
        {
            driver.Quit();
        }
    }
}