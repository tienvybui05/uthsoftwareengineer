﻿using KoiFishServiceCenter.Repositories.Entities;
using KoiFishServiceCenter.Services.Interfaces;
using KoiFishServiceCenter.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace KoiServiceCenter.WebApp.Pages.Services.Danhgiavatuvanhoca
{
    public class IndexModel : PageModel
    {
        private readonly IServiceHistoryService _service;
        private readonly IVetScheduleService _vetScheduleService;
        private readonly ICustomerService _customerService;
        private readonly IAuthorizationService _authorizationService;
        public IndexModel(IServiceHistoryService service, IVetScheduleService vetScheduleService, ICustomerService customerService, IAuthorizationService authorizationService)
        {
            _service = service;
            _vetScheduleService = vetScheduleService;
            _customerService = customerService;
            _authorizationService = authorizationService;
        }
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {

                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                customer = await _customerService.GetCustomer(int.Parse(customerId));
                if (customer != null)
                {

                    int ranDumID;
                    ranDumID = await _service.CreateId();
                    ServiceHistory = new ServiceHistory
                    {
                        HistoryId = ranDumID,
                        CustomerId = customer.CustomerId


                    };

                }
            }

            // Load các ViewData khác nếu cần
            ViewData["CustomerId"] = _service.GetServiceHistorySelect("CustomerId");
            ViewData["ServiceId"] = _service.GetServiceHistorySelect("ServiceId");
            ViewData["VeterinarianId"] = _service.GetServiceHistorySelect("VeterinarianId");

            return Page();
        }

        [BindProperty]
        public ServiceHistory ServiceHistory { get; set; }
        public VetSchedule VetSchedule { get; set; }

        public Customer customer { get; set; }
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge(); // Yêu cầu đăng nhập
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, "CustomerOnly");
            if (!authorizationResult.Succeeded)
            {
                return Forbid(); // Từ chối truy cập nếu không phải Customer
            }
            //await _service.AddServiceHistory(ServiceHistory);
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            customer = await _customerService.GetCustomer(int.Parse(customerId));
            if (customer == null)
            {
                return Forbid(); // Từ chối nếu không tìm thấy thông tin customer
            }
            ServiceHistory.CustomerId = customer.CustomerId;
            var checkDateTime = await _service.BundByDate(ServiceHistory);
             DateTime currentDate = DateTime.Today;
            if (ServiceHistory.ServiceDate<currentDate)
            {
                ModelState.AddModelError("ServiceHistory.ServiceDate", "Ngày nhập vào không hợp lệ. Vui lòng chọn ngày khác.");
                ViewData["CustomerId"] = _service.GetServiceHistorySelect("CustomerId");
                ViewData["ServiceId"] = _service.GetServiceHistorySelect("ServiceId");
                ViewData["VeterinarianId"] = _service.GetServiceHistorySelect("VeterinarianId");
                return Page();
            }
            if (checkDateTime == false)
            {
                ModelState.AddModelError("ServiceHistory.ServiceDate", "Bác sĩ đã có lịch. Vui lòng chọn ngày khác.");
                ViewData["CustomerId"] = _service.GetServiceHistorySelect("CustomerId");
                ViewData["ServiceId"] = _service.GetServiceHistorySelect("ServiceId");
                ViewData["VeterinarianId"] = _service.GetServiceHistorySelect("VeterinarianId");
                return Page();
            }
            else
            {
                await _service.AddServiceHistory(ServiceHistory);
                bool check = false;
                Random random = new Random();
                int ranDumID;
                do
                {
                    ranDumID = random.Next(1, 1001);
                    var x = await _service.GetServiceHistoryById(ranDumID);
                    if (x == null)
                    {
                        check = true;
                    }

                } while (check != true);

                VetSchedule = new VetSchedule();
                VetSchedule.ScheduleId = ranDumID;
                VetSchedule.VeterinarianId = ServiceHistory.VeterinarianId;
                VetSchedule.ScheduleDate = ServiceHistory.ServiceDate;
                ViewData["VeterinarianId"] = _vetScheduleService.GetVeterinarianSelect();
                await _vetScheduleService.AddVetSchedule(VetSchedule);// Thêm vào lịch làm việc của bác sĩ


                return Redirect(Url.Page("/Services/BookingSuccessfully/Index"));
            }
        }
    }
}
