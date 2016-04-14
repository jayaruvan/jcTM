﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using jcTM.PCL.Transports;
using jcTM.WebAPI.DataLayer.Entities;

namespace jcTM.WebAPI.Controllers {
    public class DayOverviewReportController : ApiController {
        public List<DayOverviewListingResponseItem> GET() {
            using (var eFactory = new jctmEntities()) {
                var result = eFactory.WEBAPI_getDayOverviewListingSP().ToList();

                var response = new List<DayOverviewListingResponseItem>();

                foreach (var item in result) {
                    var rItem = new DayOverviewListingResponseItem {
                        AvgTemp = Math.Round(item.AverageTemp, 0),
                        Day = item.Day,
                        MaxTemp = Math.Round(item.HighTemp, 0),
                        MinTemp = Math.Round(item.LowTemp, 0)
                    };
                    
                    response.Add(rItem);
                }

                return response;
            }
        }

        public List<DayOverviewDetailResponseItem> GET(DateTime selectedDay) {
            using (var eFactory = new jctmEntities()) {
                return eFactory.WEBAPI_getDayOverviewDetailSP(selectedDay).ToList().Select(a => new DayOverviewDetailResponseItem {
                    AvgTemp = (int) (a.AvgTemp ?? 0),
                    Hour = a.HourPart.Value
                }).ToList();
            }
        } 
    }
}