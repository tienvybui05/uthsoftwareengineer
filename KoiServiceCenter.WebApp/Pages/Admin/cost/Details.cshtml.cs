﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiFishServiceCenter.Repositories.Entities;
using KoiFishServiceCenter.Services.Interfaces;

namespace KoiServiceCenter.WebApp.Pages.Admin.cost
{
    public class DetailsModel : PageModel
    {
        private readonly ICostService _service;

        public DetailsModel(ICostService service)
        {
            _service = service;
        }

        public Cost Cost { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            int ID;
            if (id == null)
            {
                ID = 0;
                return NotFound();
            }
            ID = (int)id;
            Cost = await _service.GetCostByIdAsync(ID);

            if (Cost == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}