﻿using Newtonsoft.Json;
using System;

namespace Jojatekok.MintPalAPI.MarketTools
{
    public class MarketChartData  : IMarketChartData
    {
        [JsonProperty("date")]
        private string TimeString {
            set { Time = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm", Helper.InvariantCulture); }
        }
        public DateTime Time { get; private set; }

        [JsonProperty("open")]
        public double Open { get; private set; }
        [JsonProperty("close")]
        public double Close { get; private set; }

        [JsonProperty("high")]
        public double High { get; private set; }
        [JsonProperty("low")]
        public double Low { get; private set; }

        [JsonProperty("exchange_volume")]
        public double VolumeExchange { get; private set; }
        [JsonProperty("coin_volume")]
        public double VolumeCoin { get; private set; }
    }
}
