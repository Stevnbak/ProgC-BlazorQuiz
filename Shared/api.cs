using System.Net.NetworkInformation;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.Net.Http.Json;
using static Quiz.Shared.api;
using System.Web;

namespace Quiz.Shared
{
    public class api
    {
        static HttpClient http = new HttpClient { };
        public static async Task<Category[]> apiCategoryRequest()
        {
            //Get categories
            apiCategoryList response = await http.GetFromJsonAsync<apiCategoryList>("https://opentdb.com/api_category.php");
            return response.trivia_categories;
        }
        private class apiCategoryList
        {
            public Category[]? trivia_categories { get; set; }
        }
        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public static async Task<QuizQuestion[]> apiRequest(string category, int amount, string difficulty, string type)
        {
            //Get from API: https://opentdb.com/api_config.php
            apiResponse response = await http.GetFromJsonAsync<apiResponse>($"https://opentdb.com/api.php?amount={amount}{(category == "0" ? "" : $"&category={category}")}&difficulty={difficulty}{(type == "both" ? "" : $"&type={type}")}&encode=url3986");
            foreach(QuizQuestion question in response.Results)
            {
                question.DecodeAll();
                question.CombineAnswers();
            }
            return response.Results;
        }
        private class apiResponse
        {
            public int ResponseCode { get; set; }
            public QuizQuestion[]? Results { get; set; }
        }
    }
}
