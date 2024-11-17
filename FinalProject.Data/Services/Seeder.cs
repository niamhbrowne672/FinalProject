
using Bogus;
using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Data.Services;
public static class Seeder
{
    // use this class to seed the database with dummy test data using an IUserService 
    public static void Seed(DatabaseContext ctx)
    {

        IUserService svc = new UserServiceDb(ctx);
        IPostService psvc = new PostServiceDb(ctx);
        IEventService esvc = new EventServiceDb(ctx);
        // seeder destroys and recreates the database - NOT to be called in production!!!
        svc.Initialise();

        SeedUsers(svc);
        SeedPosts(psvc, svc);
        SeedEvents(esvc);
    }

    // add users
    private static void SeedUsers(IUserService svc)
    {
        svc.AddUser("Administrator", "admin@mail.com", "admin", Role.admin);
        svc.AddUser("Manager", "manager@mail.com", "manager", Role.manager);
        svc.AddUser("Guest", "guest@mail.com", "guest", Role.guest);


        // // optionally add some fake users
        var faker = new Faker();
        for (int i = 1; i <= 100; i++)
        {
            var s = svc.AddUser(
                faker.Name.FullName(),
                faker.Internet.Email(),
                "password",
                Role.guest
            );
        }
    }

    private static void SeedPosts(IPostService psvc, IUserService svc)
    {
        var faker = new Faker();
        // add some fake posts
        var userIdArray = svc.GetUsers().Data.Select(u => u.Id).ToArray();
        //add some fake posts
        for (int i = 1; i <= 200; i++)
        {
            var p = new Post
            {
                Title = faker.Lorem.Sentence(),
                Body = faker.Lorem.Paragraph(),
                UserId = faker.Random.ArrayElement([1, 2, 3]) //faker.Random.ArrayElement(userIdArray)
            };

            psvc.AddPost(p);
        }
    }

    //data for events
    private static void SeedEvents(IEventService esvc)
    {
        var e1 = esvc.AddEvent(new Event
        {
            Title = "Hungarian Vizsla Meet-Up",
            EventTime = new DateTime(2024, 11, 20, 14, 0, 0),
            Location = "Belfast City Hall",
            Description = "Join us for a great event play with other vizslas",
            ImageUrl = "/images/events/e1.jpg"
        });

        var e2 = esvc.AddEvent(new Event
        {
            Title = "Golden Retriever Meet-Up",
            EventTime = new DateTime(2024, 12, 15, 11, 0, 0),
            Location = "Lagan Valley Regional Park, Lisburn",
            Description = "A festive walk for golden retrievers and their owners along the scenic Lagan Towpath.",
            ImageUrl = "~/images/events/e2.jpg"
        });

        var e3 = esvc.AddEvent(new Event
        {
            Title = "Dachshund Meet-Up",
            EventTime = new DateTime(2025, 3, 10, 13, 30, 0),
            Location = "The Gobbins Cliff Path, Larne",
            Description = "Explore the breathtaking coastal paths with fellow sausage dogs!",
            ImageUrl = "~/images/events/e3.jpg"
        });

        var e4 = esvc.AddEvent(new Event
        {
            Title = "Labrador Meet-Up",
            EventTime = new DateTime(2025, 6, 5, 10, 0, 0),
            Location = "Lough Neagh Discovery Centre, Craigavon",
            Description = "A morning of fun, games, and swimming for labradors at Lough Neagh.",
            ImageUrl = "~/images/events/e4.jpg"
        });

        var e5 = esvc.AddEvent(new Event
        {
            Title = "Shih Tzu Meet-Up",
            EventTime = new DateTime(2023, 9, 28, 15, 0, 0),
            Location = "Armagh Palace Demesne Park, Armagh City",
            Description = "A social afternoon in the park for Shih Tzus to mingle and owners to share tips.",
            ImageUrl = "~/images/events/e5.jpg"
        });

        var e6 = esvc.AddEvent(new Event
        {
            Title = "Cocker Spaniel Meet-Up",
            EventTime = new DateTime(2024, 10, 12, 12, 30, 0),
            Location = "Fota House Arboretum and Gardens, Cork",
            Description = "A lively meet-up for cocker spaniels to enjoy the open spaces of Fota Gardens.",
            ImageUrl = "~/images/events/e6.jpg"
        });

        var e7 = esvc.AddEvent(new Event
        {
            Title = "Pug Meet-Up",
            EventTime = new DateTime(2025, 1, 7, 14, 0, 0),
            Location = "Elizabeth Fort, Cork City",
            Description = "Celebrate the new year with a historic walk and social time for pugs and their owners.",
            ImageUrl = "~/images/events/e7.jpg"
        });
        var e8 = esvc.AddEvent(new Event
        {
            Title = "Border Collie Meet-Up",
            EventTime = new DateTime(2025, 9, 16, 10, 30, 0),
            Location = "Phoenix Park, Dublin",
            Description = "A morning of agility and fun activities for border collies in Ireland's largest park.",
            ImageUrl = "~/images/events/e8.jpg"
        });

        var e9 = esvc.AddEvent(new Event
        {
            Title = "French Bulldog Meet-Up",
            EventTime = new DateTime(2023, 8, 20, 13, 0, 0),
            Location = "Sandymount Strand, Dublin",
            Description = "A beachside social for French bulldogs with plenty of sand and sea adventures.",
            ImageUrl = "~/images/events/e9.jpg"
        });

        var e10 = esvc.AddEvent(new Event
        {
            Title = "German Shepherd Meet-Up",
            EventTime = new DateTime(2024, 5, 18, 11, 0, 0),
            Location = "Eyre Square, Galway City",
            Description = "A group walk and social in the heart of Galway for German shepherds.",
            ImageUrl = "~/images/events/e10.jpg"
        });

        var e11 = esvc.AddEvent(new Event
        {
            Title = "Beagle Meet-Up",
            EventTime = new DateTime(2025, 2, 23, 10, 0, 0),
            Location = "Connemara National Park, Galway",
            Description = "Take in the rugged beauty of Connemara with your beagle companions.",
            ImageUrl = "~/images/events/e11.jpg"
        });

        var e12 = esvc.AddEvent(new Event
        {
            Title = "Siberian Husky Meet-Up",
            EventTime = new DateTime(2025, 12, 3, 15, 0, 0),
            Location = "Killarney National Park, Kerry",
            Description = "Experience the magical winter landscapes with huskies and their owners.",
            ImageUrl = "~/images/events/e12.jpg"
        });

        var e13 = esvc.AddEvent(new Event
        {
            Title = "Jack Russell Meet-Up",
            EventTime = new DateTime(2023, 7, 14, 11, 30, 0),
            Location = "Inch Beach, Kerry",
            Description = "A fun-filled beach meet-up for Jack Russells to run and play freely.",
            ImageUrl = "~/images/events/e13.jpg"
        });

        var e14 = esvc.AddEvent(new Event
        {
            Title = "Yorkshire Terrier Meet-Up",
            EventTime = new DateTime(2024, 4, 20, 14, 0, 0),
            Location = "Gortin Glen Forest Park, Omagh",
            Description = "An exciting walk in the scenic forests for Yorkshire Terriers and their humans.",
            ImageUrl = "~/images/events/e14.jpg"
        });

        var e15 = esvc.AddEvent(new Event
        {
            Title = "Springer Spaniel Meet-Up",
            EventTime = new DateTime(2025, 7, 22, 10, 30, 0),
            Location = "The Ulster American Folk Park, Omagh",
            Description = "A morning of play and learning at this fascinating historical site.",
            ImageUrl = "~/images/events/e15.jpg"
        });

        var e16 = esvc.AddEvent(new Event
        {
            Title = "Shetland Sheepdog Meet-Up",
            EventTime = new DateTime(2023, 10, 25, 11, 0, 0),
            Location = "Roe Valley Country Park, Limavady",
            Description = "A fun-filled meet-up in the beautiful woodlands of Roe Valley for Shetland Sheepdogs.",
            ImageUrl = "~/images/events/e16.jpg"
        });

        var e17 = esvc.AddEvent(new Event
        {
            Title = "Poodle Meet-Up",
            EventTime = new DateTime(2025, 5, 1, 14, 30, 0),
            Location = "Guildhall Square, Derry City",
            Description = "A social gathering for all poodle varieties in the heart of Derry.",
            ImageUrl = "~/images/events/e17.jpg"
        });

        var e18 = esvc.AddEvent(new Event
        {
            Title = "West Highland Terrier Meet-Up",
            EventTime = new DateTime(2024, 8, 16, 13, 0, 0),
            Location = "Rossmore Forest Park, Monaghan Town",
            Description = "A group walk and social for Westies to explore the park together.",
            ImageUrl = "~/images/events/e18.jpg"
        });

        var e19 = esvc.AddEvent(new Event
        {
            Title = "Irish Setter Meet-Up",
            EventTime = new DateTime(2025, 10, 4, 11, 0, 0),
            Location = "Lake Muckno Forest Park, Castleblayney",
            Description = "A lively meet-up for Irish Setters to enjoy the lakeside trails.",
            ImageUrl = "~/images/events/e19.jpg"
        });


        var e20 = esvc.AddEvent(new Event
        {
            Title = "Boxer Meet-Up",
            EventTime = new DateTime(2024, 6, 11, 10, 30, 0),
            Location = "Enniskillen Castle, Enniskillen",
            Description = "Explore the historic castle grounds with your playful Boxer companions.",
            ImageUrl = "~/images/events/e20.jpg"
        });

        var e21 = esvc.AddEvent(new Event
        {
            Title = "Maltese Meet-Up",
            EventTime = new DateTime(2023, 5, 28, 14, 0, 0),
            Location = "Crom Estate, Newtownbutler",
            Description = "A serene lakeside meet-up for Maltese dogs and their owners.",
            ImageUrl = "~/images/events/e21.jpg"
        });


        var e22 = esvc.AddEvent(new Event
        {
            Title = "Corgi Meet-Up",
            EventTime = new DateTime(2025, 9, 30, 15, 0, 0),
            Location = "Glenveagh National Park, Letterkenny",
            Description = "Take in the rugged beauty of Glenveagh with a friendly group of Corgis.",
            ImageUrl = "~/images/events/e22.jpg"
        });

        var e23 = esvc.AddEvent(new Event
        {
            Title = "Greyhound Meet-Up",
            EventTime = new DateTime(2024, 12, 9, 11, 0, 0),
            Location = "Malin Head, Donegal",
            Description = "Stretch those legs with your Greyhound at Ireland's most northerly point.",
            ImageUrl = "~/images/events/e23.jpg"
        });


        var e24 = esvc.AddEvent(new Event
        {
            Title = "Dalmatian Meet-Up",
            EventTime = new DateTime(2025, 3, 15, 10, 0, 0),
            Location = "Botanic Gardens, Belfast",
            Description = "Spots galore! A lively meet-up for Dalmatian owners and their beautiful dogs.",
            ImageUrl = "~/images/events/e24.jpg"
        });

        var e25 = esvc.AddEvent(new Event
        {
            Title = "Husky Meet-Up",
            EventTime = new DateTime(2023, 11, 5, 13, 30, 0),
            Location = "Ormeau Park, Belfast",
            Description = "A fun afternoon for Huskies to play and socialize in the heart of the city.",
            ImageUrl = "~/images/events/e25.jpg"
        });


        var e26 = esvc.AddEvent(new Event
        {
            Title = "Chihuahua Meet-Up",
            EventTime = new DateTime(2024, 5, 18, 12, 0, 0),
            Location = "Wallace Park, Lisburn",
            Description = "A small but mighty meet-up for Chihuahuas and their owners to connect.",
            ImageUrl = "~/images/events/e26.jpg"
        });

        var e27 = esvc.AddEvent(new Event
        {
            Title = "Whippet Meet-Up",
            EventTime = new DateTime(2025, 7, 14, 10, 30, 0),
            Location = "Hillsborough Castle Gardens, Lisburn",
            Description = "A regal gathering for Whippets to enjoy the castle grounds.",
            ImageUrl = "~/images/events/e27.jpg"
        });


        var e28 = esvc.AddEvent(new Event
        {
            Title = "Terrier Meet-Up",
            EventTime = new DateTime(2024, 3, 12, 14, 0, 0),
            Location = "Cavan Burren Park, Blacklion",
            Description = "A dynamic meet-up for all terrier breeds to explore the Burren's unique landscape.",
            ImageUrl = "~/images/events/e28.jpg"
        });

        var e29 = esvc.AddEvent(new Event
        {
            Title = "Newfoundland Meet-Up",
            EventTime = new DateTime(2025, 11, 20, 13, 0, 0),
            Location = "Killykeen Forest Park, Cavan",
            Description = "A splash-filled meet-up for Newfoundlands to enjoy the park's lakeside trails.",
            ImageUrl = "~/images/events/e29.jpg"
        });


        var e30 = esvc.AddEvent(new Event
        {
            Title = "Bulldog Meet-Up",
            EventTime = new DateTime(2024, 8, 25, 10, 0, 0),
            Location = "Downhill Demesne, Coleraine",
            Description = "A coastal stroll for Bulldogs with stunning views of Mussenden Temple.",
            ImageUrl = "~/images/events/e30.jpg"
        });

        var e31 = esvc.AddEvent(new Event
        {
            Title = "Collie Meet-Up",
            EventTime = new DateTime(2025, 4, 8, 11, 30, 0),
            Location = "Mountsandel Forest, Coleraine",
            Description = "A woodland meet-up for Collies to run, play, and socialize.",
            ImageUrl = "~/images/events/e31.jpg"
        });
    }
}



