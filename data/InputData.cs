using System.Collections.Generic;
using System.IO;
using com.knapp.CodingContest.data;
using com.knapp.CodingContest.core;
using com.knapp.CodingContest.warehouse;
using System;

namespace com.knapp.CodingContest
{
    /// <summary>
    /// Container class for all input into the solution
    /// </summary>
    public class InputData
    {

        protected readonly Dictionary<string, Product> products = new Dictionary<string, Product>();

        protected readonly List<Product> productsAtEntry = new List<Product>();

        protected readonly List<Order> allOrders = new List<Order>();

        public WarehouseCharacteristics WarehouseCharacteristics { get; private set; }


        public IReadOnlyList<Product> GetAllProductsAtEntry() => productsAtEntry;

        public IReadOnlyList<Order> GetAllOrders() => allOrders;

        public virtual void Load()
        {
            ReadWarehouseCharacteristics( Path.Combine(Settings.DataPath, Settings.WarehousePropertiesFile) );
            ReadProducts(Path.Combine(Settings.DataPath, Settings.ProductFile));
            ReadProductInQueue(Path.Combine(Settings.DataPath, Settings.InqueueFile));
            ReadOrders(Path.Combine(Settings.DataPath, Settings.OrderFile));
        }

        private void ReadWarehouseCharacteristics( string fullFilename )
        {
            var properties = new JavaPropertiesFile(fullFilename);
            properties.Load();

            WarehouseCharacteristics = new WarehouseCharacteristics(properties);

            System.Console.Out.WriteLine( $"+++ loaded: warehouse characteristics" );
        }


        private void ReadProducts(string fullFilename)
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });

                        string productCode = fields[0].Trim();

                        int length = int.Parse(fields[1]);
                        int width = int.Parse(fields[2]);

                        products.Add(productCode, new Product(productCode, length, width));
                    }
                }
            }


            System.Console.Out.WriteLine($"+++ loaded: products, {products.Count} entries");
        }

        private void ReadProductInQueue( string fullFilename )
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });

                        string productCode = fields[0].Trim();

                        productsAtEntry.Add(products[productCode]);
                    }
                }
            }


            System.Console.Out.WriteLine($"+++ loaded: products at entry, {productsAtEntry.Count} entries");
        }


        private void ReadOrders(string fullFilename )
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });
                        var orderCode = fields[0].Trim();

                        var p = new List<Product>();

                        for( int i = 1; i < fields.Length; ++i)
                        {
                            var code = fields[i].Trim();

                            if (!string.IsNullOrWhiteSpace(code))
                            {
                                p.Add(products[code]);
                            }
                        }

                        allOrders.Add(new Order( orderCode, p) );
                    }
                }
            }


            System.Console.Out.WriteLine($"+++ loaded: orders, {allOrders.Count} entries");
        }
    }
}
