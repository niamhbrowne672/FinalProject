
using Bogus;
using FinalProject.Data.Entities;

namespace FinalProject.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy test data using an IUserService 
        public static void Seed(IUserService svc, IPostService psvc)
        {
            // seeder destroys and recreates the database - NOT to be called in production!!!
            svc.Initialise();

            // add users
            svc.AddUser("Administrator", "admin@mail.com", "admin", Role.admin);
            svc.AddUser("Manager", "manager@mail.com", "manager", Role.manager);
            svc.AddUser("Guest", "guest@mail.com", "guest", Role.guest); 

            // optionally add some fake users
            var faker = new Faker();
            for(int i=1; i<=100; i++)
            {
                var s = svc.AddUser(
                    faker.Name.FullName(),
                    faker.Internet.Email(),
                    "password",
                    Role.guest
                );
            }

            // add some fake posts
            // var userIdArray = svc.GetUsers().Items.Select(u => u.Id).ToArray();
            // add some fake posts
            for(int i=1; i<=200; i++)
            {
                var p = new Post {
                    Title = faker.Lorem.Sentence(),
                    Body = faker.Lorem.Paragraph(),
                    UserId = faker.Random.ArrayElement([1,2,3]) //faker.Random.ArrayElement(userIdArray)
                };

                psvc.AddPost(p);
            }
        }
    }

}