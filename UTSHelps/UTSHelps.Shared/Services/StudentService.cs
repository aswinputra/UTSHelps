﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UTSHelps.Shared.Helpers;
using UTSHelps.Shared.Models;

namespace UTSHelps.Shared.Services
{
    public class StudentService : HelpsService
    {
        public StudentService() : base()
        {
        }

        public async Task<GenericResponse> Register(Student student)
        {
            var response = await helpsClient.PostAsJsonAsync("api/student/register", student);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<GenericResponse>();
                return result;
            }

            return ResponseHelper.CreateGenericErrorResponse("An unkown error occured");
        }

        public async Task<GenericResponse> GetStudent(int studentId)
        {
            var response = await helpsClient.GetAsync("api/student?studentId=" + studentId);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Response<Student>>();
                return result;
            }
            return ResponseHelper.CreateGenericErrorResponse("An unkown error occured");
        }
    }
}
