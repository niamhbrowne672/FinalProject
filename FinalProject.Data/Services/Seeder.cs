
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
        SeedComments(psvc);
        SeedEvents(esvc);
        SeedGalleryImages(galleryService);
        SeedCounties(calendarService);
        SeedCalendars(calendarService);
    }

    // add users
    private static void SeedUsers(IUserService svc)
    {
        svc.AddUser("Administrator", "admin@mail.com", "admin", Role.admin, "N/A", null);
        svc.AddUser("Organiser", "organiser@mail.com", "organiser", Role.manager, "N/A", null);
        svc.AddUser("Guest", "guest@mail.com", "guest", Role.guest, "Golden Retriever", null);


        // // optionally add some fake users
        var faker = new Faker();
        for (int i = 1; i <= 100; i++)
        {
            var s = svc.AddUser(
                faker.Name.FullName(),
                faker.Internet.Email(),
                "password",
                Role.guest,
                faker.Random.Word(),
                null //Default profile image
            );
        }
    }

    //data for posts
    private static void SeedPosts(IPostService psvc)
    {
        var post1 = psvc.AddPost(new Post
        {
            Title = "Welcome to the Community!",
            Content = "This is the first post in our community forum. Feel free to engage!",
            PostedOn = DateTime.Now,
            CreatedBy = "Woof & Wander",
            ImagePath = "/images/logo/2.jpg",
            UserId = 1
        });

        var post2 = psvc.AddPost(new Post
        {
            Title = "My dog won't eat his dinner!",
            Content = "My dog stopped eating his dinner about a week ago, eats snacks during the day. Is anyone able to give some tips and tricks that we could use to try and get him to eat his dinner again?",
            PostedOn = DateTime.Now,
            CreatedBy = "Niamh Browne",
            ImagePath = "/images/posts/post2.jpg",
            UserId = 2
        });

        var post3 = psvc.AddPost(new Post
        {
            Title = "Safe places to take my new puppy",
            Content = "We just got a new puppy, and we want to take him out to socialise. Does anyone know any safe and dog-friendly parks in Belfast?",
            PostedOn = DateTime.Now.AddDays(-3),
            CreatedBy = "John Smith",
            ImagePath = "/images/posts/post3.jpeg",
            UserId = 3
        });

        var post4 = psvc.AddPost(new Post
        {
            Title = "Best recipes for homemade dog treats",
            Content = "I'm looking for some easy and healthy homemade dog treat recipes. Any recommendations?",
            PostedOn = DateTime.Now.AddDays(-7),
            CreatedBy = "Emily White",
            ImagePath = "/images/posts/post4.jpg",
            UserId = 4
        });

        var post5 = psvc.AddPost(new Post
        {
            Title = "Advice needed: Rash on my dog's belly",
            Content = "My dog developed a red rash on his belly after our walk yesterday. Has anyone experienced something similar? Any suggestions on what to do?",
            PostedOn = DateTime.Now.AddDays(-2),
            CreatedBy = "Alex Johnson",
            ImagePath = "/images/posts/post5.jpeg",
            UserId = 5
        });

        var post6 = psvc.AddPost(new Post
        {
            Title = "How to clean my dog's teeth?",
            Content = "I'm new to dog ownership and wondering what the best methods or products are for cleaning my dog's teeth.",
            PostedOn = DateTime.Now.AddDays(-10),
            CreatedBy = "Sarah Black",
            ImagePath = "/images/posts/post6.jpg",
            UserId = 6
        });

        var post7 = psvc.AddPost(new Post
        {
            Title = "Dog-friendly cafes in Dublin?",
            Content = "I want to take my dog out to a cafe this weekend. Does anyone know any good dog-friendly cafes in Dublin?",
            PostedOn = DateTime.Now.AddDays(-5),
            CreatedBy = "Sheila McElroy",
            ImagePath = "/images/posts/post7.jpg",
            UserId = 7
        });

        var post8 = psvc.AddPost(new Post
        {
            Title = "Puppy training tips?",
            Content = "I have a 10-week-old Labrador puppy who is very energetic. Any tips for training or managing his energy levels?",
            PostedOn = DateTime.Now.AddDays(-1),
            CreatedBy = "Edel Browne",
            ImagePath = "/images/posts/post8.jpg",
            UserId = 8
        });

        var post9 = psvc.AddPost(new Post
        {
            Title = "Best places for dog swimming",
            Content = "Looking for lakes or beaches in Northern Ireland where dogs are allowed to swim. Any suggestions?",
            PostedOn = DateTime.Now.AddDays(-15),
            CreatedBy = "Anna Grey",
            ImagePath = "/images/posts/post9.jpg",
            UserId = 9
        });

        var post10 = psvc.AddPost(new Post
        {
            Title = "Best toys for destructive chewers",
            Content = "My German Shepherd chews through every toy I buy him. Can anyone recommend durable dog toys that will last?",
            PostedOn = DateTime.Now.AddDays(-20),
            CreatedBy = "Liam Carter",
            ImagePath = "/images/posts/post10.jpeg",
            UserId = 10
        });

        var post11 = psvc.AddPost(new Post
        {
            Title = "How to stop my dog from barking at the doorbell?",
            Content = "Every time the doorbell rings, my dog goes crazy barking. Any training advice to help with this?",
            PostedOn = DateTime.Now.AddDays(-9),
            CreatedBy = "Emily Adams",
            ImagePath = "/images/posts/post11.png",
            UserId = 11
        });

        var post12 = psvc.AddPost(new Post
        {
            Title = "Best dog harness for hiking?",
            Content = "Planning some hikes with my Border Collie. Looking for a reliable and comfortable harness for him.",
            PostedOn = DateTime.Now.AddDays(-12),
            CreatedBy = "Chris Walker",
            ImagePath = "/images/posts/post12.jpg",
            UserId = 12
        });

        var post13 = psvc.AddPost(new Post
        {
            Title = "Best food for senior dogs",
            Content = "My 10-year-old Golden Retriever has been slowing down lately. Any suggestions for the best senior dog food?",
            PostedOn = DateTime.Now.AddDays(-14),
            CreatedBy = "Sophie Brown",
            ImagePath = "/images/posts/post13.jpg",
            UserId = 13
        });

        var post14 = psvc.AddPost(new Post
        {
            Title = "Dealing with separation anxiety",
            Content = "My dog whines and barks every time I leave the house. How do I help him feel more comfortable being alone?",
            PostedOn = DateTime.Now.AddDays(-18),
            CreatedBy = "Jack Evans",
            ImagePath = "/images/posts/post14.png",
            UserId = 14
        });

        var post15 = psvc.AddPost(new Post
        {
            Title = "How to introduce a new dog to my cat?",
            Content = "We just adopted a rescue dog, but our cat is very nervous around him. Any tips on how to introduce them safely?",
            PostedOn = DateTime.Now.AddDays(-4),
            CreatedBy = "Hannah Lee",
            ImagePath = "/images/posts/post15.png",
            UserId = 15
        });

        var post16 = psvc.AddPost(new Post
        {
            Title = "Best travel carriers for small dogs",
            Content = "We're planning a trip and need a comfortable and airline-approved travel carrier for our Chihuahua. Any suggestions?",
            PostedOn = DateTime.Now.AddDays(-8),
            CreatedBy = "Jane Miller",
            ImagePath = "/images/posts/post16.jpeg",
            UserId = 16
        });

        var post17 = psvc.AddPost(new Post
        {
            Title = "Dog-friendly camping sites",
            Content = "Does anyone know of good camping sites in Ireland where dogs are allowed?",
            PostedOn = DateTime.Now.AddDays(-11),
            CreatedBy = "Laura White",
            ImagePath = "/images/posts/post17.jpg",
            UserId = 17
        });

        var post18 = psvc.AddPost(new Post
        {
            Title = "Fleas! Help needed!",
            Content = "Just discovered my dog has fleas. What's the best treatment to get rid of them quickly?",
            PostedOn = DateTime.Now.AddDays(-3),
            CreatedBy = "Paul Jones",
            ImagePath = "/images/posts/post18.jpg",
            UserId = 18
        });

        var post19 = psvc.AddPost(new Post
        {
            Title = "Are there any doggy daycare recommendations?",
            Content = "Looking for a good doggy daycare in the Dublin area for my energetic Labrador. Any recommendations?",
            PostedOn = DateTime.Now.AddDays(-6),
            CreatedBy = "Emma Taylor",
            ImagePath = "/images/posts/post19.jpg",
            UserId = 19
        });

        var post20 = psvc.AddPost(new Post
        {
            Title = "Best winter coats for small dogs",
            Content = "Winter is coming, and my small dog gets cold easily. Can anyone recommend a good dog coat for the cold weather?",
            PostedOn = DateTime.Now.AddDays(-7),
            CreatedBy = "Olivia Harris",
            ImagePath = "/images/posts/post20.jpg",
            UserId = 20
        });

        var post21 = psvc.AddPost(new Post
        {
            Title = "Dog-friendly beaches in Ireland?",
            Content = "Looking for nice beaches in Ireland where dogs are welcome. Any hidden gems?",
            PostedOn = DateTime.Now.AddDays(-5),
            CreatedBy = "Noah Thompson",
            ImagePath = "/images/posts/post21.jpg",
            UserId = 21
        });

        var post22 = psvc.AddPost(new Post
        {
            Title = "How to stop my dog from jumping on guests?",
            Content = "My dog loves people and always jumps on guests when they arrive. How do I train him to stop?",
            PostedOn = DateTime.Now.AddDays(-13),
            CreatedBy = "Ava Davis",
            ImagePath = "/images/posts/post22.jpg",
            UserId = 22
        });

        var post23 = psvc.AddPost(new Post
        {
            Title = "Dog agility classes near Belfast?",
            Content = "Does anyone know of any good agility training classes for dogs in the Belfast area?",
            PostedOn = DateTime.Now.AddDays(-19),
            CreatedBy = "Isabella Moore",
            ImagePath = "/images/posts/post23.jpg",
            UserId = 23
        });

        var post24 = psvc.AddPost(new Post
        {
            Title = "How to help my dog lose weight?",
            Content = "My vet says my dog is overweight. What are some good ways to help him lose weight safely?",
            PostedOn = DateTime.Now.AddDays(-14),
            CreatedBy = "Mason Wilson",
            ImagePath = "/images/posts/post24.jpg",
            UserId = 24
        });

        var post25 = psvc.AddPost(new Post
        {
            Title = "Is raw food diet safe for dogs?",
            Content = "I've been reading about raw food diets for dogs. Does anyone have experience with this? Pros and cons?",
            PostedOn = DateTime.Now.AddDays(-17),
            CreatedBy = "Lucas Hall",
            ImagePath = "/images/posts/post25.jpg",
            UserId = 25
        });
    }

    private static void SeedComments(IPostService psvc)
    {
        var comment1 = psvc.CreateComment(new Comment
        {
            PostId = 2,
            Comments = "Try adding a little warm chicjen broth to your dog's dinner. It worked for mine when he stopped eating!",
            CreatedBy = "Kevin Coyle",
            CreatedAt = DateTime.Now
        });

        var comment2 = psvc.CreateComment(new Comment
        {
            PostId = 3,
            Comments = "Botanic Gardens in Belfast is great for puppies! It's spacious and safe.",
            CreatedBy = "Liam Carter",
            CreatedAt = DateTime.Now.AddHours(-5)
        });

        var comment3 = psvc.CreateComment(new Comment
        {
            PostId = 4,
            Comments = "I recommend mixing peanut butter, oats, and a little honey for easy no-bake dog treats!",
            CreatedBy = "Emily Brown",
            CreatedAt = DateTime.Now.AddHours(-3)
        });

        var comment4 = psvc.CreateComment(new Comment
        {
            PostId = 5,
            Comments = "It could be an allergic reaction. Try wiping the area with a damp cloth and applying some coconut oil.",
            CreatedBy = "Paul Walker",
            CreatedAt = DateTime.Now.AddHours(-1)
        });

        var comment5 = psvc.CreateComment(new Comment
        {
            PostId = 6,
            Comments = "You can use a dog toothbrush and toothpaste. Start slow and reward your dog with treats.",
            CreatedBy = "Sophie Adams",
            CreatedAt = DateTime.Now.AddMinutes(-30)
        });

        var comment6 = psvc.CreateComment(new Comment
        {
            PostId = 7,
            Comments = "Check out 'The Barking Dog' in Dublin. It's super dog-friendly and has a great menu!",
            CreatedBy = "Hannah Lee",
            CreatedAt = DateTime.Now
        });

        var comment7 = psvc.CreateComment(new Comment
        {
            PostId = 8,
            Comments = "Consistency is key! Short, regular training sessions work wonders. Reward with treats and praise.",
            CreatedBy = "Sam Evans",
            CreatedAt = DateTime.Now
        });
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
            EventTime = new DateTime(2024, 6, 5, 10, 0, 0),
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

        var e32 = esvc.AddEvent(new Event
        {
            Title = "Puppy Training",
            EventTime = new DateTime(2024, 12, 08, 14, 0, 0),
            Location = "Belfast City Hall",
            Description = "Join us for a great event play with other vizslas",
            ImageUrl = "/images/events/e1.jpg"
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

        var r18 = esvc.CreateReview(e4.Id, "Niamh Browne", "I've been to this meet-up before and my dog Rex had the best time playing with all of his friends! Would highly recommend.", 5);
        var r19 = esvc.CreateReview(e4.Id, "John Doe", "Wonderful meet-up, the location was perfect and my dog made so many friends!", 5);
        var r20 = esvc.CreateReview(e4.Id, "Jane Smith", "This is always my favorite event to attend! Highly recommend to dog lovers.", 5);

        var r21 = esvc.CreateReview(e6.Id, "Alex Johnson", "Had a great time, the organizers were fantastic and the dogs were so well-behaved!", 5);
        var r22 = esvc.CreateReview(e6.Id, "Chris Brown", "Beautiful location, perfect for a dog meet-up!", 4);

        var r23 = esvc.CreateReview(e10.Id, "Emily White", "Lovely event, my dog absolutely loved playing in the open fields!", 5);
        var r24 = esvc.CreateReview(e10.Id, "Laura Green", "Had an amazing experience, can't wait for the next one!", 5);

        var r25 = esvc.CreateReview(e30.Id, "Michael Gold", "Serene forest park with lots of space for the dogs to run around.", 4);
        var r26 = esvc.CreateReview(e30.Id, "Sarah Black", "Highly recommend this meet-up, the location is just beautiful.", 5);
        var r27 = esvc.CreateReview(e30.Id, "Tom Blue", "Great event! My dog had the time of his life.", 5);

        var r28 = esvc.CreateReview(e26.Id, "Anna Grey", "Such a fantastic park for meet-ups, been here previously and it's always great!", 5);
        var r29 = esvc.CreateReview(e26.Id, "John Doe", "Perfect for dogs and owners alike, well-organized event.", 5);

        var r30 = esvc.CreateReview(e14.Id, "Alex Johnson", "A hidden gem for meet-ups, peaceful and spacious!", 4);
        var r31 = esvc.CreateReview(e14.Id, "Emily White", "Loved everything about this event, will definitely attend again.", 5);
        var r32 = esvc.CreateReview(e14.Id, "Chris Brown", "The dogs had so much fun, and so did I!", 5);

        var r33 = esvc.CreateReview(e1.Id, "Laura Green", "Beautiful location and great company, highly recommend!", 5);
        var r34 = esvc.CreateReview(e1.Id, "Sarah Black", "My dog loved the open spaces, great event overall!", 4);
    }

    private static void SeedGalleryImages(IGalleryService galleryService)
    {
        var images = new List<PastEventImage>
        {
            new PastEventImage
            {
                ImageTitle = "Hungarian Vizsla Meetup Highlights",
                ImageDescription = "Playful Vizslas enjoying the open spaces at Belfast City Hall.",
                GalleryImageUrls = new List<string>
                {
                    "/images/gallery/vizsla1.jpg",
                    "/images/gallery/vizsla2.jpg",
                    "/images/gallery/vizsla3.png",
                    "/images/gallery/vizsla4.jpg"
                },
                ImagePostedBy = "Niamh Browne"
            },

            new PastEventImage
        {
            ImageTitle = "Golden Retriever Festivities",
            ImageDescription = "Golden Retrievers taking a scenic walk along the Lagan Towpath.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/golden1.jpeg",
                "/images/gallery/golden2.jpg",
                "/images/gallery/golden3.jpg"
            },
            ImagePostedBy = "Emily White"
        },
        new PastEventImage
        {
            ImageTitle = "Dachshunds on the Gobbins",
            ImageDescription = "Adventurous Dachshunds exploring the Gobbins Cliff Path.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/dachshund1.jpg",
                "/images/gallery/dachshund2.png"
            },
            ImagePostedBy = "Alex Johnson"
        },
        new PastEventImage
        {
            ImageTitle = "Labrador Splash Time",
            ImageDescription = "Labradors playing fetch and swimming at Lough Neagh.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/labrador1.jpg",
                "/images/gallery/labrador2.jpg"
            },
            ImagePostedBy = "Sarah Black"
        },
        new PastEventImage
        {
            ImageTitle = "Shih Tzu Afternoon Stroll",
            ImageDescription = "Shih Tzus mingling and relaxing at Armagh Palace Demesne Park.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/shihtzu1.jpg",
                "/images/gallery/shihtzu2.jpg"
            },
            ImagePostedBy = "John Smith"
        },
        new PastEventImage
        {
            ImageTitle = "Cocker Spaniels in Action",
            ImageDescription = "Cocker Spaniels enjoying the green spaces of Fota Gardens.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/cocker1.jpg",
                "/images/gallery/cocker2.jpg"
            },
            ImagePostedBy = "Hannah Lee"
        },
        new PastEventImage
        {
            ImageTitle = "Pug Parade",
            ImageDescription = "Pugs taking over Elizabeth Fort for a fun day out.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/pug1.jpg",
                "/images/gallery/pug2.jpg",
                "/images/gallery/pug3.jpg"
            },
            ImagePostedBy = "Liam Carter"
        },
        new PastEventImage
        {
            ImageTitle = "Border Collie Agility",
            ImageDescription = "Border Collies showcasing their agility skills at Phoenix Park.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/collie1.jpg",
                "/images/gallery/collie2.jpg"
            },
            ImagePostedBy = "Emily Adams"
        },
        new PastEventImage
        {
            ImageTitle = "French Bulldogs at the Beach",
            ImageDescription = "French Bulldogs enjoying the sand and waves at Sandymount Strand.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/frenchie1.jpg",
                "/images/gallery/fenchie2.jpg"
            },
            ImagePostedBy = "Chris Walker"
        },
        new PastEventImage
        {
            ImageTitle = "German Shepherd Gathering",
            ImageDescription = "German Shepherds socialising at Eyre Square in Galway City.",
            GalleryImageUrls = new List<string>
            {
                "/images/gallery/germanshepherd1.jpg",
                "/images/gallery/germanshepherd2.jpg"
            },
            ImagePostedBy = "Sophia Brown"
        }
        };
        foreach (var image in images)
        {
            galleryService.AddImage(image);
        }
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
        var calendars = new List<Calendar>
    {
        new Calendar
        {
            Title = "Hungarian Vizsla Meet-Up",
            Location = "Belfast City Hall",
            Start = new DateTime(2024, 11, 20, 14, 0, 0),
            End = new DateTime(2024, 11, 20, 16, 0, 0),
            CountyId = 2, // Antrim
            UserId = 1
        },
        new Calendar
        {
            Title = "Golden Retriever Meet-Up",
            Location = "Lagan Valley Regional Park, Lisburn",
            Start = new DateTime(2024, 12, 15, 11, 0, 0),
            End = new DateTime(2024, 12, 15, 13, 0, 0),
            CountyId = 5, // Down
            UserId = 1
        },
        new Calendar
        {
            Title = "Dachshund Meet-Up",
            Location = "The Gobbins Cliff Path, Larne",
            Start = new DateTime(2025, 3, 10, 13, 30, 0),
            End = new DateTime(2025, 3, 10, 15, 30, 0),
            CountyId = 2, // Antrim
            UserId = 1
        },
        new Calendar
        {
            Title = "Labrador Meet-Up",
            Location = "Lough Neagh Discovery Centre, Craigavon",
            Start = new DateTime(2025, 6, 5, 10, 0, 0),
            End = new DateTime(2025, 6, 5, 12, 0, 0),
            CountyId = 3, // Armagh
            UserId = 1
        },
        new Calendar
        {
            Title = "Shih Tzu Meet-Up",
            Location = "Armagh Palace Demesne Park, Armagh City",
            Start = new DateTime(2023, 9, 28, 15, 0, 0),
            End = new DateTime(2023, 9, 28, 17, 0, 0),
            CountyId = 3, // Armagh
            UserId = 1
        },
        new Calendar
        {
            Title = "Cocker Spaniel Meet-Up",
            Location = "Fota House Arboretum and Gardens, Cork",
            Start = new DateTime(2024, 10, 12, 12, 30, 0),
            End = new DateTime(2024, 10, 12, 14, 30, 0),
            CountyId = 10, // Cork
            UserId = 1
        },
        new Calendar
        {
            Title = "Pug Meet-Up",
            Location = "Elizabeth Fort, Cork City",
            Start = new DateTime(2025, 1, 7, 14, 0, 0),
            End = new DateTime(2025, 1, 7, 16, 0, 0),
            CountyId = 10, // Cork
            UserId = 1
        },
        new Calendar
        {
            Title = "Border Collie Meet-Up",
            Location = "Phoenix Park, Dublin",
            Start = new DateTime(2025, 9, 16, 10, 30, 0),
            End = new DateTime(2025, 9, 16, 12, 30, 0),
            CountyId = 12, // Dublin
            UserId = 1
        },
        new Calendar
        {
            Title = "French Bulldog Meet-Up",
            Location = "Sandymount Strand, Dublin",
            Start = new DateTime(2023, 8, 20, 13, 0, 0),
            End = new DateTime(2023, 8, 20, 15, 0, 0),
            CountyId = 12, // Dublin
            UserId = 1
        },
        new Calendar
        {
            Title = "German Shepherd Meet-Up",
            Location = "Eyre Square, Galway City",
            Start = new DateTime(2024, 5, 18, 11, 0, 0),
            End = new DateTime(2024, 5, 18, 13, 0, 0),
            CountyId = 13, // Galway
            UserId = 1
        },
        new Calendar
        {
            Title = "Beagle Meet-Up",
            Location = "Connemara National Park, Galway",
            Start = new DateTime(2025, 2, 23, 10, 0, 0),
            End = new DateTime(2025, 2, 23, 12, 0, 0),
            CountyId = 13, // Galway
            UserId = 1
        },
        new Calendar
        {
            Title = "Siberian Husky Meet-Up",
            Location = "Killarney National Park, Kerry",
            Start = new DateTime(2025, 12, 3, 15, 0, 0),
            End = new DateTime(2025, 12, 3, 17, 0, 0),
            CountyId = 14, // Kerry
            UserId = 1
        },
        new Calendar
        {
            Title = "Jack Russell Meet-Up",
            Location = "Inch Beach, Kerry",
            Start = new DateTime(2023, 7, 14, 11, 30, 0),
            End = new DateTime(2023, 7, 14, 13, 30, 0),
            CountyId = 14, // Kerry
            UserId = 1
        },
        new Calendar
        {
            Title = "Yorkshire Terrier Meet-Up",
            Location = "Gortin Glen Forest Park, Omagh",
            Start = new DateTime(2024, 4, 20, 14, 0, 0),
            End = new DateTime(2024, 4, 20, 16, 0, 0),
            CountyId = 1, // Tyrone
            UserId = 1
        }
    };

        // Add the calendars to the service
        foreach (var calendar in calendars)
        {
            calendarService.AddCalendar(calendar);
        }

        // var calendar1 = new Calendar
        // {
        //     Title = "Dog Walk in Belfast",
        //     Location = "Belfast",
        //     Start = new DateTime(2024, 11, 20, 10, 0, 0),
        //     End = new DateTime(2024, 11, 20, 12, 0, 0),
        //     CountyId = 1,
        //     UserId = 1
        // };
        // calendarService.AddCalendar(calendar1);

        // var calendar2 = new Calendar
        // {
        //     Title = "Dog Walk in Belfast",
        //     Location = "Belfast",
        //     Start = new DateTime(2024, 11, 20, 10, 0, 0),
        //     End = new DateTime(2024, 11, 20, 12, 0, 0),
        //     CountyId = 2,
        //     UserId = 1
        // };
        // calendarService.AddCalendar(calendar2);
    }
}



