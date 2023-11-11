using AmootSearch.Entities;
using AmootSearch.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace AmootSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {

        private static List<string> Remove = new()
        {
            ".","?",",","!",";",":","@","-","_","(",")","+","*","/","]","[","{","}","\\"
        };


        [HttpPost, Route("AddText")]
        public async Task<IActionResult> AddText(string text)
        {
            using var dc=new DataBaseContext(); 

            foreach (var item in Remove)
            {
                StringBuilder builder = new(text);

                builder.Replace(item, " ");

                text = builder.ToString();
            }

            var words = text.Split(' ').ToList();

            List<string> combinations = new();

            string numberPattern = @"\d+";
            string notNumberPattern= @"[^\d]+";
            string englishPattern = @"[a-zA-Z]+";
            string notEnglishPattern = @"[^a-zA-Z]+";



            foreach (var word in words)
            {
                bool containsNumber = Regex.IsMatch(word, numberPattern);
                bool containsEnglish = Regex.IsMatch(word, englishPattern);


                if (word.Length >= 2)
                {

                    var result = Regex.Replace(word, numberPattern, "");
                    result = Regex.Replace(result, englishPattern, "");

                    for (int i = 0; i < result.Length; i++)
                    {
                        for (int j = 2; j <= result.Length - i; j++)
                        {
                            string substring = result.Substring(i, j);

                            combinations.Add(substring);
                        }
                    }
                }
                if (containsEnglish)
                {
                    var result = Regex.Replace(word, notEnglishPattern, "");

                    for (int i = 0; i < result.Length; i++)
                    {
                        for (int j = 1; j <= result.Length - i; j++)
                        {
                            string substring = result.Substring(i, j);

                            combinations.Add(substring);
                        }
                    }
                }
                if (containsNumber)
                {
                    var result = Regex.Replace(word, notNumberPattern, "");

                    for (int i = 0; i < result.Length; i++)
                    {
                        for (int j = 1; j <= result.Length - i; j++)
                        {
                            string substring = result.Substring(i, j);

                            combinations.Add(substring);
                        }
                    }
                }
            }

            combinations = combinations.ToList();

            var distinctCombinations = combinations.Distinct().ToList();

            //برای تکراری وارد نشدن متن ی کاری بکن

            var textCount =await dc.Texts.CountAsync();

            var newText = new Text()
            {
                Content = text,
            };

            await dc.Texts.AddAsync(newText);

            await dc.SaveChangesAsync();

            foreach (var item in distinctCombinations)
            {
                var model = dc.Models.FirstOrDefault(a => a.Index == item);

                if (model != null)
                {
                    model.PrepareSubs();

                    model.TotalNumber++;
                    model.Subs.Add(new SubModel()
                    {
                        TextId = newText.Id,
                        TF = ((double)combinations.Count(a => a == item) / ((double)combinations.Count))
                    });
                    model.Info = JsonConvert.SerializeObject(model.Subs);

                }
                else
                {

                    model = new Model()
                    {
                        Index = item,
                        TotalNumber = 1,
                        Subs = new List<SubModel>()
                        { 
                            new SubModel()
                            {
                                TextId = newText.Id,
                                TF = ((double)combinations.Count(a => a == item) / ((double)combinations.Count))
                            } 
                        }
                    };
                    model.Info = JsonConvert.SerializeObject(model.Subs);

                    await dc.Models.AddAsync(model);

                }

            }

            await dc.SaveChangesAsync();

            return Ok();
        }

        [HttpPost, Route("AddSynonym")]
        public async Task<IActionResult> AddSynonym([FromForm]string main, [FromForm] string equivalent)
        {
            using var dc = new DataBaseContext();

            if (await dc.Synonyms.AnyAsync(a=>(a.Main==main && a.Equivalent==equivalent) || (a.Equivalent==main && a.Main==equivalent)))
            {
                return Ok();
            }

            var synonym = new Synonym()
            {
                Main = main,
                Equivalent = equivalent
            };

            await dc.Synonyms.AddAsync(synonym);

            await dc.SaveChangesAsync();

            return Ok();

        }

        [HttpGet, Route("GetText")]
        public async Task<IActionResult> GetText([FromQuery]string text)
        {
            foreach (var item in Remove)
            {
                StringBuilder builder = new(text);

                builder.Replace(item, " ");

                text = builder.ToString();
            }

            var words = text.Split(' ').ToList();

            List<string> combinations = new();

            List<string> importantcombinations = new();

            string numberPattern = @"\d+";
            string notNumberPattern = @"[^\d]+";
            string englishPattern = @"[a-zA-Z]+";
            string notEnglishPattern = @"[^a-zA-Z]+";

            foreach (var word in words)
            {
                bool containsNumber = Regex.IsMatch(word, numberPattern);
                bool containsEnglish = Regex.IsMatch(word, englishPattern);


                if (word.Length >= 2)
                {
                    var result = Regex.Replace(word, numberPattern, "");

                    importantcombinations.Add(result);

                    for (int i = 0; i < result.Length; i++)
                    {
                        for (int j = 2; j <= result.Length - i; j++)
                        {
                            string substring = result.Substring(i, j);

                            combinations.Add(substring);
                        }
                    }
                }
                if (containsEnglish)
                {
                    var result = Regex.Replace(word, notEnglishPattern, "");
                    importantcombinations.Add(result);

                    for (int i = 0; i < result.Length; i++)
                    {
                        for (int j = 1; j <= result.Length - i; j++)
                        {
                            string substring = result.Substring(i, j);

                            combinations.Add(substring);
                            importantcombinations.Add(substring);

                        }
                    }
                }
                if (containsNumber)
                {
                    var result = Regex.Replace(word, notNumberPattern, "");

                    importantcombinations.Add(result);

                    for (int i = 0; i < result.Length; i++)
                    {
                        for (int j = 1; j <= result.Length - i; j++)
                        {
                            string substring = result.Substring(i, j);

                            combinations.Add(substring);
                            importantcombinations.Add(substring);

                        }
                    }
                }
            }
            using var dc = new DataBaseContext();

            importantcombinations.AddRange(combinations);

            var synonyms =await dc.Synonyms.Where(a=>importantcombinations.Contains(a.Main) || importantcombinations.Contains(a.Equivalent)).ToListAsync();

            var synonymWords=synonyms.Select(a=>a.Main).ToList();

            synonymWords.AddRange(synonyms.Select(a => a.Equivalent).ToList());

            synonymWords=synonymWords.Distinct().ToList();

            importantcombinations.AddRange(synonymWords);

            importantcombinations=importantcombinations.Distinct().ToList();

            var models = new List<Model>();

            var modelCount = await dc.Models.CountAsync();

            var maxTotalNumber = await dc.Models.MaxAsync(a => a.TotalNumber);

            var constant = (modelCount / maxTotalNumber);

            var views = new List<View>();

            foreach (var item in importantcombinations)
            {
                var model = await dc.Models.FirstOrDefaultAsync(a => a.Index == item);

                if (model != null)
                {
                    model.PrepareSubs();

                    var totalCount = model.TotalNumber;

                    var value = ((double)modelCount /( (double)totalCount* constant));

                    if (value > 2)
                    {
                        var log = Math.Log(value);
                        
                        foreach (var sub in model.Subs)
                        {
                            views.Add(new View()
                            {
                                TextId = sub.TextId,
                                TFIDF = sub.TF *log,
                                Length= item.Length
                            });
                        }
                    }
                }

            }


            var textIds=views.Select(a=>a.TextId).Distinct().ToList();

            var texts = await dc.Texts.Where(a => textIds.Contains(a.Id)).ToListAsync();


            foreach (var item in texts)
            {
                var thisViews = views.Where(a => a.TextId == item.Id).ToList();
                //item.Score = thisViews.Sum(a => a.TFIDF) +
                //             thisViews.Count() +
                //             thisViews.Sum(a => a.Length);

                item.Score = thisViews.Sum(a => a.TFIDF);
            }

            return Ok(texts.OrderByDescending(a=>a.Score).ToList());
        }
    }
}




public class View
{
    public long TextId { get; set; }
    public int Length { get; set; }
    public double TFIDF { get; set; }
}