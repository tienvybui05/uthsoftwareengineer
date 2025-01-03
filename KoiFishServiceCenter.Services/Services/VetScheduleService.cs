﻿using KoiFishServiceCenter.Repositories.Entities;
using KoiFishServiceCenter.Repositories.Interfaces;
using KoiFishServiceCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFishServiceCenter.Services.Services
{
    public class VetScheduleService : IVetScheduleService
    {
        private readonly IVetScheduleRepository _repository;
        public VetScheduleService(IVetScheduleRepository repository)
        {
            _repository = repository;
        }
        public Task<bool> AddVetSchedule(VetSchedule vetSchedule)
        {
            return  _repository.AddVetSchedule(vetSchedule);
        }

        public  Task<bool> DelVetSchedule(int Id)
        {
            return  _repository.DelVetSchedule(Id);
        }

        public  Task<bool> DelVetSchedule(VetSchedule vetSchedule)
        {
            return  _repository.DelVetSchedule(vetSchedule);
        }

        public Task<VetSchedule> GetVetScheduleById(int Id)
        {
            return _repository.GetVetScheduleById(Id);
        }

        public Task<List<VetSchedule>> GetVetSchedulesAsync()
        {
            return _repository.GetVetSchedulesAsync();
        }

        public  Task<bool> UpdateVetSchedule(VetSchedule vetSchedule)
        {
           return  _repository.UpdateVetSchedule(vetSchedule);
        }
        public Task<List<VetSchedule>> SearchAsync(DateTime dateTime)
        {
            return _repository.SearchAsync(dateTime);
        }
        public SelectList GetVeterinarianSelect()
        {
            return _repository.GetVeterinarianSelect();
        }
        public async Task<int> CountSchedule()
        {
            return await _repository.CountVetSchedule();
        }
        public Task<bool> BundByDate(VetSchedule vetSchedule)
        {
            return _repository.BundByDate(vetSchedule);
        }

        public Task<int> CreateId()
        {
            return _repository.CreateId();
        }
        public Task<List<VetSchedule>> GetWordSchedule(int id, DateTime dateTime)
        {

            return _repository.GetWordSchedule(id, dateTime);
        }
    }
}
