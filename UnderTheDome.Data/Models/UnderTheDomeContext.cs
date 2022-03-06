using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using UnderTheDome.Data.Interfaces;

namespace UnderTheDome.Data.Models
{
    public class UnderTheDomeContext : IUnderTheDomeContext
    {
        private string? _responseFromServer;
        private readonly string _url = @"https://api.tvmaze.com/shows/1/cast";
        private const string Key = "ActorsList";
        private readonly IMemoryCache _memoryCache;
        private INoteContext _noteContext;
        private List<Actor> _actors;
        private string _cacheTimeFilePath = Directory.GetCurrentDirectory() + @"\cacheTime.txt";


        public UnderTheDomeContext(IMemoryCache memoryCache, INoteContext noteContext)
        {
            _memoryCache = memoryCache;
            _noteContext = noteContext;
            if (!File.Exists(_cacheTimeFilePath))
                using (StreamWriter sw = new StreamWriter(_cacheTimeFilePath)) { sw.Write(@"Cache ttl in seconds: 300"); };
        }

        public List<Actor> Actors
        {
            get
            {
                return getActorsFromCache();
            }

            set
            {
                _actors = value;
            }
        }

        private List<Actor> getActorsFromCache()
        {
            if(!_memoryCache.TryGetValue(Key, out List<Actor> cachedValue))
            {
                _actors = new List<Actor>();
                buildActorsList();
                cachedValue = _actors;
                _memoryCache.Set(Key, _actors, TimeSpan.FromSeconds(getCacheTtl()));
            }
            return cachedValue;
        }
        private bool buildActorsList()
        {
            if (!getDataFromWeb(_url))
            {
                return false;
            }

            parseResponseJson();
            foreach (Actor actor in _actors)
            {
                actor.Note = _noteContext.GetNoteForActor(actor.Id);
            }
            return true;
        }

        private bool getDataFromWeb(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                _responseFromServer = reader.ReadToEnd();
            }
            response.Close();
            if(_responseFromServer != null)
                return true;
            return false;
        }

        private void parseResponseJson()
        {
            using (JsonDocument document = JsonDocument.Parse(_responseFromServer))
            {
                JsonElement root = document.RootElement;
                JsonElement personElement, idElement, nameElement, birthdayElement, genderElement;

                foreach (JsonElement objElement in root.EnumerateArray())
                {
                    personElement = objElement.GetProperty("person");
                    idElement = personElement.GetProperty("id");
                    nameElement = personElement.GetProperty("name");
                    birthdayElement = personElement.GetProperty("birthday");
                    genderElement = personElement.GetProperty("gender");
                    _actors.Add(new Actor
                    {
                        Id = idElement.GetInt32(),
                        ActorName = nameElement.GetString(),
                        Birthday = birthdayElement.GetString(),
                        Gender = genderElement.GetString()
                    });
                }
            }
        }

        private int getCacheTtl()
        {
            string fileOutput;
            string[] fileOutputSplitted;
            int cacheTtl;
            using (StreamReader sr = new StreamReader(_cacheTimeFilePath))
            {
                fileOutput = sr.ReadToEnd();
            }
            fileOutputSplitted = fileOutput.Split("Cache ttl in seconds: ");
            try
            {
                cacheTtl = int.Parse(fileOutputSplitted[1]);
            }
            catch (FormatException)
            {
                Console.WriteLine("Cache ttl incorrect format. Setting to default 300 sec.");
                cacheTtl = 300;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Cache ttl incorrect format. Setting to default 300 sec.");
                cacheTtl = 300;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Cache ttl incorrect format. Setting to default 300 sec.");
                cacheTtl = 300;
            }
            return cacheTtl;
        }
    }
}
