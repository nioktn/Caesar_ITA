﻿using CaesarLib;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace CaesarTests
{
    [TestFixture]
    class TC_1_01_03
    {
        IWebDriver driver;
        LoginPage loginPageInstance;
        WebDriverWait wait;

        [OneTimeSetUp]
        public void FirstInitialize()
        {
            driver = new ChromeDriver();
        }

        [SetUp]
        public void Initialize()
        {
            driver.Url = @"http://localhost:3000/logout";
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => LoginPage.IsLoginPageOpened(d));
            loginPageInstance = new LoginPage(driver);
        }

        static object[] loginCredentials = { new String[] { "dmytro", "1234" } };

        [Test, TestCaseSource("loginCredentials")]
        public void Test_EscKey_EmptyFields(String login, String password)
        {
            Acts.InputValue(loginPageInstance.LoginField, login);
            Acts.InputValue(loginPageInstance.PasswordField, password);
            loginPageInstance.PasswordField.SendKeys(Keys.Escape);
            bool loginFieldEmpty = String.Empty.Equals(loginPageInstance.LoginField.GetAttribute("value"));
            bool passFieldEmpty = String.Empty.Equals(loginPageInstance.PasswordField.GetAttribute("value"));
            Assert.IsTrue(loginFieldEmpty & passFieldEmpty);
        }

        [Test, TestCaseSource("loginCredentials")]
        public void Test_EnterKey_Login(String login, String password)
        {
            Acts.InputValue(loginPageInstance.LoginField, login);
            Acts.InputValue(loginPageInstance.PasswordField, password);
            new Actions(driver).SendKeys(Keys.Enter).Perform();
            Assert.IsTrue(wait.Until(d => MainPage.IsMainPageOpened(d)));
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
