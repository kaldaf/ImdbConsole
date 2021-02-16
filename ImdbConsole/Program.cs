using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ImdbConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new ImdbContext();
            db.Database.EnsureCreated();

            var root = new MenuItem("Hlavní menu");
            var manage = new Configuration();
            var movie = new MovieManage();
            var actor = new ActorManage();

            var current = root;

            #region Movie
            var moviesMain = new MenuItem("Filmy");
            var addMovie = new MenuItem("Přidat film");
            var addMovieOmdb = new MenuItem("Přidat film z OMDb");
            var listMovie = new MenuItem("Seznam filmů");
            var informationMovie = new MenuItem("Informace o filmu");
            var openLink = new MenuItem("Otevřít odkaz");

            var addGenre = new MenuItem("Přidat žánr");
            var moreNames = new MenuItem("Přidat více jmen");


            var manageMovie = new MenuItem("Spravovat film");
            var changeName = new MenuItem("Změnit název");
            var changeDescription = new MenuItem("Změnit popis");
            var changeRating = new MenuItem("Změnit hodnocení");
            var changeLink = new MenuItem("Změnit odkaz");
            var deleteMovie = new MenuItem("Smazat film");


            root.Items.Add(moviesMain);
            moviesMain.Items.Add(addMovie);
            moviesMain.Items.Add(addMovieOmdb);
            moviesMain.Items.Add(listMovie);
            moviesMain.Items.Add(informationMovie);
            moviesMain.Items.Add(openLink);
            moviesMain.Items.Add(addGenre);
            moviesMain.Items.Add(moreNames);



            moviesMain.Items.Add(manageMovie);
            manageMovie.Items.Add(changeName);
            manageMovie.Items.Add(changeDescription);
            manageMovie.Items.Add(changeRating);
            manageMovie.Items.Add(changeLink);
            manageMovie.Items.Add(deleteMovie);

            #endregion

            #region Actor
            var actorMain = new MenuItem("Herci");
            var addActor = new MenuItem("Přidat herce");
            var listActors = new MenuItem("Seznam Herců");
            var assignActor = new MenuItem("Přiřadit herce k filmu");
            var removeActor = new MenuItem("Odebrat herce z filmu");
            var deleteActor = new MenuItem("Smazat herce");
            var listActorMovies = new MenuItem("listActorMovies");
            root.Items.Add(actorMain);
            actorMain.Items.Add(addActor);
            actorMain.Items.Add(listActors);
            actorMain.Items.Add(assignActor);
            actorMain.Items.Add(removeActor);
            actorMain.Items.Add(listActorMovies);
            actorMain.Items.Add(deleteActor);

            #endregion

            var userMain = new MenuItem("Uživatelé");

            while (true)
            {
                current.Draw();
                var key = Console.ReadKey(true);
                //int i = 0;
                //int b=i++; // int b=i; i=i+1;
                //int c=++i; // i=i+1; int c=i;
                switch (key.Key)
                {
                    #region EventHandle



                    case ConsoleKey.UpArrow:
                        if (current.SelectedItemIndex - 1 >= 0)
                            current.SelectedItemIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (current.SelectedItemIndex + 1 < current.Items.Count)
                            current.SelectedItemIndex++;
                        break;

                    case ConsoleKey.Enter:
                        var selected = current.Items[current.SelectedItemIndex];
                        #endregion

                        #region Movie
                        if (selected == addMovie)
                        {
                            movie.AddMovie();
                            Console.ReadLine();
                        }

                        else if (selected == listMovie)
                        {
                            movie.listMovie();
                            Console.ReadLine();
                        }
                        else if (selected == informationMovie)
                        {
                            movie.informationMovie();
                            Console.ReadLine();
                        }
                        else if (selected == openLink)
                        {
                            movie.openLink();
                            Console.ReadLine();

                        }
                        else if (selected == deleteMovie)
                        {
                            manage.deleteMovie();
                            Console.ReadLine();
                        }
                        else if (selected == changeName)
                        {
                            manage.changeMovieName();
                            Console.ReadLine();
                        }
                        else if (selected == moreNames)
                        {
                            movie.addMoreNames();
                            Console.ReadLine();
                        }
                        else if (selected == addMovieOmdb)
                        {
                            movie.omdb();
                            Console.ReadLine();
                        }
                        else if (selected == addGenre)
                        {
                            movie.addGenres();
                            Console.ReadLine();
                        }
                        else if (selected == changeDescription)
                        {
                            manage.changeDescription();
                            Console.ReadLine();
                        }
                        else if (selected == changeLink)
                        {
                            manage.changeLink();
                            Console.ReadLine();
                        }
                        else if (selected == changeRating)
                        {
                            manage.changeRating();
                            Console.ReadLine();
                        }
                        #endregion

                        #region Actor
                        else if (selected == addActor)
                        {
                            actor.AddActor();
                            Console.ReadLine();
                        }
                        else if (selected == listActors)
                        {
                            actor.listActors();
                            Console.ReadLine();
                        }
                        else if (selected == assignActor)
                        {
                            actor.AssignActor();
                            Console.ReadLine();
                        }
                        else if (selected == removeActor)
                        {
                            actor.removeActor();
                            Console.ReadLine();
                        }
                        else if (selected == deleteActor)
                        {
                            actor.deleteActor();
                            Console.ReadLine();
                        }
                        else if (selected == listActorMovies)
                        {
                            actor.listActorMovies();
                            Console.ReadLine();
                        }
                        #endregion
                        else
                        {
                            current = selected;
                        }
                        break;
                    #region EvenHandle



                    case ConsoleKey.Escape:
                    case ConsoleKey.Backspace:
                        current = root;
                        break;
                        #endregion
                }
            }
        }
    }
}

class MovieManage
{
    Actor _actor;
    Movie _movie;
    ImdbContext _context = new ImdbContext();

    internal void AddMovie()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();
        Console.WriteLine("\r\nNapiš název filmu");
        string jmenoFilmu = Console.ReadLine();

        if (jmenoFilmu != "")
        {
            Console.WriteLine("\r\nHodnocení filmu 0-100b");
            string ratingMovie = Console.ReadLine();
            int result;
            if (int.TryParse(ratingMovie, out result) || ratingMovie == "")
            {
                if (Int32.Parse(ratingMovie) >= 0 && Int32.Parse(ratingMovie) <= 100 || ratingMovie == "")
                {
                    Console.WriteLine("\r\nPopis filmu");
                    string descriptionMovie = Console.ReadLine();
                    Console.WriteLine("\r\nOdkaz na film");
                    string urlMovie = Console.ReadLine();

                    db.Movies.Add(new Movie()
                    {
                        Name = jmenoFilmu,
                        Description = descriptionMovie,
                        Rating = Int32.Parse(ratingMovie),
                        Link = urlMovie,
                    });

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\r\nGratuluji přidal jsi film jménem: " + jmenoFilmu);
                    Console.ResetColor();

                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Jejda chyba, zkus to znovu.");
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Jejda chyba, zkus to znovu.");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("Jejda chyba, zkus to znovu.");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }

    }
    internal void listMovie()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var list in db.Movies)
        {
            Console.WriteLine("ID: " + list.Id + " " + list.Name);
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\r\nToto jsou všechny filmy co evidujeme.");
        Console.ResetColor();

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    internal void informationMovie()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();


        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }

        Console.WriteLine("Napiš id filmu");
        string idMovie = Console.ReadLine();
        var informationsList = db.Movies.Include(x => x.Actors).ThenInclude(x => x.Actor).Include(x => x.MovieNames).Include(x => x.MovieGenres).FirstOrDefault(x => x.Id == int.Parse(idMovie));
        if (informationsList != null)
        {
            Console.WriteLine("\r\nNázev: " + informationsList.Name);
            Console.WriteLine("Popis: " + informationsList.Description);
            Console.WriteLine("Hodnocení: " + informationsList.Rating + "%");
            Console.WriteLine("Odkaz: " + informationsList.Link);
            Console.WriteLine("Seznam herců:");
            if (informationsList.Actors.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tento film neobsahuje žádné herce");
                Console.ResetColor();
            }
            else
            {
                int i = 0;
                foreach (var actor in informationsList.Actors)
                {
                    i++;
                    Console.WriteLine(i + ". " + actor.Actor.Name);
                }
            }
            Console.WriteLine("Seznam alternativních názvů:");
            if (informationsList.MovieNames.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tento film neobsahuje žádné jiné jména");
                Console.ResetColor();
            }
            else
            {
                int i = 0;
                foreach (var name in informationsList.MovieNames)
                {
                    i++;
                    Console.WriteLine(i + ". " + name.AnotherName);
                }
            }
            Console.WriteLine("Seznam žánrů:");
            if (informationsList.MovieGenres.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tento film nemá určený žánr");
                Console.ResetColor();
            }
            else
            {
                int i = 0;
                foreach (var genre in informationsList.MovieGenres)
                {
                    i++;
                    Console.WriteLine(i + ". " + genre.Gendre);
                }
            }
        }
        else
        {
            Console.WriteLine("Promiň, ale takový film neevidujeme.");
        }

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("\r\nSTISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    internal void openLink()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }


        Console.WriteLine("Napiš id filmu");
        string idMovie = Console.ReadLine();


        var movieDb = db.Movies.FirstOrDefault(x => x.Id == int.Parse(idMovie));


        if (movieDb != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Odkaz filmu: " + movieDb.Name + " byl úspěšně otevřen." + "\n\r" + movieDb.Link);
            Console.ResetColor();
            var psi = new ProcessStartInfo("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe");
            psi.Arguments = $"{movieDb.Link}";
            Process.Start(psi);
        }
        else
        {
            Console.WriteLine("Promiň, ale takový film neevidujeme.");
        }
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("\r\nSTISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    internal void addMoreNames()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();


        foreach (var list in db.Movies)
        {
            Console.WriteLine("Id " + list.Id + " " + list.Name);
        }

        Console.WriteLine("Vyber si film ke kterému chceš zapsat více jmen");
        string movieId = Console.ReadLine();
        var movieDb = db.Movies.FirstOrDefault(x => x.Id == int.Parse(movieId));
        if (movieDb != null)
        {
            Console.WriteLine("Napiš alternativní název");
            string alternativeName = Console.ReadLine();
            if (alternativeName != "")
            {
                db.Movies.Include(x => x.MovieNames).FirstOrDefault(x => x.Id == int.Parse(movieId)).MovieNames.Add(new MovieName()
                {
                    MovieId = int.Parse(movieId),
                    AnotherName = alternativeName,
                });
                db.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Gratuluji, přidal alternativní název :" + alternativeName + " k filmu : " + movieDb.Name);
                Console.ResetColor();

            }
            else
            {
                Console.WriteLine("Prosím zadej id filmu.");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                Console.ResetColor();
            }

        }
        else
        {
            Console.WriteLine("Omlouváme se, ale tento film neevidujeme.");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }
    }
    internal void addGenres()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();


        foreach (var list in db.Movies)
        {
            Console.WriteLine("Id " + list.Id + " " + list.Name);
        }

        Console.WriteLine("Vyber si film ke kterému chceš nový žánr");
        string movieId = Console.ReadLine();
        var movieDb = db.Movies.FirstOrDefault(x => x.Id == int.Parse(movieId));
        if (movieDb != null)
        {
            Console.WriteLine("Napiš nový žánr");
            string genreName = Console.ReadLine();
            if (genreName != "")
            {
                db.Movies.Include(x => x.MovieGenres).FirstOrDefault(x => x.Id == int.Parse(movieId)).MovieGenres.Add(new MovieGenre()
                {
                    MovieId = int.Parse(movieId),
                    Gendre = genreName,
                });
                db.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Gratuluji, přidal jsi zánr :" + genreName + " k filmu : " + movieDb.Name);
                Console.ResetColor();

            }
            else
            {
                Console.WriteLine("Prosím zadej id filmu.");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                Console.ResetColor();
            }

        }
        else
        {
            Console.WriteLine("Omlouváme se, ale tento film neevidujeme.");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }
    }
    internal async Task omdb()
    {
        //http://www.omdbapi.com/?t=james+bond?&apikey=9553bb93

        var apiKey = "?&apikey=9553bb93";
        Console.WriteLine("Napiš co potřebuješ načíst za film");
        string inputMovie = Console.ReadLine();

        string html = string.Empty;
        string url = @"http://www.omdbapi.com/?t=" + inputMovie + "?&apikey=9553bb93";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.AutomaticDecompression = DecompressionMethods.GZip;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            html = reader.ReadToEnd();
        }
        DataOmdb omdb = JsonConvert.DeserializeObject<DataOmdb>(html);

        if (omdb.Title != null)
        {


            string actors = omdb.Actors;
            Console.WriteLine(omdb.Title);
            Console.WriteLine(omdb.Plot);
            Console.WriteLine(omdb.Genre);
            Console.WriteLine(omdb.imdbRating);
            Console.WriteLine(omdb.Actors);
            Console.WriteLine(omdb.Website);

            Console.WriteLine("\r\nChceš přidat tento film do db? y/N");
            string action = Console.ReadLine();

            if (action == "y" || action == "Y")
            {
                Console.WriteLine("");
                var db = new ImdbContext();
                db.Database.EnsureCreated();

                //pridat film

                var addMovie = db.Movies.Add(new Movie()
                {
                    Name = omdb.Title,
                    Description = omdb.Plot,
                    Rating = Int32.Parse(omdb.imdbRating.Replace(".", "")),
                    Link = omdb.Website,
                });

                db.SaveChanges();

                //pridat herce
                String[] pattern = { ", " };
                String[] strlist = actors.Split(pattern, actors.Length, StringSplitOptions.RemoveEmptyEntries);
                string[] a = strlist;
                foreach (String s in strlist)
                {
                    var addActor = db.Actors.Add(new Actor()
                    {
                        Name = s
                    });

                    db.SaveChanges();

                    //priradit herce

                    db.ActorMovie.Add(new ActorMovie()
                    {
                        ActorId = addActor.Entity.Id,
                        MovieId =  addMovie.Entity.Id
                    });

                    db.SaveChanges();


                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Úspěšně jsi přidal film" + omdb.Title + " do databáze.");
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                Console.ResetColor();
            }
            else if (action == "N" || action == "n")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("OK, VYBER JINÝ FILM - STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Ano / Ne - to je to tak těžký?");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("STISKNI ENTER A ZKUS TO ZNOVU");
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("Omlouváme se, ale takový film nenabízíme.");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }
    }
}

class ActorManage
{
    Actor _actor;
    Movie _movie;
    ImdbContext _context = new ImdbContext();

    internal void AddActor()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();
        Console.WriteLine("\r\nNapiš jméno Herce");
        string jmenoHerce = Console.ReadLine();
        if (jmenoHerce != "")
        {
            db.Actors.Add(new Actor()
            {
                Name = jmenoHerce
            });

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\r\nÚspěšně byl přidát herec/čka: " + jmenoHerce);
            Console.ResetColor();
            db.SaveChanges();
        }
        else
        {
            Console.WriteLine("\r\nJejda chyba, zkus to znovu.");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }
    }
    internal void listActors()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var list in db.Actors)
        {
            Console.WriteLine("ID: " + list.Id + " " + list.Name);
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\r\nToto jsou všichní herci/čky co evidujeme.");
        Console.ResetColor();

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    internal void AssignActor()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        if (db.Movies.ToList().Count != 0 || db.Actors.ToList().Count != 0)
        {
            foreach (var actor in db.Actors)
            {
                Console.WriteLine("ID: " + actor.Id + " " + actor.Name);
            }

            Console.WriteLine("Napiš id herce");
            string idActor = Console.ReadLine();
            var selectedActor = db.Actors.FirstOrDefault(x => x.Id == int.Parse(idActor));
            if (selectedActor != null)
            {
                foreach (var movie in db.Movies)
                {
                    Console.WriteLine("ID: " + movie.Id + " " + movie.Name);

                }

                Console.WriteLine("Napiš id filmu");
                string idMovie = Console.ReadLine();
                var selectedMovie = db.Movies.FirstOrDefault(x => x.Id == int.Parse(idMovie));


                var containtUser = db.ActorMovie.Any(x => x.ActorId == int.Parse(idActor) && x.MovieId == int.Parse(idMovie));
                if (selectedMovie != null && containtUser == false)
                {
                    var moviedb = db.Movies.Include(x => x.Actors).FirstOrDefault(x => x.Id == int.Parse(idMovie));
                    moviedb.Actors.Add(new ActorMovie() { ActorId = int.Parse(idActor) });
                    db.SaveChanges();


                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n\rÚspěšně jsi přiřadil herce: " + selectedActor.Name + " k filmu:" + selectedMovie.Name);
                    Console.ResetColor();

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\r\nTakový film neevidujeme nebo uživatel již existuje ve filmu");
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\nTakového herce neevidujeme");
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\nTabulka s herci nebo filmy jsou prázdné, naplň je prosím.");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }

    }
    internal void removeActor()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }

        Console.WriteLine("Napiš id filmu");
        string idMovie = Console.ReadLine();

        foreach (var actor in db.Actors)
        {
            Console.WriteLine("ID: " + actor.Id + " " + actor.Name);
        }
        Console.WriteLine("Napiš id herce");
        string idActor = Console.ReadLine();

        var containtUser = db.ActorMovie.Any(x => x.ActorId == int.Parse(idActor) && x.MovieId == int.Parse(idMovie));
        if (containtUser == true)
        {
            var movieDb = db.ActorMovie.FirstOrDefault(x => x.ActorId == int.Parse(idActor));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\nÚspěšně jsi odebral herce: " + movieDb.Actor.Name);
            Console.ResetColor();
            var dbRemove = db.ActorMovie.FirstOrDefault(x => x.ActorId == int.Parse(idActor) && x.MovieId == int.Parse(idMovie));

            db.ActorMovie.Remove(dbRemove);
            db.SaveChanges();

            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();

        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\nTento film neobsahuje tohoto herce nebo tento herec neexistuje.");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }
    }

    internal void deleteActor()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var actor in db.Actors)
        {
            Console.WriteLine("ID: " + actor.Id + " " + actor.Name);

        }

        Console.WriteLine("Napiš id herce");
        string idActor = Console.ReadLine();
        var actorDb = db.Actors.FirstOrDefault(x => x.Id == int.Parse(idActor));
        if (actorDb != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Úspěšně jsi smazal herce: " + actorDb.Name);
            Console.ResetColor();
            db.Actors.Remove(actorDb);
            db.SaveChanges();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\nZkus to znovu a lépe.");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }


    }

    internal void listActorMovies()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var actor in db.Actors)
        {
            Console.WriteLine("ID: " + actor.Id + " " + actor.Name);

        }

        Console.WriteLine("Napiš id herce");
        string idActor = Console.ReadLine();
        var n = _context.ActorMovie.Include(x=>x.Movie).Where(x => x.ActorId == int.Parse(idActor)).Select(x=>new {x.Movie.Name});
        if (n != null)
        {
            int i = 0;
            Console.WriteLine("Tento herec hrál ve filmu:");
            foreach (var moviePlay in n)
            {
                i++;
                Console.WriteLine(i + ". " +moviePlay.Name);
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\nTento herec nehrál v žádném filmu z databáze, jaká škoda..");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
            Console.ResetColor();
        }
    }
}
class Configuration
{

    Actor _actor;
    Movie _movie;
    ImdbContext _context = new ImdbContext();

    #region Movie
    internal void deleteMovie()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }

        Console.WriteLine("Napiš id filmu");
        string idMovie = Console.ReadLine();

        var movieDb = db.Movies.FirstOrDefault(x => x.Id == int.Parse(idMovie));
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Úspěšně jsi smazal film: " + movieDb.Name);
        Console.ResetColor();
        db.Movies.Remove(movieDb);
        db.SaveChanges();

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();

    }

    internal void changeMovieName()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }

        Console.WriteLine("\r\nNapiš id filmu u kterého chceš změnit jméno");
        string idName = Console.ReadLine();

        var movieDb = db.Movies.SingleOrDefault(x => x.Id == int.Parse(idName));

        Console.WriteLine("Napiš nový název: ");
        string newName = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Úspěšně jsi změnil název filmu : " + movieDb.Name + " na " + newName);
        Console.ResetColor();
        movieDb.Name = newName;
        db.SaveChanges();

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    internal void changeDescription()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }

        Console.WriteLine("\r\nNapiš id filmu u kterého chceš změnit popis");
        string idName = Console.ReadLine();

        var movieDb = db.Movies.SingleOrDefault(x => x.Id == int.Parse(idName));

        Console.WriteLine("Napiš nový popis: ");
        string newDescription = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Úspěšně jsi změnil popis filmu : " + movieDb.Description + " na " + newDescription);
        Console.ResetColor();
        movieDb.Description = newDescription;
        db.SaveChanges();

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    internal void changeLink()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }

        Console.WriteLine("\r\nNapiš id filmu u kterého chceš změnit link");
        string idName = Console.ReadLine();

        var movieDb = db.Movies.SingleOrDefault(x => x.Id == int.Parse(idName));

        Console.WriteLine("Napiš link: ");
        string newLink = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Úspěšně jsi změnil link filmu : " + movieDb.Description + " na " + newLink);
        Console.ResetColor();
        movieDb.Link = newLink;
        db.SaveChanges();

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    internal void changeRating()
    {
        var db = new ImdbContext();
        db.Database.EnsureCreated();

        foreach (var movie in db.Movies)
        {
            Console.WriteLine("ID: " + movie.Id + " " + movie.Name);
        }

        Console.WriteLine("\r\nNapiš id filmu u kterého chceš změnit hodnocení");
        string idName = Console.ReadLine();

        var movieDb = db.Movies.SingleOrDefault(x => x.Id == int.Parse(idName));

        Console.WriteLine("Napiš hodnocení 0-100b: ");
        string newRating = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Úspěšně jsi změnil hodnocení filmu : " + movieDb.Description + " na " + newRating);
        Console.ResetColor();
        movieDb.Rating = int.Parse(newRating);
        db.SaveChanges();

        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("STISKNI ENTER PRO DALŠÍ POUŽÍVÁNÍ PROGRAMU");
        Console.ResetColor();
    }
    #endregion

    #region Actor
    internal void changeActorName()
    {

    }
    #endregion

}

#region EFTables

public class ActorMovie
{
    public int ActorId { get; set; }
    public int MovieId { get; set; }
    public Actor Actor { get; set; }
    public Movie Movie { get; set; }
}
public class Actor
{
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<ActorMovie> Movies { get; set; }
}

//public class FavoriteMovie
//{
//    public int ActorId { get; set; }
//    public int MovieId { get; set; }
//    public int UserId { get; set; }
//    public Actor Actor { get; set; }
//    public Movie Movie { get; set; }
//    public virtual User Users { get; set; }
//}
public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Rating { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public virtual IList<ActorMovie> Actors { get; set; }
    public virtual IList<MovieName> MovieNames { get; set; }
    public virtual IList<MovieGenre> MovieGenres { get; set; }

}
public class MovieName
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string AnotherName { get; set; }
    public virtual Movie Movie { get; set; }
}
public class MovieGenre
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Gendre { get; set; }
    public virtual Movie Movie { get; set; }
}
//public class User
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public string Password { get; set; }
//    public virtual IList<FavoriteMovie> FavoriteMovie { get; set; }
//}
public class ImdbContext : DbContext
{
    public DbSet<ActorMovie> ActorMovie { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    //public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var nameDataSource = Environment.MachineName;
        optionsBuilder.UseSqlServer($"Data source={nameDataSource};Integrated security=True;Initial Catalog=ImdbHomework");
        //Environment.MachineName = DESKTOP-P4UE1O5
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActorMovie>().HasKey(ac => new { ac.ActorId, ac.MovieId });
        base.OnModelCreating(modelBuilder);
    }
}
#endregion

public class DataOmdb
{
    public string Title { get; set; }
    public string Actors { get; set; }
    public string Plot { get; set; }
    public string Website { get; set; }
    public string Genre { get; set; }

    public string imdbRating { get; set; }
}
#region Menu
class MenuItem
{
    public string Title { get; set; } = "Titulek";
    public List<MenuItem> Items { get; set; } = new List<MenuItem>();

    private int _selectedItemIndex;
    public int SelectedItemIndex
    {
        get { return _selectedItemIndex; }
        set
        {
            if (value < 0) throw new Exception("Vybraná položka menu nemůže být záporná");
            if (value >= Items.Count) throw new Exception("Vybraná položka menu musí být v rozsahu 0-počet položek");
            _selectedItemIndex = value;
        }
    }

    public MenuItem(string title)
    {
        this.Title = title;
    }

    public int Width
    {
        get
        {
            int width = Title.Length;
            for (int j = 0; j < Items.Count; j++)
            {
                if (width < Items[j].Title.Length)
                    width = Items[j].Title.Length;
            }
            return width;
        }
    }

    public void Draw()
    {
        Console.Clear();
        Console.OutputEncoding = Encoding.UTF8;

        var w = Width;

        var centerX = Console.WindowWidth / 2 - w / 2;

        Console.CursorTop = Console.WindowHeight / 2 - (Items.Count + 4) / 2;
        Console.CursorLeft = centerX - w / 2 - 1; //posunu kurzor na zacatek menicka
                                                  //Kreslim horni ohraniceni
        Console.Write("┌");
        for (int i = 0; i < w; i++)
        {
            Console.Write("─");
        }
        Console.WriteLine("┐");

        Console.CursorLeft = centerX - w / 2 - 1; //posunu kurzor na zacatek menicka
        Console.WriteLine($"│{Title.PadLeft(w)}│");
        Console.CursorLeft = centerX - w / 2 - 1; //posunu kurzor na zacatek menicka
                                                  //Kreslim prostredni caru
        Console.Write("├");
        for (int i = 0; i < w; i++)
        {
            Console.Write("─");
        }
        Console.WriteLine("┤");

        for (var index = 0; index < Items.Count; index++)
        {
            var menuItem = Items[index];
            Console.CursorLeft = centerX - w / 2 - 1; //posunu kurzor na zacatek menicka
            Console.Write($"│");
            if (index == _selectedItemIndex)
            {
                var tmp = Console.BackgroundColor;
                Console.BackgroundColor = Console.ForegroundColor;
                Console.ForegroundColor = tmp;
            }
            Console.Write($"{menuItem.Title.PadLeft(w)}");
            if (index == _selectedItemIndex)
            {
                var tmp = Console.BackgroundColor;
                Console.BackgroundColor = Console.ForegroundColor;
                Console.ForegroundColor = tmp;
            }
            Console.WriteLine("│");
        }

        Console.CursorLeft = centerX - w / 2 - 1; //posunu kurzor na zacatek menicka
                                                  //Kreslim spodni ohraniceni
        Console.Write("└");
        for (int i = 0; i < w; i++)
        {
            Console.Write("─");
        }
        Console.WriteLine("┘");



    }

}
#endregion
