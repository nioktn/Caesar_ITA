﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CaesarLib
{
    public class CreateEditUsersForm : AdminPage
    {
        private IWebElement _firstName;
        private IWebElement _lastName;
        private IWebElement _role;
        private IWebElement _location;
        private IWebElement _photo;
        private IWebElement _login;
        private IWebElement _password;
        private IWebDriver _driver;
        private IWebElement _table;

        private IList<IWebElement> _EditButtons;
        private IList<IWebElement> _DeleteButtons;
               
        public IWebElement GetTable
        {
            get
            {
                if (_table != null) return _table;
                else
                {
                    _table = _driver.FindElement(By.Id("users"));
                    return _table;
                }
            }
        }
        public IList<IWebElement> Delete
        {
            get
            {
                if (_DeleteButtons != null) return _DeleteButtons;
                else
                {
                    _DeleteButtons = GetTable.FindElements(By.ClassName("btn-danger"));
                    return _DeleteButtons;
                }
            }
        }

        public CreateEditUsersForm DeleteUser(int index)
        {
            Delete[index - 1].Click();
            return this;
        }
        public CreateEditUsersForm EditUser(int index)
        {
            Edit[index - 1].Click();
            return this;
        }
        public IList<IWebElement> Edit
        {
            get
            {
                if (_EditButtons != null) return _EditButtons;
                else
                {
                    _EditButtons = GetTable.FindElements(By.ClassName("edit"));
                    return _EditButtons;
                }
            }
        }
        

        public IWebElement FirstNameField
        {
            get
            {
                if (_firstName != null) return _firstName;
                else
                {
                    _firstName = _driver.FindElement(By.Name("firstName"));
                    return _firstName;
                }
            }
        }
        public CreateEditUsersForm addUsers()
        {
            AddButtonClick(0);
            return this;
        }
        public CreateEditUsersForm setFirstName(string value)
        {
            FirstNameField.SendKeys(value);
            return this;
        }

        public IWebElement LastNameField
        {
            get
            {
                if (_lastName != null) return _lastName;
                else
                {
                    _lastName = _driver.FindElement(By.Name("lastName"));
                    return _lastName;
                }
            }
        }

        public CreateEditUsersForm setLastName(string value)
        {
            LastNameField.SendKeys(value);
            return this;
        }

        public IWebElement RoleDDL
        {
            get
            {
                if (_role != null) return _role;
                else
                {
                    _role = _driver.FindElement(By.Name("role"));
                    return _role;
                }
            }
        }

        public CreateEditUsersForm selectRole(int value)
        {
            Acts.SelectOptionFromDDL(RoleDDL, value);
            return this;
        }

        public IWebElement LocationDDL
        {
            get
            {
                if (_location != null) return _location;
                else
                {
                    _location = _driver.FindElement(By.Name("location"));
                    return _location;
                }
            }
        }

        public CreateEditUsersForm selectLocation(int value)
        {
            Acts.SelectOptionFromDDL(LocationDDL, value);
            return this;
        }
        public IWebElement Photo
        {
            get
            {
                if (_photo != null) return _photo;
                else
                {
                    _photo = _driver.FindElement(By.Name("photo"));
                    return _photo;
                }
            }
        }

        public CreateEditUsersForm setPhoto(string value)
        {
            Photo.SendKeys(value);
            return this;
        }

        public IWebElement Login
        {
            get
            {
                if (_login != null) return _login;
                else
                {
                    _login = _driver.FindElement(By.Name("login"));
                    return _login;
                }
            }
        }

        public CreateEditUsersForm setLogin(string value)
        {
            Login.SendKeys(value);
            return this;
        }

        public IWebElement Password
        {
            get
            {
                if (_password != null) return _password;
                else
                {
                    _password = _driver.FindElement(By.Name("password"));
                    return _password;
                }
            }
        }

        public CreateEditUsersForm setPassword(string value)
        {
            Password.SendKeys(value);
            return this;
        }

        public List<string> RememberUser()
        {
            List<string> user = new List<string>();
            user.Add(FirstNameField.GetAttribute("value"));
            user.Add(LastNameField.GetAttribute("value"));
            user.Add(RoleDDL.GetAttribute("value"));
            user.Add(LocationDDL.GetAttribute("value"));
            user.Add(Photo.GetAttribute("value"));
            user.Add(Login.GetAttribute("value"));
            user.Add(Password.GetAttribute("value"));

            return user;
        }

        public CreateEditUsersForm(IWebDriver driver) : base(driver)
        {
            _driver = driver;
        }       

    }
}
