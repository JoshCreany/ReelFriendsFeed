using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReelFriendsFeed.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public Command GetFeed { get; }
        public ObservableCollection<Post> Posts { get; }
        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public MainViewModel()
        {
            Posts = GetPostData();
            GetFeed = new Command(RefreshFeed);
            Posts.Add(new Post { Author = "test", Content = "temporary" });
        }

        private ObservableCollection<Post> GetPostData()
        {
            Console.WriteLine("Fetching Json.....");
            var client = new RestClient("https://lucidsauce.ml/api/statuses/user_timeline");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "lucidsauce.ml");
            request.AddHeader("Postman-Token", "331c863f-a332-4bec-9ab2-1335318d50f1,c9d4bfe7-bc11-44c7-99d6-98f989fb1437");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.18.0");
            request.AddHeader("Authorization", "Basic dXNlcjFAZ2V0bmFkYS5jb206dXNlci0x");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var deserializer = new JsonDeserializer();
            deserializer.Deserialize<List<string>>(response);
            dynamic json = JArray.Parse(response.Content);

            var tempList = new ObservableCollection<Post>();
            for (int i = 0; i < 20; i++)
            {
                tempList.Add(new Post { Author = json[i]["user"]["screen_name"], Content = json[i]["text"] });
            }
            return tempList;
        }


        public void RefreshFeed()
        {
            Console.WriteLine("Refreshing..................");
            if (IsBusy)
                return;
            IsBusy = true;
            Posts.Clear();

            //fetch json
            Console.WriteLine("Fetching Json.....");
            var client = new RestClient("https://lucidsauce.ml/api/statuses/user_timeline");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "lucidsauce.ml");
            request.AddHeader("Postman-Token", "331c863f-a332-4bec-9ab2-1335318d50f1,c9d4bfe7-bc11-44c7-99d6-98f989fb1437");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.18.0");
            request.AddHeader("Authorization", "Basic dXNlcjFAZ2V0bmFkYS5jb206dXNlci0x");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var deserializer = new JsonDeserializer();
            deserializer.Deserialize<List<string>>(response);
            dynamic json = JArray.Parse(response.Content);

            //foreach (dynamic JsonEntry in json)
            for (int i = 0; i < 20; i++)
            {
                /*string txt = json[i]["text"];
                foreach (Match item in Regex.Matches(txt, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
                {
                    Console.WriteLine(item.Value);
                }*/
                Posts.Add(new Post { Author = json[i]["user"]["screen_name"], Content = json[i]["text"], PhotoURL = json[i]["user"]["profile_image_url_https"] });
            }
            //PostData();
            IsBusy = false;
        }

        public void PostData()
        {
            //Posting
            string username = "user1@getnada.com";
            string password = "user-1";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", username, password))));
            Console.WriteLine("HASH INCOMING:                " + httpClient.DefaultRequestHeaders.Authorization);

            var client2 = new RestClient("https://lucidsauce.ml/api/z/1.0/item/update?body=This%20is%20a");
            client2.Timeout = -1;
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("Text", "This is wildddd test with postman");
            request2.AddHeader("Authorization", "Basic dXNlcjFAZ2V0bmFkYS5jb206dXNlci0x");
            request2.AddHeader("Cookie", "PHPSESSID=hn9vr1j090f8213uas5bn5bcfh");
            IRestResponse response2 = client2.Execute(request2);
        }
    }

    public class Post
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public string PhotoURL { get; set; }
    }
}
