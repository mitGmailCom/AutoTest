using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AutoTest.pages
{
    class PageSortResult
    {
        /// <summary>
        /// Selected currensy
        /// </summary>
        public string SelectedCurrency { get; set; }
        
        /// <summary>
        /// Collection of goods
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "#js-product-list > div.products.row > article")]
        public IList<IWebElement> Products { get; set; }

        /// <summary>
        /// Collection of prices and discount each item in collection goods
        /// </summary>
        [FindsBy(How = How.ClassName, Using = "product-price-and-shipping")]
        public IList<IWebElement> ProductCollection { get; set; }




        //public void Temp()
        //{
        //    bool result = false;
        //    for (int i = 0; i < Products.Count; i++)
        //    {
        //        var t = Products[i];
        //        //string symbolCurrensy = SelectedCurrency.Substring(SelectedCurrency.Length - 1).ToString();
        //        //var res = (tempItem.Text).FirstOrDefault(x => x.ToString() == symbolCurrensy);
        //        //if (res.ToString() != null)
        //        //    result = true;
        //        //if (res.ToString() == null)
        //        //{
        //        //    result = false;
        //        //    break;
        //        //}
        //    }
        //}

        
        /// <summary>
        /// Collection of regular prices
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> CollectionRegularPrice(int i)
        {
            ReadOnlyCollection<IWebElement> tempCollect = ProductCollection[i].FindElements(By.ClassName($"regular-price"));
            return tempCollect;
        }

        /// <summary>
        /// Collection of prices
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> CollectionPrice(int i)
        {
            ReadOnlyCollection<IWebElement> tempCollect = ProductCollection[i].FindElements(By.ClassName($"price"));
            return tempCollect;
        }

        /// <summary>
        /// Collections of discounts
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> CollectionPriceDiscount(int i)
        {
            ReadOnlyCollection<IWebElement> tempCollect = ProductCollection[i].FindElements(By.ClassName($"discount-percentage"));
            return tempCollect;
        }

        /// <summary>
        /// Collection of all span in each item in collection of goods
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> CollectionAllPrices(int i)
        {
            ReadOnlyCollection<IWebElement> tempCollect = ProductCollection[i].FindElements(By.ClassName($"#js-product-list > div.products.row > article:nth-child({i + 1}) > div > div.product-description > div > span"));
            return tempCollect;
        }


        /// <summary>
        /// Convert to double price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public double ConvertToDoublePrice(ReadOnlyCollection<IWebElement> price)
        {
            string temp1 = (((price[0].Text.Substring(0, price[0].Text.Length - 2)).Trim(' '))).ToString();
            double sum = 0.0;
            try
            {
                sum = Convert.ToDouble(temp1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sum;
        }

        public double ConvertToDoubleRegularSum(ReadOnlyCollection<IWebElement> regularPrice)
        {
            string temp1 = (((regularPrice[0].Text.Substring(0, regularPrice[0].Text.Length - 2)).Trim(' '))).ToString();
            double sum = 0.0;
            try
            {
                sum = Convert.ToDouble(temp1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sum;
        }


        /// <summary>
        /// Sort with regular price
        /// </summary>
        /// <returns></returns>
        public bool IsSortRegularPrice()
        {
            List<double> PriceWithRegular = new List<double>();
            List<double> Price = new List<double>();
            for (int i = 0; i < ProductCollection.Count; i++)
            {
                var regularPrice = CollectionRegularPrice(i);
                var price = CollectionPrice(i);
                if (price.Count != 0)
                {
                    if (regularPrice.Count != 0)
                    {
                        PriceWithRegular.Add(ConvertToDoublePrice(regularPrice));
                        Price.Add(ConvertToDoubleRegularSum(regularPrice));
                    }
                    if (regularPrice.Count == 0)
                    {
                        PriceWithRegular.Add(ConvertToDoublePrice(price));
                        Price.Add(ConvertToDoubleRegularSum(price));
                    }
                }
            }
            var resRegPrice = Price.SequenceEqual(PriceWithRegular);
            return resRegPrice;
        }


        /// <summary>
        /// Check discount and all prices
        /// </summary>
        /// <returns></returns>
        public bool CheckAllFieldsDiscount()
        {
            bool result = false;
            for (int i = 0; i < ProductCollection.Count; i++)
            {
                var regularPrice = CollectionRegularPrice(i);
                var price = CollectionPrice(i);
                var discountPercentage = CollectionPriceDiscount(i);

                if (discountPercentage.Count != 0)
                {
                    if (regularPrice.Count == 0 || price.Count == 0)
                    {
                        result = false;
                        return result;
                    }
                    if (discountPercentage[0].Text.Contains("%"))
                        result = true;
                    if (!(discountPercentage[0].Text.Contains("%")))
                    {
                        result = false;
                        return result;
                    }
                    if (regularPrice.Count != 0 || price.Count != 0)
                        result = true;
                }
            }
            return result;
        }


        /// <summary>
        /// Check prices befor discount and after discount
        /// </summary>
        /// <returns></returns>
        public bool CheckPricesBeforAfter()
        {
            bool result = false;
            for (int i = 0; i < ProductCollection.Count; i++)
            {
                var regularPrice = CollectionRegularPrice(i);
                var price = CollectionPrice(i);
                var discountPercentage = CollectionPriceDiscount(i);

                if (discountPercentage.Count > 0)
                {
                    double beforPrice = ConvertToDoubleRegularSum(regularPrice);
                    double afterPrice = ConvertToDoublePrice(price);
                    double percent = Convert.ToDouble(((discountPercentage[0].Text).Replace('%', ' ')).Trim(' ', '-'));
                    var t = beforPrice - (beforPrice * percent / 100);
                    if (Math.Round(beforPrice - (beforPrice * percent / 100), 2) == afterPrice)
                        result = true;
                    if (Math.Round(beforPrice - (beforPrice * percent / 100), 2) != afterPrice)
                    {
                        result = false;
                        return result;
                    }
                }
            }
            return result;
        }




    }
}
