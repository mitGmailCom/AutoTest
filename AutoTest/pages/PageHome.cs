/* Описывает главную страницу */
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace AutoTest.pages
{
    class PageHome
    {

        /// <summary>Строка для ввода текста поиска</summary>
        [FindsBy(How = How.CssSelector, Using = "input[title='Поиск']")]
        public IWebElement TxtSearchForm { get; set; }

        /// <summary>Кнопка Поиск в Google</summary>
        [FindsBy(How = How.CssSelector, Using = "input[value='Поиск в Google']")]
        public IWebElement BtnSearchSubmit { get; set; }

        /// <summary>Кнопка Поиск в Google</summary>
        [FindsBy(How = How.XPath, Using = "//*[@id='rso']/div/div/div[1]/div/div/div[1]/a")]
        public IWebElement LinkToSite { get; set; }


//# rso > div > div > div:nth-child(1) > div > div > div.r > a


        public string Selected { get; set; }


        #region 2
        const string txtPopuarGoods = "#content>section>h1";
        const string txtPrice = "#content>section>div>article:nth-child(1)>div>div.product-description>div>span";
        const string txtCmBox = "#_desktop_currency_selector>div>span.expand-more._gray-darker.hidden-sm-down";


        ///// <summary>
        ///// All goods in head
        ///// </summary>
        //[FindsBy(How = How.CssSelector, Using = "# js-product-list > div.products.row")]
        //public IWebElement AllPopularGoods { get; set; }




        /// <summary>
        /// First item in Popular goods
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = txtPrice)]
        public IWebElement TxtPrice { get; set; }

        /// <summary>
        /// Element Популярные товары
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = txtPopuarGoods)]
        //[FindsBy(How = How.XPath, Using = "//a[text='Популярные товары']")]
        public IWebElement TxtPopularGoods { get; set; }

        /// <summary>
        /// Currency in Head site
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = txtCmBox)]
        public IWebElement TxtCmBombox { get; set; }
        #endregion


        #region 3
        /// <summary>
        /// Element Select
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "#_desktop_currency_selector>div>select")]
        public IWebElement elementGen { get; set; }



        /// <summary>
        /// Select item in dropdown
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public PageResultSearch SelectValDropDown(IWebDriver driver)
        {
            SelectElement select;
            IJavaScriptExecutor exec = (IJavaScriptExecutor)driver;
            exec.ExecuteScript("arguments[0].removeAttribute('class');", elementGen);
            select = new SelectElement(elementGen);
            select.SelectByText("USD $");
            exec.ExecuteScript("arguments[0].removeAttribute('class');", elementGen);
            select = new SelectElement(elementGen);
            Selected = select.SelectedOption.Text;
            exec.ExecuteScript("arguments[0].setAttribute('class','link hidden-md-up');", elementGen);

            return new PageResultSearch();
        }

        #endregion


        //public bool EqualsCurrency()
        //{
        //    bool result = false;
        //    var collection = AllPopularGoods.FindElements(By.ClassName("product-miniature js-product-miniature"));
        //    if (collection.Count > 0)
        //    {
        //        if (HasCurrencyHead())
        //            result = true;
        //        if (!HasCurrencyHead())
        //        {
        //            result = false;
        //            return result;
        //        }

        //        if (HasCurrencyItem())
        //            result = true;
        //        if (!HasCurrencyItem())
        //        {
        //            result = false;
        //            return result;
        //        }


        //        for (int i = 0; i < collection.Count; i++)
        //        {
        //            if (collection[i].Text[collection[i].Text.Length - 1].ToString().ToLower() == (TxtCmBombox.Text[TxtCmBombox.Text.Length - 1]).ToString().ToLower())
        //                result = true;
        //            if (collection[i].Text[collection[i].Text.Length - 1].ToString().ToLower() != (TxtCmBombox.Text[TxtCmBombox.Text.Length - 1]).ToString().ToLower())
        //            {
        //                result = false;
        //                return result;
        //            }
        //        }
        //    }

        //    return result;
        //}


        /// <summary>
        /// HasItemInPopularGoods
        /// </summary>
        /// <returns></returns>
        public bool Compare()
        {
            bool res = false;
            string strIntersect = txtPrice.Intersect(txtPopuarGoods).ToString();
            if (strIntersect!= null)
                res = true;
            return res;
        }

        /// <summary>
        /// Head contains currency
        /// </summary>
        /// <returns></returns>
        public bool HasCurrencyHead()
        {
            bool res = false;
            if (TxtCmBombox.Text != null)
                res = true;
            return res;
        }


        /// <summary>
        /// Item contains currency
        /// </summary>
        /// <returns></returns>
        public bool HasCurrencyItem()
        {
            bool res = false;
            if (TxtPrice.Text != null)
                res = true;
            return res;
        }


        /// <summary>
        /// Get currency from head of site
        /// </summary>
        /// <returns></returns>
        public string EndCmBox()
        {
            return (TxtCmBombox.Text[TxtCmBombox.Text.Length - 1]).ToString().ToLower();
        }

        /// <summary>
        /// Get currency from item goods
        /// </summary>
        /// <returns></returns>
        public string EndPrice()
        {
            return (TxtPrice.Text[TxtPrice.Text.Length - 1]).ToString().ToLower();
        }

    }
}
