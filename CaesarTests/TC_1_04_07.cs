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
    class TC_1_04_07
    {
        IWebDriver driver;
        Actions action;
        WebDriverWait wait;
        LoginPage loginPageInstance;
        MainPage mainPageInstance;
        GroupCreateWindow groupCreateWindow;

        public Dictionary<String, String> warnings = new Dictionary<String, String>
        {
            ["emptyGroupName"] = "Please, enter group name!",
            ["emptyDirection"] = "Please, select direction!",
            ["emptyStartDate"] = "Start date is required!",
            ["emptyFinishDate"] = "Finish date is required!",
            ["groupNameInvalid"] = "Invalid name! Allowed symbols: a-z, 0-9, \"space\", \"/\", \"-\"",
            ["wrongDateFormat"] = "Wrong date format!",
            ["groupNameMinLength"] = "Name must be at least 4 characters!",
            ["groupNameMaxLength"] = "Name must be at most 20 characters!",
            ["expNameSpecSymbolsNA"] = "Special symbols are not allowed.",
            ["expNameLength"] = "Name should be from 5 to 25 chars."
        };

        [OneTimeSetUp]
        public void FirstInitialize()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [SetUp]
        public void Initialize()
        {
            driver.Url = "http://localhost:3000/logout";
            action = new Actions(driver);
            loginPageInstance = new LoginPage(driver);
            loginPageInstance.LogIn("dmytro", "1234", wait);
            mainPageInstance = new MainPage(driver);
            groupCreateWindow = mainPageInstance.ModalWindow.GroupCreateWindow;
            groupCreateWindow.Open(action, wait);
        }

        [Test]
        public void Test_EmptyFieldsClickConfirm_FourHintsDisplayed()
        {
            groupCreateWindow.SaveGroupButton.Click();
            wait.Until((d) => groupCreateWindow.IsGroupNameHintVisible());
            bool emptyGroupName = warnings["emptyGroupName"].Equals(groupCreateWindow.GroupNameHint.GetAttribute("textContent").Trim());
            bool emptyDirection = warnings["emptyDirection"].Equals(groupCreateWindow.DirectionHint.GetAttribute("textContent").Trim());
            bool emptyStartDate = warnings["emptyStartDate"].Equals(groupCreateWindow.StartDateHint.GetAttribute("textContent").Trim());
            bool emptyFinishDate = warnings["emptyFinishDate"].Equals(groupCreateWindow.FinishDateHint.GetAttribute("textContent").Trim());
            
            Assert.IsTrue(emptyGroupName & emptyDirection & emptyStartDate & emptyFinishDate);
        }

        static IEnumerable<object[]> GroupNameStartDateInvalidData = Instruments.ReadXML("CreateGroupInvalidData.xml",
            "testData", "groupNameInvalid", "startDateInvalid");

        [Test, TestCaseSource("GroupNameStartDateInvalidData")]
        public void Test_GroupNameStardDateInvalidData_Warns(String groupNameInvalid, String startDateInvalid)
        {
            groupCreateWindow
                .SetStartDate(startDateInvalid)
                .SetGroupName(groupNameInvalid);
            groupCreateWindow.SaveGroupButton.Click();
            bool isGroupCreateWindowOpened = groupCreateWindow.IsOpened();
            wait.Until((d) => groupCreateWindow.IsGroupNameHintVisible());
            bool invalidGroupNameWarn = warnings["groupNameInvalid"].Equals(groupCreateWindow.GroupNameHint.GetAttribute("textContent").Trim());
            wait.Until((d) => groupCreateWindow.IsStartDateHintVisible());
            bool invalidStartDateWarn = warnings["wrongDateFormat"].Equals(groupCreateWindow.StartDateHint.GetAttribute("textContent").Trim());
            bool invalidDateFinishDataValue = "Invalid date".Equals(groupCreateWindow.FinishDateField.GetAttribute("value"));
            
            Assert.IsTrue(isGroupCreateWindowOpened & invalidGroupNameWarn & invalidStartDateWarn & invalidDateFinishDataValue);
        }

        static IEnumerable<object[]> GroupNameMinLengthData = Instruments.ReadXML("CreateGroupInvalidData.xml",
            "testData", "groupNameMinLength");

        [Test, TestCaseSource("GroupNameMinLengthData")]
        public void Test_GroupNameBelowLowerBoundary_Warn(String groupNameMinLength)
        {
            groupCreateWindow.SetGroupName(groupNameMinLength);
            groupCreateWindow.SaveGroupButton.Click();
            bool isGroupCreateWindowOpened = groupCreateWindow.IsOpened();
            wait.Until((d) => groupCreateWindow.IsGroupNameHintVisible());
            bool symbolsDeficiencyWarn = warnings["groupNameMinLength"].Equals(groupCreateWindow.GroupNameHint.GetAttribute("textContent").Trim());
            
            Assert.IsTrue(isGroupCreateWindowOpened & symbolsDeficiencyWarn);
        }

        static IEnumerable<object[]> GroupNameMaxLengthData = Instruments.ReadXML("CreateGroupInvalidData.xml",
     "testData", "groupNameMaxLength");

        [Test, TestCaseSource("GroupNameMaxLengthData")]
        public void Test_GroupNameAboveMaxBoundary_Warn(String groupNameMaxLength)
        {
            groupCreateWindow.SetGroupName(groupNameMaxLength);
            groupCreateWindow.SaveGroupButton.Click();
            bool isGroupCreateWindowOpened = groupCreateWindow.IsOpened();
            wait.Until((d) => groupCreateWindow.IsGroupNameHintVisible());
            bool symbolsExcessWarn = warnings["groupNameMaxLength"].Equals(groupCreateWindow.GroupNameHint.GetAttribute("textContent").Trim());
            
            Assert.IsTrue(isGroupCreateWindowOpened & symbolsExcessWarn);
        }

        static IEnumerable<object[]> ExpertNameInvalidData = Instruments.ReadXML("CreateGroupInvalidData.xml",
     "testData", "expertNameSpecSymb");

        [Test, TestCaseSource("ExpertNameInvalidData")]
        public void Test_ExpertNameInvalidData_Warn(String expertNameSpecSymb)
        {
            groupCreateWindow.AddExpertButton.Click();
            groupCreateWindow
                .SetExpertName(expertNameSpecSymb)
                .AcceptInputExpertButton.Click();
            wait.Until((d) => groupCreateWindow.IsExpertHintVisible());
            
            Assert.AreEqual(warnings["expNameSpecSymbolsNA"], groupCreateWindow.ExpertHint.GetAttribute("textContent").Trim());
        }

        static IEnumerable<object[]> ExpertInputMinMaxBoundaryData = Instruments.ReadXML("CreateGroupInvalidData.xml",
    "testData", "expertNameMinMaxLength");

        [Test, TestCaseSource("ExpertInputMinMaxBoundaryData")]
        public void Test_ExpertBeyondMinMaxBoundary_Warn(String expertNameMinMaxLength)
        {
            groupCreateWindow.AddExpertButton.Click();
            groupCreateWindow.SetExpertName(expertNameMinMaxLength)
                .AcceptInputExpertButton.Click();
            wait.Until((d) => groupCreateWindow.IsExpertHintVisible());
            
            Assert.AreEqual(warnings["expNameLength"], groupCreateWindow.ExpertHint.GetAttribute("textContent").Trim());
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
