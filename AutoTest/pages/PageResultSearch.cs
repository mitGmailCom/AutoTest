/* Описывает страницу результатов поиска */
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace AutoTest.pages
{
    class PageResultSearch
    {
        ///// <summary>Логотип-ссылка</summary>
        //[FindsBy(How = How.CssSelector, Using = "a#logo")]
        //public IWebElement LnkLogo { get; set; }


        /// <summary>Field for input text</summary> 
        [FindsBy(How = How.CssSelector, Using = "#search_widget > form > input.ui-autocomplete-input")]
        public IWebElement SearchInput { get; set; }


        /// <summary>
        /// Button for search
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "#search_widget > form > button")]
        public IWebElement SearchButton { get; set; }


        /// <summary>
        /// Collection of goods(result of search)
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "#js-product-list > div.products.row > article")]
        public IList<IWebElement> Products { get; set; }


        /// <summary>
        /// Number of goods
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "#js-product-list-top > div.col-md-6.hidden-sm-down.total-products > p")]
        public IWebElement ResSearch { get; set; }


        /// <summary>Currency in head site</summary>
        [FindsBy(How = How.CssSelector, Using = "#_desktop_currency_selector>div>span.expand-more._gray-darker.hidden-sm-down")]
        public IWebElement elementGen { get; set; }


        /// <summary>Шапка с валютой</summary>
        [FindsBy(How = How.CssSelector, Using = "#js-product-list-top > div:nth-child(2) > div > div > div")]
        public IWebElement LinkOrderMenu { get; set; }

        /// <summary>DropDown</summary>
        [FindsBy(How = How.CssSelector, Using = "#js-product-list-top > div:nth-child(2) > div > div > a")]
        public IWebElement DropDown { get; set; }
        
        /// <summary>Collection goods</summary>
        [FindsBy(How = How.CssSelector, Using = "#js-product-list-top > div:nth-child(2) > div > div > div")]
        public IList<IWebElement> DropDownMenu { get; set; }



        // number elements
        public string CountElem { get; set; }
        // selected currency
        public string SelectedCurrency { get; set; }



        /// <summary>
        /// Find text
        /// </summary>
        /// <param name="txt"></param>
        public void SearchText(string txt)
        {
            SearchInput.SendKeys(txt);
        }


        /// <summary>
        /// Click on button Search
        /// </summary>
        /// <returns></returns>
        public PageResultSearch ClickBtnSearch()
        {
            SearchButton.Click();
            return new PageResultSearch();
        }



        /// <summary>
        /// Set number of found elements
        /// </summary>
        public string CountProducts()
        {
            //CountElem = Products.Count;
            return Products.Count.ToString();
        }

        /// <summary>
        /// Get value from result search
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public string ResSearchMeth(IWebDriver driver)
        {
            IJavaScriptExecutor exec = (IJavaScriptExecutor)driver;
            exec.ExecuteScript("arguments[0].removeAttribute('class');", ResSearch);
            CountElem = ResSearch.Text;
            exec.ExecuteScript("arguments[0].setAttribute('class','col-md-6 hidden-sm-down total-products');", ResSearch);

            string t = CountElem.Substring(CountElem.LastIndexOf(' ')).Trim('.', ' ');
            return t;
        }


        /// <summary>
        /// Set selected currency
        /// </summary>
        /// <param name="driver"></param>
        public void SelectedCurrencyMeth(IWebDriver driver)
        {
            SelectedCurrency = elementGen.Text; //select.SelectedOption.Text;
        }

        /// <summary>
        /// Compare price of results with USD
        /// </summary>
        /// <returns></returns>
        public bool WebElemToList()
        {
            bool result = false;
            for (int i = 0; i < Products.Count; i++)
            {
                var tempItem = Products[i];
                string symbolCurrensy = SelectedCurrency.Substring(SelectedCurrency.Length - 1).ToString();
                var res = (tempItem.Text).FirstOrDefault(x => x.ToString() == symbolCurrensy);
                if (res.ToString() != null)
                    result = true;
                if (res.ToString() == null)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Click to element for sort
        /// </summary>
        public void ClickToOrder()
        {
            DropDown.Click();
        }

        /// <summary>
        /// Select item in dropdown
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public bool SelectItemDropDownMenu(IWebDriver driver)
        {
            bool result = false;
            for (int i = 0; i < DropDownMenu.Count; i++)
            {
                var w = DropDownMenu[i].Text.Replace('\r', ' ');
                var t = DropDownMenu[i].Text.Split('\n');
                for (int j = 0; j < t.Length; j++)
                {
                    t[j].Trim(' ');
                    if (t[j] == "Цене: от высокой к низкой")
                    {
                        (driver.FindElement(By.CssSelector($"#js-product-list-top > div:nth-child(2) > div > div > div > a:nth-child({j + 1})"))).Click();
                        result = true;
                    }
                }
            }
            return result;
        }


        


    }
}
