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
    public class BaseTest
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected LoginPage loginPageInstance;
        protected MainPage MainPageInstance;
        protected string baseURL;

        [OneTimeSetUp]

        public void OneTimeSetUp()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
            //Navigating to Caesar app
            baseURL = "localhost:3000";
        }

        [TearDown]
        public void TearDown()
        {
            //Log out from app
            wait.Until((d) => MainPageInstance.MoveToTopMenu().IsOpened());
            MainPageInstance.TopMenu.LogoutButton.Click();
            Log4Caesar.Log();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Close();
            driver.Quit();           
        }

    }
}