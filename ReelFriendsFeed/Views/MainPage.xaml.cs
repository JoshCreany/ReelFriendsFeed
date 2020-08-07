using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ReelFriendsFeed.ViewModels;
using RestSharp;
using System.Reflection;

namespace ReelFriendsFeed.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        public void OnButtonClicked(object sender, EventArgs args)
        {
            var authData = "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Username.Text, Password.Text)));
            //Console.WriteLine("---------------HASH INCOMING: " + authData);
            var client2 = new RestClient("https://lucidsauce.ml/api/z/1.0/item/update?body="+PostContent.Text);
            //client2.Timeout = -1;
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("Authorization", authData);
            //request2.AddHeader("Cookie", "PHPSESSID=hn9vr1j090f8213uas5bn5bcfh");
            IRestResponse response2 = client2.Execute(request2);
            Console.WriteLine(response2.Content);
        }
    }
}