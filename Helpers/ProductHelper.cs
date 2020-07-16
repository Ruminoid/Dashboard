﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ruminoid.Dashboard.Models;

namespace Ruminoid.Dashboard.Helpers
{
    public sealed class ProductHelper : INotifyPropertyChanged
    {
        #region Current

        public static ProductHelper Current { get; } = InitializeProductHelper();

        #endregion

        #region Utilities

        private static IEnumerable<string> GetProducts() => Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
            .Where(file => file.EndsWith(".exe")).Where(file => !file.Contains("Ruminoid.Dashboard.exe"))
            .Where(file => !file.Contains("ChromeHook"))
            .Select(file => Path.GetFileNameWithoutExtension(file));

        #endregion

        #region Constructor

        private ProductHelper()
        {

        }

        private static ProductHelper InitializeProductHelper()
        {
            ProductHelper productHelper = new ProductHelper();
            foreach (string product in GetProducts())
            {
                FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{product}.exe.config"), FileMode.Open);
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(fs);
                    XmlNode prod = xmlDocument.SelectSingleNode("/configuration/product");
                    productHelper.ProductDictionary.Add(product, new Product
                    {
                        Category = prod?.SelectSingleNode("category")?.InnerText,
                        Description = prod?.SelectSingleNode("description")?.InnerText,
                        Id = prod?.SelectSingleNode("id")?.InnerText,
                        Title = prod?.SelectSingleNode("title")?.InnerText
                    });
                }
                catch (Exception)
                {
                    // Ignore
                }
                finally
                {
                    fs.Close();
                }
            }

            return productHelper;
        }

        #endregion

        #region DataContext

        public Dictionary<string, Product> ProductDictionary { get; } = new Dictionary<string, Product>();

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}