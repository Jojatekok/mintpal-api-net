﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Jojatekok.MintPalAPI
{
    static class Helper
    {
        internal const string ApiUrlBase = "https://api.mintpal.com/";
        private const string ApiUrlVersionString = "v2/";
        internal const string ApiUrlPrefixMarket = ApiUrlVersionString + "market/";
        internal const string ApiUrlPrefixTrading = ApiUrlVersionString + "trading/";
        internal const string ApiUrlPrefixWallet = ApiUrlVersionString + "wallet/";

        internal const int RequestsTimeoutMilliseconds = 8192;
        internal const byte AuthRequestsExtraTimeSeconds = 4;

        internal static readonly string AssemblyVersionString = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        internal static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        internal static async Task<string> GetResponseStringAsync(this HttpWebRequest request)
        {
            using (var response = await request.GetResponseAsync()) {
                using (var stream = response.GetResponseStream()) {
                    if (stream == null) throw new NullReferenceException("The HttpWebRequest's response stream is empty.");

                    using (var reader = new StreamReader(stream)) {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        internal static T DeserializeObject<T>(this JsonSerializer serializer, string value)
        {
            using (var stringReader = new StringReader(value)) {
                using (var jsonTextReader = new JsonTextReader(stringReader)) {
                    return (T)serializer.Deserialize(jsonTextReader, typeof(T));
                }
            }
        }

        internal static string ToStringUniform(this double value)
        {
            return value.ToString("0.########", InvariantCulture);
        }

        internal static string ToHttpPostString(this Dictionary<string, object> dictionary)
        {
            var output = string.Empty;
            foreach (var entry in dictionary)
            {
                var valueString = entry.Value as string;
                if (valueString == null) {
                    output += "&" + entry.Key + "=" + entry.Value;
                } else {
                    output += "&" + entry.Key + "=" + valueString.Replace(' ', '+');
                }
            }

            return output.Substring(1);
        }

        internal static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
        }

        internal static double DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        internal static string[] SplitCoinPair(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException("input");

            var inputSplit = input.Split('/');
            if (inputSplit.Length < 2) {
                throw new ArgumentException("The string format of the provided exchange pair is invalid.", "input");
            }

            return inputSplit;
        }
    }
}
