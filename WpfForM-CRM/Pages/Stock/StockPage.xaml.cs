﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfForM_CRM.Context;
using WpfForM_CRM.Pages.Shop;

namespace WpfForM_CRM.Pages.Stock;

/// <summary>
/// Interaction logic for StockPage.xaml
/// </summary>
public partial class StockPage : Page
{
    public StockPage(ShopsPage shopsPage)
    {
        InitializeComponent();
        this.shopsPage = shopsPage;
        Load();
    }

    public ShopsPage shopsPage;

    public void Load()
    {
        var db = new AppDbContext();


        var items = new List<Entities.Stock>();
        var products = db.Products.Where(p => p.UserId == shopsPage.UserId).OrderByDescending(p => p.CreatedDate).ToList();
        int i = 1;

        if (products.Count > 0)
        {
            foreach (var product in products)
            {
                var childCategory = db.ChildCategories.FirstOrDefault(p => p.Id == product.ChildCategoryId);
                if (childCategory != null)
                {
                    var category = db.Categories.FirstOrDefault(p => p.Id == product.CategoryId);
                    

                    var item = new Entities.Stock
                    {

                        Номер = i,
                        Магазин = shopsPage.ShopName,
                        Продукт = product.Name,
                        Штрихкод = product.Barcode,
                        Подкатегория = childCategory.Name,
                        Категория = category!.Name,
                        Текущая = product.OriginalPrice + " UZS",
                        Прибывшая = product.SellingPrice + " UZS",
                        Количство = (product.Count ?? 1).ToString(),
                    };

                    items.Add(item);
                    i++;
                }

            }

            stockData.ItemsSource = items;
        }

    }

    private void AddProductForStockBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var addProductStock = new AddProductStock(this);
        addProductStock.ShowDialog();
    }

    private void UpdateProductForStockBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var indexItem = stockData.SelectedIndex;
        if (indexItem == -1)
        {
            MessageBox.Show("Выберите продукт");
            return;
        }

        var stocks = (List<Entities.Stock>)stockData.ItemsSource;
        var selectedStock = stocks[indexItem];
        var updateProductStock = new UpdateProductStock(this, selectedStock);
        updateProductStock.ShowDialog();
    }

    private void DeleteProductForStockBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var indexItem = stockData.SelectedIndex;
        if (indexItem == -1)
        {
            MessageBox.Show("Выберите продукт");
            return;
        }

        var stocks = (List<Entities.Stock>)stockData.ItemsSource;
        var selectedStock = stocks[indexItem];
        var resultQuestion = MessageBox.Show("Вы уверены, что хотите удалить товар?", "", 
            MessageBoxButton.YesNo,
            MessageBoxImage.Stop);

        if (resultQuestion == MessageBoxResult.Yes)
        {
            var db = new AppDbContext();
            var product = db.Products
                .First(p => p.Barcode == selectedStock.Штрихкод);

            db.Products.Remove(product);
            db.SaveChanges();
            Load();
        }
    }

    private void FastArrivalForStockBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var indexItem = stockData.SelectedIndex;
        if (indexItem == -1)
        {
            MessageBox.Show("Выберите продукт");
            return;
        }

        var stocks = (List<Entities.Stock>)stockData.ItemsSource;
        var selectedStock = stocks[indexItem];
        var fastArrivalProductStock  = new FastArrivalProductStock(this, selectedStock);
        fastArrivalProductStock.ShowDialog();
    }

}