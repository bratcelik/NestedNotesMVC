using Microsoft.AspNetCore.Mvc;
using NestedNotesMVC.Models;
using NestedNotesMVC.Models.Common;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace NestedNotesMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
        string baseUrl = "https://localhost:7111/api/";

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            var response = new ServiceResponse<NotesVM>();

            HttpResponseMessage getData = await _httpClient.GetAsync("Notes");


            if (getData.IsSuccessStatusCode)
            {
                string json = await getData.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<ServiceResponse<List<NotesVM>>>(json);

                return View(responseData);


            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var response = new ServiceResponse<NotesVM>();


            HttpResponseMessage getData = await _httpClient.GetAsync("Notes/"+id);


            if (getData.IsSuccessStatusCode)
            {
                string json = await getData.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<ServiceResponse<NotesVM>>(json);

                return View(responseData);

            }
            else
            {
                return View("Error");
            }

        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] NotesVM notesVM)
        {

            var response = new ServiceResponse<Guid>();


            HttpResponseMessage getData = await _httpClient.GetAsync("Notes");

            CreateNoteModel model = new CreateNoteModel();
            model.title = notesVM.Title;
            model.content = notesVM.Content;
            model.parentId = notesVM.Id;

            var postTask = await _httpClient.PostAsJsonAsync<CreateNoteModel>("Notes", model);


            if (postTask.IsSuccessStatusCode)
            {
                if (notesVM.Id is not null)
                    return RedirectToAction("Details", new {id=notesVM.Id});

                return RedirectToAction("Index");

            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Content")] NotesVM notesVM)
        {
            var response = new ServiceResponse<Guid>();

            EditNoteModel model = new EditNoteModel();
            model.title = notesVM.Title;
            model.content = notesVM.Content;

            var postTask = await _httpClient.PutAsJsonAsync<EditNoteModel>("Notes/"+id, model);
            var responseTask = await _httpClient.GetAsync("Notes?id=" + id.ToString());


            if (postTask.IsSuccessStatusCode)
            {
                if (notesVM.Id is not null)
                    return RedirectToAction("Details", new { id = notesVM.Id });

                return RedirectToAction("Index");

            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {


            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var response = new ServiceResponse<Guid>();


            var deleteTask = await _httpClient.DeleteAsync("Notes/" + id);


            if (deleteTask.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            else
            {
                return View("Error");
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}