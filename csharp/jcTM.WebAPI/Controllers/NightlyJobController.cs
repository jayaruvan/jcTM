﻿using jcTM.WebAPI.DataLayer.Entities;

namespace jcTM.WebAPI.Controllers {
    public class NightlyJobController : BaseController {
        public void GET() {
            using (var eFactory = new jctmEntities()) {
                eFactory.Database.ExecuteSqlCommand("EXEC dbo.SQL_runNightlyJobsSP");
            }
        }
    }
}