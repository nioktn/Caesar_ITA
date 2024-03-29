﻿using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace CaesarLib
{
    public class GroupsInLocation
    {
        private IWebDriver driver;
        private IWebElement _filterSearchButton;
        private IWebElement _myGroupsButton;
        private IWebElement _allGroupsButton;
        private IWebElement _futureGroupsToggle;
        private IWebElement _currentGroupsToggle;
        private IWebElement _endedGroupsToggle;

        public IWebElement FilterSearchButton
        {
            get
            {
                if (_filterSearchButton != null) return _filterSearchButton;
                else
                {
                    _filterSearchButton = driver.FindElement(By.XPath("//div[@class='group-list-view']//div[@class='search']/img"));
                    return _filterSearchButton;
                }
            }
        }

        public IWebElement MyGroupsFilter
        {
            get
            {
                if (_myGroupsButton != null) return _myGroupsButton;
                else
                {
                    _myGroupsButton = driver.FindElement(By.XPath("//div[@class='group-list-footer']/button[contains(text(), 'My Groups')]"));
                    return _myGroupsButton;
                }
            }
        }

        public IWebElement AllGroupsButton
        {
            get
            {
                if (_allGroupsButton != null) return _allGroupsButton;
                else
                {
                    _allGroupsButton = driver.FindElement(By.XPath("//div[@class='group-list-footer']/button[contains(text(), 'All Groups')]"));
                    return _allGroupsButton;
                }
            }
        }

        public IWebElement FutureGroupsToggle
        {
            get
            {
                if (_futureGroupsToggle != null) return _futureGroupsToggle;
                else
                {
                    _futureGroupsToggle = driver.FindElement(By.XPath("//div[@class='stage-toggle']/label[3]/div"));
                    return _futureGroupsToggle;
                }
            }
        }

        public IWebElement CurrentGroupsToggle
        {
            get
            {
                if (_currentGroupsToggle != null) return _currentGroupsToggle;
                else
                {
                    _currentGroupsToggle = driver.FindElement(By.XPath("//div[@class='stage-toggle']/label[2]/div"));
                    return _currentGroupsToggle;
                }
            }
        }

        public IWebElement EndedGroupsToggle
        {
            get
            {
                if (_endedGroupsToggle != null) return _endedGroupsToggle;
                else
                {
                    _endedGroupsToggle = driver.FindElement(By.XPath("//div[@class='stage-toggle']/label[1]/div"));
                    return _endedGroupsToggle;
                }
            }
        }

        public GroupsInLocation(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool AreGroupsVisible()
        {
            return Acts.IsElementVisible(driver, By.XPath("//div[@class='group-collection row']"));
        }

        public bool AreTogglesVisible()
        {
            return Acts.IsElementVisible(driver, By.XPath("//div[@class='stage-toggle']/label"));
        }

        public List<String> GetAvailableGroupsNames(WebDriverWait wait)
        {
            wait.Until((d) => AreGroupsVisible());
            IList<IWebElement> elements = driver.FindElements(By.XPath("//div[@class='group-collection row']/div//p"));
            List<String> groupsNames = new List<String>();
            foreach (var item in elements)
            {
                groupsNames.Add(item.Text);
            }
            return groupsNames;
        }

        public bool IsGroupChosen(IWebElement group)
        {
            return Acts.GetAttribute(group, "class").Contains("chosen");
        }

        public bool IsButtonChosen(IWebElement button)
        {
            return Acts.GetAttribute(button, "class").Contains("pressed");
        }

        public IWebElement GetGroupByName(String name)
        {
            IList<IWebElement> elements = driver.FindElements(By.XPath("//div[@class='group-collection row']/div"));
            List<String> groupsNames = new List<String>();
            foreach (var item in elements)
            {
                if (item.FindElement(By.TagName("p")).Text.Equals(name)) return item;
            }
            throw new InvalidOperationException("There is no group with such name");
        }

        public IWebElement GetGroupByName(String name, WebDriverWait wait)
        {
            wait.Until((d) => AreGroupsVisible());
            IList<IWebElement> elements = driver.FindElements(By.XPath("//div[@class='group-collection row']/div"));
            List<String> groupsNames = new List<String>();
            foreach (var item in elements)
            {
                if (item.FindElement(By.TagName("p")).Text.Equals(name)) return item;
            }
            return null;
        }

        public IList<IWebElement> GetGroupsByNames(params String[] names)
        {
            List<String> Names = new List<String>();
            Names.AddRange(names);
            IList<IWebElement> elements = driver.FindElements(By.XPath("//div[@class='group-collection row']/div"));
            List<String> groupsNames = new List<String>();
            foreach (var item in elements)
            {
                if (!Names.Contains(item.FindElement(By.TagName("p")).Text)) elements.Remove(item);
            }
            return elements;
        }

        public void DeleteGroup(String name, Actions action, WebDriverWait wait)
        {
            MainPage mainPage = new MainPage(driver);
            var leftMenu = mainPage.LeftMenu;
            var grDelConfWind = mainPage.ModalWindow.GroupDeleteConfirmationWindow;
            var groupsList = mainPage.LeftContainer.GroupsInLocation;
            IWebElement findElement;

            groupsList.CurrentGroupsToggle.Click();
            findElement = GetGroupByName(name, wait);
            if (findElement != null) findElement.Click();
            else
            {
                groupsList.EndedGroupsToggle.Click();
                findElement = GetGroupByName(name, wait);
                if (findElement != null) findElement.Click();
                else
                {
                    groupsList.FutureGroupsToggle.Click();
                    findElement = GetGroupByName(name, wait);
                    if (findElement != null) findElement.Click();
                }
            }

            leftMenu.Open(action, wait);
            wait.Until((d) => leftMenu.IsDeleteButtonVisible());
            leftMenu.DeleteButton.Click();
            grDelConfWind.DeleteButton.Click();
        }
    }
}