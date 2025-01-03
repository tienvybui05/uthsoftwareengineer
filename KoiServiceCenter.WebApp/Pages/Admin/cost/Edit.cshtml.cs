﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiFishServiceCenter.Repositories.Entities;
using KoiFishServiceCenter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace KoiServiceCenter.WebApp.Pages.Admin.cost
{
	[Authorize(Policy = "ManagerOrStaffOnly")]
	public class EditModel : PageModel
    {
        private readonly ICostService _service;

        public EditModel(ICostService service)
        {
            _service = service; 
        }

        [BindProperty]
        public Cost Cost { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            int Id;
            if (id == null)
            {
                Id = 0;
                return NotFound();
            }
            Id = (int)id;
            Cost = await _service.GetCostByIdAsync(Id);
            if (Cost == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = _service.GetCostSelect("ServiceId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Cost.Cost1 < 0)
            {
                ModelState.AddModelError("Cost.Cost1", "Không được phép âm.");

            }
            if (Cost.AdditionalFees < 0)
            {
                ModelState.AddModelError("Cost.AdditionalFees", "Không được phép âm.");

            }
            if (await _service.UpdateCostAsync(Cost)==false)
            {
                ViewData["ServiceId"] = _service.GetCostSelect("ServiceId");
                ModelState.AddModelError(string.Empty, "Cập nhật không thành công. Vui lòng thử lại.");
                return Page();
            }    

            try
            {
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostExists(Cost.CostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CostExists(int id)
        {
            return _service.GetCostByIdAsync(id) != null;
        }
    }
}
