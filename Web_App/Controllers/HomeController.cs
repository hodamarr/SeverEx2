using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Web_App.Controllers;

public class HomeController : Controller
{
    private static List<Rate> rates = new List<Rate>();

    public HomeController()
    {
        rates.Add(new Rate { ID = 1, Name = "ofek", Content = "good", Score = "5", DateTime = DateTime.Now });
    }

    public IActionResult Index()
    {
        return View(rates);
    }

    [HttpPost]
    public IActionResult AddReview(string name, string score, string content, string button)
    {
        int count = rates.Max(i => i.ID) + 1;
        rates.Add(new Rate()
        {
            Content = content,
            Name = name,
            Score = score,
            ID = count,
            DateTime = DateTime.Now
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
    public IActionResult Edit(string name, string score, string content, int id)
    {
        Rate r = rates.Find(i => i.ID == id);
        r.Score = score;
        r.Name = name;
        r.Content = content;
        r.DateTime = DateTime.Now;
        return RedirectToAction("Index", r);
    }
}

