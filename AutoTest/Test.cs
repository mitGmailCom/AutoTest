using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Threading;
using AutoTest.pages;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace AutoTest
{
    [TestFixture(typeof(ChromeDriver))]
    //[TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    public class Test<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        public string PathDress { get; set; }
        public string PathDressSort { get; set; }
        private string path = @"D:\Log.txt";
        FileStream fs;
        StreamWriter sw;

        public void goToPrestashop()
        {
            if (driver.Url == "data:,")
                driver.Navigate().GoToUrl("http://prestashop-automation.qatestlab.com.ua/ru/");
        }

        public void goToDress()
        {
            if (PathDress == "data:,")
                PathDress = "http://prestashop-automation.qatestlab.com.ua/ru/search?controller=search&s=dress";
            //driver.Navigate().GoToUrl(PathDress);
        }

        public void goToDressSort()
        {
            if (PathDressSort == "data:," || PathDressSort == null)
                PathDressSort = "http://prestashop-automation.qatestlab.com.ua/ru/search?SubmitCurrency=1&id_currency=3&order=product.price.desc&s=dress";
            //driver.Navigate().GoToUrl(PathDressSort);
        }

        public static IWebDriver driver;

        [OneTimeSetUp] // вызывается перед началом запуска всех тестов
        public void OneTimeSetUp()
        {
            fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            sw = new StreamWriter(fs, Encoding.Unicode);
            //ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--ignore-certificate-errors");
            //options.AddArguments("--ignore-ssl-errors");
            //driver = new ChromeDriver();// (igWorkDir, options);

            driver = new TWebDriver();
            driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown] //вызывается после завершения всех тестов
        public void OneTimeTearDown()
        {
            //driver.Quit();
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }
            if (fs != null)
                fs.Close();
        }

        [SetUp] // вызывается перед каждым тестом
        public void SetUp()
        {
            //driver = new ChromeDriver();
            //driver.Manage().Window.Maximize();
        }

        [TearDown] // вызывается после каждого теста
        public void TearDown()
        {
            //driver.Close();
            //driver.Quit();
        }

        //1
        [Test]
        [Order(1)]
        public void Test_001_OpenSite()
        {
            try
            {
                // Второй вариант - запуск начальной страницы Google

                //driver.Navigate().GoToUrl("https://www.google.ru"); // 
                PageHome pageHome = new PageHome();
                PageFactory.InitElements(driver, pageHome);
                //pageHome.TxtSearchForm.Click(); // кликаем в поле ввода поисковой фразы
                //pageHome.TxtSearchForm.SendKeys($"http://prestashop-automation.qatestlab.com.ua/ru/"); // вводим поисковую фразу
                //pageHome.BtnSearchSubmit.Click();
                //pageHome.LinkToSite.Click();
                //var tabs = driver.WindowHandles;
                //if (tabs.Count > 1)
                //{
                //    driver.SwitchTo().Window(tabs[0]);
                //    driver.Close();
                //    driver.SwitchTo().Window(tabs[1]);
                //}
                goToPrestashop();
                if (driver.Title == "prestashop-automation")
                    Console.WriteLine("Сайт http://prestashop-automation.qatestlab.com.ua/ru/ успешно открыт.");

                Assert.AreEqual("prestashop-automation", driver.Title);
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена тест: Сайт http://prestashop-automation.qatestlab.com.ua/ru/ успешно открыт.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Не удалось открыть сайт.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Не удалось открыть сайт.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        // 2.1
        [Test]
        [Order(2)]
        public void Test_002_HasPopularGods()
        {
            try
            {
                PageHome pageHome = new PageHome();
                PageFactory.InitElements(driver, pageHome);
                goToPrestashop();
                Assert.That(pageHome.TxtPopularGoods.Text.ToLower(), Is.EqualTo("популярные товары"));
                Console.WriteLine("Найдена секция Популярные товары.");
                
            }
            catch (AssertionException aex)
            {
                System.Console.WriteLine("Секция Популярные товары на сайте отсутствует.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Цена в секции Популярные товары не соответствует валюте установленной в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        // 2.2
        [Test]
        [Order(3)]
        public void Test_003_HasCurrencyHead()
        {
            try
            {
                PageHome pageHome = new PageHome();
                PageFactory.InitElements(driver, pageHome);
                goToPrestashop();
                Assert.That(pageHome.HasCurrencyHead(), Is.True);
                Console.WriteLine("Найдено значение валюты в шапке сайта.");
            }
            catch (AssertionException aex)
            {
                System.Console.WriteLine("Значение валюты в шапке сайта отсутсвует.");
                System.Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Цена в секции Популярные товары не соответствует валюте установленной в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        // 2.3
        [Test]
        [Order(4)]
        public void Test_004_HasCurrencyItem()
        {
            try
            {
                PageHome pageHome = new PageHome();
                PageFactory.InitElements(driver, pageHome);
                goToPrestashop();
                Assert.That(pageHome.HasCurrencyItem(), Is.True);
                Console.WriteLine("Найдено значение валюты в позиции продукции.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Позиция продукции не содержить цены.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Цена в секции Популярные товары не соответствует валюте установленной в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }

        // 2.4
        [Test]
        [Order(5)]
        public void Test_005_CmBoxVSPopularGods()
        {
            try
            {
                PageHome pageHome = new PageHome();
                PageFactory.InitElements(driver, pageHome);

                Assert.That(pageHome.Compare(), Is.True);
                Console.WriteLine("Позиция продукции находится в секции Популярные товары.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Позиция продукции не находится в секции Популярные товары.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Цена в секции Популярные товары не соответствует валюте установленной в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        // 2.5(End)
        [Test]
        [Order(6)]
        public void Test_006_CmBoxIntersectPrice()
        {
            try
            {
                PageHome pageHome = new PageHome();
                PageFactory.InitElements(driver, pageHome);
                goToPrestashop();
                Assert.That(pageHome.EndCmBox(), Does.EndWith(pageHome.EndPrice()));
                Console.WriteLine("Цена в секции Популярные товары соответствует валюте установленной в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена тест: Цена в секции Популярные товары соответствует валюте установленной в шапке сайта.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Цена в секции Популярные товары не соответствует валюте установленной в шапке сайта.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Цена в секции Популярные товары не соответствует валюте установленной в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }



        //3
        [Test]
        [Order(7)]
        public void Test_007_CmBoxSelect()
        {
            try
            {
                PageHome pageHome = new PageHome();
                PageFactory.InitElements(driver, pageHome);
                goToPrestashop();
                PageResultSearch pageResultSearch = pageHome.SelectValDropDown(driver);
                sw.WriteLine($"{DateTime.Now.ToString()} - Значение валюты установлено в USD $");
                Assert.That(pageHome.Selected, Is.EqualTo("USD $"));
                Console.WriteLine("Валюта USD в шапке сайта установлена.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена тест: Валюта USD в шапке сайта установлена.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Валюта USD в шапке сайта не установлена.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Валюта USD $ в шапке сайта не установлена.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }



        //4
        [Test]
        [Order(8)]
        public void Test_008_FindWord()
        {
            try
            {
                PageResultSearch pageResultSearch = new PageResultSearch();
                PageFactory.InitElements(driver, pageResultSearch);
                goToPrestashop();
                pageResultSearch.SearchText("dress");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнен ввод текста 'dress'");
                PageResultSearch pageResultSearch2 = pageResultSearch.ClickBtnSearch();
                sw.WriteLine($"{DateTime.Now.ToString()} - Нажата кнопка Поиск введенного текста");
                PathDress = driver.Url;
                
                Assert.That(driver.Title, Is.EqualTo("Поиск"));
                Console.WriteLine("Выполнен поиск по слову dress.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена тест: Выполнен поиск по слову dress.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Не удалось произвести поиск по слову dress.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Не удалось произвести поиск по слову dress.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        //5
        [Test]
        [Order(9)]
        public void Test_009_CountProducts()
        {
            try
            {
                PageResultSearch pageResultSearch = new PageResultSearch();
                PageFactory.InitElements(driver, pageResultSearch);
                goToDress();
                Assert.That(pageResultSearch.ResSearchMeth(driver), Is.EqualTo(pageResultSearch.CountProducts()));
                Console.WriteLine("Проверено количество найденных элементов.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена проверка: Проверено количество найденных элементов.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Не удалось проверить количество найденых эелементов.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Не удалось проверить количество найденых эелементов.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        //6
        [Test]
        [Order(10)]
        public void Test_010_EqualsCurrency()
        {
            try
            {
                PageResultSearch pageResultSearch = new PageResultSearch();
                PageFactory.InitElements(driver, pageResultSearch);
                goToDress();
                pageResultSearch.SelectedCurrencyMeth(driver);
                Assert.That(pageResultSearch.WebElemToList(), Is.True);
                Console.WriteLine("Валюта найденных товаров совпадает с указаной валютой в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена проверка: Валюта найденных товаров совпадает с указаной валютой в шапке сайта.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Валюта найденных товаров не совпадает с указаной валютой в шапке сайта.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Валюта найденных товаров не совпадает с указаной валютой в шапке сайта.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        //7
        [Test]
        [Order(11)]
        public void Test_011_SortGoods()
        {
            try
            {
                PageResultSearch pageResultSearch = new PageResultSearch();
                PageFactory.InitElements(driver, pageResultSearch);
                goToDress();
                pageResultSearch.ClickToOrder();
                sw.WriteLine($"{DateTime.Now.ToString()} - Click на выпадающий список Сортировки");
                bool res = pageResultSearch.SelectItemDropDownMenu(driver);
                sw.WriteLine($"{DateTime.Now.ToString()} - Выбран метод сортировки 'Цене: от высокой к низкой'");
                PathDressSort = driver.Url;
                Assert.That(res, Is.True);
                Console.WriteLine("Товары отсортированы по уменьшению цен.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена проверка: Товары отсортированы по уменьшению цен.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Не удалось отсортировать товары по уменьшению цен.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Не удалось отсортировать товары по уменьшению цен.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        //8
        [Test]
        [Order(12)]
        public void Test_012_CheckSort()
        {
            try
            {
                PageSortResult pageSortResult = new PageSortResult();
                PageFactory.InitElements(driver, pageSortResult);
                goToDressSort();

                Assert.That(pageSortResult.IsSortRegularPrice(), Is.True);
                Console.WriteLine("Товары отсортированы по c учетоом регулярных цен.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена проверка: Товары отсортированы по c учетоом регулярных цен.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Не удалось отсортировать товары c учетоом регулярных цен");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Не удалось отсортировать товары c учетоом регулярных цен");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }



        //9
        [Test]
        [Order(13)]
        public void Test_013_CheckDiscountPrices()
        {
            try
            {
                PageSortResult pageSortResult = new PageSortResult();
                PageFactory.InitElements(driver, pageSortResult);
                goToDressSort();

                Assert.That(pageSortResult.CheckAllFieldsDiscount(), Is.True);
                Console.WriteLine("Скидка товаров указана в процентах вместе с ценой до и после скидки.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена проверка: Скидка товаров указана в процентах вместе с ценой до и после скидки.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Скидка товаров не указана в процентах вместе с ценой до и после скидки.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Скидка товаров не указана в процентах вместе с ценой до и после скидки.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }


        //10
        [Test]
        [Order(14)]
        public void Test_014_CheckPrices()
        {
            try
            {
                PageSortResult pageSortResult = new PageSortResult();
                PageFactory.InitElements(driver, pageSortResult);
                goToDressSort();

                Assert.That(pageSortResult.CheckPricesBeforAfter(), Is.True);
                Console.WriteLine("Цена до и после скидки совпадает с указанным размером скидки.");
                sw.WriteLine($"{DateTime.Now.ToString()} - Выполнена проверка: Цена до и после скидки совпадает с указанным размером скидки.");
            }
            catch (AssertionException aex)
            {
                Console.WriteLine("Цена до и после скидки не совпадает с указанным размером скидки.");
                Console.WriteLine(aex.Message);
                sw.WriteLine($"{DateTime.Now.ToString()} - Цена до и после скидки не совпадает с указанным размером скидки.");
                sw.WriteLine($"{DateTime.Now.ToString()} - aex.Message");
            }
        }

    }
}
