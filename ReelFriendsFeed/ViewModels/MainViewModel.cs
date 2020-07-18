using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
                Posts.Add(new Post { Author = json[i]["user"]["screen_name"], Content = json[i]["text"] });
            }

            //Posting
            //var client2 = new RestClient("https://lucidsauce.ml/api/z/1.0/item/update?body=The%20postman%20is%20always%20here%21%21");
            //var request2 = new RestRequest(Method.GET);
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("Connection", "keep-alive");
            //request.AddHeader("Cookie", "PHPSESSID=tmqkg5tfbtrno5dg9p5f7acj1q");
            //request.AddHeader("Accept-Encoding", "gzip, deflate");
            //request.AddHeader("Host", "lucidsauce.ml");
            //request.AddHeader("Postman-Token", "c3753572-cab3-4fb4-b022-4ec49e1a22cf,a4eb2ffd-4042-48fe-b5ff-f4f36b59991f");
            //request.AddHeader("Cache-Control", "no-cache");
            //request.AddHeader("Accept", "*/*");
            //request.AddHeader("User-Agent", "PostmanRuntime/7.18.0");
            //request.AddHeader("Authorization", "Basic dXNlcjFAZ2V0bmFkYS5jb206dXNlci0x");
            //IRestResponse response2 = client2.Execute(request2);

            IsBusy = false;
        }
    }

    public class Post
    {
        public string Author { get; set; }
        public string Content { get; set; }
    }
}
