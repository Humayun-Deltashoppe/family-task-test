using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Extensions.ModelConversion;
using Microsoft.AspNetCore.Components;
using Domain.Queries;
using Domain.ViewModel;

using WebClient.Abstractions;
using WebClient.Shared.Models;
using Domain.Commands;
using System.Text.Json;

namespace WebClient.Services
{
    public class TaskDataService: ITaskDataService
    {
        private readonly HttpClient _httpClient;
        private IEnumerable<TaskVm> _tasks;
        
        public TaskDataService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            _tasks = new List<TaskVm>();

            LoadTasks();
        }

        public IEnumerable<TaskVm> Tasks => _tasks;
        public TaskVm SelectedTask { get; private set; }


        public event EventHandler TasksUpdated;
        public event EventHandler TaskSelected;
        public event EventHandler<string> UpdateTaskFailed;
        public event EventHandler<string> CreateTaskFailed;

        private async void LoadTasks()
        {
            _tasks = (await GetAllTasks()).Payload;
            TasksUpdated?.Invoke(this, null);
        }

        private async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await _httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasks");
        }

        private async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await _httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }
        private async Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateTaskCommandResult>($"tasks/{command.Id}", command);
        }
        public void SelectTask(Guid id)
        {
            SelectedTask = Tasks.SingleOrDefault(t => t.Id == id);
            TasksUpdated?.Invoke(this, null);
        }


        public async Task ToggleTask(Guid id)
        {

           var task = _tasks.First(x => x.Id == id);
           task.IsComplete = !task.IsComplete;

            var result = await Update(task.ToUpdateTaskCommand());

            Console.WriteLine(JsonSerializer.Serialize(result));

            if (result != null)
            {
                var updatedList = (await GetAllTasks()).Payload;

                if (updatedList != null)
                {
                    _tasks = updatedList;
                    TasksUpdated?.Invoke(this, null);
                    return;
                }
                UpdateTaskFailed?.Invoke(this, "The save was successful, but we can no longer get an updated list of tasks from the server.");
            }

            UpdateTaskFailed?.Invoke(this, "Unable to save changes.");
        }

        public async Task AddTask(TaskVm model)
        {
            var result = await Create(model.ToCreateTaskCommand());
            if (result != null)
            {
                var updatedList = (await GetAllTasks()).Payload;

                if (updatedList != null)
                {
                    _tasks = updatedList;
                    TasksUpdated?.Invoke(this, null);
                    return;
                }
                UpdateTaskFailed?.Invoke(this, "The creation was successful, but we can no longer get an updated list of tasks from the server.");
            }
            UpdateTaskFailed?.Invoke(this, "Unable to create record.");
        }
    }
}