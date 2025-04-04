using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace WebUITests
{
    [TestFixture]
    public class TestCases : BaseTest
    {
        [Test]
        public void TC1_VerifyAboutEhuPage()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var aboutLink = wait.Until(d =>
                d.FindElement(By.XPath("//a[contains(text(),'About') or contains(text(),'Apie')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", aboutLink);

            wait.Until(d => d.Url.Contains("/about/"));
            Assert.That(driver.FindElement(By.TagName("h1")).Text,
                Does.Contain("About"));
        }

        [Test]
        public void TC2_VerifySearchFunctionality()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var searchIcon = wait.Until(d =>
                d.FindElement(By.CssSelector(".header-search__link")));
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].style.display='block'; arguments[0].style.visibility='visible'; arguments[0].click();",
                searchIcon);

            Thread.Sleep(500);

            var searchInput = (IWebElement)((IJavaScriptExecutor)driver)
                .ExecuteScript("return document.querySelector('input[name=\"s\"]');");

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].value = 'study programs';",
                searchInput);

            var searchButton = (IWebElement)((IJavaScriptExecutor)driver)
                .ExecuteScript("return document.querySelector('button[type=\"submit\"]');");
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", searchButton);

            wait.Until(d => d.Url.Contains("?s=study+programs"));
            Assert.That(driver.FindElements(By.CssSelector(".search-results li, .post-item")).Count > 0);
        }

        [Test]
        public void TC3_VerifyLanguageChange()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var languageSwitcher = wait.Until(d =>
                d.FindElement(By.CssSelector(".language-switcher > li")));
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].style.visibility='visible'; arguments[0].click();",
                languageSwitcher);

            var lithuanianOption = wait.Until(d =>
                d.FindElement(By.XPath("//a[contains(@href, 'lt.ehu.lt') and (contains(text(),'lt') or contains(text(),'Lietuvių'))]")));
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView(); arguments[0].click();",
                lithuanianOption);

            wait.Until(d => d.Url.StartsWith("https://lt.ehu.lt/"));
        }

        [Test]
        public void TC4_VerifyContactInfo()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");

            var pageText = wait.Until(d => d.PageSource);

            Assert.Multiple(() =>
            {
                var emailElement = wait.Until(d =>
                    d.FindElement(By.XPath("//a[contains(@href, 'mailto:')]")));
                Assert.That(emailElement.GetAttribute("href"),
                    Is.EqualTo("mailto:franciskscarynacr@gmail.com"));

                Assert.That(pageText, Does.Contain("+370 68 771365"));
                Assert.That(pageText, Does.Contain("+375 29 5781488"));

                Assert.That(pageText, Does.Contain("facebook.com"));
                Assert.That(pageText, Does.Contain("t.me"));
                Assert.That(pageText, Does.Contain("vk.com"));
            });
        }
    }
}