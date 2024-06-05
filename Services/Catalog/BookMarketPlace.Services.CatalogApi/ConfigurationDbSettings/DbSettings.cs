﻿namespace BookMarketPlace.Services.CatalogApi.ConfigurationDbSettings
{
    public class DbSettings : IDbSettings
    {
        public string BookCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string AuthorCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DBName { get; set; }
    }
}
