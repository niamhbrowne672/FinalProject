
using System.Reflection;
using Bogus;
using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;

namespace FinalProject.Data.Services;
public static class Seeder
{
    // use this class to seed the database with dummy test data using an IUserService 
    public static void Seed(DatabaseContext ctx)
    {

        IUserService svc = new UserServiceDb(ctx);
        IPostService psvc = new PostServiceDb(ctx);
        IEventService esvc = new EventServiceDb(ctx);
        IGalleryService galleryService = new GalleryServiceDb(ctx);
        ICalendarService calendarService = new CalendarServiceDb(ctx);
        // seeder destroys and recreates the database - NOT to be called in production!!!
        svc.Initialise();

        SeedUsers(svc);
        SeedPosts(psvc);
        SeedEvents(esvc);
        SeedGalleryImages(galleryService);
        SeedCounties(calendarService);
        SeedCalendars(calendarService);
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

    // private static void SeedPosts(IPostService psvc)
    // {
    //     var faker = new Faker();
    //     // add some fake posts
    //     var userIdArray = svc.GetUsers().Data.Select(u => u.Id).ToArray();
    //     //add some fake posts
    //     for (int i = 1; i <= 200; i++)
    //     {
    //         var p = new Post
    //         {
    //             Title = faker.Lorem.Sentence(),
    //             Body = faker.Lorem.Paragraph(),
    //             UserId = faker.Random.ArrayElement([1, 2, 3]) //faker.Random.ArrayElement(userIdArray)
    //         };

    //         psvc.AddPost(p);
    //     }
    // }

    //data for posts
    private static void SeedPosts(IPostService psvc)
    {
        // var p1 = psvc.AddPost(new Post
        // {
        //     Title = "Hungarian Vizsla Meet-Up",
        //     Content = "Join us for a great event play with other vizslas",
        //     PostedOn = new DateTime(2024, 11, 20, 14, 0, 0),
        //     ImagePath = "/images/events/e1.jpg",
        //     CreatedBy = "Niamh Browne"
        // });

        // var p2 = psvc.AddPost(new Post
        // {
        //     Title = "Sample Title",
        //     Content = "Sample content",  // No need to set Body anymore
        //     CreatedAt = DateTime.Now,
        //     ModifiedAt = DateTime.Now,
        //     PostedOn = DateTime.Now,
        //     UserId = 1
        // });
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

        var r1 = esvc.CreateReview(e2.Id, "Niamh Browne", "I've been to this meet-up before and my dog Rex had the best time playing with all of his friends! Would highly recommend.", 5);
        var r2 = esvc.CreateReview(e2.Id, "John Doe", "Wonderful meet-up, the location was perfect and my dog made so many friends!", 5);
        var r3 = esvc.CreateReview(e2.Id, "Jane Smith", "This is always my favorite event to attend! Highly recommend to dog lovers.", 5);

        var r4 = esvc.CreateReview(e5.Id, "Alex Johnson", "Had a great time, the organizers were fantastic and the dogs were so well-behaved!", 5);
        var r5 = esvc.CreateReview(e5.Id, "Chris Brown", "Beautiful location, perfect for a dog meet-up!", 4);

        var r6 = esvc.CreateReview(e9.Id, "Emily White", "Lovely event, my dog absolutely loved playing in the open fields!", 5);
        var r7 = esvc.CreateReview(e9.Id, "Laura Green", "Had an amazing experience, can't wait for the next one!", 5);

        var r8 = esvc.CreateReview(e13.Id, "Michael Gold", "Serene forest park with lots of space for the dogs to run around.", 4);
        var r9 = esvc.CreateReview(e13.Id, "Sarah Black", "Highly recommend this meet-up, the location is just beautiful.", 5);
        var r10 = esvc.CreateReview(e13.Id, "Tom Blue", "Great event! My dog had the time of his life.", 5);

        var r11 = esvc.CreateReview(e16.Id, "Anna Grey", "Such a fantastic park for meet-ups, been here previously and it's always great!", 5);
        var r12 = esvc.CreateReview(e16.Id, "John Doe", "Perfect for dogs and owners alike, well-organized event.", 5);

        var r13 = esvc.CreateReview(e21.Id, "Alex Johnson", "A hidden gem for meet-ups, peaceful and spacious!", 4);
        var r14 = esvc.CreateReview(e21.Id, "Emily White", "Loved everything about this event, will definitely attend again.", 5);
        var r15 = esvc.CreateReview(e21.Id, "Chris Brown", "The dogs had so much fun, and so did I!", 5);

        var r16 = esvc.CreateReview(e25.Id, "Laura Green", "Beautiful location and great company, highly recommend!", 5);
        var r17 = esvc.CreateReview(e25.Id, "Sarah Black", "My dog loved the open spaces, great event overall!", 4);

        var r18 = esvc.CreateReview(e3.Id, "Niamh Browne", "I've been to this meet-up before and my dog Rex had the best time playing with all of his friends! Would highly recommend.", 5);
        var r19 = esvc.CreateReview(e3.Id, "John Doe", "Wonderful meet-up, the location was perfect and my dog made so many friends!", 5);
        var r20 = esvc.CreateReview(e3.Id, "Jane Smith", "This is always my favorite event to attend! Highly recommend to dog lovers.", 5);

        var r21 = esvc.CreateReview(e6.Id, "Alex Johnson", "Had a great time, the organizers were fantastic and the dogs were so well-behaved!", 5);
        var r22 = esvc.CreateReview(e6.Id, "Chris Brown", "Beautiful location, perfect for a dog meet-up!", 4);

        var r23 = esvc.CreateReview(e10.Id, "Emily White", "Lovely event, my dog absolutely loved playing in the open fields!", 5);
        var r24 = esvc.CreateReview(e10.Id, "Laura Green", "Had an amazing experience, can't wait for the next one!", 5);

        var r25 = esvc.CreateReview(e30.Id, "Michael Gold", "Serene forest park with lots of space for the dogs to run around.", 4);
        var r26 = esvc.CreateReview(e30.Id, "Sarah Black", "Highly recommend this meet-up, the location is just beautiful.", 5);
        var r27 = esvc.CreateReview(e30.Id, "Tom Blue", "Great event! My dog had the time of his life.", 5);

        var r28 = esvc.CreateReview(e26.Id, "Anna Grey", "Such a fantastic park for meet-ups, been here previously and it's always great!", 5);
        var r29 = esvc.CreateReview(e26.Id, "John Doe", "Perfect for dogs and owners alike, well-organized event.", 5);

        var r30 = esvc.CreateReview(e12.Id, "Alex Johnson", "A hidden gem for meet-ups, peaceful and spacious!", 4);
        var r31 = esvc.CreateReview(e12.Id, "Emily White", "Loved everything about this event, will definitely attend again.", 5);
        var r32 = esvc.CreateReview(e12.Id, "Chris Brown", "The dogs had so much fun, and so did I!", 5);

        var r33 = esvc.CreateReview(e1.Id, "Laura Green", "Beautiful location and great company, highly recommend!", 5);
        var r34 = esvc.CreateReview(e1.Id, "Sarah Black", "My dog loved the open spaces, great event overall!", 4);
    }

    private static void SeedGalleryImages(IGalleryService galleryService)
    {
        // var i1 = galleryService.AddImage(new PastEventImage
        // {
        //     ImageTitle = "Splash Time",
        //     ImageDescription = "Dogs playing in a shallow stream to beat the heat.",
        //     GalleryImageUrl = "~/images/events/i24",
        //     ImagePostedBy = "Niamh Browne"
        // });
    }

    private static void SeedCounties(ICalendarService calendarService)
{
    // Add counties in Ireland
    var counties = new List<County>
    {
        new County { Name = "Tyrone" },
        new County { Name = "Antrim" },
        new County { Name = "Armagh" },
        new County { Name = "Derry" },
        new County { Name = "Down" },
        new County { Name = "Fermanagh" },
        new County { Name = "Carlow" },
        new County { Name = "Cavan" },
        new County { Name = "Clare" },
        new County { Name = "Cork" },
        new County { Name = "Donegal" },
        new County { Name = "Dublin" },
        new County { Name = "Galway" },
        new County { Name = "Kerry" },
        new County { Name = "Kildare" },
        new County { Name = "Kilkenny" },
        new County { Name = "Laois" },
        new County { Name = "Leitrim" },
        new County { Name = "Limerick" },
        new County { Name = "Longford" },
        new County { Name = "Louth" },
        new County { Name = "Mayo" },
        new County { Name = "Meath" },
        new County { Name = "Monaghan" },
        new County { Name = "Offaly" },
        new County { Name = "Roscommon" },
        new County { Name = "Sligo" },
        new County { Name = "Tipperary" },
        new County { Name = "Waterford" },
        new County { Name = "Westmeath" },
        new County { Name = "Wexford" },
        new County { Name = "Wicklow" }
    };

    // Add counties to the service
    foreach (var county in counties)
    {
        calendarService.AddCounty(county);
    }
}


    private static void SeedCalendars(ICalendarService calendarService)
    {
        var calendar1 = new Calendar
        {
            Title = "Dog Walk in Belfast",
            Location = "Belfast",
            Start = new DateTime(2024, 11, 20, 10, 0, 0),
            End = new DateTime(2024, 11, 20, 12, 0, 0),
            CountyId = 1,
            UserId = 1
        };
        calendarService.AddCalendar(calendar1);
    }
}



