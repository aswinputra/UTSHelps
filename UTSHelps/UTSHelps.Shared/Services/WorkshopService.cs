﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSHelps.Shared.Models;
using System.Net.Http;
using UTSHelps.Shared.Helpers;

namespace UTSHelps.Shared.Services
{
    public class WorkshopService : HelpsService
    {
        public WorkshopService() : base()
        {
        }

        public async Task<Response<WorkshopSet>> GetWorkshopSets()
        {
            TestConnection();

            var response = await helpsClient.GetAsync("api/workshop/workshopSets/true");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsAsync<Response<WorkshopSet>>();
                return results;
            }

            return ResponseHelper.CreateErrorResponse<WorkshopSet>("Could not find workshop sets");
        }

        public async Task<Response<Workshop>> GetWorkshops(int workshopSetId)
        {
            TestConnection();

            var queryString = "workshopSetId=" + workshopSetId + "&active=true" + "&startingDtBegin=" + DateTime.Now.ToString(DateFormat);
            var response = await helpsClient.GetAsync("api/workshop/search?" + queryString);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Response<Workshop>>();
                return result;
            }
            return ResponseHelper.CreateErrorResponse<Workshop>("Could not find workshops");
        }

        public async Task<Response<Workshop>> GetWorkshop(int workshopId)
        {
            TestConnection();

            var response = await helpsClient.GetAsync("api/workshop/" + workshopId.ToString());
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Response<Workshop>>();
                return result;
            }
            return ResponseHelper.CreateErrorResponse<Workshop>("An unknown error occured");
        }

		public async Task<GenericResponse> CreateWorkshopBooking(int workshopId, string studentId)
        {
            if (!IsConnected())
                return ResponseHelper.CreateGenericErrorResponse("No Network Connection");

            var queryString = "workshopId=" + workshopId + "&studentId=" + studentId  + "&userId=" + studentId;
            var response = await helpsClient.PostAsync("api/workshop/booking/create?" + queryString, null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<GenericResponse>();
                return result;
            }

            return ResponseHelper.CreateGenericErrorResponse("An unknown error occured");
        }

        public async Task<GenericResponse> CancelWorkshopBooking(int workshopId, string studentId)
        {
            if (!IsConnected())
                return ResponseHelper.CreateGenericErrorResponse("No Network Connection");

            var queryString = "workshopId=" + workshopId + "&studentId=" + studentId + "&userId=" + studentId;
            var response = await helpsClient.PostAsync("api/workshop/booking/cancel?" + queryString, null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<GenericResponse>();
                return result;
            }

            return ResponseHelper.CreateGenericErrorResponse("An unknown error occured");
        }

        public async Task<GenericResponse> AddNotes(int workshopId, string studentId, string notes, string attendanceCode = null)
        {
            if (!IsConnected())
                return ResponseHelper.CreateGenericErrorResponse("No Network Connection");

            var request = new WorkshopUpdateRequest {
                workshopId = workshopId,
                studentId = studentId,
                userId = studentId,
                attended = attendanceCode != null ? 1 : 0,
                notes = notes
            };
            var response = await helpsClient.PutAsJsonAsync("api/workshop/booking/update", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<GenericResponse>();
                return result;
            }
            return ResponseHelper.CreateGenericErrorResponse("An unknown error occurred, please try again");
        }

		public async Task<GenericResponse> CreateWorkshopWaiting(int workshopId, string studentId)
		{
			if (!IsConnected())
				return ResponseHelper.CreateGenericErrorResponse("No Network Connection");

			var queryString = "workshopId=" + workshopId + "&studentId=" + studentId + "&userId=" + studentId;
			var response = await helpsClient.PostAsync("api/workshop/wait/create?" + queryString, null);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<GenericResponse>();
				return result;
			}

			return ResponseHelper.CreateGenericErrorResponse("An unknown error occured");
		}

		public async Task<WaitListCount> GetWaitListCount(int workshopId)
		{
			TestConnection();

			var queryString = "workshopId=" + workshopId;
			var response = await helpsClient.GetAsync("api/workshop/wait/count?" + queryString);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<WaitListCount>();
				return result;
			}
			return ResponseHelper.GetWaitListedErrorResponse("Request failed");
		}

		public async Task<WaitListed> IsWaitListed(int workshopId, string studentId)
		{
			TestConnection();

			var queryString = "workshopId=" + workshopId + "&studentId=" + studentId;
			var response = await helpsClient.GetAsync("api/workshop/wait?" + queryString);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsAsync<WaitListed>();
				return result;
			}
			return ResponseHelper.CreateWaitListErrorResponse("Request failed");
		}
    }
}
