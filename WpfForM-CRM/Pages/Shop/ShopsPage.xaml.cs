﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfForM_CRM.Context;
using WpfForM_CRM.Pages.CashRegister;
using WpfForM_CRM.Pages.Category;
using WpfForM_CRM.Pages.ChildCategory;
using WpfForM_CRM.Pages.Product;
using WpfForM_CRM.Pages.Stock;

namespace WpfForM_CRM.Pages.Shop;

/// <summary>
/// Interaction logic for ShopsPage.xaml
/// </summary>
public partial class ShopsPage : Page
{
    private MainWindow window;

    public Guid? UserId;
    public string? UserName;

    public ShopsPage(MainWindow window, Guid? userId = null, string? userName = null, Guid? shopId = null)
    {
        Window = window;
        UserName = userName;
        this.UserId = userId;
        InitializeComponent();
        Load();
    }

    public MainWindow Window;
    private string AddText { get; set; }

    public void UserShopsCount(Guid userId)
    {
        AppDbContext appDbContext = new AppDbContext();
        var shops = appDbContext.Shops.Where(u => u.UserId == userId);
        MessageBox.Show($"{shops.Count()}");
    }


    private string _exitButtonMenu = "";

    public string? KassName { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string CategoryName { get; set; }
    public string ShopName { get; set; }
    public Guid? ChildCategoryId { get; set; }
    public string? ChildCategoryName { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? KassaId { get; set; }
    public Guid? CategoryId { get; set; }

    public void Load()
    {
           
        stockFrameButton.Visibility = Visibility.Hidden;
        Title.Visibility = Visibility.Visible;
        addShopButton.Visibility = Visibility.Visible;
        SearchText.Visibility = Visibility.Visible;
        ShopNameTitle.Visibility = Visibility.Hidden;

        _exitButtonMenu = "shop";
        //CategoryNameTitle.Text = "Category";
        //CategoryNameTitle.Visibility = Visibility.Hidden;

        //ShopNameTitle.Text = "Магазин: ";
        //ShopNameTitle.Visibility = Visibility.Hidden;
        //ReadShopsButton.Content = "Мои магазины";
        AppDbContext appDbContext = new AppDbContext();
        Title.Text = "Мои магазины";
        AddText = "shop";
        var shops = appDbContext.Shops
            .Where(shop => shop.UserId == UserId)
            .OrderByDescending(shop => shop.CreatedDate)
            .ToList();

        var list = new List<ShopControl>();

        foreach (var shop in shops)
        {
            var model = new ShopControl(this);
            model.Width = 200;
            model.Height = 70;
            model.ShopId = shop.Id;
            model.Name = shop.Name;
            list.Add(model);
        }

        addShopButton.Visibility = Visibility.Visible;
        shopsFrame.ItemsSource = list;
    }

    public void ReadCategories()
    {
        ShopNameTitle.Visibility = Visibility.Collapsed;
        shopsFrame.Visibility = Visibility.Visible;

        _exitButtonMenu = "category";

        stockFrameButton.Visibility = Visibility.Visible;
        ReadShopsButton.Visibility = Visibility.Collapsed;
        categoriesButton.Visibility = Visibility.Visible;
        exitButton.Visibility = Visibility.Visible;


        //ReadShopsButton.Content = "Назад";
        AppDbContext appDbContext = new AppDbContext();
        SearchText.Visibility = Visibility.Collapsed;
        AddText = "category";
        Title.Text = "Категории";
        var categories = appDbContext.Categories
            //.Where(category => category.ShopId == ShopId)
            .OrderByDescending(category => category.CreatedDate).ToList();

        ShopNameTitle.Text = "Магазин: " + ShopName;
        ShopNameTitle.Visibility = Visibility.Visible;

        var categoryControls = new List<CategoryControl>();
        foreach (var category in categories)
        {
            var categoryControl = new CategoryControl(this);
            categoryControl.Name = category.Name;
            categoryControl.CategoryId = category.Id;
            categoryControl.ShopId = ShopId;
            categoryControls.Add(categoryControl);
        }


        shopsFrame.ItemsSource = categoryControls;
    }

    public void ReadChildCategories()
    {
        _exitButtonMenu = "childCategory";

        Title.Text = "Под категории ";
        AddText = "childCategory";
        var db = new AppDbContext();
        stockFrameButton.Visibility = Visibility.Visible;

        var childCategories = db.ChildCategories.Where(childCategory => childCategory.CategoryId == CategoryId)
            .OrderByDescending(childCategory => childCategory.CreatedDate).ToList();

        ShopNameTitle.Text = "Магазин: " + ShopName + ", Категория: " + CategoryName;

        var childCategoryControls = new List<ChildCategoryControl>();

        foreach (var childCategory in childCategories)
        {
            var childCategoryControl = new ChildCategoryControl(this);
            childCategoryControl.ChildCategoryId = childCategory.Id;
            childCategoryControl.ChildCategoryName = childCategory.Name;
            childCategoryControl.ShopId = ShopId;
            childCategoryControl.CategoryId = CategoryId;
            childCategoryControls.Add(childCategoryControl);
        }

        shopsFrame.ItemsSource = childCategoryControls;

    }

    public void ReadProducts()
    {
        _exitButtonMenu = "product";
        shopsFrame.Visibility = Visibility.Visible;
        AddText = "product";
        ShopNameTitle.Text = "МагазинМагазин: " + ShopName + ", Категория: " + CategoryName + ", Под категория: " + ChildCategoryName;
        Title.Text = "Продукты";

        var db = new AppDbContext();

        var products = db.Products.Where(p => p.ChildCategoryId == ChildCategoryId)
            .OrderByDescending(p => p.CreatedDate).ToList();

        var productControls = new List<ProductControl>();

        foreach (var product in products)
        {
            var productControl = new ProductControl(this);
            productControl.ProductId = product.Id;
            productControl.ProductName = product.Name;
            productControl.SellingProductPrice = (long)product.SellingPrice!;
            productControl.OriginalProductPrice = product.OriginalPrice.ToString()!;
            productControl.ProductCount = product.Count.ToString()!;
            productControls.Add(productControl);
        }

        shopsFrame.ItemsSource = productControls;
    }

    public void ReadCashRegisters()
    {

        var db = new AppDbContext();

        var cashRegisters = db.CashRegisters.Where(c => c.ShopId == ShopId).ToList()
            .OrderByDescending(c => c.CreatedDate);


        var cashRegisterControls = new List<CashRegisterControl>();

        foreach (var cashRegister in cashRegisters)
        {
            var cashRegisterControl = new CashRegisterControl(mainWindow: Window, this)
            {
                CashRegisterName = cashRegister.Name,
                CashRegisterId = cashRegister.Id
            };
            cashRegisterControls.Add(cashRegisterControl);
        }

        shopsFrame.ItemsSource = cashRegisterControls;
    }

    private void Button_ReadShops(object sender, RoutedEventArgs e)
    {
           
        shopsFrame.Visibility = Visibility.Visible;
        stockFrame.Visibility = Visibility.Collapsed;
        ExitMenuImage.Visibility = Visibility.Collapsed;
        Load();
    }

    private void Button_Add(object sender, RoutedEventArgs e)
    {
        if (AddText == "shop")
        {
            AddShop();
        }

        if (AddText == "category")
        {
            AddCategory();
        }

        if (AddText == "childCategory")
        {
            AddChildCategory();
        }

        if (AddText == "product")
        {
            AddProduct();
        }

        if (AddText == "cashRegister")
        {
            AddCashRegister();
        }
    }

    private void AddShop()
    {
        Add add = new Add(mainWindow: window, this);
        add.ShowDialog();
    }
    private void AddCategory()
    {
        var addCategory = new AddCategory(this);
        addCategory.ShowDialog();
    }

    public void AddChildCategory()
    {
        var addChildCategory = new AddChildCategory(this);
        addChildCategory.ShowDialog();
    }

    public void AddProduct()
    {
        var addProduct = new AddProduct(this);
        addProduct.ShowDialog();
    }

    public void AddCashRegister()
    {
        var addCashRegister = new AddCashRegister(this);
        addCashRegister.ShowDialog();
    }
    private void search_txt_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchTxt = SearchText.Text;

        var dbContext = new AppDbContext();

        var matchingShops = dbContext.Shops
            .Where(shop => shop.Name.Contains(searchTxt) && shop.UserId == UserId)
            .ToList();



        List<ShopControl> list = new List<ShopControl>();

        foreach (var shop in matchingShops)
        {
            var model = new ShopControl(this);
            //model.Width = 200;
            //model.Height = 40;
            model.Name = shop.Name;
            list.Add(model);
        }

        shopsFrame.ItemsSource = list;

        
    }

    

    private void ExitButton_OnClick(object sender, RoutedEventArgs e)
    {
        categoriesButton.Visibility = Visibility.Hidden;
        ReadShopsButton.Visibility = Visibility.Visible;
        exitButton.Visibility = Visibility.Hidden;
        stockFrameButton.Visibility = Visibility.Hidden;
        stockFrame.Visibility = Visibility.Collapsed;
        addShopButton.Visibility = Visibility.Collapsed;
        ExitMenuImage.Visibility = Visibility.Hidden;
        shopsFrame.Visibility = Visibility.Collapsed;
        ShopNameTitle.Visibility = Visibility.Collapsed;
        Title.Visibility = Visibility.Collapsed;
        checkoutBtn.Visibility = Visibility.Collapsed;
        Button_ReadShops(sender, e);
    }


    private void CategoriesButton_OnClick(object sender, RoutedEventArgs e)
    {
        ExitMenuImage.Visibility = Visibility.Visible;
        Title.Visibility = Visibility.Visible;
        stockFrame.Visibility = Visibility.Collapsed;
        addShopButton.Visibility = Visibility.Visible;
        ReadCategories();
    }

    private void StockFrameButton_OnClick(object sender, RoutedEventArgs e)
    {
           
        shopsFrame.Visibility = Visibility.Hidden;
        Title.Visibility = Visibility.Hidden;
        addShopButton.Visibility = Visibility.Hidden;
        ShopNameTitle.Visibility = Visibility.Hidden;
        ExitMenuImage.Visibility = Visibility.Collapsed;
        stockFrame.Visibility = Visibility.Visible;
        stockFrame.Navigate(new StockPage(this));
    }


    private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        
        if (_exitButtonMenu == "category")
        {
            Load();
            stockFrameButton.Visibility = Visibility.Visible;
            ExitMenuImage.Visibility = Visibility.Collapsed;
        }
        if (_exitButtonMenu == "childCategory")
        {
            ReadCategories();
        }
        if (_exitButtonMenu == "product")
        {
            ReadChildCategories();
        }
    }

    private void CashRegisterBtn_OnClick(object sender, RoutedEventArgs e)
    {
        AddText = "cashRegister";
        Title.Visibility = Visibility.Visible;
        addShopButton.Visibility = Visibility.Visible;
        ShopNameTitle.Visibility = Visibility.Visible;
        ExitMenuImage.Visibility = Visibility.Collapsed;
        Title.Text = "Kassa";
        stockFrame.Visibility = Visibility.Collapsed;
        addShopButton.Visibility = Visibility.Visible;
        ShopNameTitle.Text = "Магазин: " + ShopName;
        shopsFrame.Visibility = Visibility.Visible;

        ReadCashRegisters();
    }

}