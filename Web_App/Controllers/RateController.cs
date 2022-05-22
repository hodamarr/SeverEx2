using System;
using Microsoft.AspNetCore.Mvc;

namespace Web_App.Controllers
{
	public class RateController : Controller
	{
        private static List<Rate> rates = new List<Rate>();
        int mean;

        public RateController()
        {
            if (rates.Count == 0)
            {
                rates.Add(new Rate { ID = 1, Name = "ofek", Content = "good", Score = 5 });
                rates.Add(new Rate { ID = 2, Name = "hod", Content = "nice", Score = 3 });
            }
            mean = MakeMean();
        }

        private int MakeMean()
        {
            int m = 0;
            if (rates.Count == 0) { return m; }
            foreach (Rate r in rates)
            {
                m += r.Score;
            }
            return m;

        }

        public int getMean()
        {
            return mean;
        }

        public IActionResult Index()
        {
            return View(rates);
        }

        [HttpPost]
        public IActionResult AddReview(string name, int score, string content)
        {
            int count = rates.Max(i => i.ID) + 1;

            rates.Add(new Rate()
            {
                Content = content,
                Name = name,
                Score = score,
                ID = count
            });

            return Redirect("Index");

        }

        public IActionResult Watch(int id)
        {
            Rate r = rates.Find(i => i.ID == id);

            return View(r);

        }

        public IActionResult Edit(int id)
        {
            Rate r = rates.Find(i => i.ID == id);
            return View(r);
        }
        /// TODO
        [HttpPost]
        public IActionResult Edit(string name, int score, string content, int id)
        {
            Rate r = rates.Find(i => i.ID == id);
            r.Score = score;
            r.Name = name;
            r.Content = content;
            return Redirect("Index");
        }
    }
}

