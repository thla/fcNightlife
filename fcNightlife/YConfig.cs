﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using YelpSharp;

namespace fcNightlife
{
    public class YConfig
    {
        private static Options _options;

        /// <summary>
        /// return the oauth options for using the Yelp API.  I store my keys in the environment settings, but you
        /// can just write them out here, or put them into an app.config file.  For more info, visit
        /// http://www.yelp.com/developers/getting_started/api_access
        /// </summary>
        /// <returns></returns>
        public static Options Options
        {
            get
            {
                if (_options == null)
                {
                    // get all of the options out of EnvironmentSettings.  You can easily just put your own keys in here without
                    // doing the env dance, if you so choose
                    _options = new Options()
                    {
                        AccessToken = ConfigurationManager.AppSettings["YelpToken"],
                        AccessTokenSecret = ConfigurationManager.AppSettings["YelpTokenSecret"],
                        ConsumerKey = ConfigurationManager.AppSettings["YelpConsumerKey"],
                        ConsumerSecret = ConfigurationManager.AppSettings["YelpConsumerSecret"]
                    };

                    if (String.IsNullOrEmpty(_options.AccessToken) ||
                        String.IsNullOrEmpty(_options.AccessTokenSecret) ||
                        String.IsNullOrEmpty(_options.ConsumerKey) ||
                        String.IsNullOrEmpty(_options.ConsumerSecret))
                    {
                        throw new InvalidOperationException("No OAuth info available.  Please modify Config.cs to add your YELP API OAuth keys");
                    }
                }
                return _options;
            }
        }
    }
}